USE [MinistryPlatform]
GO

DELETE FROM [dp_Communications] WHERE [Communication_ID] = 12259 AND [Template] <> 1;

SET IDENTITY_INSERT [dbo].[dp_Communications] ON
GO

INSERT INTO [dp_Communications]
  (
     [Communication_ID]
    ,[Author_User_ID]
    ,[Subject]
    ,[Body]
    ,[Domain_ID]
    ,[Start_Date]
    ,[From_Contact]
    ,[Reply_to_Contact]
    ,[Template]
    ,[Active]
  )
VALUES
  (
     12259
    ,5
    ,'Check Scanner Batch Successfully Processed'
    ,'Successfully Processed check scanner batch [batchName] with checks for program id [programId].<div><br /></div>Batch Result:<br /><pre>[batch]</pre>'
    ,1
    ,CAST('20150801 00:00:00.000' as DATETIME)
    ,7
    ,7
    ,1
    ,1
  );

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO

DELETE FROM [dp_Communications] WHERE [Communication_ID] = 12260 AND [Template] <> 1;

SET IDENTITY_INSERT [dbo].[dp_Communications] ON
GO

INSERT INTO [dp_Communications]
  (
     [Communication_ID]
    ,[Author_User_ID]
    ,[Subject]
    ,[Body]
    ,[Domain_ID]
    ,[Start_Date]
    ,[From_Contact]
    ,[Reply_to_Contact]
    ,[Template]
    ,[Active]
  )
VALUES
  (
     12260
    ,5
    ,'Check Scanner Batch Failed to Process'
    ,'Failed to Process check scanner batch [batchName] with checks for program id [programId].<div><br /></div>Batch Result:<br /><pre>[error]</pre>'
    ,1
    ,CAST('20150801 00:00:00.000' as DATETIME)
    ,7
    ,7
    ,1
    ,1
  );

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO
