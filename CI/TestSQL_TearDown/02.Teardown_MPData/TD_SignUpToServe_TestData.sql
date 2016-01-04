Use [MinistryPlatform]
GO

DELETE FROM Opportunities WHERE Add_to_Group IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) SU2S KC Mason Group');

DELETE FROM Event_Groups WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) SU2S KC Mason Group');

DELETE FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type like '(t) Serve - SU2S%');

DELETE FROM Groups WHERE Group_Name = '(t) SU2S KC Mason Group';

DELETE FROM Event_Types WHERE Event_Type like '(t) Serve - SU2S%';
