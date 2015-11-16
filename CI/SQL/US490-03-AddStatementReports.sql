USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Reports] WHERE [Report_ID] = 257)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Reports] ON;

	INSERT INTO [dbo].[dp_Reports] (
		  [Report_ID]
		, [Report_Name]
		, [Description]
		, [Report_Path]
		, [Pass_Selected_Records]
		, [Pass_LinkTo_Records]
		, [On_Reports_Tab]
	) VALUES (
		  257
		, 'CRDS - Donor Stmts: 4 Column'
		, 'Create a statement with 4 colums of detail from statement headers for all donors.'
		, '/MPReports/Statement 4 Columns - CRDS'
		, 0
		, 0
		, 0
	);

	SET IDENTITY_INSERT [dbo].[dp_Reports] OFF;
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Report_Pages] WHERE [Report_ID] = 257 AND [Page_ID] = 299)
BEGIN
	INSERT INTO [dbo].[dp_Report_Pages] (
		  Report_ID
		, Page_ID
	) VALUES (
		  257
		, 299
	);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Role_Reports] WHERE [Report_ID] = 257 AND [Domain_ID] = 1 AND [Role_ID] = 7)
BEGIN
	INSERT INTO [dbo].[dp_Role_Reports] (
		  Role_ID
		, Report_ID
		, Domain_ID
	) VALUES (
		  7
		, 257
		, 1
	);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Reports] WHERE [Report_ID] = 261)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Reports] ON;

	INSERT INTO [dbo].[dp_Reports] (
		  [Report_ID]
		, [Report_Name]
		, [Description]
		, [Report_Path]
		, [Pass_Selected_Records]
		, [Pass_LinkTo_Records]
		, [On_Reports_Tab]
	) VALUES (
		  261
		, 'CRDS - Donor Stmts: 4 Column Selected'
		, 'Create a statement with 4 colums of detail from statement headers for selected donors.'
		, '/MPReports/Statement 4 Columns - CRDS'
		, 1
		, 0
		, 0
	);

	SET IDENTITY_INSERT [dbo].[dp_Reports] OFF;
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Report_Pages] WHERE [Report_ID] = 261 AND [Page_ID] = 299)
BEGIN
	INSERT INTO [dbo].[dp_Report_Pages] (
		  Report_ID
		, Page_ID
	) VALUES (
		  261
		, 299
	);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Role_Reports] WHERE [Report_ID] = 261 AND [Domain_ID] = 1 AND [Role_ID] = 7)
BEGIN
	INSERT INTO [dbo].[dp_Role_Reports] (
		  Role_ID
		, Report_ID
		, Domain_ID
	) VALUES (
		  7
		, 261
		, 1
	);
END;