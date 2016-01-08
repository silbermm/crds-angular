Use [MinistryPlatform]
GO

update contacts set household_id = null, participant_record = null, user_account = null
Where contact_id in (select contact_id from contacts where email_address like 'mpcrds+1Time%');

delete from dp_user_roles where user_id in (select user_id from dp_users where contact_id in (select contact_id from contacts where email_address like 'mpcrds+1Time%'));

delete from dp_users where contact_id in (select contact_id from contacts where email_address like 'mpcrds+1Time%');

delete from Responses where participant_id in (select participant_id from participants where contact_id in (select contact_id from contacts where email_address like 'mpcrds+ITime%'));

delete from group_participants where participant_id in (select participant_id from participants where contact_id in (select contact_id from contacts where email_address like 'mpcrds+1Time%'));

delete from event_participants where participant_id in (select participant_id from participants where contact_id in (select contact_id from contacts where email_address like 'mpcrds+1Time%'));

delete from participants where contact_id in (select contact_id from contacts where email_address like 'mpcrds+1Time%');

delete from dp_commands where communication_id in (select communication_id from dp_communications where to_contact in (select contact_id from contacts where email_address like 'mpcrds+1Time%'));

delete from dp_contact_publications where contact_id in (select contact_id from contacts where email_address like 'mpcrds+1Time%');

delete from dp_Communication_Messages where contact_id in (select contact_id from contacts where email_address='mpcrds+1TimeMother@gmail.com');

delete from dp_Communication_Messages where contact_id in (select contact_id from contacts where email_address='mpcrds+1TimeFather@gmail.com');

delete from dp_Communication_Messages where contact_id in (select contact_id from contacts where email_address='mpcrds+1TimeChild19@gmail.com');

delete from dp_Communication_Messages where contact_id in (select contact_id from contacts where email_address='mpcrds+1TimeChild4@gmail.com');

delete from dp_communications where To_Contact in (select contact_id from contacts where email_address like 'mpcrds+1Time%');

delete from households where household_name = '1TimeHousehold';

delete from Contact_Households where household_id in (select household_id from households where household_name = '1TimeHousehold');

delete from Contact_Households where contact_id in (select contact_id from contacts where email_address='mpcrds+1TimeFather@gmail.com');

delete from Contact_Households where contact_id in (select contact_id from contacts where email_address='mpcrds+1TimeMother@gmail.com');

delete from Contact_Households where contact_id in (select contact_id from contacts where email_address='mpcrds+1TimeChild19@gmail.com');

delete from Contact_Households where contact_id in (select contact_id from contacts where email_address='mpcrds+1TimeChild4@gmail.com');

delete from contact_relationships where contact_id in (select contact_id from contacts where email_address='mpcrds+1TimeMother@gmail.com');

delete from contact_relationships where contact_id in (select contact_id from contacts where email_address='mpcrds+1TimeChild19@gmail.com');

delete from contact_relationships where contact_id in (select contact_id from contacts where email_address='mpcrds+1TimeFather@gmail.com');

delete from contact_relationships where contact_id in (select contact_id from contacts where email_address='mpcrds+1TimeChild4@gmail.com');

delete from Activity_Log where contact_id in (select contact_id from contacts where email_address like 'mpcrds+1Time%');

delete from contacts where email_address like 'mpcrds+1Time%';
