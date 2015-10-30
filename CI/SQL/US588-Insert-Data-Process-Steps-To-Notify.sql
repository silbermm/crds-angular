USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Process_Steps] ON
GO
INSERT INTO [dbo].[dp_Process_Steps] ([Process_Step_ID]
, [Step_Name]
, [Instructions]
, [Process_Step_Type_ID]
, [Escalation_Only]
, [Order]
, [Process_ID]
, [Specific_User]
, [Supervisor_User]
, [Lookup_User_Field]
, [Domain_ID])
  VALUES (66, 'Form Submitted', 'A form has been submitted', 2, 0, 1, 31, NULL, 0, 'Opportunity_ID_Table_Add_to_Group_Table_Primary_Contact_Table.[User_Account]', 1)

GO
SET IDENTITY_INSERT [dbo].[dp_Process_Steps] OFF
GO
