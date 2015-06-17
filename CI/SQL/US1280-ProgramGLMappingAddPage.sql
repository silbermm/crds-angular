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
			(504,
           'GL Account Mapping',
           'GL Account Mapping',
           'This Crossroads custom table is use to map GL accounts to Programs',
           40,
           'GL_Account_Mapping',
           'GL_Account_Mapping_ID',
           1,
           'Program_ID_Table.Program_Name, Congregation_ID_Table.Congregation_Name, GL_Account_Mapping.GL_Account',
           'GL_Account_Mapping_ID',         
           0)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO


