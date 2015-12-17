USE [MinistryPlatform]
GO

-- Create a placeholder function, so the below ALTER will always work.
-- This allows the same SQL file to be run in any environment, without errors.
-- This is preferable to dropping the function, as permissions will be maintained
-- on an existing function.
IF OBJECT_ID('dbo.crds_udfGetPledgeIdsForUser', 'TF') IS NULL
    EXEC('CREATE FUNCTION dbo.crds_udfGetPledgeIdsForUser(@User_ID INT) RETURNS @PledgeIdTable TABLE (Pledge_ID INT) AS BEGIN; RETURN; END')
GO

/****** Object:  UserDefinedFunction [dbo].[crds_udfGetPledgeIdsForUser]    Script Date: 10/27/2015 12:25:00 PM ******/
-- =============================================
-- Author:      Sandi Ritter
-- Create date: 10/27/2015
-- Description: Retrieve a table of all recurring gift IDs for a given
--              user.  This takes into account household (Family) giving.
-- =============================================
ALTER FUNCTION [dbo].[crds_udfGetPledgeIdsForUser](@User_ID INT)
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
  END
  RETURN
END
GO

-- Create a placeholder function, so the below ALTER will always work.
-- This allows the same SQL file to be run in any environment, without errors.
-- This is preferable to dropping the function, as permissions will be maintained
-- on an existing function.
IF OBJECT_ID('dbo.crds_udfSumDonationByPledgeId', 'FN') IS NULL
    EXEC('CREATE FUNCTION dbo.crds_udfSumDonationByPledgeId(@Pledge_ID INT) RETURNS MONEY AS BEGIN; RETURN 0; END')
GO

-- =========================================================================
-- Author:		Sandi Ritter
-- Create date: 10/28/2015
-- Description:	This function will sum the donation amounts by pledge ID
-- =========================================================================
ALTER FUNCTION [dbo].[crds_udfSumDonationByPledgeId]
(
	@Pledge_ID INT
)
RETURNS MONEY
AS
BEGIN

	DECLARE @Donation_Amount MONEY;

	SELECT @Donation_Amount = SUM(Amount) FROM dbo.Donation_Distributions D
		WHERE  D.Pledge_ID = @Pledge_ID

	RETURN @Donation_Amount

END
GO


IF NOT EXISTS(SELECT 1 FROM [dbo].[dp_Pages] WHERE [Page_ID] = 525)
BEGIN
    SET IDENTITY_INSERT [dbo].[dp_Pages] ON;
    
    INSERT INTO [dbo].[dp_Pages]
               ([Page_ID]
               ,[Display_Name]
               ,[Singular_Name]
               ,[Description]
               ,[View_Order]
               ,[Table_Name]
               ,[Primary_Key]
               ,[Display_Search]
               ,[Default_Field_List]
               ,[Selected_Record_Expression]
               ,[Filter_Clause]
               ,[Contact_ID_Field]
               ,[Display_Copy])
         VALUES
               (525
                   ,'My Household Pledges'
               ,'My Household Pledge'
               ,'All of a Households Pledges.  This is used by the Profile page to show Pledges in CR.net'
               ,100
               ,'Pledges'
               ,'Pledge_ID'
               ,1
               ,'Pledge_ID
                  ,Donor_ID_Table.[Donor_ID]
                  ,Pledges.Pledge_Campaign_ID
                  ,Donor_ID_Table_Contact_ID_Table.Display_Name
                  ,Pledge_Status_ID_Table.Pledge_Status
                  ,Total_Pledge
                  ,Pledge_Campaign_ID_Table.Campaign_Goal
                  ,Pledge_Campaign_ID_Table.Start_Date
                  ,Pledge_Campaign_ID_Table.End_Date
                  ,Pledge_Campaign_ID_Table.Campaign_Name
                  ,[dbo].[crds_udfSumDonationByPledgeId](Pledge_ID) AS Donation_Total'
               ,'Pledge_ID'
               ,'Pledges.[Pledge_ID] IN (SELECT * FROM [dbo].[crds_udfGetPledgeIdsForUser](dp_UserID))'
               ,'Donor_ID_Table_Contact_ID_Table.[Contact_ID]'
               ,0);
    
    SET IDENTITY_INSERT [dbo].[dp_Pages] OFF;
    
    -- Grant "Full" access on this page to "All Platform Users" role
    INSERT INTO [dbo].[dp_Role_Pages] (
      [Role_ID],
      [Page_ID],
      [Access_Level],
      [Scope_All],
      [Approver],
      [File_Attacher],
      [Data_Importer],
      [Data_Exporter],
      [Secure_Records],
      [Allow_Comments],
      [Quick_Add]
    ) VALUES (
      39,
      525,
      3,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      0
    );    
END;