DECLARE @Contact int;
DECLARE @Married int;
DECLARE @SpouseID int;
DECLARE @SpouseName varchar(50);
DECLARE @TripEvent int;

SET @Contact = 768379;
SET @Married = 1;
SET @TripEvent = 1599781;

SELECT @SpouseID = [Related_Contact_ID], @SpouseName = C.First_Name + ' ' + C.Last_Name FROM [dbo].[Contact_Relationships] CR
JOIN [dbo].[Contacts] C ON C.Contact_ID = CR.[Related_Contact_ID]
WHERE [Relationship_ID] = @Married AND CR.[Contact_ID] = @Contact

IF EXISTS (SELECT EP.Participant_ID FROM [dbo].[Event_Participants] EP
JOIN [dbo].[Participants] P ON EP.Participant_ID = P.Participant_ID
JOIN [dbo].[Events] E ON E.Event_ID = EP.Event_ID
WHERE P.Contact_ID = @SpouseID AND E.Event_ID = @TripEvent)
SELECT @SpouseID, @SpouseName