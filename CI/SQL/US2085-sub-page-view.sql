USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON
GO
DELETE FROM [dbo].[dp_Sub_Page_Views]
WHERE Sub_Page_View_ID = 111
GO

INSERT INTO [dbo].[dp_Sub_Page_Views]
           ([Sub_Page_View_ID]
           ,[View_Title]
           ,[Sub_Page_ID]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (111
           ,'Events with Congregation'
           ,302
           ,'Event_ID_Table.[Event_ID], Event_ID_Table.[Event_Title], Event_ID_Table_Congregation_ID_Table.[Congregation_Name], Event_ID_Table.[Event_Start_Date]'
           ,'Event_ID_Table.[Event_ID] IS NOT NULL')
GO

SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] OFF
GO
