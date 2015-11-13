USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] ON
GO

INSERT INTO [dbo].[dp_Pages]
			([Page_ID]
           ,[Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Display_Search]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Display_Copy])
     VALUES
			(515,
           'Invitations',
           'Invitation',
           'Page for displaying private invitations for a Trip Pledge Campaign.',
           100,
           'cr_Campaign_Private_Invitation',
           'Private_Invitation_ID',
           1,
           'cr_Campaign_Private_Invitation.Email_Address, cr_Campaign_Private_Invitation.Recipient_Name, cr_Campaign_Private_Invitation._Invitation_Used as Used',
           'cr_Campaign_Private_Invitation.Recipient_Name',
           1)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO

INSERT INTO [dbo].[dp_Role_Pages]
           ([Role_ID]
           ,[Page_ID]
           ,[Access_Level]
           ,[Scope_All]
           ,[Approver]
           ,[File_Attacher]
           ,[Data_Importer]
           ,[Data_Exporter]
           ,[Secure_Records]
           ,[Allow_Comments]
           ,[Quick_Add])
     VALUES
           (2,515,3,0,0,0,0,0,0,0,1)
GO
