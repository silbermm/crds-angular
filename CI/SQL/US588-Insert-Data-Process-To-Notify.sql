USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Processes] ON 

GO
INSERT [dbo].[dp_Processes] ([Process_ID], [Process_Name], [Process_Manager], [Active], [Description], [Record_Type], [Domain_ID], [On_Submit], [On_Complete], [Trigger_Fields], [Dependent_Condition]) VALUES (31, N'Notify of Volunteer Form Response', 5, 1, NULL, 424, 1, NULL, NULL, N'Form_Response_ID', NULL)
GO
SET IDENTITY_INSERT [dbo].[dp_Processes] OFF
GO
