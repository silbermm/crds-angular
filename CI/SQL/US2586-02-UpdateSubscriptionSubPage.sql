USE [MinistryPlatform]
GO

-- Fix issue where the same subpage was inserted with different ID's
DELETE FROM [dbo].[dp_Role_Sub_Pages] WHERE sub_Page_ID = 376
DELETE FROM [dbo].[dp_Sub_Pages] WHERE [Sub_Page_ID] = 376 AND Display_Name = 'Subscriptions'

-- Now use the correct ID 424
IF EXISTS (SELECT * FROM [dbo].[dp_Sub_Pages] WHERE [Sub_Page_ID] = 424)

BEGIN

UPDATE [dbo].[dp_Sub_Pages] 

SET
	[Display_Name] = 'Subscriptions',
    [Singular_Name] = 'Subscription',
	[Page_ID] = 455,
    [View_Order] = 5,
	[Link_To_Page_ID] = 417,
	[Link_From_Field_Name] = 'Contact_Publication_ID',
	[Select_To_Page_ID] = 376,
	[Select_From_Field_Name] = 'dp_Contact_Publications.Publication_ID',
	[Primary_Table] = 'dp_Contact_Publications',
    [Primary_Key] = 'Contact_Publication_ID',
    [Default_Field_List] = 'Publication_ID_Table.Publication_ID
			,Publication_ID_Table.Title AS Publication_Title
			,dp_Contact_Publications.Unsubscribed',
    [Selected_Record_Expression] = 'Publication_ID_Table.Title',
	[Filter_Key] = 'Contact_ID',
	[Relation_Type_ID] = 2,
	[On_Quick_Add] = 0,
	[Contact_ID_Field] = 'dp_Contact_Publications.Contact_ID',
    [Default_View] = 59

WHERE [Sub_Page_ID] = 424

END

ELSE

BEGIN
SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON 

INSERT INTO [dbo].[dp_Sub_Pages]
           ([Sub_Page_ID]
           ,[Display_Name]
           ,[Singular_Name]
		   ,[Page_ID]
           ,[View_Order]
		   ,[Link_To_Page_ID]
		   ,[Link_From_Field_Name]
		   ,[Select_To_Page_ID]
		   ,[Select_From_Field_Name]
		   ,[Primary_Table]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
		   ,[Filter_Key]
		   ,[Relation_Type_ID]
		   ,[On_Quick_Add]
		   ,[Contact_ID_Field]
           ,[Default_View])
     VALUES
           (424,
           'Subscriptions',
           'Subscription',
			455,
			5,
			417,
            'Contact_Publication_ID',
			376,
            'dp_Contact_Publications.Publication_ID',
            'dp_Contact_Publications',           
            'Contact_Publication_ID',
		    'Publication_ID_Table.Publication_ID,
			Publication_ID_Table.Title AS Publication_Title,
			dp_Contact_Publications.Unsubscribed',
			'Publication_ID_Table.Title',
			'Contact_ID',
			2,
			0,
			'dp_Contact_Publications.Contact_ID',
			59)

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF
END
GO