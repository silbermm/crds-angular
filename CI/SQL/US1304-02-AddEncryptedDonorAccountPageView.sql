USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO

INSERT INTO [dbo].[dp_Page_Views]
  (
     [Page_View_ID]
    ,[View_Title]
    ,[Page_ID]
    ,[Description]
    ,[Field_List]
    ,[View_Clause]
  )
VALUES
  (
     2179
    ,'Donor Lookup By Encrypted Account'
    ,298
    ,'Lookup Donors By Encrypted Account and Routing Number'
    ,'Donor_ID_Table_Contact_ID_Table.[Display_Name] AS [Display_Name]
      , Donor_Accounts.[Encrypted_Account] AS [Encrypted_Account]
      , Donor_ID_Table.[Donor_ID] AS [Donor_ID]
      , Donor_ID_Table_Contact_ID_Table.[Contact_ID] AS [Contact_ID]'
    ,'Donor_Accounts.[Encrypted_Account] IS NOT NULL'
  );

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO
