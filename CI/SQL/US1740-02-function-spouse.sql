USE [MinistryPlatform]
GO

/****** Object:  UserDefinedFunction [dbo].[crds_SpouseOnTrip]    Script Date: 10/29/2015 3:37:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_SpouseOnTrip]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'ALTER FUNCTION [dbo].[crds_SpouseOnTrip](
                @Contact   INT,
                @TripEvent INT )
RETURNS 
@SpouseTable TABLE( SpouseOnTrip       bit NOT NULL,
                         SpouseName  VARCHAR(50) NULL )
AS
     BEGIN

     DECLARE @Married INT = 1;
	 DECLARE @ActivePledge INT = 1;
     DECLARE @SpouseID INT;
     DECLARE @SpouseName VARCHAR(50);

     SELECT @SpouseID = [Related_Contact_ID],
            @SpouseName = C.First_Name + '' '' + C.Last_Name
     FROM [dbo].[Contact_Relationships] CR
          JOIN [dbo].[Contacts] C ON C.Contact_ID = CR.[Related_Contact_ID]
     WHERE [Relationship_ID] = @Married
       AND CR.[Contact_ID] = @Contact;
     IF EXISTS( SELECT EP.Participant_ID
                FROM [dbo].[Event_Participants] EP
                     JOIN [dbo].[Participants] P ON EP.Participant_ID = P.Participant_ID
                     JOIN [dbo].[Events] E ON E.Event_ID = EP.Event_ID
					 JOIN [dbo].[Donors] D ON D.Contact_ID = P.Contact_ID
					 JOIN [dbo].[Pledge_Campaigns] PC ON PC.Event_ID = EP.Event_ID
					 JOIN [dbo].[Pledges] PL ON PL.Donor_ID = D.Donor_ID AND PL.Pledge_Campaign_ID = PC.Pledge_Campaign_ID
                WHERE P.Contact_ID = @SpouseID
                  AND E.Event_ID = @TripEvent 
				  And PL.Pledge_Status_ID = @ActivePledge )
         BEGIN
		   INSERT INTO @SpouseTable
                    SELECT 1,
                           @SpouseName;
         END;
    ELSE
	   BEGIN
	   INSERT INTO @SpouseTable
                    SELECT 0,
                           @SpouseName;
	   END;
    RETURN;
    END;
' 
END

GO