USE [MinistryPlatform];
GO

/****** Object:  UserDefinedFunction [dbo].[crds_TripDeposit]    Script Date: 10/27/2015 8:26:00 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE FUNCTION [dbo].[crds_TripDeposit](
                @Contact          INT,
                @EventId          INT,
                @PledgeCampaignID INT )
RETURNS @TripDepositTable TABLE( Deposit       VARCHAR(5) NOT NULL,
                                 Payment_Type  VARCHAR(20) NULL,
                                 Donation_Date DATETIME NULL,
                                 Pledge_Status NVARCHAR(25) NULL,
                                 Amount        MONEY NULL )
AS
     BEGIN
     DECLARE @DepositId INT;
     SELECT @DepositId = MIN(DD.Donation_Distribution_ID)
     FROM [dbo].[Donation_Distributions] DD
          JOIN [dbo].[Pledges] P ON DD.Pledge_ID = P.Pledge_ID
          JOIN [dbo].[Donors] D ON D.Donor_ID = P.Donor_ID
     WHERE D.Contact_ID = @Contact
       AND P.Pledge_Campaign_ID = @PledgeCampaignID;
     IF @DepositId IS NOT NULL
         BEGIN
             INSERT INTO @TripDepositTable
                    SELECT 'Yes' Deposit,
                           pt.Payment_Type,
                           d.Donation_Date,
                           ps.Pledge_Status,
                           dd.Amount
                    FROM dbo.Donation_Distributions dd
                         INNER JOIN dbo.Donations d ON dd.donation_Id = d.donation_Id
                         INNER JOIN dbo.payment_types pt ON d.payment_type_id = pt.Payment_Type_ID
                         INNER JOIN dbo.Pledges p ON dd.Pledge_ID = p.Pledge_ID
                         INNER JOIN dbo.Pledge_Statuses ps ON p.Pledge_Status_ID = ps.Pledge_Status_ID
                    WHERE dd.Donation_Distribution_ID = @DepositId;
         END;
     ELSE
         BEGIN
             INSERT INTO @tripdeposittable
                    SELECT 'No' Deposit,
                           NULL Payment_Type,
                           NULL Donation_Date,
                           NULL Pledge_Status,
                           NULL Amount; --from dbo.Donation_Distributions dd
         END;
    RETURN;
    END;
GO
