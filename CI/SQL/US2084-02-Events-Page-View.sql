USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO
DELETE FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = 2188
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
           ,[View_Title]
           ,[Page_ID]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (2188
           ,'Event with Details'
           ,308
           ,'Events.[Event_ID], Events.[Event_Title], Event_Type_ID_Table.[Event_Type], Event_Type_ID_Table.[Event_Type_ID], Events.[Event_Start_Date], Events.[Event_End_Date], Primary_Contact_Table.[Contact_ID], Primary_Contact_Table.[Email_Address]'
           ,'Events.[Event_ID] IS NOT NULL')
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO
