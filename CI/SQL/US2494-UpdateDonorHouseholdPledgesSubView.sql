USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON;

UPDATE [dbo].[dp_Sub_Page_Views]
  SET [Field_List] = 'Donor_ID_Table_Contact_ID_Table.[Display_Name]
  , Donor_ID_Table_Contact_ID_Table.[Nickname]
  , Donor_ID_Table_Contact_ID_Table.[Last_Name]
  , Donor_ID_Table_Contact_ID_Table.[Email_Address]
  , Donor_ID_Table_Contact_ID_Table.[Mobile_Phone]
  , Donor_ID_Table_Contact_ID_Table.[Company_Phone]
  , Donor_ID_Table_Contact_ID_Table_Household_ID_Table_Address_ID_Table.[Address_Line_1]
  , Donor_ID_Table_Contact_ID_Table_Household_ID_Table_Address_ID_Table.[City]
  , Donor_ID_Table_Contact_ID_Table_Household_ID_Table_Address_ID_Table.[State/Region]
  , Donor_ID_Table_Contact_ID_Table_Household_ID_Table_Address_ID_Table.[Postal_Code]
  , Pledge_Campaign_ID_Table.[Campaign_Name]
  , Pledges.[Total_Pledge]
  , Pledge_Status_ID_Table.[Pledge_Status]'
  ,[View_Clause] = 'Pledges.Pledge_Status_ID IN (1, 4) AND
  Pledges.Pledge_ID IN (SELECT * FROM [dbo].[crds_udfGetPledgeIdsForDonor](dp_ParentID))'
WHERE [Sub_Page_View_ID] = 113;

GO
