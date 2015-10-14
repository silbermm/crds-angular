USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Pages] ON
GO

-- Add a new "My Household Recurring Gifts" page for giving history
INSERT INTO [dbo].[dp_Pages]([Page_ID]
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
  ,[Display_Copy])
  VALUES (523
  ,'My Household Recurring Gifts'
  ,'My Household Recurring Gift'
  ,'All of a Households Recurring Gifts.  This is used by the Profile page to show Recurring Gifts in CR.net'
  ,100
  ,'Recurring_Gifts'
  ,'Recurring_Gift_ID'
  ,1
  ,'Recurring_Gifts.[Recurring_Gift_ID]
, Donor_ID_Table.[Donor_ID]
, Donor_ID_Table_Contact_ID_Table_User_Account_Table.[User_Email]
, Frequency_ID_Table.[Frequency]
, CASE(Frequency_ID_Table.Frequency_ID)
WHEN 1 THEN
  CONCAT(REPLACE(Day_Of_Week_ID_Table.Day_Of_Week, '' '', ''''), ''s Weekly'')
ELSE
  CONCAT(
    [dbo].[crds_udfGetOrdinalNumber](Day_Of_Month),
    '' Monthly''
  )
END AS Recurrence
, Recurring_Gifts.[Start_Date]
, Recurring_Gifts.[End_Date]
, Recurring_Gifts.[Amount]
, Program_ID_Table.[Program_Name]
, Congregation_ID_Table.[Congregation_Name]
, Donor_Account_ID_Table_Account_Type_ID_Table.[Account_Type_ID]
, Donor_Account_ID_Table.[Account_Number]
, Donor_Account_ID_Table.[Institution_Name]
, Recurring_Gifts.[Subscription_ID]
, Donor_Account_ID_Table.[Processor_ID]
, Donor_Account_ID_Table.[Processor_Account_ID]'
  ,'Program_ID_Table.Program_Name'
  ,'Recurring_Gifts.[Recurring_Gift_ID] IN (SELECT * FROM [dbo].[crds_udfGetRecurringGiftIdsForUser](dp_UserID))'
  ,'Donor_ID_Table_Contact_ID_Table.[Contact_ID]'
  ,0)


SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO

-- Add this page to the "My Pages" section
INSERT INTO [dbo].[dp_Page_Section_Pages] (
  [Page_ID],
  [Page_Section_ID]
) VALUES (
  523,
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
  523,
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

GO
