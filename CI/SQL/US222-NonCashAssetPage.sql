USE [MinistryPlatform]
GO

IF (NOT EXISTS (SELECT 1
                FROM [dbo].[dp_Pages]
                WHERE [Page_ID] = 509
                ))
BEGIN
    SET IDENTITY_INSERT [dbo].[dp_Pages] ON;

    INSERT INTO [dbo].[dp_Pages]
               ([Page_ID]
               ,[Display_Name]
               ,[Singular_Name]
               ,[Description]
               ,[View_Order]
               ,[Table_Name]
               ,[Primary_Key]
               ,[Default_Field_List]
               ,[Selected_Record_Expression]
               ,[Display_Copy])
         VALUES
               (509
               ,'Non-Cash Asset Types'
               ,'Non-Cash Asset Type'
               ,'This Crossroads specific table contains all possible values associated with a non-cash donation'
               ,90
               ,'Donation_Non_Cash_Assets'
               ,'Non_Cash_Asset_Type_ID'
               ,'Non_Cash_Asset_Type'
               ,'Non_Cash_Asset_Type'
               ,0);

    SET IDENTITY_INSERT [dbo].[dp_Pages] OFF;
END;

IF (NOT EXISTS (SELECT 1
                FROM [dbo].[dp_Role_Pages]
                WHERE [Page_ID] = 509
                AND [Role_ID] = 2
                ))
BEGIN
  INSERT INTO [dbo].[dp_Role_Pages](
         [Role_ID]
        ,[Page_ID]
        ,[Access_Level]
        ,[Scope_All]
        ,[Approver]
        ,[File_Attacher]
        ,[Data_Importer]
        ,[Data_Exporter]
        ,[Secure_Records]
        ,[Allow_Comments]
        ,[Quick_Add]
    ) VALUES (
         2
        ,509
        ,3
        ,0
        ,0
        ,1
        ,0
        ,1
        ,1
        ,0
        ,0
    );
END;