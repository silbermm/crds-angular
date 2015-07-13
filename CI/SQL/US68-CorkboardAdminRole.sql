USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Roles] ON
GO

INSERT INTO [dbo].[dp_Roles]
           ([Role_ID]
					 ,[Role_Name]
           ,[Description]
           ,[Domain_ID]
           ,[Mass_Email_Quota]
           ,[_AdminRole])
     VALUES
           (67
					 ,'CorkboardAdmin'
           ,'Ability to remove posts rather they are yours or not.'
           ,1
           ,NULL
           ,0)
GO

SET IDENTITY_INSERT [dbo].[dp_Roles] OFF
GO
