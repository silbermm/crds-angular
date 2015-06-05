USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Pages] ON
GO
DELETE FROM [dbo].[dp_Pages]
WHERE Page_ID = 481
GO

INSERT INTO [dbo].[dp_Pages]
           ([Page_ID]
           ,[Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Display_Search]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Filter_Clause]
           ,[Contact_ID_Field]
           ,[Direct_Delete_Only]
           ,[System_Name]
           ,[Custom_Form_Name]
           ,[Display_Copy])
     VALUES
           (481
           ,'Emails'
           ,'Email'
           ,'The master list of active email addresses that are, or have been, involved in some way with your crossroads.'
           ,1
           ,'Contacts'
           ,'Contact_ID'
           ,1
           ,'Contacts.Display_Name, Contacts.Contact_ID, Contacts.Email_Address, User_Account_Table.[User_ID]'
           ,'Contacts.Email_Address'
           ,'User_Account_Table.[User_ID] > 0'
           ,'Contacts.Contact_ID'
           ,1
           ,'contacts'
           ,'Contact'
           ,0)
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO
