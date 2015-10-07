USE [MinistryPlatform]
GO

IF EXISTS (SELECT 1 FROM [dbo].[dp_Page_Views] where Page_View_ID = 92233)
BEGIN
	UPDATE [dbo].[dp_Page_Views] 
	SET View_Clause = 'cr_Document_Destinations.[Document_Destination_ID] IS NOT NULL'
	WHERE Page_View_ID = 92233
END
ELSE
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
	INSERT INTO [dbo].[dp_Page_Views]
			   ([Page_View_ID]
			   ,[View_Title]
			   ,[Page_ID]
			   ,[Field_List]
			   ,[View_Clause])
		 VALUES
			   (92233
				,'TripDestinationDocuments'
				,520
				,'cr_Document_Destinations.[Document_Destination_ID], Destination_ID_Table.[Destination_ID], Document_ID_Table.[Document_ID], Document_ID_Table.[Document], Document_ID_Table.[Description]'
				,'cr_Document_Destinations.[Document_Destination_ID] IS NOT NULL')

	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END