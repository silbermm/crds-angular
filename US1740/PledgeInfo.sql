DECLARE @Contact int;
DECLARE @PledgeCampaignID int;
DECLARE @TripEvent int;

SET @Contact = 768379;
SET @TripEvent = 1599781;

SELECT @PledgeCampaignID = Pledge_Campaign_ID FROM [dbo].[Pledge_Campaigns]
WHERE Event_ID = @TripEvent

SELECT * FROM [dbo].[Donation_Distributions] DD
JOIN [dbo].[Pledges] P ON DD.Pledge_ID = P.Pledge_ID
JOIN [dbo].[Donors] D ON D.Donor_ID = P.Donor_ID
WHERE D.Contact_ID = @Contact AND P.Pledge_Campaign_ID = @PledgeCampaignID AND D.Contact_ID = @Contact