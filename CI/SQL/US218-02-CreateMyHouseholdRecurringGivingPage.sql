USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Pages] ON
GO

-- Add a new "My Household Recurring Gifts" page for giving history
UPDATE [dbo].[dp_Pages]
SET [Default_Field_List] = 'Recurring_Gifts.[Recurring_Gift_ID]
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
, Program_ID_Table.[Program_ID]
, Program_ID_Table.[Program_Name]
, Congregation_ID_Table.[Congregation_Name]
, Donor_Account_ID_Table_Account_Type_ID_Table.[Account_Type_ID]
, Donor_Account_ID_Table.[Account_Number]
, Donor_Account_ID_Table.[Institution_Name]
, Recurring_Gifts.[Subscription_ID]
, Donor_Account_ID_Table.[Processor_ID]
, Donor_Account_ID_Table.[Processor_Account_ID]'
where [Page_ID] = 523

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO
