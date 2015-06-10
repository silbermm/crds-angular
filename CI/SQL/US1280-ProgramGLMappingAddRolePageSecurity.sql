USE [MinistryPlatform]
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
           (2,
           504,
           3,
           0,
           0,
           0,
           0,
           0,
           0,
           0,
           0)
GO


