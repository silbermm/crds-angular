USE MinistryPlatform
GO	

DELETE FROM Group_Participants WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) Fathers Oakley CG');

DELETE FROM Group_Participants WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) Fathers Oakley CG - Waitlist');

UPDATE Groups SET Parent_Group = NULL  WHERE Group_Name = '(t) Fathers Oakley CG - Waitlist';

DELETE FROM Groups WHERE  Group_Name = '(t) Fathers Oakley CG';

DELETE FROM Groups WHERE  Group_Name = '(t) Fathers Oakley CG - Waitlist';

