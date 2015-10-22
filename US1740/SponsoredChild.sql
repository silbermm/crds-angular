DECLARE @Contact int;
DECLARE @SponsoredRelationshipID int;
DECLARE @SignUpDate date; --Should have this from the form application?

SET @Contact = 768379;
SET @SponsoredRelationshipID = 44;
SET @SignUpDate = GETDATE();

SELECT First_Name, Last_Name, ID_Card FROM [dbo].[Contacts] C
JOIN [dbo].[Contact_Relationships] CR ON C.Contact_ID = CR.Contact_ID
WHERE CR.Relationship_ID = @SponsoredRelationshipID AND CR.Related_Contact_ID = @Contact AND (CR.End_Date <= @SignUpDate OR CR.End_Date IS NULL)

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
			  and (cr.End_Date <= @SignUpDate or cr.End_Date is null)
               FOR XML PATH( '' ), TYPE ).value( '.', 'nvarchar(max)' ), 1, 1, '');