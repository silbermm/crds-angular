Use [MinistryPlatform]
GO
---Update contact records 

update contacts set household_id = null, participant_record = null, Donor_Record = null, user_account = null
Where contact_id in (select contact_id from contacts where email_address like 'mpcrds+tremplay%');

DELETE FROM Background_Checks WHERE Contact_ID in (SELECT Contact_ID FROM Contacts WHERE Email_Address like 'mpcrds+tremplay%');

DELETE FROM dp_user_roles where user_id in (select user_id from dp_users where contact_id in (select contact_id from contacts where email_address like 'mpcrds+tremplay%'));

DELETE FROM dp_users where contact_id in (select contact_id from contacts where email_address like 'mpcrds+tremplay%');

DELETE FROM Responses where participant_id in (select participant_id from participants where contact_id in (select contact_id from contacts where email_address like 'mpcrds+tremplay%'));

DELETE FROM group_participants where participant_id in (select participant_id from participants where contact_id in (select contact_id from contacts where email_address like 'mpcrds+tremplay%'));

DELETE FROM event_participants where participant_id in (select participant_id from participants where contact_id in (select contact_id from contacts where email_address like 'mpcrds+tremplay%'));

DELETE FROM participants where contact_id in (select contact_id from contacts where email_address like 'mpcrds+tremplay%');

DELETE FROM dp_commands where communication_id in (select communication_id from dp_communications where to_contact in (select contact_id from contacts where email_address like 'mpcrds+tremplay%'));

DELETE FROM dp_contact_publications where contact_id in (select contact_id from contacts where email_address like 'mpcrds+tremplay%');

DELETE FROM dp_communications where To_Contact in (select contact_id from contacts where email_address like 'mpcrds+tremplay%');

DELETE FROM dp_Communication_Messages where contact_id in (select contact_id from contacts where email_address='mpcrds+tremplay%');

DELETE FROM households where household_name = 'Tremplay';

DELETE FROM Addresses WHERE Address_ID IN (SELECT Address_ID FROM Addresses WHERE Address_Line_1 = '123 Towne Commons Way' AND Postal_Code =45067);

DELETE FROM Contact_Households where contact_id in (select contact_id from contacts where email_address like 'mpcrds+tremplay%');

DELETE FROM contact_relationships where contact_id in (select contact_id from contacts where email_address like 'mpcrds+tremplay%');

DELETE FROM Donors WHERE Contact_ID in (SELECT Contact_ID FROM Contacts WHERE Email_Address like 'mpcrds+tremplay%');

DELETE FROM Activity_Log where contact_id in (select contact_id from contacts where email_address like 'mpcrds+tremplay%');

DELETE FROM contacts where email_address like 'mpcrds+tremplay%';