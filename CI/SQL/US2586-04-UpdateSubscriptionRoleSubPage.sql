USE [MinistryPlatform]
GO

IF EXISTS (SELECT * FROM [dbo].[dp_Role_Sub_Pages] WHERE [Role_Sub_Page_ID] = 15483)
BEGIN

UPDATE [dbo].[dp_Role_Sub_Pages]

SET
	[Role_ID] = 39,
    [Sub_Page_ID] = 424,
    [Access_Level] = 2
	
WHERE
	[Role_Sub_Page_ID] = 15483


END

ELSE

BEGIN

SET IDENTITY_INSERT [dbo].[dp_Role_Sub_Pages] ON 

INSERT INTO [dbo].[dp_Role_Sub_Pages]
           ([Role_Sub_Page_ID]
           ,[Role_ID]
           ,[Sub_Page_ID]
           ,[Access_Level])
     VALUES
           (15483
           ,39
           ,424
           ,2)

SET IDENTITY_INSERT [dbo].[dp_Role_Sub_Pages] OFF

END
GO