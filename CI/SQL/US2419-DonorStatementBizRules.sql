USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (2187
		   ,'DI - Fix Too Many Heads'
           ,327
           ,'Data Integrity View - Households should have no more than 2 heads. Please review and correct'
           ,NULL
           ,'EXISTS (SELECT 1 FROM Contacts C WHERE C.Household_ID = Households.Household_ID AND Household_Position_ID = 1 AND C.Contact_Status_ID = 1 GROUP BY Household_ID, C.Contact_Status_ID HAVING Count(*)>2)')

GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO





/****** Object:  StoredProcedure [dbo].[crds_Update_Donor_Statement_Type]    Script Date: 10/14/2015 8:57:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- Author:    Sandi Ritter
-- Create date: 10/12/2015
-- Description: This SP will change the donor Statement Type to Individual 
--               when the Contact is not a Head of Household
-- =======================================================================
CREATE PROCEDURE  [dbo].[crds_Update_Donor_Statement_Type]  
  
AS
BEGIN
  
  DECLARE @DonorIdTbl TABLE (ID INT, New_Stmt_Type INT, Prev_Stmt_Type INT)

  UPDATE  D
  SET Statement_Type_ID = 1 
  OUTPUT INSERTED.Donor_ID, INSERTED.Statement_Type_ID, DELETED.Statement_Type_ID INTO @DonorIdTbl
  FROM Donors D 
    INNER JOIN Contacts C ON C.Contact_ID = D.Contact_ID
    INNER JOIN Households H ON H.Household_ID = C.Household_ID
    INNER JOIN Statement_Types S ON S.Statement_Type_ID =  D.Statement_Type_ID
  WHERE D.Statement_Type_ID = 2  and C.Household_Position_ID > 1
  
  DECLARE @AuditLogTbl TABLE (AID INT, Record_ID INT)
    INSERT INTO dbo.dp_Audit_Log
       (Table_Name, Record_ID, Audit_Description, User_Name, User_ID, Date_Time)
        OUTPUT INSERTED.Audit_Item_ID, INSERTED.Record_ID INTO @AuditLogTbl    
       SELECT 'Donors', ID,'Updated','crds_Update_Donor_Statement_Type', -1, GETDATE() FROM @DonorIdTbl
      
  INSERT INTO dbo.dp_Audit_Detail
    (Audit_Item_ID, Field_Name, Field_Label, Previous_Value, New_Value, Previous_ID, New_ID)
  SELECT AId, 'Statement_Type_ID','Stmt Type','Family', 'Individual', Prev_Stmt_Type, New_Stmt_Type 
  FROM @AuditLogTbl A 
    INNER JOIN @DonorIdTbl T on T.ID = A.Record_ID    
             
END





USE [msdb]
GO

/****** Object:  Job [Crossroads.Daily]    Script Date: 10/13/2015 3:07:47 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 10/13/2015 3:07:47 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'Crossroads.Daily', 
    @enabled=1, 
    @notify_level_eventlog=0, 
    @notify_level_email=0, 
    @notify_level_netsend=0, 
    @notify_level_page=0, 
    @delete_level=0, 
    @description=N'Crossroads defined jobs that should on a daily schedule', 
    @category_name=N'[Uncategorized (Local)]', 
    @owner_login_name=N'MP-INT-DB\MPAdmin', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Update Donor Statement Type - DI]    Script Date: 10/13/2015 3:07:48 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Update Donor Statement Type - DI', 
    @step_id=1, 
    @cmdexec_success_code=0, 
    @on_success_action=1, 
    @on_success_step_id=0, 
    @on_fail_action=3, 
    @on_fail_step_id=0, 
    @retry_attempts=0, 
    @retry_interval=0, 
    @os_run_priority=0, @subsystem=N'TSQL', 
    @command=N'EXEC  [dbo].[crds_Update_Donor_Statement_Type]', 
    @database_name=N'MinistryPlatform', 
    @flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Once Per Day', 
    @enabled=1, 
    @freq_type=4, 
    @freq_interval=1, 
    @freq_subday_type=1, 
    @freq_subday_interval=1, 
    @freq_relative_interval=0, 
    @freq_recurrence_factor=0, 
    @active_start_date=20151016, 
    @active_end_date=99991231, 
    @active_start_time=20000, 
    @active_end_time=235959, 
    @schedule_uid=N'95f7f968-a050-407e-b101-5588f8cd3109'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO


