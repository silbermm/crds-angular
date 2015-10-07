USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
		   ,[Field_List]
           ,[View_Clause])
     VALUES
           (92180
		   ,'Groups By Event Id'
		   ,408
           ,NULL
		   ,'Event_ID_Table.[Event_ID] , Group_ID_Table.[Group_ID] , Group_ID_Table.[Group_Name]'
           ,'Event_ID_Table.[Event_ID] IS NOT NULL')

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO