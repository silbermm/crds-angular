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
           ,[Filter_Clause]
					 ,[Display_Copy])
     VALUES
			(507,
           'My Roles',
           'My Role',
           'Displays the security roles of the currently logged in user',
           10,
           'dp_User_Roles',
           'User_Role_ID',
           NULL,
           'Role_ID_Table.Role_ID, Role_ID_Table.Role_Name',
           'Role_ID_Table.Role_Name',
           'dp_User_Roles.User_ID = dp_UserID',
					 0)

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
           (39,
           507,
           0,
           0,
           0,
           0,
           0,
           0,
           0,
           0,
           0)
GO
