USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[crds_di_PopulatePreferredName]    Script Date: 10/22/2015 2:57:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.crds_di_PopulatePreferredName', 'p') IS NULL
    EXEC('CREATE PROCEDURE dbo.crds_di_PopulatePreferredName AS SELECT 1')
GO

ALTER PROCEDURE [dbo].[crds_di_PopulatePreferredName]
AS
BEGIN

DECLARE @ContactIdTbl TABLE (ID INT, New_Nickname VARCHAR(MAX), Prev_Nickname VARCHAR(MAX))

  UPDATE  C
  SET Nickname = First_Name 
  OUTPUT INSERTED.Contact_ID, INSERTED.Nickname, DELETED.Nickname INTO @ContactIdTbl
  FROM Contacts C 
  WHERE Nickname IS NULL and First_Name IS NOT NULL
  
  DECLARE @AuditLogTbl TABLE (AID INT, Record_ID INT)
    INSERT INTO dbo.dp_Audit_Log
       (Table_Name, Record_ID, Audit_Description, User_Name, User_ID, Date_Time)
        OUTPUT INSERTED.Audit_Item_ID, INSERTED.Record_ID INTO @AuditLogTbl    
       SELECT 'Contacts', ID,'Updated','crds_di_PopulatePreferredName', -1, GETDATE() FROM @ContactIdTbl
      
  INSERT INTO dbo.dp_Audit_Detail
    (Audit_Item_ID, Field_Name, Field_Label, Previous_Value, New_Value)
  SELECT AId, 'Nickname','Nickname', Prev_Nickname, New_Nickname 
  FROM @AuditLogTbl A 
    INNER JOIN @ContactIdTbl T on T.ID = A.Record_ID   

END