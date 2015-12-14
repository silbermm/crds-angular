USE [MinistryPlatform];
GO

/****** Object:  UserDefinedFunction [dbo].[crds_SponsoredChild]    Script Date: 11/19/2015 3:36:48 PM ******/

IF OBJECT_ID('dbo.crds_FindParentAttendingRelatedEvent', 'TF') IS NOT NULL
BEGIN
	DROP FUNCTION [dbo].[crds_FindParentAttendingRelatedEvent]
END
GO

/****** Object:  UserDefinedFunction [dbo].[crds_SponsoredChild]    Script Date: 11/19/2015 3:03:49 PM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE FUNCTION [dbo].[crds_FindParentAttendingRelatedEvent](
                @Contact INT,
                @Event   INT )
RETURNS @ParentTable TABLE( DisplayName VARCHAR(50) NULL,
                            Email       VARCHAR(50) NULL )
AS
     BEGIN
     DECLARE @DisplayName VARCHAR(50);
     DECLARE @Email VARCHAR(50);
     SELECT TOP 1 @DisplayName = c.Display_Name,
                  @Email = c.Email_Address
     FROM MinistryPlatform.dbo.Event_Participants ep
          INNER JOIN ministryPlatform.dbo.Participants p ON ep.Participant_ID = p.Participant_ID
          INNER JOIN MinistryPlatform.dbo.Contacts c ON p.Contact_ID = c.Contact_ID
          INNER JOIN MinistryPlatform.dbo.Contact_Relationships cr ON c.Contact_ID = cr.Contact_ID
                                                                  AND cr.Related_Contact_ID = @Contact
          INNER JOIN MinistryPlatform.dbo.Relationships r ON cr.Relationship_ID = r.Relationship_ID
                                                         AND r.ImmediateFamily = 1
     WHERE ep.Participation_Status_ID = 2 
	      AND ep.Event_ID IN( 
                           SELECT childEvent.Parent_Event_ID
                           FROM MinistryPlatform.dbo.Events childEvent
                           WHERE childEvent.Event_ID = @Event );

    IF @DisplayName IS NOT NULL
        BEGIN
            INSERT INTO @ParentTable
                    SELECT @DisplayName,
                        @Email;
        END;
    ELSE
        BEGIN
            INSERT INTO @ParentTable
                    SELECT NULL,
                        NULL;
    END;
    RETURN;
    END;
GO