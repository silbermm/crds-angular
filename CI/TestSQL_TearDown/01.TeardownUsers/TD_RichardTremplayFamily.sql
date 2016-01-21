Use [MinistryPlatform]
GO

DECLARE @fatherDonorId AS INT
DECLARE @motherDonorId AS INT
DECLARE @donationId AS INT


---delete Donations and donation distributions

SET @fatherDonorId = (SELECT Donor_ID FROM Donors WHERE Contact_ID IN (SELECT Contact_ID FROM Contacts WHERE Email_Address = 'mpcrds+tremplay.richard@gmail.com'));
SET @motherDonorId = (SELECT Donor_ID FROM Donors WHERE Contact_ID IN (SELECT Contact_ID FROM Contacts WHERE Email_Address = 'mpcrds+tremplay.richard@gmail.com'));

WHILE EXISTS (SELECT * FROM Donations WHERE Donor_ID = @fatherDonorId)
BEGIN
SET @donationId = (SELECT TOP 1 Donation_ID FROM Donations WHERE Donor_ID = @fatherDonorId);
DELETE FROM Donation_Distributions WHERE Donation_ID = @donationId;
DELETE FROM Donations WHERE Donation_ID = @donationId;
END

WHILE EXISTS (SELECT * FROM Donations WHERE Donor_ID = @motherDonorId)
BEGIN
SET @donationId = (SELECT TOP 1 Donation_ID FROM Donations WHERE Donor_ID = @motherDonorId);
DELETE FROM Donation_Distributions WHERE Donation_ID = @donationId;
DELETE FROM Donations WHERE Donation_ID = @donationId;
END

---delete from recurring gifts

DELETE FROM Recurring_Gifts WHERE Donor_ID = @fatherDonorId;
DELETE FROM Recurring_Gifts WHERE Donor_ID = @motherDonorId;

---delete pledges 

DELETE FROM Form_Response_Answers WHERE Pledge_ID IN (SELECT Pledge_ID FROM Pledges WHERE Donor_ID = @fatherDonorId);
DELETE FROM Pledges WHERE Donor_ID = @fatherDonorId;
DELETE FROM Form_Response_Answers WHERE Pledge_ID IN (SELECT Pledge_ID FROM Pledges WHERE Donor_ID = @motherDonorId);
DELETE FROM Pledges WHERE Donor_ID = @motherDonorId;

---delete form responses and answers
WHILE EXISTS (SELECT Form_Response_Id from Form_Responses WHERE Contact_ID IN (SELECT Contact_ID from Contacts WHERE Email_Address like 'mpcrds+tremplay%'))
BEGIN 
DECLARE @formResponseId AS INT
SET @formResponseId = (SELECT Top 1 Form_Response_Id from Form_Responses WHERE Contact_ID IN (SELECT Contact_ID from Contacts WHERE Email_Address like 'mpcrds+tremplay%'));
DELETE FROM Form_Response_Answers WHERE Form_Response_ID = @formResponseId;
DELETE FROM Form_Responses WHERE Form_Response_ID = @formResponseId;
END

DECLARE @householdID as int
set @householdID = (select top 1 household_id from contacts where email_address = 'mpcrds+tremplay.richard@gmail.com');

---Update contact records 
UPDATE Contacts SET Household_ID = null, Participant_Record = null, Donor_Record = null, user_account = null
WHERE Contact_ID IN (SELECT Contact_ID FROM Contacts WHERE Email_Address like 'mpcrds+tremplay%');

---delete Back ground checks
DELETE FROM Background_Checks WHERE Contact_ID in (SELECT Contact_ID FROM Contacts WHERE Email_Address like 'mpcrds+tremplay%');

--- delete user roles
DELETE FROM dp_User_Roles WHERE user_id in (SELECT user_id FROM dp_users WHERE Contact_ID in (SELECT Contact_ID FROM contacts WHERE email_address like 'mpcrds+tremplay%'));

---delete user
DELETE FROM dp_users WHERE Contact_ID in (SELECT Contact_ID FROM contacts WHERE email_address like 'mpcrds+tremplay%');

---delete responses
DELETE FROM Responses WHERE participant_id in (SELECT participant_id FROM participants WHERE Contact_ID in (SELECT Contact_ID FROM contacts WHERE email_address like 'mpcrds+tremplay%'));

---delete  group participants
DELETE FROM group_participants WHERE participant_id in (SELECT participant_id FROM participants WHERE Contact_ID in (SELECT Contact_ID FROM contacts WHERE email_address like 'mpcrds+tremplay%'));

---delete event participants
DELETE FROM event_participants WHERE participant_id in (SELECT participant_id FROM participants WHERE Contact_ID in (SELECT Contact_ID FROM contacts WHERE email_address like 'mpcrds+tremplay%'));

--- delete participant record
DELETE FROM participants WHERE Contact_ID in (SELECT Contact_ID FROM contacts WHERE email_address like 'mpcrds+tremplay%');

---delete commands
DELETE FROM dp_commands WHERE communication_id in (SELECT communication_id FROM dp_communications WHERE to_contact in (SELECT Contact_ID FROM contacts WHERE email_address like 'mpcrds+tremplay%'));

---delete publications
DELETE FROM dp_contact_publications WHERE Contact_ID in (SELECT Contact_ID FROM contacts WHERE email_address like 'mpcrds+tremplay%');

---delete communication messages
DELETE FROM dp_Communication_Messages WHERE Contact_ID in (SELECT Contact_ID FROM contacts WHERE email_address='mpcrds+tremplay%');

DELETE FROM [dbo].dp_communication_messages where communication_id in (select communication_id from dp_communications where TO_Contact in (select contact_id from contacts where email_address like 'mpcrds+tremplay%'));

---delete communication
DELETE FROM dp_communications WHERE To_Contact in (SELECT Contact_ID FROM contacts WHERE email_address like 'mpcrds+tremplay%');

---delete households
DELETE FROM Activity_Log where Household_ID = @householdID;

DELETE FROM households WHERE household_name = 'Tremplay';

---delete address since we hard coded the id in the setup script
DELETE FROM Addresses WHERE Address_ID =100000020;

---delete contact households
DELETE FROM Contact_Households WHERE Contact_ID in (SELECT Contact_ID FROM contacts WHERE email_address like 'mpcrds+tremplay%');

---delete contact relationships
DELETE FROM contact_relationships WHERE Contact_ID in (SELECT Contact_ID FROM contacts WHERE email_address like 'mpcrds+tremplay%');

---delete donor accounts
DELETE FROM Donor_Accounts WHERE Donor_ID = @fatherDonorId;
DELETE FROM Donor_Accounts WHERE Donor_ID = @motherDonorId;

---delete donor records
DELETE FROM Donors WHERE Contact_ID in (SELECT Contact_ID FROM Contacts WHERE Email_Address like 'mpcrds+tremplay%');

---delete contact log
DELETE FROM Contact_Log WHERE Contact_ID in (SELECT Contact_ID FROM Contacts WHERE Email_Address like 'mpcrds+tremplay%');

---delete serve restricions
DELETE FROM cr_Serve_Restrictions WHERE Contact_ID in (SELECT Contact_ID FROM Contacts WHERE Email_Address like 'mpcrds+tremplay%');

--delete activity los
DELETE FROM Activity_Log WHERE Contact_ID in (SELECT Contact_ID FROM contacts WHERE email_address like 'mpcrds+tremplay%');

---delete contacts
DELETE FROM contacts WHERE email_address like 'mpcrds+tremplay%';
GO