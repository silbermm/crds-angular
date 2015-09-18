USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
SET [Field_List] = CONCAT([Field_List], ', Contact_ID_Table_Household_ID_Table.[Household_ID] AS [Household_ID]
, Statement_Type_ID_Table.[Statement_Type_ID] AS [Statement_Type_ID]')
WHERE [Page_View_ID] = 1058;
GO
