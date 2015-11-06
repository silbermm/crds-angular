USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

DECLARE @PageViewId INT = 1100;
DECLARE @PageId INT = 305;
DECLARE @PageViewTitle VARCHAR(50) = 'Email Process: Child Care';
DECLARE @FieldList VARCHAR(1000) = '(DATEDIFF(day, GETDATE(), Event_ID_Table.[Event_Start_Date])) AS [DaysUntilEvent]
                                  , Event_Participants.[Event_Participant_ID]
                                  , Event_ID_Table.[Event_ID]
                                  , Event_ID_Table.[Event_Title]
                                  , Event_ID_Table.[Event_Start_Date]
                                  , Group_ID_Table.[Group_ID]
                                  , Group_ID_Table.[Group_Name]
                                  , Group_Participant_ID_Table.[Group_Participant_ID]
                                  , Group_Participant_ID_Table.[Child_Care_Requested]
						                      , Participant_ID_Table_Contact_ID_Table.[Email_Address]
                                  , Participant_ID_Table_Contact_ID_Table.[Contact_ID]
                                  , Participant_ID_Table.[Participant_ID]';
DECLARE @ViewClause VARCHAR(1000) = 'Event_Participants.[Event_Participant_ID] IS NOT NULL AND Group_Participant_ID_Table.[Child_Care_Requested] = 1 AND Event_ID_Table.[Event_Start_Date] > GETDATE()';
DECLARE @Description VARCHAR(1000) = 'Childcare email process uses this view to determine if an event participant should receive an email regarding childcare for an upcoming event.';

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
