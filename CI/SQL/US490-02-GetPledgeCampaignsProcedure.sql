USE [MinistryPlatform]
GO

-- Create a placeholder procedure, so the below ALTER will always work.
-- This allows the same SQL file to be run in any environment, without errors.
-- This is preferable to dropping the procedure, as permissions will be maintained
-- on an existing function.
IF OBJECT_ID('dbo.api_CRDS_MPP_GetPledgeCampaigns', 'p') IS NULL
    EXEC('CREATE PROCEDURE dbo.api_CRDS_MPP_GetPledgeCampaigns AS SELECT 1')
GO

-- =============================================
-- Author:      Kriz, Jim
-- Create date: 11/04/2015
-- Description: Get list of pledge campaigns
-- =============================================
ALTER PROCEDURE [dbo].[api_CRDS_MPP_GetPledgeCampaigns]
	@DomainID varchar(40)
AS
BEGIN
	-- Add a "None" selection to the campaigns.
	-- "None" is added as a valid value in dbo.api_CRDS_MPP_GetPledgeCampaigns as a hack to workaround
	-- the fact that SSRS requires a multi-select dropdown to have a value, but in this case, we want to
	-- allow the user to not choose any campaign.
	SELECT
		 -1 AS Sort_Order
		,-1 AS Pledge_Campaign_ID
		,'None' AS Display_Name
		,-1 AS Pledge_Campaign_Type_ID
		,0 AS Pledge_Beyond_End_Date
		,'true' AS Is_Current
	UNION ALL
	SELECT
		 1 AS Sort_Order
		,Pledge_Campaign_ID
		,Campaign_Name AS Display_Name
		,Pledge_Campaign_Type_ID
		,Pledge_Beyond_End_Date
		,Is_Current = CASE WHEN Getdate() BETWEEN [Start_Date] AND COALESCE(End_Date,Getdate()) THEN 'true' ELSE 'false' END
	FROM Pledge_Campaigns p, dp_Domains d
	WHERE
		p.Domain_ID = d.Domain_ID
		and d.Domain_GUID = @DomainID
		--and d.Domain_ID = 1
	ORDER BY Sort_Order, Display_Name
END