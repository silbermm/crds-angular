USE [MinistryPlatform]
GO
/****** Object:  UserDefinedFunction [dbo].[crds_udfGetPledgeIdsForUser]    Script Date: 10/27/2015 12:25:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Sandi Ritter
-- Create date: 10/27/2015
-- Description: Retrieve a table of all recurring gift IDs for a given
--              user.  This takes into account household (Family) giving.
-- =============================================
CREATE FUNCTION [dbo].[crds_udfGetPledgeIdsForUser](@User_ID INT)
RETURNS @PledgeIdTable TABLE (Pledge_ID INT)
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
    -- just return pledges for the individual
    -- donor id.
    INSERT @PledgeIdTable
      SELECT P.Pledge_ID
      FROM [dbo].[Pledges] P
      WHERE P.Donor_ID = @Donor_ID
      AND P.Pledge_Status_ID = 1
  END
  ELSE
  BEGIN
    -- If the Statement_Type is 2, giving type is Family, so we'll
    -- return pledges for all members of
    -- this donor's household who also have giving type of Family.
    INSERT @PledgeIdTable
      SELECT P.Pledge_ID
      FROM [dbo].Pledges P
      INNER JOIN Donors D on D.Donor_ID = P.Donor_ID
      INNER JOIN Contacts C on C.Contact_ID = D.Contact_ID
      WHERE d.Statement_Type_ID = 2
      AND C.Household_ID = @Household_ID
      AND P.Pledge_Status_ID = 1
  END
  RETURN
END
