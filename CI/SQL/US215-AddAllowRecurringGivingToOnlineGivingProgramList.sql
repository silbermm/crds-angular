USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO

UPDATE [dbo].[dp_Page_Views]
SET Field_List = 'Programs.[Program_ID] AS [Program_ID]
,Programs.[Program_Name] AS [Program_Name]
,Program_Type_ID_Table.[Program_Type] AS [Program_Type]
,Program_Type_ID_Table.[Program_Type_ID] AS [Program_Type_ID]
,Programs.[Online_Sort_Order] AS [Online_Sort_Order]
,Programs.[Allow_Recurring_Giving] AS [Allow_Recurring_Giving]'
WHERE [Page_View_ID] = 1038;

GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO
