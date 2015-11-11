USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

DECLARE @PageViewId INT = 1101;
DECLARE @PageId INT = 308;
DECLARE @PageViewTitle VARCHAR(50) = 'Events By Parent ID';
DECLARE @FieldList VARCHAR(1000) = 'Events.[Event_ID]
                                  , Events.[Event_Title]
                                  , Primary_Contact_Table.[Contact_ID]
                                  , Parent_Event_ID_Table.[Event_ID] AS [Parent_Event ID]
                                  , Events.[Event_Start_Date]
                                  , Events.[Event_End_Date]
                                  , Primary_Contact_Table.[Email_Address]
                                  , Event_Type_ID_Table.[Event_Type]';
DECLARE @ViewClause VARCHAR(1000) = 'Events.[Event_ID] IS NOT NULL';
DECLARE @Description VARCHAR(1000) = 'A view that lists Events with Parent ID to enable queries by Parent Event ID';

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
