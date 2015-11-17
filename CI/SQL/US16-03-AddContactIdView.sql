USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 2194)
BEGIN 

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
		   ,[Field_List]
           ,[View_Clause])
     VALUES
           (2194
		   ,'ContactIdByUser'
		   ,401
           ,'ContactIdByUser'
		   ,'dp_Users.[User_ID] AS [User ID], Contact_ID_Table.[Contact_ID] AS [Contact ID]'
           ,'Contact_ID_Table.[User_Account] IS NOT NULL')

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
GO