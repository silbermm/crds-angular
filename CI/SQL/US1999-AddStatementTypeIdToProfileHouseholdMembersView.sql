USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Sub_Page_Views]
SET [Field_List] = 'Contacts.[Contact_ID], Contacts.[First_Name], Contacts.[Nickname], Contacts.[Last_Name], Household_Position_ID_Table.[Household_Position_ID]
, Household_Position_ID_Table.[Household_Position], Contacts.[Date_of_Birth], Contacts.[__Age], Donor_Record_Table_Statement_Type_ID_Table.[Statement_Type_ID], Donor_Record_Table.[Donor_ID]'
WHERE [Sub_Page_View_ID] = 102;
GO
