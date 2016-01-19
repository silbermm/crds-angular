USE [MinistryPlatform]
GO

update contacts set Participant_Record = 1, donor_record = 1 where contact_id = 1;
GO