USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

DECLARE @PageViewId INT = 1106;
DECLARE @PageId INT = 386;
DECLARE @PageViewTitle VARCHAR(50) = 'RoomsByLocationId';
DECLARE @FieldList VARCHAR(1000) = 'Rooms.[Room_ID]
    , Rooms.[Room_Name]
    , Rooms.[Room_Number]
    , Building_ID_Table.[Building_ID]
    , Building_ID_Table_Location_ID_Table.[Location_ID]
    , Rooms.[Description]
    , Rooms.[Theater_Capacity]
    , Rooms.[Banquet_Capacity]';
DECLARE @ViewClause VARCHAR(1000) = 'Rooms.[Room_ID] IS NOT NULL AND Rooms.[Bookable] = 1';
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
