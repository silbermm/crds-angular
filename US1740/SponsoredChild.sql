DECLARE @Contact int;
DECLARE @SponsoredRelationshipID int;

SET @Contact = 768379;
SET @SponsoredRelationshipID = 44;

SELECT First_Name, Last_Name, ID_Card FROM [dbo].[Contacts] C
JOIN [dbo].[Contact_Relationships] CR ON C.Contact_ID = CR.Contact_ID
WHERE CR.Relationship_ID = @SponsoredRelationshipID AND CR.Related_Contact_ID = @Contact

select c.First_Name
from dbo.Contact_Relationships cr 
inner join dbo.Contacts c on cr.Contact_ID = c.Contact_ID
where cr.Related_Contact_ID = @Contact
  and cr.Relationship_ID = @SponsoredRelationshipID

SELECT STUFF(( 
               SELECT '|' + c.First_Name
               FROM MinistryPlatform.dbo.Contact_Relationships cr
                    INNER JOIN dbo.Contacts c ON cr.Contact_ID = c.Contact_ID 
			where cr.Relationship_ID = @SponsoredRelationshipID
			  and cr.Related_Contact_ID = @Contact
               FOR XML PATH( '' ), TYPE ).value( '.', 'nvarchar(max)' ), 1, 1, '');