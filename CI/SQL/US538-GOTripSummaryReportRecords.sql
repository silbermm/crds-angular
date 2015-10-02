USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Reports] WHERE Report_ID = 255)
BEGIN
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
           (255
		   	,'CRDS GO Trip Summary Report'
			,NULL
			,'/MPReports/Go Trip Summary-CRDS'
			,0
			,0
			,1)

	SET IDENTITY_INSERT [dbo].[dp_Reports] OFF
END

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Report_Pages] WHERE Report_Page_ID = 1604)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Report_Pages] ON

	INSERT INTO [dbo].[dp_Report_Pages]
           ([Report_Page_ID]
		   ,[Report_ID]
           ,[Page_ID])
     VALUES
           (1604
		   ,255
		   ,308)

	SET IDENTITY_INSERT [dbo].[dp_Report_Pages] OFF
END

GO