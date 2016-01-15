USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

DECLARE @PageViewId INT = 1107;
DECLARE @PageId INT = 383;
DECLARE @PageViewTitle VARCHAR(50) = 'LayoutsByRoomId';
DECLARE @FieldList VARCHAR(1000) = 'Room_Layouts.[Room_Layout_ID]
    , Room_Layouts.[Layout_Name]
    , Room_ID_Table.[Room_ID]';
DECLARE @ViewClause VARCHAR(1000) = 'Room_Layouts.[Room_Layout_ID] IS NOT NULL';
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
