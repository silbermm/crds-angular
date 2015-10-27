USE [MinistryPlatform];
GO

/****** Object:  UserDefinedFunction [dbo].[crds_SponsoredChild]    Script Date: 10/27/2015 8:26:41 AM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE FUNCTION [dbo].[crds_SponsoredChild](
                @Contact INT )
RETURNS @SponsoredChildTable TABLE( SponsoredChild          BIT NOT NULL,
                                    SponsoredChildFirstName VARCHAR(50) NULL,
                                    SponsoredChildLastName  VARCHAR(50) NULL,
                                    SponsoredChildIdNumber  VARCHAR(50))
AS
     BEGIN
     DECLARE @RelationshipId INT = 44;
     DECLARE @ChildFirstName VARCHAR(50);
     DECLARE @ChildLastName VARCHAR(50);
     DECLARE @ChildIdNumber VARCHAR(50);
     SELECT TOP 1 @ChildFirstName = c.First_Name,
                  @ChildLastName = c.Last_Name,
                  @ChildIdNumber = c.ID_Card
     FROM MinistryPlatform.dbo.Contact_Relationships cr
          INNER JOIN MinistryPlatform.dbo.Contacts c ON cr.Contact_ID = c.Contact_ID
     WHERE cr.Related_Contact_ID = @Contact
       AND cr.Relationship_ID = @RelationshipId
       AND cr.Start_Date <= GETDATE()
       AND cr.End_Date IS NULL
     ORDER BY CR.Contact_Relationship_ID DESC;
     IF @ChildIdNumber IS NOT NULL
         BEGIN
             INSERT INTO @SponsoredChildTable
                    SELECT 1,
                           @ChildFirstName,
                           @ChildLastName,
                           @ChildIdNumber;
         END;
     ELSE
         BEGIN
             INSERT INTO @SponsoredChildTable
                    SELECT 0,
                           NULL,
                           NULL,
                           NULL;
         END;
    RETURN;
    END;
GO
