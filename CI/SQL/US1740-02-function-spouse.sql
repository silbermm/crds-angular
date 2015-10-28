USE [MinistryPlatform];
GO

/****** Object:  UserDefinedFunction [dbo].[crds_SpouseOnTrip]    Script Date: 10/27/2015 8:25:03 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE FUNCTION [dbo].[crds_SpouseOnTrip](
                @Contact   INT,
                @TripEvent INT )
RETURNS @SpouseTable TABLE( SpouseOnTrip BIT NOT NULL,
                            SpouseName   VARCHAR(50) NULL )
AS
     BEGIN
	    DECLARE @Married INT = 1;
	    DECLARE @SpouseID INT;
	    DECLARE @SpouseName VARCHAR(50);
	    SELECT @SpouseID = [Related_Contact_ID],
			 @SpouseName = C.First_Name + ' ' + C.Last_Name
	    FROM [dbo].[Contact_Relationships] CR
		    JOIN [dbo].[Contacts] C ON C.Contact_ID = CR.[Related_Contact_ID]
	    WHERE [Relationship_ID] = @Married
		 AND CR.[Contact_ID] = @Contact;
	    IF EXISTS( SELECT EP.Participant_ID
				FROM [dbo].[Event_Participants] EP
					JOIN [dbo].[Participants] P ON EP.Participant_ID = P.Participant_ID
					JOIN [dbo].[Events] E ON E.Event_ID = EP.Event_ID
				WHERE P.Contact_ID = @SpouseID
				  AND E.Event_ID = @TripEvent )
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
GO
