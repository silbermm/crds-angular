USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON
GO

INSERT INTO [dbo].[dp_Sub_Pages]
           ([Sub_Page_ID], [Display_Name]
           ,[Singular_Name]
           ,[Page_ID]
           ,[View_Order]
           ,[Primary_Table]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Filter_Key]
           ,[Relation_Type_ID]
           ,[Display_Copy])
     VALUES
           (534, 'Invitations'
           ,'Invitation'
           ,514
           ,3
           ,'cr_Campaign_Private_Invitation'
           ,'Private_Invitation_ID'
           ,'cr_Campaign_Private_Invitation.Email_Address, cr_Campaign_Private_Invitation.Recipient_Name,cr_Campaign_Private_Invitation._Invitation_Used as Used'
           ,'cr_Campaign_Private_Invitation.Recipient_Name'
           ,'Pledge_Campaign_ID'
           ,1
           ,0)
GO

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF
GO

INSERT INTO [dbo].[dp_Role_Sub_Pages]
           ([Role_ID]
           ,[Sub_Page_ID]
           ,[Access_Level])
     VALUES
           (2
           ,534
           ,3)
GO

INSERT INTO [dbo].[dp_Role_Sub_Pages]
           ([Role_ID]
           ,[Sub_Page_ID]
           ,[Access_Level])
     VALUES
           (62
           ,534
           ,0)
GO
