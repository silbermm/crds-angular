USE [MinistryPlatform]
GO

DECLARE @Process_ID int;

INSERT INTO [dbo].[dp_Processes]
	([Process_Name], [Process_Manager], [Active], [Description], [Record_Type], [Domain_ID], [Trigger_Fields], [Dependent_Condition])
VALUES
	('Receive a request to join a group', 1, 1, 'Process that defines the steps necessary when a community member wants to join a group', 382, 1, 
	'Opportunity_ID', 'EXISTS(select 1 from Opportunities as o join Groups as g on g.Group_ID = o.Add_to_Group join Responses as r on r.Opportunity_ID = o.Opportunity_ID and r.Response_ID=Responses.Response_ID WHERE g.Group_Type_ID=22)')

SET @Process_ID = SCOPE_IDENTITY();

INSERT INTO [dbo].[dp_Process_Steps]
	([Step_Name], [Instructions], [Process_Step_Type_ID], [Escalation_Only], [Order], [Process_ID], [Specific_User], [Supervisor_User], 
	[Lookup_User_Field], [Domain_ID], [Escalate_to_Step], [Task_Deadline], [Email_Template], [To_Specific_Contact], [Email_To_Contact_Field], [SQL_Statement])
VALUES
	('Copy Onboarding', 'Copy Onboarding steps from group to response.', 3,	0, 1, @Process_ID, NULL, 0,
	'Opportunity_ID_Table_Contact_Person_Table.[User_Account]',	1, NULL, NULL, NULL, NULL, NULL, 'EXECUTE cr_CopyOnboardingSteps @ResponseID = dp_RecordID'),
	('Email person that responded', 'An email to the community member has been sent', 4, 0, 2, @Process_ID, 5, 0, 
	NULL, 1, NULL, NULL, 52, NULL, 'Participant_ID_Table.Contact_ID', NULL),
	('Request to join a team', 'Please review the attached record and once the appropriate action has been taken, complete this task.',	2, 0, 3, @Process_ID, NULL, 0, 
	'Opportunity_ID_Table_Contact_Person_Table.[User_Account]', 1, NULL, NULL, NULL, NULL, NULL, NULL),
	('Handle Placed or Not', 'Determines if the response was placed in the group or not placed in the group', 3, 0, 4, @Process_ID, NULL, 0,
	'Opportunity_ID_Table_Contact_Person_Table.[User_Account]', 1, NULL, NULL, NULL, NULL, NULL, 
	'IF (Select count(*) from Responses where Response_ID = dp_RecordID and Response_Result_ID = 1) =1
Begin 
   DECLARE @Contact_ID int

    SELECT @Contact_ID = P.Contact_ID 
     FROM Responses R
     JOIN Participants P on P.Participant_ID = R.Participant_ID
     WHERE R.Response_ID = dp_RecordID
    
     EXECUTE cr_SendMessage @UserID = dp_UserID, @ToContactID = @Contact_ID, @MessageTemplateID = 1262
   End
ELSE
   Begin
      EXECUTE cr_CreateTask @Title = ''Follow up with Participant'', @Description = ''Please follow up with the community member who wanted to join your group.'', @StartDate = {d ''2015-02-19''}, @AssignedUserID = dp_UserID, @AuthorID = dp_UserID, @RecordID = dp_RecordID, @PageID = 382
   End')
GO