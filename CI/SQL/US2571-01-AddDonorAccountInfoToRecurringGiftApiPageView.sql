use [MinistryPlatform]
GO

IF (NOT EXISTS (SELECT *
                FROM [dbo].[dp_Page_Views]
                WHERE [Page_View_ID] = 2184))
BEGIN
    SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;

    INSERT INTO [dbo].[dp_Page_Views] (
         [Page_View_ID]
        ,[View_Title]
        ,[Page_ID]
        ,[Description]
        ,[Field_List]
        ,[View_Clause]
        ,[Order_By]
        ,[User_ID]
        ,[User_Group_ID]
    ) VALUES (
         2184
        ,'My Household Recurring Gifts API'
        ,523
        ,'My Household Recurring Gifts view for API DonationService'
        ,'Recurring_Gifts.[Recurring_Gift_ID]
          , Donor_ID_Table.[Donor_ID]
          , Frequency_ID_Table.[Frequency_ID]
          , Day_Of_Week_ID_Table.[Day_Of_Week_ID]
          , Recurring_Gifts.[Day_Of_Month]
          , Recurring_Gifts.[Start_Date]
          , Recurring_Gifts.[Amount]
          , Program_ID_Table.[Program_ID]
          , Congregation_ID_Table.[Congregation_ID]
          , Donor_Account_ID_Table.[Donor_Account_ID]
          , Donor_Account_ID_Table_Account_Type_ID_Table.[Account_Type_ID]
          , Recurring_Gifts.[Subscription_ID]
          , Donor_Account_ID_Table.[Processor_Account_ID]
          , Donor_Account_ID_Table.[Processor_ID]'
        ,'1 = 1'
        ,NULL
        ,NULL
        ,NULL
    );

    SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF;
END
ELSE
BEGIN
    UPDATE [dbo].[dp_Page_Views]
    SET [Field_List] = CONCAT([Field_List], '
      , Donor_Account_ID_Table.[Processor_Account_ID]
      , Donor_Account_ID_Table.[Processor_ID]')
    WHERE [Page_View_ID] = 2184;
END