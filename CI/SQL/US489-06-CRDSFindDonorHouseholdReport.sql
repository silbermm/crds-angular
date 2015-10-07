USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Reports] ON

INSERT INTO [dbo].[dp_Reports]
           ([Report_ID]
           ,[Report_Name]
           ,[Description]
           ,[Report_Path]
           ,[Pass_Selected_Records]
           ,[Pass_LinkTo_Records]
           ,[On_Reports_Tab])
     VALUES
           (253
           ,'CRDS Find Donors Household'
           ,'Find Donors by Household who have given to a specific program or programs. Define a minimum amount during a period and even find people who are giving for the first time.'
           ,'/MPReports/Find Donor Households CRDS'
           ,0
           ,0
           ,1)
GO

SET IDENTITY_INSERT [dbo].[dp_Reports] OFF
GO
