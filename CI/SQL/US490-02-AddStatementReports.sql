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

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Reports] WHERE [Report_ID] = 258)
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
		  258
		, 'CRDS - Donor Stmts: 4 Column Selected'
		, 'Create a statement with 4 colums of detail from statement headers for selected donors.'
		, '/MPReports/Statement 4 Columns - CRDS'
		, 1
		, 0
		, 0
	);

	SET IDENTITY_INSERT [dbo].[dp_Reports] OFF;
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Report_Pages] WHERE [Report_ID] = 258 AND [Page_ID] = 299)
BEGIN
	INSERT INTO [dbo].[dp_Report_Pages] (
		  Report_ID
		, Page_ID
	) VALUES (
		  258
		, 299
	);
END;