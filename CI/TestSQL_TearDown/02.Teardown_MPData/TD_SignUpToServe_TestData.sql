Use [MinistryPlatform]
GO

DELETE FROM Opportunities WHERE Add_to_Group IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) KidsClub Oakley Group');

DELETE FROM Opportunities WHERE Add_to_Group IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) Superbowl Oakley Group');

DELETE FROM Event_Groups WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) KidsClub Oakley Group');

DELETE FROM Event_Rooms WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 11:00'));

DELETE FROM Event_Equipment WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 11:00'));

DELETE FROM Event_Participants WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 11:00'));

DELETE FROM Event_Rooms WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 1:00'));

DELETE FROM Event_Equipment WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 1:00'));

DELETE FROM Event_Participants WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 1:00'));

DELETE FROM Event_Groups WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) Superbowl Oakley Group');

DELETE FROM Event_Rooms WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 10:00'));

DELETE FROM Event_Equipment WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 10:00'));

DELETE FROM Event_Participants WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 10:00'));

DELETE FROM Event_Rooms WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 3:00'));

DELETE FROM Event_Equipment WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 3:00'));

DELETE FROM Event_Participants WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 3:00'));

DELETE FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 10:00');

DELETE FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 3:00');

DELETE FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 11:00');

DELETE FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 1:00');

DELETE FROM Group_Participants WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) KidsClub Oakley Group');

DELETE FROM Group_Participants WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) Superbowl Oakley Group');

DELETE FROM Groups WHERE Group_Name = '(t) KidsClub Oakley Group';

DELETE FROM Groups WHERE Group_Name = '(t) Superbowl Oakley Group';

DELETE FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 10:00';

DELETE FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 3:00';

DELETE FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 11:00';

DELETE FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 1:00';

