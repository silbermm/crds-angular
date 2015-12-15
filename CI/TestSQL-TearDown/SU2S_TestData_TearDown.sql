Use [MinistryPlatform]
GO

DECLARE @groupId as int ;


Delete from Event_Participants where Group_ID = (Select group_ID from Groups where Group_Name = '(t) SU2S KC Mason Group');

Delete from Event_Groups where Group_ID = (Select group_ID from Groups where Group_Name = '(t) SU2S KC Mason Group');
