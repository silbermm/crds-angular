USE [MinistryPlatform]
GO

IF (NOT EXISTS (SELECT *
                FROM [dbo].[dp_Sub_Pages]
                WHERE [Sub_Page_ID] = 538))
BEGIN
  SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON;

  INSERT INTO [dbo].[dp_Sub_Pages](
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
     538
    ,'Recurring Gifts'
    ,'Recurring Gift'
    ,299
    ,20
    ,517
    ,'Recurring_Gift_ID'
    ,517
    ,'Recurring_Gifts.Recurring_Gift_ID'
    ,'Recurring_Gifts'
    ,'Recurring_Gift_ID'
    ,'Frequency_ID_Table.Frequency
  ,CASE(Frequency_ID_Table.Frequency_ID)
    WHEN 1
      THEN
        CONCAT(
          ''Every '',
          Day_Of_Week_ID_Table.Day_Of_Week
        )
    ELSE
      CONCAT(
        [dbo].[crds_udfGetOrdinalNumber](Day_Of_Month),
        '' of the month''
      )
    END AS Recurrence
  ,Amount
  ,Start_Date
  ,End_Date
  ,Subscription_ID'
    ,'Recurring_Gift_ID'
    ,'Donor_ID'
    ,3
    ,NULL
    ,'Donor_ID_Table.Contact_ID'
    ,NULL
    ,NULL
    ,NULL
    ,'Start_Date'
    ,'End_Date'
    ,NULL
    ,1
  );

  SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF;

  -- Grant "Full" access on this page to "Stewardship Donation Processor" role
  INSERT INTO [dbo].[dp_Role_Sub_Pages] (
    [Role_ID],
    [Sub_Page_ID],
    [Access_Level]
  ) VALUES (
    7,
    538,
    3
  );
END

