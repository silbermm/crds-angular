USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[Forms] ON 


GO
INSERT [dbo].[Forms] ([Form_ID], [Form_Title], [Instructions], [Standalone_Form_Use_Only], [Get_Contact_Info], [Get_Address_Info], [Domain_ID], [Form_GUID]) VALUES (16, N'Kids'' Club Student Volunteer Application ', N'Please complete the form and submit for consideration.', NULL, 1, 1, 1, N'102dabf6-9aa5-485e-bfe1-bef1880c7b8b')
GO
INSERT [dbo].[Forms] ([Form_ID], [Form_Title], [Instructions], [Standalone_Form_Use_Only], [Get_Contact_Info], [Get_Address_Info], [Domain_ID], [Form_GUID]) VALUES (17, N'Kids'' Club Adult Volunteer Application', N'Complete the following information for consideration.', NULL, 0, 0, 1, N'edc94c78-12c2-49dc-9792-cfb169d68c47')

GO
SET IDENTITY_INSERT [dbo].[Forms] OFF
GO
