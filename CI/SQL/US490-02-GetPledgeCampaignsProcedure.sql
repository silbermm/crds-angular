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
	SELECT
		 Pledge_Campaign_ID
		,Campaign_Name AS Display_Name
		,Pledge_Campaign_Type_ID
		,Pledge_Beyond_End_Date
		,Is_Current = CASE WHEN Getdate() BETWEEN [Start_Date] AND COALESCE(End_Date,Getdate()) THEN 'true' ELSE 'false' END
	FROM Pledge_Campaigns p, dp_Domains d
	WHERE
		p.Domain_ID = d.Domain_ID
		and d.Domain_GUID = @DomainID
	ORDER BY Display_Name
END