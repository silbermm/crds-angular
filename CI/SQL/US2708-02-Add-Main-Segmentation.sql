USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 2197)
BEGIN
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (2197
		   ,'Segmentation: Base'
           ,292
           ,'Mailchimp Segmentation -- Preschool Children of n Age'
           ,'Contacts.Contact_ID, Contacts.Nickname, Contacts.Last_Name, Contacts.Email_Address, Gender_ID_Table.Gender, Marital_Status_ID_Table.Marital_Status'
           ,'Contacts.Email_Address is not null AND Contacts.__Age>12')

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
GO
