USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Reports] WHERE Report_ID = 256)
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
           (256
		   	,'CRDS Detailed Trip Export'
			,NULL
			,'/MPReports/CRDS Trip Export'
			,0
			,0
			,1)

	SET IDENTITY_INSERT [dbo].[dp_Reports] OFF
END

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Report_Pages] WHERE Report_Page_ID = 1605)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Report_Pages] ON

	INSERT INTO [dbo].[dp_Report_Pages]
           ([Report_Page_ID]
		   ,[Report_ID]
           ,[Page_ID])
     VALUES
           (1605
		   ,256
		   ,308)

	SET IDENTITY_INSERT [dbo].[dp_Report_Pages] OFF
END

GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Role_Reports] WHERE [Role_Report_ID] = 858)
BEGIN
SET IDENTITY_INSERT [dbo].[dp_Role_Reports] ON

INSERT INTO [dbo].[dp_Role_Reports]
           ([Role_Report_ID]
		   ,[Role_ID]
           ,[Report_ID]
           ,[Domain_ID])
     VALUES
           (858
		   ,2
           ,256
           ,1)

SET IDENTITY_INSERT [dbo].[dp_Role_Reports] OFF
END
GO

