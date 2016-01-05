use MinistryPlatform;

DECLARE @VIEW_ID int = 2203;
DECLARE @RESPONSE_PAGE_ID int = 382;
DECLARE @VIEW_TITLE varchar(255) = N'Signup To Serve Reminders';

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = @VIEW_ID)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

	INSERT INTO [dbo].[dp_Page_Views] (
		 [Page_View_ID]
		,[View_Title]
		,[Page_ID]
		,[Description]
		,[Field_List]
		,[View_Clause]
	) VALUES (
		 @VIEW_ID
		,@VIEW_TITLE
		,@RESPONSE_PAGE_ID
		,N'What Reminders need to be sent out for SU2S'
		,N'Opportunity_ID_Table.[Opportunity_Title]
, Participant_ID_Table_Contact_ID_Table.[Contact_ID]
, Participant_ID_Table_Contact_ID_Table.[Email_Address]
, Participant_ID_Table_Contact_ID_Table.[Display_Name]
, Event_ID_Table.[Event_Title]
, Event_ID_Table.[Event_Start_Date]
, Event_ID_Table.[Event_End_Date]
, Opportunity_ID_Table.[Send_Reminder]
, Opportunity_ID_Table.[Reminder_Days_Prior]
, Opportunity_ID_Table_Reminder_Template_Table.[Communication_ID]
, Opportunity_ID_Table_Contact_Person_Table.[Contact_ID] AS [Opportunity_Contact_ID]
, Opportunity_ID_Table_Contact_Person_Table.[Email_Address] AS [Opportunity_Contact_Email_Address]
, Opportunity_ID_Table.[Shift_Start]
, Opportunity_ID_Table.[Shift_End]'
		,N'Opportunity_ID_Table.[Send_Reminder] = 1 
AND Event_ID_Table.[Event_Start_Date] >= GETDATE()
AND Opportunity_ID_Table.[Reminder_Days_Prior] = DATEDIFF(DAY, GETDATE(), Event_ID_Table.[Event_Start_Date])'
	)

	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
ELSE
BEGIN
	UPDATE [dbo].[dp_Page_Views] 
		SET [Field_List] = N'Opportunity_ID_Table.[Opportunity_Title]
, Participant_ID_Table_Contact_ID_Table.[Contact_ID]
, Participant_ID_Table_Contact_ID_Table.[Email_Address]
, Participant_ID_Table_Contact_ID_Table.[Display_Name]
, Event_ID_Table.[Event_Title]
, Event_ID_Table.[Event_Start_Date]
, Event_ID_Table.[Event_End_Date]
, Opportunity_ID_Table.[Send_Reminder]
, Opportunity_ID_Table.[Reminder_Days_Prior]
, Opportunity_ID_Table_Reminder_Template_Table.[Communication_ID]
, Opportunity_ID_Table_Contact_Person_Table.[Contact_ID] AS [Opportunity_Contact_ID]
, Opportunity_ID_Table_Contact_Person_Table.[Email_Address] AS [Opportunity_Contact_Email_Address]
, Opportunity_ID_Table.[Shift_Start]
, Opportunity_ID_Table.[Shift_End]'
		,[View_Clause] = N'Opportunity_ID_Table.[Send_Reminder] = 1 
AND Event_ID_Table.[Event_Start_Date] >= GETDATE()
AND Opportunity_ID_Table.[Reminder_Days_Prior] = DATEDIFF(DAY, GETDATE(), Event_ID_Table.[Event_Start_Date])'
		,[View_Title] = @VIEW_TITLE
	WHERE [Page_View_ID] = @VIEW_ID
END