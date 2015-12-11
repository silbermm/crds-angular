USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_TeamSummaryByDay]    Script Date: 12/10/2015 4:45:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_TeamSummaryByDay]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_TeamSummaryByDay] AS' 
END
GO

-- =============================================

ALTER PROCEDURE [dbo].[report_CRDS_TeamSummaryByDay]
	-- Add the parameters for the stored procedure here
	@Day datetime, 
	@GroupID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @endTime TIME = '23:59:00';
	-- declare @dayvar VARCHAR = CONVERT(VARCHAR, @Day, 111);
	declare @endDay datetime = @Day + @endTime;


	SELECT op.Opportunity_Title, c.Nickname, C.Last_Name, e.Event_Start_Date, Convert(varchar(15), op.Shift_Start,100) as Shift_Start, Convert(varchar(15), op.Shift_End,100) as Shift_End, op.Room, sud.Sign_Up_Deadline FROM Responses r
	JOIN Opportunities op ON op.Opportunity_ID = r.Opportunity_ID
	JOIN Participants p on r.Participant_ID = p.Participant_ID
	JOIN Contacts c on p.Contact_ID = c.Contact_ID
	JOIN Events e ON r.Event_ID = e.Event_ID
	JOIN cr_Sign_Up_Deadline sud ON sud.Sign_Up_Deadline_ID = op.Sign_Up_Deadline_ID
	
	WHERE op.Add_to_Group = @GroupID AND r.Response_Result_ID = 1 AND e.Event_Start_Date BETWEEN  @Day AND @endDay
END

GO


