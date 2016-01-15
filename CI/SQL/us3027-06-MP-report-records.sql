USE [MinistryPlatform];
GO
DECLARE @ReportId INT = 264;
DECLARE @ReportPageId INT = 1911;
DECLARE @PageId INT = 308;
DECLARE @RoleReportId INT = 1471;
DECLARE @ReportName VARCHAR(50) = 'Event Setup';
DECLARE @ReportDesc VARCHAR(100) = 'View a detailed list of all rooms, equipment, and services needed for an event.';
DECLARE @ReportPath VARCHAR(100) = '/MPReports/crossroads/CRDS Event Setup';
IF NOT EXISTS( SELECT *
               FROM [dbo].[dp_Reports]
               WHERE Report_ID = @ReportId )
    BEGIN
        SET IDENTITY_INSERT [dbo].[dp_Reports] ON;
        INSERT INTO [dbo].[dp_Reports]
               ( [Report_ID],
                 [Report_Name],
                 [Description],
                 [Report_Path],
                 [Pass_Selected_Records],
                 [Pass_LinkTo_Records],
                 [On_Reports_Tab]
               )
        VALUES( @ReportId, @ReportName, @ReportDesc, @ReportPath, 0, 0, 1 );
        SET IDENTITY_INSERT [dbo].[dp_Reports] OFF;
    END;
ELSE
    BEGIN
        UPDATE [dbo].[dp_Reports]
          SET [Report_Name] = @ReportName,
              [Description] = @ReportDesc,
              [Report_Path] = @ReportPath
        WHERE Report_ID = @ReportId;
    END;
IF NOT EXISTS( SELECT *
               FROM [dbo].[dp_Report_Pages]
               WHERE Report_Page_ID = @ReportPageId )
    BEGIN
        SET IDENTITY_INSERT [dbo].[dp_Report_Pages] ON;
        INSERT INTO [dbo].[dp_Report_Pages]
               ( [Report_Page_ID],
                 [Report_ID],
                 [Page_ID]
               )
        VALUES( @ReportPageId, @ReportId, @PageId );
        SET IDENTITY_INSERT [dbo].[dp_Report_Pages] OFF;
    END;
IF NOT EXISTS( SELECT *
               FROM [dbo].[dp_Role_Reports]
               WHERE [Role_Report_ID] = @RoleReportId )
    BEGIN
        SET IDENTITY_INSERT [dbo].[dp_Role_Reports] ON;
        INSERT INTO [dbo].[dp_Role_Reports]
               ( [Role_Report_ID],
                 [Role_ID],
                 [Report_ID],
                 [Domain_ID]
               )
        VALUES( @RoleReportId, 2, @ReportId, 1 );
        SET IDENTITY_INSERT [dbo].[dp_Role_Reports] OFF;
    END;
GO