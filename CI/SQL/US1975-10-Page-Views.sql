USE [MinistryPlatform]
GO
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
               ,NULL
               ,'cr_Document_Destinations.[Document_Destination_ID], Destination_ID_Table.[Destination_ID], Document_ID_Table.[Document_ID], Document_ID_Table.[Document], Document_ID_Table.[Description]'
               ,'cr_Document_Destinations.[Document_Destination_ID] IS NOT NULL')

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO
