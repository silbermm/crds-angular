USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

DECLARE @PageViewId INT = 1109;
DECLARE @PageId INT = 302;
DECLARE @PageViewTitle VARCHAR(50) = 'ApiEquipmentReservations';
DECLARE @FieldList VARCHAR(1000) = 'Event_Equipment.[Event_Equipment_ID] 
, Event_ID_Table.[Event_ID] 
, Room_ID_Table.[Room_ID] 
, Equipment_ID_Table.[Equipment_ID] 
, Event_Equipment.[Notes] 
, Event_Equipment.[Cancelled] 
, Event_Equipment.[Quantity_Requested] ';
DECLARE @ViewClause VARCHAR(1000) = 'Event_Equipment.[Event_Equipment_ID] IS NOT NULL';
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
