USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
           ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (2191
           ,'Pledges by Household'
           ,363
           ,'Allows searching for pledges based on household.'
           ,'Donor_ID_Table_Contact_ID_Table_Household_ID_Table.[Household_Name]
, Donor_ID_Table_Statement_Type_ID_Table.[Statement_Type]
, Donor_ID_Table_Contact_ID_Table.[Display_Name]
, Donor_ID_Table_Contact_ID_Table.[Nickname]
, Donor_ID_Table_Contact_ID_Table.[First_Name]
, Pledges.[Total_Pledge]
, Pledge_Campaign_ID_Table.[Campaign_Name]
, Pledges.[Beneficiary] AS [Beneficiary]
, Pledge_Status_ID_Table.[Pledge_Status]
, Pledges.[First_Installment_Date]'
           ,'1=1')

GO
