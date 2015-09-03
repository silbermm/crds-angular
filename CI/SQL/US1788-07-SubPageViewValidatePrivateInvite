USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON
GO

DELETE FROM [dbo].[dp_Sub_Page_Views]
WHERE Sub_Page_View_ID = 101
GO

INSERT INTO [dbo].[dp_Sub_Page_Views]
           ([Sub_Page_View_ID]
           ,[View_Title]
           ,[Sub_Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause]
           ,[Order_By]
           ,[User_ID])
     VALUES
           (101
           ,'ValidatePrivateInvite'
           ,534
           ,'Validate Private Invitations'
           ,'cr_Campaign_Private_Invitation.[Private_Invitation_ID], Pledge_Campaign_ID_Table.[Pledge_Campaign_ID], cr_Campaign_Private_Invitation.[Invitation_GUID], cr_Campaign_Private_Invitation.[_Invitation_Used], cr_Campaign_Private_Invitation.[Email_Address]'
           ,'cr_Campaign_Private_Invitation.[Private_Invitation_ID] IS NOT NULL'
           ,null
           ,null)
GO

SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] OFF
GO
