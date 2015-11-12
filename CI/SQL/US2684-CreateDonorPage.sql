USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Pages] ON
GO

-- Add a new "My Donors" page for statment types
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
  VALUES (526
  ,'My Donors'
  ,'My Donor'
  ,'My donor record(s). This is used by the Profile page to allow seeing and changing the statment type.'
  ,100
  ,'Donors'
  ,'Donor_ID'
  ,1
  ,'Contact_ID_Table.Display_Name
,Contact_ID_Table.Nickname
,Contact_ID_Table.First_Name
,Contact_ID_Table_Contact_Status_ID_Table.Contact_Status
,Donors.Envelope_No
,Donors.Setup_Date
,Statement_Frequency_ID_Table.Statement_Frequency
,Statement_Method_ID_Table.Statement_Method
,Statement_Type_ID_Table.Statement_Type
,Statement_Method_ID_Table.Statement_Method_ID'
  ,'Contact_ID_Table.Display_Name'
  ,'Contact_ID_Table.User_Account = dp_UserID' -- filter
  ,'Donors.Contact_ID'
  ,0)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO

-- Add this page to the "My Pages" section
INSERT INTO [dbo].[dp_Page_Section_Pages] (
  [Page_ID],
  [Page_Section_ID]
) VALUES (
  526,
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
  526,
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
