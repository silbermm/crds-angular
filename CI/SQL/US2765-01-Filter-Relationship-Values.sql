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
		, Relationships.[Relationship_ID] AS [Relationship ID]
		, Relationships.[Description] AS [Description]
		, Relationships.[Male_Label] AS [Male Label]
		, Relationships.[Female_Label] AS [Female Label]
		, Reciprocal_Relationship_ID_Table.[Relationship_ID] AS [Relationship ID]
		, Reciprocal_Relationship_ID_Table.[Relationship_Name] AS [Relationship Name]
		, Reciprocal_Relationship_ID_Table.[Description] AS [Description]
		, Reciprocal_Relationship_ID_Table.[Male_Label] AS [Male Label]
		, Reciprocal_Relationship_ID_Table.[Female_Label] AS [Female Label]
		',
		'Relationships.[Relationship_ID] = 8 OR 
		Relationships.[Relationship_ID] = 7 OR
		Relationships.[Relationship_ID] = 5 OR 
		Relationships.[Relationship_ID] = 42 OR
		Relationships.[Relationship_ID] = 41 OR 
		Relationships.[Relationship_ID] = 27 OR
		Relationships.[Relationship_ID] = 14 OR 
		Relationships.[Relationship_ID] = 36 OR
		Relationships.[Relationship_ID] = 40 OR
		 Relationships.[Relationship_ID] = 45 OR
		Relationships.[Relationship_ID] = 29 OR
		 Relationships.[Relationship_ID] = 30 OR
		Relationships.[Relationship_ID] = 1 OR
		 Relationships.[Relationship_ID] = 6 OR
		Relationships.[Relationship_ID] = 37 OR 
		Relationships.[Relationship_ID] = 28 OR
		Relationships.[Relationship_ID] = 43 OR
		 Relationships.[Relationship_ID] = 44',
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



