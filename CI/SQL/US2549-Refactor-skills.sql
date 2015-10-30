USE MinistryPlatform
GO

-- Update attribute page view
UPDATE dbo.dp_Page_Views 
	SET 
		Field_List = 'Attributes.[Attribute_ID], Attributes.[Attribute_Name], Attribute_Type_ID_Table.[Attribute_Type_ID], Attribute_Type_ID_Table.[Attribute_Type], Attribute_Type_ID_Table.[Prevent_Multiple_Selection], Attribute_Category_ID_Table.[Attribute_Category_ID], Attribute_Category_ID_Table.[Attribute_Category], Attributes.[Sort_Order], Attribute_Category_ID_Table.[Description] AS Attribute_Category_Description'
	WHERE 
		Page_View_ID = 2185

-- Update MyContactAttributes sub page
UPDATE dbo.dp_Sub_Pages 
	SET 
		Default_Field_List = 'Attribute_ID_Table.Attribute_ID
,Attribute_ID_Table_Attribute_Type_ID_Table.Attribute_Type
,Attribute_ID_Table.Attribute_Name
,Contact_Attributes.Start_Date
,Contact_Attributes.Notes
,Contact_Attributes.End_Date'
	WHERE
		Sub_Page_Id = 421


-- Create sub page view to limit to only active attributes
SET IDENTITY_INSERT dbo.dp_Sub_Page_Views ON
INSERT INTO dbo.dp_Sub_Page_Views
	(
		Sub_Page_View_ID , 
		View_Title, 
		Sub_Page_ID,
		Field_List,	 
		View_Clause
	)
	VALUES
	(
		117, 
		'Current Attributes', 
		421,
		'Contact_Attributes.[Contact_Attribute_ID], Contact_Attributes.[Start_Date], Contact_Attributes.[End_Date], Contact_Attributes.[Notes], Attribute_ID_Table.[Attribute_ID], Attribute_ID_Table_Attribute_Type_ID_Table.[Attribute_Type_ID],  Attribute_ID_Table_Attribute_Type_ID_Table.[Attribute_Type]',
		'GetDate() BETWEEN Contact_Attributes.Start_Date AND ISNULL(Contact_Attributes.End_Date, GetDate())'
	)

SET IDENTITY_INSERT dbo.dp_Sub_Page_Views OFF

-- Correct display of category
UPDATE Attribute_Categories 
	SET 
		Attribute_Category = 'Construction', 
		Description = '(You get paid to do this)' 
	WHERE Attribute_Category_ID = 7

GO