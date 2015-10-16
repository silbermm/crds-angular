/****** Object:  StoredProcedure [dbo].[crds_Update_Donor_Statement_Type]    Script Date: 10/14/2015 8:57:30 AM ******/
USE [MinistryPlatform]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Create a placeholder proc, so the below ALTER will always work.
-- This allows the same SQL file to be run in any environment, without errors.
-- This is preferable to dropping the proc, as permissions will be maintained
-- on an existing proc.
IF OBJECT_ID('dbo.crds_Update_Donor_Statement_Type', 'p') IS NULL
    EXEC('CREATE PROCEDURE dbo.crds_Update_Donor_Statement_Type AS SELECT 1')
GO
-- =======================================================================
-- Author:    Sandi Ritter
-- Create date: 10/12/2015
-- Description: This SP will change the donor Statement Type to Individual 
--               when the Contact is not a Head of Household
-- =======================================================================
ALTER PROCEDURE [dbo].[crds_Update_Donor_Statement_Type]
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
