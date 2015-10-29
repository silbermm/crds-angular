USE MinistryPlatform
GO

-- Update attribute page view
UPDATE dbo.dp_Page_Views 
	SET 
		Field_List = 'Attributes.[Attribute_ID], Attributes.[Attribute_Name], Attribute_Type_ID_Table.[Attribute_Type_ID], Attribute_Type_ID_Table.[Attribute_Type], Attribute_Type_ID_Table.[Prevent_Multiple_Selection], Attribute_Category_ID_Table.[Attribute_Category_ID], Attribute_Category_ID_Table.[Attribute_Category], Attributes.[Sort_Order], Attribute_Category_ID_Table.[Description] AS Attribute_Category_Description'
	WHERE 
		Page_View_ID = 2185

UPDATE Attribute_Categories 
	SET 
		Attribute_Category = 'Construction', 
		Description = '(You get paid to do this)' 
	WHERE Attribute_Category_ID = 7

GO