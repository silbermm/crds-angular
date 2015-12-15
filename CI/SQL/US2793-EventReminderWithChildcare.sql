USE MinistryPlatform
GO

IF EXISTS (SELECT * FROM [dbo].[dp_Pages] WHERE [Page_ID] = 492)
BEGIN
	UPDATE [dbo].[dp_Pages] 
	SET [Default_Field_List] = N'Events.Event_Start_Date
,Events.Event_Title
,Event_Type_ID_Table.Event_Type
,Congregation_ID_Table.Congregation_Name
,Program_ID_Table.Program_Name
,Events.Event_End_Date
,Visibility_Level_ID_Table.Visibility_Level, Events.Send_Reminder, Events.Event_ID, Events.Reminder_Sent,
Primary_Contact_Table.Contact_ID as [Primary_Contact_ID],
Primary_Contact_Table.Email_Address as [Primary_Contact_Email_Address]'
	WHERE [Page_ID] = 492
END