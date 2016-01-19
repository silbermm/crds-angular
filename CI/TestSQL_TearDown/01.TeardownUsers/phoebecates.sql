USE [MinistryPlatform]
GO

--Get the required data to add to our contact. 
Declare @contactID as int
Set @contactID = (select top 1 contact_id from contacts where email_address = 'mpcrds+6@gmail.com' and last_name = 'Cates');

Declare @houseHoldID as int
set @houseHoldID = (select houseHold_ID from contacts where contact_id = @contactID);

Declare @participantID as int
set @participantID = (select participant_id from participants where contact_id = @contactID);

Declare @userAccount as int
set @userAccount = (select user_id from dp_users where contact_id = @contactID);

Declare @donorID as int
set @donorID = (select donor_id from donors where contact_id = @contactID);

--Update old contact record so we can delete it. 
UPDATE [dbo].Contacts
SET Household_ID = null, Participant_Record = null, User_Account = null, Donor_record = null
WHERE email_address = 'mpcrds+6@gmail.com' and last_name = 'Cates';

--Delete the address if it exists.
IF  (select address_id from households where Household_ID = @houseHoldID) is not Null
BEGIN
DECLARE @addressID as int
set @addressID = (select address_ID from households where houseHold_ID = @houseHoldID);

update households set address_id = null where houseHold_ID = @houseHoldID;

Delete from addresses where address_id = @addressID;
END

--Just get rid of this so we can delete Phoebe's old contact record
DELETE From [dbo].CONTACT_HOUSEHOLDS
WHERE CONTACT_ID = @contactID;

DECLARE @communicationID as int
set @communicationID = (Select Communication_ID from dp_Communications where TO_CONTACT = @contactID);

DELETE from [dbo].dp_commands 
WHERE communication_id = @communicationID;

DELETE from [dbo].dp_Contact_Publications 
WHERE contact_id = @contactID;

DELETE from [dbo].dp_communication_messages 
WHERE Communication_ID = @communicationID;

Delete from [dbo].dp_Communications
WHERE Communication_ID = @communicationID;

Delete from [dbo].Activity_Log
WHERE Contact_id = @contactID;

Delete from [dbo].Activity_Log
WHERE houseHold_ID = @houseHoldID;

--Delete the household
UPDATE [dbo].Contacts Set houseHold_ID = null
WHERE Household_ID = @houseHoldID;

DELETE from [dbo].CONTACT_HOUSEHOLDS 
WHERE houseHold_ID = @houseHoldID;

Delete from [dbo].Households where houseHold_ID = @houseHoldID;

--delete relationships
delete from [dbo].contact_relationships where contact_id = @contactID;

delete from [dbo].contact_relationships where related_contact_id = @contactID;

--Delete all donations for Phoebe's trip pledge program.
DECLARE @donationsTable table
(
	donation_id int
)

insert into @donationsTable (donation_id) (select donation_id from donation_distributions where program_id = (select program_id from programs where program_name like '(t) GO Midgar%'));

delete from donation_distributions where donation_id in (select donation_id from @donationsTable);

delete from donations where donation_id in (select donation_id from @donationsTable);

--Delete all of Phoebe's donations.
delete from @donationsTable;

insert into @donationsTable (donation_id) (select donation_id from donations where donor_id = @donorId);

delete from donation_distributions where donation_id in (select donation_id from @donationsTable);

delete from donations where donation_id in (select donation_id from @donationsTable);

delete from pledges where donor_id = @donorID;

--delete the donor
delete from donors where donor_id = @donorID;

--Lets start getting rid of the participant record
delete from Form_Response_Answers where form_response_id = (select form_response_id from form_responses where contact_id = @contactID);

delete from form_responses where contact_id = @contactID;

delete from response_attributes where response_id in (select response_id from responses where participant_id = @participantID);

delete from responses where participant_id = @participantID;

delete from event_participants where participant_id = @participantID;

delete from group_participants where participant_id = @participantID;

delete from participants where participant_id = @participantID;
GO

--Delete userAccount
delete from dp_user_roles where user_id = (select user_id from dp_users where user_email = 'mpcrds+6@gmail.com');

delete from dp_users where user_email = 'mpcrds+6@gmail.com';

--Delete Phoebe's old contact record
DELETE FROM [dbo].Contacts where contact_id = (select top 1 contact_id from contacts where email_address = 'mpcrds+6@gmail.com' and last_name = 'Cates');
GO