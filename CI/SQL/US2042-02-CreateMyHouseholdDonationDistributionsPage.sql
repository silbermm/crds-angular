USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Pages] ON
GO

-- Add a new "My Household Donation Distributions" page for giving history
INSERT INTO [dbo].[dp_Pages](
   [Page_ID]
  ,[Display_Name]
  ,[Singular_Name]
  ,[Description]
  ,[View_Order]
  ,[Table_Name]
  ,[Primary_Key]
  ,[Display_Search]
  ,[Default_Field_List]
  ,[Selected_Record_Expression]
  ,[Filter_Clause]
  ,[Contact_ID_Field]
  ,[Display_Copy]
) VALUES (
   516
  ,'My Household Donation Distributions'
  ,'My Household Donation Distribution'
  ,'Distributions of the donation amount to programs, event projects, or pledges.  This is used by the Giving History page in CR.net'
  ,20
  ,'Donation_Distributions'
  ,'Donation_Distribution_ID'
  ,1
  ,'Donation_ID_Table.Donation_Date
,CASE WHEN (Donation_Distributions.Soft_Credit_Donor IS NULL) THEN ''False'' ELSE ''True'' END AS [Soft_Credit_Donation]
,ISNULL(Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.Last_Name,Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.Display_Name) AS [Last_Name]
,Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.First_Name
,Donation_Distributions.Amount
,Donation_ID_Table_Payment_Type_ID_Table.Payment_Type
,Donation_ID_Table.Item_Number
,Program_ID_Table.Program_Name
,Program_ID_Table.Statement_Title
,Pledge_ID_Table_Pledge_Campaign_ID_Table.Campaign_Name
,Donation_Distributions.Donation_ID
,Donation_ID_Table.Donor_ID
,Donation_ID_Table_Batch_ID_Table.Batch_ID
,Pledge_ID_Table.Pledge_ID
,Target_Event_Table.Event_Title AS [Target_Event]
,Donation_ID_Table.Donation_Status_Date ,Donation_ID_Table.Donation_Status_ID ,Donation_ID_Table.Transaction_Code ,Donation_ID_Table.Payment_Type_ID ,Soft_Credit_Donor_Table.Donor_ID AS [Soft_Credit_Donor_ID] ,Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.[Display_Name] AS [Donor_Display_Name]
'
  ,'Program_ID_Table.Program_Name','Donation_Distributions.Donation_Distribution_ID IN (
SELECT * FROM [dbo].[crds_udfGetDonationDistributionIdsForUser](dp_UserID)
)
'
  ,'Donation_ID_Table_Donor_ID_Table.Contact_ID'
  ,0);

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO

-- Add this page to the "My Pages" section
INSERT INTO [dbo].[dp_Page_Section_Pages] (
  [Page_ID],
  [Page_Section_ID]
) VALUES (
  516,
  17
);

-- Grant "Full" access on this page to "All Platform Users" role
INSERT INTO [dbo].[dp_Role_Pages] (
  [Role_ID],
  [Page_ID],
  [Access_Level],
  [Scope_All],
  [Approver],
  [File_Attacher],
  [Data_Importer],
  [Data_Exporter],
  [Secure_Records],
  [Allow_Comments],
  [Quick_Add]
) VALUES (
  39,
  516,
  3,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0
);
