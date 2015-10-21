DECLARE @Contact int;
DECLARE @SponsoredRelationshipID int;

SET @Contact = 768379;
SET @SponsoredRelationshipID = 44;

SELECT First_Name, Last_Name, ID_Card FROM [dbo].[Contacts] C
JOIN [dbo].[Contact_Relationships] CR ON C.Contact_ID = CR.Contact_ID
WHERE CR.Relationship_ID = @SponsoredRelationshipID AND CR.Related_Contact_ID = @Contact