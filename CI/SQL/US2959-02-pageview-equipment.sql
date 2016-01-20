USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

DECLARE @PageViewId INT = 1105;
DECLARE @PageId INT = 301;
DECLARE @PageViewTitle VARCHAR(50) = 'EquipmentByLocation';
DECLARE @FieldList VARCHAR(1000) = 'Equipment.[Equipment_ID]
    , Equipment.[Equipment_Name]
    , Room_ID_Table.[Room_ID] 
    , Room_ID_Table_Building_ID_Table.[Building_ID]
    , Room_ID_Table_Building_ID_Table_Location_ID_Table.[Location_ID]
    ,Equipment.[Quantity_On_Hand]';
DECLARE @ViewClause VARCHAR(1000) = 'Equipment.[Equipment_ID] IS NOT NULL AND Equipment.[Bookable] = 1';
DECLARE @Description VARCHAR(1000) = 'API View';

DELETE FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = @PageViewId;

INSERT INTO [dbo].[dp_Page_Views]
       ( [Page_View_ID],
         [View_Title],
         [Page_ID],
         [Field_List],
         [View_Clause],
         [Description]
       )
VALUES( @PageViewId, @PageViewTitle, @PageId, @FieldList, @ViewClause, @Description );
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF;
GO
