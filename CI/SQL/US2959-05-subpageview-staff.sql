USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON
GO
DELETE FROM [dbo].[dp_Sub_Page_Views]
WHERE Sub_Page_View_ID = 121
GO

INSERT INTO [dbo].[dp_Sub_Page_Views]
           ([Sub_Page_View_ID]
           ,[View_Title]
		 ,[Description]
           ,[Sub_Page_ID]
           ,[Field_List]
           ,[View_Clause]
		 ,[Order_By])
     VALUES
           (121
           ,'UserDetails'
		 ,'Display details for api'
           ,363
           ,'dp_User_Roles.[User_Role_ID], User_ID_Table_Contact_ID_Table.[Contact_ID], User_ID_Table_Contact_ID_Table.[Display_Name], Role_ID_Table.[Role_ID], Role_ID_Table.[Role_Name]'
           ,'dp_User_Roles.[User_Role_ID] IS NOT NULL'
		 ,'User_ID_Table_Contact_ID_Table.[Display_Name]')
GO

SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] OFF
GO