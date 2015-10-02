SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Count_Trip_Participants]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Count_Trip_Participants] AS' 
END
GO

ALTER PROCEDURE dbo.report_CRDS_Count_Trip_Participants 
	@Event_ID int,
	@Pledge_Status_ID int
AS
BEGIN
	DECLARE @Participant_Count int
	DECLARE @General_Fund_Total int

	SET @Participant_Count = (SELECT COUNT(*) AS Participants FROM [dbo].[Pledges] P
	JOIN [dbo].[Pledge_Campaigns] PC ON PC.[Pledge_Campaign_ID] = P.[Pledge_Campaign_ID]
	JOIN [dbo].[Events] E ON E.[Event_ID] = PC.[Event_ID] AND E.[Event_ID] = @Event_ID
	WHERE P.[Pledge_Status_ID] = @Pledge_Status_ID AND (P.[Trip_General_Fund] = 0 OR P.[Trip_General_Fund] is null))

	SET @General_Fund_Total = (SELECT SUM(DD.Amount) FROM [dbo].[Donation_Distributions] DD
	JOIN [dbo].[Pledges] P ON P.Pledge_ID = DD.Pledge_ID AND P.[Pledge_Status_ID] = @Pledge_Status_ID AND (P.[Trip_General_Fund] = 1)
	JOIN [dbo].[Pledge_Campaigns] PC ON PC.[Pledge_Campaign_ID] = P.[Pledge_Campaign_ID]
	JOIN [dbo].[Events] E ON E.[Event_ID] = PC.[Event_ID] AND E.[Event_ID] = @Event_ID)

	SELECT @Participant_Count AS Participants, @General_Fund_Total AS General_Fund_Total
END
GO
