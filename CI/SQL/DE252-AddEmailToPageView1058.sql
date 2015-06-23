USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
SET  [Field_List] = 'Contact_ID_Table.Contact_ID
					,Donors.Donor_ID
					,Donors.Processor_ID
					,Statement_Frequency_ID_Table.Statement_Frequency
					,Statement_Type_ID_Table.[Statement_Type]
					,Statement_Method_ID_Table.[Statement_Method]
					,Donors.[Setup_Date]
					,Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.[Congregation_ID]
					,Contact_ID_Table.[Email_Address] AS [Email]'
		
WHERE [Page_View_ID] = 1058;
GO