USE [MinistryPlatform]
GO

SET IDENTITY_INSERT dbo.dp_Page_Views ON 

INSERT INTO dbo.dp_Page_Views 
    (
        Page_View_ID, 
        View_Title, 
        Page_ID, 
        [Description], 
        Field_List, 
        View_Clause, 
        Order_By, 
        [User_ID], 
        User_Group_ID
    )
    VALUES
    (
        2185, 
        'Attributes With Type', 
        277, 
        'Attribute List with Type and Category information', 
		'Attributes.[Attribute_ID], Attributes.[Attribute_Name], Attribute_Type_ID_Table.[Attribute_Type_ID], Attribute_Type_ID_Table.[Attribute_Type], Attribute_Type_ID_Table.[Prevent_Multiple_Selection], Attribute_Category_ID_Table.[Attribute_Category_ID], Attribute_Category_ID_Table.[Attribute_Category]',
        'Attributes.[Attribute_ID] IS NOT NULL', 
        'Attribute_Type_ID_Table.[Attribute_Type_ID], Attributes.[Attribute_Name]', 
        NULL, 
        NULL
    )

SET IDENTITY_INSERT dbo.dp_Page_Views OFF
GO

SET IDENTITY_INSERT dbo.dp_Sub_Page_Views ON 

INSERT INTO dbo.dp_Sub_Page_Views 
    (
        Sub_Page_View_ID, 
        View_Title, 
        Sub_Page_ID, 
        [Description], 
        Field_List, 
        View_Clause, 
        Order_By, 
        [User_ID]
    )
    VALUES
    (
        109, 
        'Current Selected', 
        269, 
        'Currently selected contract attributes with AttributeType', 
        'Contact_Attributes.[Contact_Attribute_ID], Contact_Attributes.[Start_Date], Contact_Attributes.[End_Date], Contact_Attributes.[Notes], Attribute_ID_Table.[Attribute_ID], Attribute_ID_Table_Attribute_Type_ID_Table.[Attribute_Type_ID],  Attribute_ID_Table_Attribute_Type_ID_Table.[Attribute_Type]', 
        'GetDate() BETWEEN Contact_Attributes.Start_Date AND ISNULL(Contact_Attributes.End_Date, GetDate())', 
        NULL,
        NULL
    )

SET IDENTITY_INSERT dbo.dp_Sub_Page_Views OFF
GO

-- Grant "Full" access on "Contact Attributes" Page to "unauthenticatedCreate"
INSERT [dbo].dp_Role_Sub_Pages
	(
		[Role_ID],
		[Sub_Page_ID],
		[Access_Level]
	 )
VALUES
	(
		62,
		269,
		3
	);
GO