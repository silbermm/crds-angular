USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

DECLARE @PageViewId INT = 1102;
DECLARE @PageId INT = 292;
DECLARE @PageViewTitle VARCHAR(50) = 'Contact By Participant ID';
DECLARE @FieldList VARCHAR(1000) = 'Participant_Record_Table.[Participant_ID]
                                    , Contacts.[Contact_ID]
                                    , Contacts.[First_Name]
                                    , Contacts.[Last_Name]
                                    , Contacts.[Email_Address]';
DECLARE @ViewClause VARCHAR(1000) = 'Contacts.[Contact_ID] IS NOT NULL';
DECLARE @Description VARCHAR(1000) = 'A view that returns contacts with their participant id';

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
