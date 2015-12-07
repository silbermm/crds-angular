USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

DECLARE @PageViewId INT = 1103;
DECLARE @PageId INT = 382;
DECLARE @PageViewTitle VARCHAR(50) = 'Opportunity Responses By Group & Event';
DECLARE @FieldList VARCHAR(1000) = 'Responses.[Response_ID]
							 , Opportunity_ID_Table_Add_to_Group_Table.[Group_ID]
							 , Event_ID_Table.[Event_ID]
							 , Participant_ID_Table.[Participant_ID]
							 , Participant_ID_Table_Contact_ID_Table.[Contact_ID]';
DECLARE @ViewClause VARCHAR(1000) = 'Responses.[Response_ID] IS NOT NULL';
DECLARE @Description VARCHAR(1000) = 'View to search opportunity respones by group and event';

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
