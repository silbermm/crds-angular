USE [MinistryPlatform]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Create a placeholder function, so the below ALTER will always work.
-- This allows the same SQL file to be run in any environment, without errors.
-- This is preferable to dropping the function, as permissions will be maintained
-- on an existing function.
IF OBJECT_ID('dbo.crds_udfGetDonationDistributionIdsForUser', 'TF') IS NULL
    EXEC('CREATE FUNCTION dbo.crds_udfGetDonationDistributionIdsForUser(@User_ID INT) RETURNS @DonationDistributionIdTable TABLE (Distribution_ID INT) AS BEGIN; RETURN; END')
GO
-- =============================================
-- Author:      Jim Kriz
-- Create date: 09/24/2015
-- Description: Retrieve a table of all donation distribution IDs for a given
--              user.  This takes into account household (Family) giving, and
--              soft credit donation distributions.
-- =============================================
ALTER FUNCTION [dbo].[crds_udfGetDonationDistributionIdsForUser](@User_ID INT)
RETURNS @DonationDistributionIdTable TABLE (Distribution_ID INT)
AS
BEGIN
	DECLARE @Donor_Statement_Type INT;
	DECLARE @Donor_ID INT;
	DECLARE @Household_ID INT;

	-- Retrieve some important information that we'll need for decisions later.
	-- Namely, we need the statement type for the donor associated to this user,
	-- to determine if we need to look at household donations, then we need the 
	-- Donor ID for Individual gifts, and the household ID for Family gifts.
	SELECT
		  @Donor_Statement_Type = d.Statement_Type_ID
		, @Donor_ID = d.Donor_ID
		, @Household_ID = c.Household_ID
	FROM [dbo].[Donors] d, [dbo].[Contacts] c
	WHERE d.Contact_ID = c.Contact_ID
	AND c.User_Account = @User_ID;

	IF @Donor_Statement_Type = 1
	BEGIN
		-- If the Statement_Type is 1, giving type is Individual, so we'll
		-- just return donations (and soft credit donations) for the individual
		-- donor id.
		--
		-- NOTE: The UNION ALL is here to make the query faster.  The execution
		-- plan was much more complicated when trying to get soft credit and
		-- "normal" donations in the same WHERE clause, so broke it out to get
		-- a pretty substantial speed improvement.
		INSERT @DonationDistributionIdTable
			SELECT e.Donation_Distribution_ID
			FROM [dbo].[Donations] d, [dbo].[Donation_Distributions] e
			WHERE d.Donor_ID = @Donor_ID 
			AND d.Donation_ID = e.Donation_ID
			UNION ALL
			SELECT e.Donation_Distribution_ID
			FROM [dbo].[Donations] d, [dbo].[Donation_Distributions] e
			WHERE e.Soft_Credit_Donor = @Donor_ID
			AND d.Donation_ID = e.Donation_ID
	END
	ELSE
	BEGIN
		-- If the Statement_Type is 2, giving type is Family, so we'll
		-- return donations (and soft credit donations) for all members of
		-- this donor's household who also have giving type of Family.
		--
		-- NOTE: The UNION ALL is here to make the query faster.  The execution
		-- plan was much more complicated when trying to get soft credit and
		-- "normal" donations in the same WHERE clause, so broke it out to get
		-- a pretty substantial speed improvement.
		INSERT @DonationDistributionIdTable
			SELECT b.Donation_Distribution_ID
			FROM Donations a, Donation_Distributions b, Donors d, Contacts c
			WHERE d.Contact_ID = c.Contact_ID
			AND c.Household_ID = @Household_ID
			AND d.Statement_Type_ID = 2
			AND b.Donation_ID = a.Donation_ID
			AND a.Donor_ID = d.Donor_ID
			UNION ALL
			SELECT b.Donation_Distribution_ID
			FROM Donations a, Donation_Distributions b, Donors d, Contacts c
			WHERE d.Contact_ID = c.Contact_ID
			AND c.Household_ID = @Household_ID
			AND d.Statement_Type_ID = 2
			AND b.Donation_ID = a.Donation_ID
			AND b.Soft_Credit_Donor = d.Donor_ID
	END
	RETURN
END
GO