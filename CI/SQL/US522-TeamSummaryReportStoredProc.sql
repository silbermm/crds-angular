USE [MinistryPlatform];
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
IF NOT EXISTS
             (
              SELECT *
              FROM sys.objects
              WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_TeamSummaryByDay]')
                    AND type IN (N'P', N'PC')
             )
    BEGIN
        EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_TeamSummaryByDay] AS';
    END;
GO

-- =============================================

ALTER PROCEDURE [dbo].[report_CRDS_TeamSummaryByDay]
-- Add the parameters for the stored procedure here
      @Day DATETIME, @GroupID INT, @Opportunities AS VARCHAR(MAX)
AS
     BEGIN
         -- SET NOCOUNT ON added to prevent extra result sets from
         -- interfering with SELECT statements.
         SET NOCOUNT ON;
         DECLARE @endTime TIME= '23:59:00';
         DECLARE @endDay DATETIME= @Day + @endTime;
         SELECT op.Opportunity_Title, op.Opportunity_ID, gr.Role_Title, c.Nickname, C.Last_Name, e.Event_Start_Date
	    , CONVERT(VARCHAR(15), op.Shift_Start, 100) AS Shift_Start
	    , CONVERT(VARCHAR(15), op.Shift_End, 100) AS Shift_End
	    , op.Room, sud.Sign_Up_Deadline
         FROM Responses r
              JOIN Opportunities op ON op.Opportunity_ID = r.Opportunity_ID
                                       AND op.Opportunity_ID IN (SELECT Item FROM dbo.dp_Split(@Opportunities, ','))
              JOIN Group_Roles gr ON op.Group_Role_ID = gr.Group_Role_ID
              JOIN Participants p ON r.Participant_ID = p.Participant_ID
              JOIN Contacts c ON p.Contact_ID = c.Contact_ID
              JOIN Events e ON r.Event_ID = e.Event_ID
              JOIN cr_Sign_Up_Deadline sud ON sud.Sign_Up_Deadline_ID = op.Sign_Up_Deadline_ID
         WHERE op.Add_to_Group = @GroupID
               AND r.Response_Result_ID = 1
               AND e.Event_Start_Date BETWEEN @Day AND @endDay;
     END;
GO