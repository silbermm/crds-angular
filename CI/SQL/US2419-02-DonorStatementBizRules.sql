USE [msdb]
GO
/****** Object:  Job [Crossroads.Daily]    Script Date: 10/14/2015 11:49:05 AM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 10/14/2015 11:49:06 AM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
    EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
END

IF NOT EXISTS (SELECT name FROM msdb.dbo.sysjobs WHERE name = 'Crossroads.Daily')
BEGIN
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
            @owner_login_name=N'MPTEST02\Administrator', @job_id = @jobId OUTPUT
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
    /****** Object:  Step [Update Donor Statement Type - DI]    Script Date: 10/14/2015 11:49:07 AM ******/
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
END
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO
