USE MinistryPlatform
GO

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Sort_Order' AND Object_ID = Object_ID(N'dbo.Attributes'))
BEGIN
	ALTER TABLE dbo.Attributes ADD Sort_Order INT CONSTRAINT df_Sort_Order DEFAULT 0 NOT NULL
END
GO

-- Update ethnicity 'some other race' to be at the end of the list
UPDATE dbo.Attributes SET Sort_Order = 1 WHERE Attribute_ID = 3781

-- Update attribute page view
UPDATE dbo.dp_Page_Views 
	SET 
		Field_List = 'Attributes.[Attribute_ID], Attributes.[Attribute_Name], Attribute_Type_ID_Table.[Attribute_Type_ID], Attribute_Type_ID_Table.[Attribute_Type], Attribute_Type_ID_Table.[Prevent_Multiple_Selection], Attribute_Category_ID_Table.[Attribute_Category_ID], Attribute_Category_ID_Table.[Attribute_Category], Attributes.[Sort_Order], Attribute_Category_ID_Table.[Description] AS Attribute_Category_Description'
	WHERE 
		Page_View_ID = 2185

GO


UPDATE dbo.Attributes SET Attribute_Name = 'Hispanic' WHERE Attribute_ID = 3777

IF NOT EXISTS (SELECT 1 FROM dbo.Attributes WHERE Attribute_ID = 3783)
BEGIN
	SET IDENTITY_INSERT dbo.Attributes ON 

	INSERT INTO dbo.Attributes 
		(
			Attribute_ID, 
			Attribute_Name, 
			Attribute_Type_ID, 
			Domain_ID, 
			__ExternalAttributeID, 
			Sort_Order
		)
		VALUES 
		(
			3783, 
			'Latino', 
			20, 
			1, 
			8, 
			0
		)

	SET IDENTITY_INSERT dbo.Attributes OFF 
END
GO