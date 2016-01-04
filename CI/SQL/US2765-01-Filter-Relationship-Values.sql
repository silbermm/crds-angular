USE MinistryPlatform
GO

SET IDENTITY_INSERT dbo.dp_Page_Views ON

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 2202)

BEGIN
INSERT INTO dbo.dp_Page_Views
	(
		Page_View_ID,
		View_Title,
		Page_ID,
		Description,
		Field_List,
		View_Clause,
		Order_By
	)
	VALUES
	(
		2202,
		'Filtered Relationship Types',
		379,
		'Filtered view of relationship types.',
		'Relationships.[Relationship_Name] AS [Relationship Name]
		, Relationships.[Male_Label] AS [Male Label]
		, Relationships.[Female_Label] AS [Female Label]
		, Reciprocal_Relationship_ID_Table.[Relationship_ID] AS [Reciprocal Relationship ID]
		',
		'Relationships.[Relationship_ID] IN (8,7,5,42,41,27,14,36,40,45,29,30,1,6,37,28,43,44)',
		NULL
	)

SET IDENTITY_INSERT dbo.dp_Page_Views OFF
END
GO

BEGIN
UPDATE [dbo].[dp_Pages]

SET Pick_List_View=2202

WHERE Page_ID=379

END



