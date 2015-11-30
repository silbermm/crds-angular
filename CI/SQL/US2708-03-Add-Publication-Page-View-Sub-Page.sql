USE MinistryPlatform
GO

SET IDENTITY_INSERT dbo.dp_Pages ON

INSERT INTO dbo.dp_Pages
	(
		Page_ID,
		Display_Name,
		Singular_Name,
		[Description],
		View_Order, 
		Table_Name,
		Primary_Key,
		Default_Field_List,
		Selected_Record_Expression,
		Display_Copy
	)
	VALUES
	(
		535,
		'Publication Page Views',
		'Publication Page View',
		'Publications and the related Page Views to be used for segmentation for MailChimp',
		3,
		'Publication_Page_Views',
		'Publication_Page_View_ID',
		'Publication_Page_View_ID, Publication_ID_Table.Title AS Publication_Title, Publication_ID_Table.Description AS Publication_Description, Page_View_ID_Table.View_Title AS View_Title, Page_View_ID_Table.Description AS View_Description',
		'Publication_Page_View_ID',
		0
	)

SET IDENTITY_INSERT dbo.dp_Pages OFF


INSERT INTO dbo.dp_Page_Section_Pages 
	(
		Page_ID,
		Page_Section_ID
	)
	VALUES
	(
		535,
		1
	)

SET IDENTITY_INSERT dbo.dp_Sub_Pages ON

INSERT INTO dbo.dp_Sub_Pages
	(
		Sub_Page_ID,
		Display_Name,
		Singular_Name,
		Page_ID,
		View_Order,
		Link_To_Page_ID,
		Link_From_Field_Name,
		Select_To_Page_ID,
		Select_From_Field_Name,
		Primary_Table,
		Primary_Key,
		Default_Field_List,
		Selected_Record_Expression,
		Filter_Key,
		Relation_Type_ID,
		Display_Copy
	)
	VALUES
	(
		543,
		'Page Views',
		'Page View',
		376,
		3,
		535,
		'Publication_Page_View_ID',
		352,
		'Publication_Page_Views.Page_View_ID',
		'Publication_Page_Views',
		'Publication_Page_View_ID',
		'Publication_Page_View_ID, Page_View_ID_Table.Page_View_ID, Page_View_ID_Table.View_Title, Page_View_ID_Table.Description',
		'Publication_Page_View_ID',
		'Publication_ID',
		2,
		1
	)	

SET IDENTITY_INSERT dbo.dp_Sub_Pages OFF
GO



