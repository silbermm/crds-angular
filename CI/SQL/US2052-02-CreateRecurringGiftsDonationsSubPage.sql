USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON
GO

INSERT INTO [dbo].[dp_Sub_Pages] (
   [Sub_Page_ID]
  ,[Display_Name]
  ,[Singular_Name]
  ,[Page_ID]
  ,[View_Order]
  ,[Link_To_Page_ID]
  ,[Link_From_Field_Name]
  ,[Select_To_Page_ID]
  ,[Select_From_Field_Name]
  ,[Primary_Table]
  ,[Primary_Key]
  ,[Default_Field_List]
  ,[Selected_Record_Expression]
  ,[Filter_Key]
  ,[Relation_Type_ID]
  ,[On_Quick_Add]
  ,[Contact_ID_Field]
  ,[Default_View]
  ,[System_Name]
  ,[Date_Pivot_Field]
  ,[Start_Date_Field]
  ,[End_Date_Field]
  ,[Custom_Form_Name]
  ,[Display_Copy]
) VALUES (
   537
  ,'Donations'
  ,'Donation'
  ,517
  ,1
  ,297
  ,'Donation_ID'
  ,297
  ,'Donations.Donation_ID'
  ,'Donations'
  ,'Donation_ID'
  ,'Donations.[Donation_Date]
, Donation_Status_ID_Table.[Donation_Status]
, Donations.[Donation_Amount]
, Donations.[Transaction_Code]
, Donations.[Processor_ID]
, Batch_ID_Table.[Batch_ID]
, Batch_ID_Table.[Setup_Date] AS [Batch_Date]
, Batch_ID_Table_Deposit_ID_Table.[Deposit_ID]
, Batch_ID_Table_Deposit_ID_Table.[Deposit_Date]'
  ,'Donations.Donation_Amount'
  ,'Recurring_Gift_ID'
  ,1
  ,NULL
  ,'Recurring_Gift_ID_Table.Donor_ID_Table.Contact_ID'
  ,NULL
  ,NULL
  ,NULL
  ,'Start_Date'
  ,'End_Date'
  ,NULL
  ,1);

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF
GO

-- Grant "Full" access on this page to "Stewardship Donation Processor" role
INSERT INTO [dbo].[dp_Role_Sub_Pages] (
  [Role_ID],
  [Sub_Page_ID],
  [Access_Level]
) VALUES (
  7,
  537,
  3
);
