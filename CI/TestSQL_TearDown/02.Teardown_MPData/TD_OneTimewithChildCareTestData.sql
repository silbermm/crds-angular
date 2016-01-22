Use [MinistryPlatform]
GO

UPDATE Events SET Parent_Event_ID = null WHERE Event_Title = '(t) 1Time Mason with ChildCare - Childcare';

DELETE FROM Event_Groups WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) 1Time Mason Group with ChildCare');

DELETE from EVENT_Participants where group_id in (select group_id from groups where group_name like '(t) 1Time Mason Group%');

DELETE FROM Events WHERE Event_Title = '(t) 1Time Mason with ChildCare';

DELETE FROM Events WHERE Event_Title = '(t) 1Time Mason with ChildCare - Childcare';

DELETE from group_participants where group_id in (select group_id from groups where group_name like '(t) 1Time Mason Group%');

DELETE FROM Groups WHERE Group_Name = '(t) 1Time Mason Group with ChildCare';
GO