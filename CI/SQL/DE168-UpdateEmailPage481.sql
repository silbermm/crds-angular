USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
SET [Display_Name] = 'Emails'
  ,[Singular_Name] = 'Email'
  ,[Description] = 'The master list of active email addresses that are, or have been, involved in some way with your crossroads.'
  ,[View_Order] = 1
  ,[Table_Name] = 'Contacts'
  ,[Primary_Key] = 'Contact_ID'
  ,[Display_Search] = 1
  ,[Default_Field_List] = 'Contacts.Display_Name, Contacts.Contact_ID, Contacts.Email_Address, User_Account_Table.[User_ID]'
  ,[Selected_Record_Expression] = 'Contacts.Email_Address'
  ,[Filter_Clause] = 'User_Account_Table.[User_ID] > 0'
  ,[Contact_ID_Field] = 'Contacts.Contact_ID'
  ,[Direct_Delete_Only] = 1
  ,[System_Name] = 'contacts'
  ,[Custom_Form_Name] = 'Contact'
  ,[Display_Copy] = 0
WHERE [Page_ID] = 481;
GO
