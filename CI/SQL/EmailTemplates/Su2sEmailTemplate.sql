USE [MinistryPlatform]
GO

DECLARE @TemplateID int = 14567;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Communications] WHERE Communication_ID = @TemplateID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Communications] ON
	INSERT INTO [dp_Communications]
	(
		 [Communication_ID]
		,[Author_User_ID]
		,[Subject]
		,[Body]
		,[Domain_ID]
		,[Start_Date]
		,[From_Contact]
		,[Reply_to_Contact]
		,[Template]
		,[Active]
	)
	VALUES
	(
		 @TemplateID
		,1
		,'Reminder: Serve Signup'
		,N'[Nickname], <br /> <br />
		 This is a reminder that you have signed up to serve for [Opportunity_Title] on [Event_Start_Date]. 
		 Your shift starts at [Shift_Start] and ends at [Shift_End]. <br /> <br />
		 If you need to modify your service, please contact your team lead by replying to this email'
		,1
		,GetDate()
		,7
		,7
		,1
		,1
	);

	SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
END