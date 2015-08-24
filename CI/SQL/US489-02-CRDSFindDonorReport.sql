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
           (252
           ,'CRDS Find Donors'
           ,'Crossroads specific report similar to Find Donors reports except it will search for donors using the congregation from the Donation Distribution.'
           ,'/MPReports/Find Donors CRDS'
           ,0
           ,0
           ,1)
GO

SET IDENTITY_INSERT [dbo].[dp_Reports] OFF
GO
