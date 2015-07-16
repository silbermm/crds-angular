USE [MinistryPlatform]
GO

DECLARE @CANCEL_SUBJECT varchar(50);
SET @CANCEL_SUBJECT = N'Serve Signup Cancelation';
DECLARE @CONFIRM_BODY varchar(255)
SET @CONFIRM_BODY = N'Thanks for signing up [Volunteer_Name] to serve<br/><br/> [Html_Table]';

DECLARE @CANCEL_BODY varchar(255)
SET @CANCEL_BODY = N'Thanks for letting us know that [Volunteer_Name] can''t make it.<br/><br/> [Html_Table]';
DECLARE @CONFIRM_SUBJECT varchar(50)
SET @CONFIRM_SUBJECT = N'Serve Signup Confirmation';

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Communications]
  WHERE [Subject] = @CONFIRM_SUBJECT
  AND [Template] = 1)
  BEGIN
    INSERT INTO [dbo].[dp_Communications]
               ([Author_User_ID]
               ,[Subject]
               ,[Body]
               ,[Domain_ID]
               ,[Start_Date]
               ,[Expire_Date]
               ,[Communication_Status_ID]
               ,[From_Contact]
               ,[Reply_to_Contact]
               ,[_Sent_From_Task]
               ,[Selection_ID]
               ,[Template]
               ,[Active]
               ,[To_Contact])
            VALUES
               (5
               ,@CONFIRM_SUBJECT
               ,@CONFIRM_BODY
               ,1
               ,GETDATE()
               ,NULL
               ,NULL
               ,7
               ,7
               ,NULL
               ,NULL
               ,1
               ,1
               ,NULL)
            END
        ELSE
          BEGIN
            UPDATE [dbo].[dp_Communications]
            SET [Body] = @CONFIRM_BODY
            WHERE [Subject] = @CONFIRM_SUBJECT
            AND [Template] = 1
          END
IF NOT EXISTS (SELECT * FROM [dbo].[dp_Communications]
  WHERE [Subject] = @CANCEL_SUBJECT
  AND [Template] = 1)
  BEGIN
    INSERT INTO [dbo].[dp_Communications]
           ([Author_User_ID]
           ,[Subject]
           ,[Body]
           ,[Domain_ID]
           ,[Start_Date]
           ,[Expire_Date]
           ,[Communication_Status_ID]
           ,[From_Contact]
           ,[Reply_to_Contact]
           ,[_Sent_From_Task]
           ,[Selection_ID]
           ,[Template]
           ,[Active]
           ,[To_Contact])
        VALUES
           (5
           ,@CANCEL_SUBJECT
           ,@CANCEL_BODY 
           ,1
           ,GETDATE()
           ,NULL
           ,NULL
           ,7
           ,7
           ,NULL
           ,NULL
           ,1
           ,1
           ,NULL)
      END
    ELSE
      BEGIN
        UPDATE [dbo].[dp_Communications]
            SET [Body] = @CANCEL_BODY 
            WHERE [Subject] = @CANCEL_SUBJECT
            AND [Template] = 1
      END
GO

