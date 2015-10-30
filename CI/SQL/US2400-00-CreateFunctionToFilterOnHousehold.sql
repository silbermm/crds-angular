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
IF OBJECT_ID('dbo.crds_udfGetPledgeIdsForDonor', 'TF') IS NULL
    EXEC('CREATE FUNCTION dbo.crds_udfGetPledgeIdsForDonor(@Donor_ID INT) RETURNS @PledgeIdTable TABLE (Pledge_ID INT) AS BEGIN; RETURN; END')
GO
-- =============================================
-- Author:      Dustin Kocher
-- Create date: 10/06/2015
-- Description: Retrieve a table of all pledge IDs for a given
--              user.  This takes into account household (Family) giving.
-- =============================================
ALTER FUNCTION [dbo].[crds_udfGetPledgeIdsForDonor](@Donor_ID INT)
RETURNS @PledgeIdTable TABLE (Pledge_ID INT)
AS
BEGIN
  DECLARE @Donor_Statement_Type INT;
  DECLARE @Household_ID INT;

  -- Retrieve some important information that we'll need for decisions later.
  -- Namely, we need the statement type for the donor associated to this user,
  -- to determine if we need to look at household donations, then we need the 
  -- Donor ID for Individual gifts, and the household ID for Family gifts.
  SELECT
      @Donor_Statement_Type = d.Statement_Type_ID
    , @Household_ID = c.Household_ID
  FROM [dbo].[Donors] d, [dbo].[Contacts] c
  WHERE d.Contact_ID = c.Contact_ID
  AND d.Donor_ID = @Donor_ID;

  IF @Donor_Statement_Type = 1
  BEGIN
    -- If the Statement_Type is 1, giving type is Individual, so we'll
    -- just return Pledge for the individual
    -- donor id.
    INSERT @PledgeIdTable
      SELECT p.Pledge_ID
      FROM [dbo].[Pledges] p
      WHERE p.Donor_ID = @Donor_ID
  END
  ELSE
  BEGIN
    -- If the Statement_Type is 2, giving type is Family, so we'll
    -- return Pledge for all members of
    -- this donor's household who also have giving type of Family.
    INSERT @PledgeIdTable
      SELECT p.Pledge_ID
      FROM [dbo].[Pledges] p
      INNER JOIN Donors d on d.Donor_ID = p.Donor_ID
      INNER JOIN Contacts c on c.Contact_ID = d.Contact_ID
      WHERE d.Statement_Type_ID = 2
      AND c.Household_ID = @Household_ID
  END
  RETURN
END

GO
