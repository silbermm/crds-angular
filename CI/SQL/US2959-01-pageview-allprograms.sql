USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

DECLARE @PageViewId INT = 1104;
DECLARE @PageId INT = 375;
DECLARE @PageViewTitle VARCHAR(50) = 'All Programs List';
DECLARE @FieldList VARCHAR(1000) = 'Programs.[Program_ID] 
    ,Programs.[Program_Name]
    ,Program_Type_ID_Table.[Program_Type]
    ,Program_Type_ID_Table.[Program_Type_ID]
    ,Programs.[Online_Sort_Order] 
    ,Programs.[Allow_Recurring_Giving] ';
DECLARE @ViewClause VARCHAR(1000) = 'GetDate() BETWEEN Programs.Start_Date AND ISNULL(Programs.End_Date, GetDate())';
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
