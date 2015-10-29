USE [MinistryPlatform]
GO

IF (NOT EXISTS (SELECT *
                FROM [dbo].[dp_Sub_Pages]
                WHERE [Sub_Page_ID] = 540))
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
     540
    ,'Household Pledges'
    ,'Household Pledge'
    ,299
    ,21
    ,363
    ,'Pledge_ID'
    ,363
    ,'Pledges.Pledge_ID'
    ,'Pledges'
    ,'Pledge_ID'
    ,'Pledge_Campaign_ID_Table.[Campaign_Name]
, Pledges.[Total_Pledge]
, Pledges.[First_Installment_Date]
, Pledge_Status_ID_Table.[Pledge_Status]
, Donor_ID_Table_Contact_ID_Table.[Display_Name]
, Donor_ID_Table_Contact_ID_Table_Household_ID_Table.[Household_Name]
, Donor_ID_Table_Contact_ID_Table_Household_ID_Table.[Household_ID]'
    ,'Pledge_ID'
    ,''
    ,3
    ,NULL
    ,NULL
    ,NULL
    ,NULL
    ,NULL
    ,NULL
    ,NULL
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
    540,
    3
  );

  SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON;

  INSERT INTO [dbo].[dp_Sub_Page_Views]
           ([Sub_Page_View_ID]
           ,[View_Title]
           ,[Sub_Page_ID]
           ,[View_Clause])
     VALUES
           (113
           ,'Household Pledges'
           ,540
           ,'Pledges.Pledge_ID IN (SELECT * FROM [dbo].[crds_udfGetPledgeIdsForDonor](dp_ParentID))');

  UPDATE [dbo].[dp_Sub_Pages]
  SET [Default_View] = 113
  WHERE Sub_Page_ID = 540;
END

