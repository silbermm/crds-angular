USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
   SET [Selected_Record_Expression] = 'Contacts.Display_Name + ''; '' + ISNULL(Contacts.Email_Address,'''')'
 WHERE Page_ID = 292
GO

UPDATE [dbo].[dp_Pages]
   SET [Selected_Record_Expression] = 'Contact_ID_Table.Display_Name + ''; '' + ISNULL(Contact_ID_Table.Email_Address,'''')'
 WHERE Page_ID = 299
GO

