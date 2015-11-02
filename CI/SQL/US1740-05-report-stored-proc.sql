USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[report_CRDS_Trip_Export]

      @EventID INT
AS
    BEGIN

        SET NOCOUNT ON;

        DECLARE @MissionTripEventType INT = 6;
	      DECLARE @PledgeDiscontinued INT = 3;

        -- MAPPING VALUES
	   DECLARE @ValidPassport INT = 1464;
        DECLARE @TShirtSize INT = 21;
        DECLARE @ScrubSizeTop INT = 22;
        DECLARE @ScrubSizeBottom INT = 23;
        DECLARE @DietaryRestrictions INT = 65;
        DECLARE @Allergies INT = 67;
        DECLARE @SpritualLife INT = 60;
        DECLARE @PreviousTripExperience INT = 62;
        DECLARE @Profession INT = 61;
        DECLARE @FrequentFlyerDelta INT = 3958;
        DECLARE @FrequentFlyerSouthAfrica INT = 3959;
        DECLARE @FrequentFlyerUnited INT = 3960;
        DECLARE @FrequentFlyerUsAir INT = 3980;
        DECLARE @InternationalTravelExperience INT = 66;
        DECLARE @ExperienceServingAbroad INT = 68;
        DECLARE @AbuseVictim INT = 69;
        DECLARE @Why INT = 1434;
        DECLARE @EmergencyContactFirstName INT = 1439;
        DECLARE @EmergencyContactLastName INT = 1440;
        DECLARE @EmergencyContactPrimaryPhone INT = 1442;
        DECLARE @EmergencyContactSecondaryPhone INT = 1443;
        DECLARE @EmergencyContactEmailAddress INT = 1441;
        DECLARE @LotteryPreference INT = 1444;
        DECLARE @CommonName INT = 1445;
        DECLARE @RequestedRoommate1 INT = 1446;
        DECLARE @RequestedRoommate2 INT = 1447;
        DECLARE @SupportPersonEmail INT = 1448;
        DECLARE @GoGroupLeaderInterest INT = 1449;
        DECLARE @TripGuardianFirstName INT = 1426;
        DECLARE @TripGuardianLastName INT = 1427;
        DECLARE @HowDidYouHearAboutTrip INT = 1433;
        DECLARE @MedicalConditions INT = 1432;
        DECLARE @WorkTeamPreference1 INT = 1423;
        DECLARE @WorkTeamPreference2 INT = 1425;
        DECLARE @WorkTeamExperience INT = 1424;
        DECLARE @StewardsOfChildren INT = 9;
        DECLARE @Visa INT = 11;
        DECLARE @Diploma INT = 1;
        DECLARE @MedicalLicense INT = 2;
        DECLARE @LetterVerification INT = 3;
        DECLARE @GoodFaithLetter INT = 4;
        DECLARE @GoodStandingLetter INT = 5;
        DECLARE @Form10 INT = 6;
        DECLARE @MedicalApplication INT = 7;
        DECLARE @OrchardAfricaWaiver INT = 8;
        DECLARE @CopyOfPassport INT = 10;
        DECLARE @IPromise INT = 12;
        DECLARE @Waiver INT = 13;
        DECLARE @ProofOfGuardianship INT = 16;
        DECLARE @CountryDocumentation INT = 17;
        SELECT c.First_Name,
               c.Middle_Name,
               c.Last_Name,
               c.Maiden_Name,
               c.Nickname,
               c.Email_Address,
               c.Date_of_Birth,
               ms.Marital_Status,
               g.Gender,
               c.Employer_Name,
               p.Participant_Start_Date AS First_Attendance_Ever,
               c.Mobile_Phone,
               a.Address_Line_1,
               a.Address_Line_2,
               a.City,
               a.[State/Region],
               a.Postal_Code,
               a.County,
               a.Foreign_Country,
               h.Home_Phone,
               cn.Congregation_Name,
			(
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @ValidPassport
                   AND r.Form_Response_ID = fr.Form_Response_ID ) ValidPassport,
			c.Passport_Firstname,
			c.Passport_Middlename,
			c.Passport_Lastname,
			c.Passport_Number,
			c.Passport_Country,
			c.Passport_Expiration,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @TripGuardianFirstName
                   AND r.Form_Response_ID = fr.Form_Response_ID ) TripGuardianFirstName,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @TripGuardianLastName
                   AND r.Form_Response_ID = fr.Form_Response_ID ) TripGuardianLastName,
               (
                 SELECT Attribute_Name
                 FROM vw_crds_Contact_Attributes
                 WHERE Contact_ID = c.Contact_ID
                   AND Attribute_Type_ID = @TShirtSize ) AS TShirtSize,
               (
                 SELECT Attribute_Name
                 FROM vw_crds_Contact_Attributes
                 WHERE Contact_ID = c.Contact_ID
                   AND Attribute_Type_ID = @ScrubSizeTop ) AS ScrubSizeTop,
               (
                 SELECT Attribute_Name
                 FROM vw_crds_Contact_Attributes
                 WHERE Contact_ID = c.Contact_ID
                   AND Attribute_Type_ID = @ScrubSizeBottom ) AS ScrubSizeBottom,
               (
                 SELECT STUFF((
                          SELECT '|' + attribute_name
                          FROM dbo.vw_crds_Contact_Attributes AS ca
                          WHERE ca.contact_id = c.Contact_ID
                            AND ca.Attribute_Type_ID = @DietaryRestrictions
                          FOR XML PATH( '' ), TYPE ).value( '.', 'nvarchar(max)' ), 1, 1, '')) AS Dietary_Restrictions,
               (
                 SELECT Notes
                 FROM vw_crds_Contact_Attributes
                 WHERE Contact_ID = c.Contact_ID
                   AND Attribute_Type_ID = @Allergies ) AS Allergies,
               (
                 SELECT STUFF((
                          SELECT '|' + attribute_name
                          FROM dbo.vw_crds_Contact_Attributes AS ca
                          WHERE ca.contact_id = c.Contact_ID
                            AND ca.Attribute_Type_ID = @SpritualLife
                          FOR XML PATH( '' ), TYPE ).value( '.', 'nvarchar(max)' ), 1, 1, '')) AS Spritual_Life,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @Why
                   AND r.Form_Response_ID = fr.Form_Response_ID ) WhyGoOnTrip,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @EmergencyContactFirstName
                   AND r.Form_Response_ID = fr.Form_Response_ID ) EmergencyContactFirstName,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @EmergencyContactLastName
                   AND r.Form_Response_ID = fr.Form_Response_ID ) EmergencyContactLastName,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @EmergencyContactPrimaryPhone
                   AND r.Form_Response_ID = fr.Form_Response_ID ) EmergencyContactPrimaryPhone,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @EmergencyContactSecondaryPhone
                   AND r.Form_Response_ID = fr.Form_Response_ID ) EmergencyContactSecondaryPhone,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @EmergencyContactEmailAddress
                   AND r.Form_Response_ID = fr.Form_Response_ID ) EmergencyContactEmailAddress,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @LotteryPreference
                   AND r.Form_Response_ID = fr.Form_Response_ID ) LotteryPreference,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @CommonName
                   AND r.Form_Response_ID = fr.Form_Response_ID ) CommonName,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @RequestedRoommate1
                   AND r.Form_Response_ID = fr.Form_Response_ID ) RequestedRoommate1,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @RequestedRoommate2
                   AND r.Form_Response_ID = fr.Form_Response_ID ) RequestedRoommate2,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @SupportPersonEmail
                   AND r.Form_Response_ID = fr.Form_Response_ID ) SupportPersonEmail,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @GoGroupLeaderInterest
                   AND r.Form_Response_ID = fr.Form_Response_ID ) GoGroupLeaderInterest,
			 sponsoredChild.SponsoredChild,
			 sponsoredChild.SponsoredChildFirstName,
			 sponsoredChild.SponsoredChildLastName,
			 sponsoredChild.SponsoredChildIdNumber,
               (
                 SELECT Notes
                 FROM vw_crds_Contact_Attributes
                 WHERE Contact_ID = c.Contact_ID
                   AND Attribute_Type_ID = @PreviousTripExperience ) AS Previous_Trip_Experience,
               (
                 SELECT STUFF((
                          SELECT '|' + attribute_name
                          FROM dbo.vw_crds_Contact_Attributes AS ca
                          WHERE ca.contact_id = c.Contact_ID
                            AND ca.Attribute_Type_ID = @Profession
                          FOR XML PATH( '' ), TYPE ).value( '.', 'nvarchar(max)' ), 1, 1, '')) AS Profession,
               (
                 SELECT Notes
                 FROM vw_crds_Contact_Attributes
                 WHERE Contact_ID = c.Contact_ID
                   AND Attribute_ID = @FrequentFlyerDelta ) AS FF_Delta,
               (
                 SELECT Notes
                 FROM vw_crds_Contact_Attributes
                 WHERE Contact_ID = c.Contact_ID
                   AND Attribute_ID = @FrequentFlyerSouthAfrica ) AS FF_South_African_Airlines,
               (
                 SELECT Notes
                 FROM vw_crds_Contact_Attributes
                 WHERE Contact_ID = c.Contact_ID
                   AND Attribute_ID = @FrequentFlyerUnited ) AS FF_United,
               (
                 SELECT Notes
                 FROM vw_crds_Contact_Attributes
                 WHERE Contact_ID = c.Contact_ID
                   AND Attribute_ID = @FrequentFlyerUsAir ) AS FF_USAir,
               (
                 SELECT Attribute_Name
                 FROM vw_crds_Contact_Attributes
                 WHERE Contact_ID = c.Contact_ID
                   AND Attribute_Type_ID = @InternationalTravelExperience ) AS International_Travel_Experience,
               (
                 SELECT Notes
                 FROM vw_crds_Contact_Attributes
                 WHERE Contact_ID = c.Contact_ID
                   AND Attribute_Type_ID = @ExperienceServingAbroad ) AS Experience_Serving_Abroad,
               ( CONVERT( VARCHAR(10), fr.Response_Date, 101)) SignUpDate,
			spouse.SpouseOnTrip,
			spouse.SpouseName,
			dep.Deposit,
			dep.Payment_Type,
			dep.Donation_Date,
			dep.Pledge_Status,
			dep.amount DepositAmount,
               (
                 SELECT Attribute_Name
                 FROM vw_crds_Contact_Attributes
                 WHERE Contact_ID = c.Contact_ID
                   AND Attribute_Type_ID = @AbuseVictim ) AS Abuse_Victim,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @StewardsOfChildren ) StewardsOfChildren,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @Visa ) Visa,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @Diploma ) Diploma,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @MedicalLicense ) MedicalLicense,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @LetterVerification ) LetterVerification,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @GoodFaithLetter ) GoodFaithLetter,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @GoodStandingLetter ) GoodStandingLetter,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @Form10 ) Form10,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @MedicalApplication ) MedicalApplication,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @OrchardAfricaWaiver ) OrchardAfricaWaiver,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @CopyOfPassport ) CopyOfPassport,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @IPromise ) IPromise,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @Waiver ) Waiver,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @ProofOfGuardianship ) ProofOfGuardianship,
               (
                 SELECT( CASE d.Received
                             WHEN 0
                             THEN 'No'
                             WHEN 1
                             THEN 'Yes'
                         END ) AS DocReceived
                 FROM dbo.cr_EventParticipant_Documents d
                 WHERE d.Event_Participant_ID = ep.Event_Participant_ID
                   AND d.Document_ID = @CountryDocumentation ) CountryDocumentation,
               (
                 SELECT STUFF((
                          SELECT '|' + e.Event_Title
                          FROM MinistryPlatform.dbo.Event_Participants trip
                               INNER JOIN dbo.Events e ON trip.Event_ID = e.Event_ID
                                                      AND e.Event_Type_ID = @MissionTripEventType
                                                      AND e.Event_ID != @EventId
                          WHERE trip.Participant_ID = ep.Participant_ID
                          FOR XML PATH( '' ), TYPE ).value( '.', 'nvarchar(max)' ), 1, 1, '')) PreviousTrips,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @WorkTeamPreference1
                   AND r.Form_Response_ID = fr.Form_Response_ID ) WorkTeamPreference1,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @WorkTeamPreference2
                   AND r.Form_Response_ID = fr.Form_Response_ID ) WorkTeamPreference2,
               (
                 SELECT Response
                 FROM [MinistryPlatform].[dbo].[Form_Response_Answers] r
                 WHERE r.Form_Field_ID = @WorkTeamExperience
                   AND r.Form_Response_ID = fr.Form_Response_ID ) WorkTeamExperience
        FROM [MinistryPlatform].[dbo].[Event_Participants] ep
             INNER JOIN dbo.Participants p ON ep.participant_id = p.participant_id
             INNER JOIN dbo.Pledge_Campaigns pc ON ep.Event_ID = pc.Event_ID
             INNER JOIN dbo.Form_Responses fr ON p.Contact_ID = fr.Contact_ID
                                             AND pc.Pledge_Campaign_ID = fr.Pledge_Campaign_ID
             INNER JOIN dbo.Contacts c ON p.Contact_ID = c.Contact_ID
		   INNER JOIN dbo.Pledges pg ON pc.Pledge_Campaign_ID = pg.Pledge_Campaign_ID
								    AND c.Donor_Record = pg.Donor_ID
								    AND pg.Pledge_Status_ID != @PledgeDiscontinued
             INNER JOIN Households h ON c.Household_ID = h.Household_ID
             INNER JOIN Addresses a ON h.Address_ID = a.Address_ID
             INNER JOIN Congregations cn ON h.Congregation_ID = cn.Congregation_ID
             INNER JOIN Genders g ON c.Gender_ID = g.Gender_ID
             INNER JOIN Marital_Statuses ms ON c.Marital_Status_ID = ms.Marital_Status_ID
		   CROSS APPLY dbo.crds_TripDeposit(c.Contact_ID, ep.Event_ID, pc.Pledge_Campaign_ID) dep
		   CROSS APPLY dbo.crds_SpouseOnTrip(c.Contact_ID, ep.Event_ID) spouse
		   CROSS APPLY dbo.crds_SponsoredChild(c.Contact_ID) sponsoredChild
        WHERE ep.Event_ID = @EventID;
    END;
