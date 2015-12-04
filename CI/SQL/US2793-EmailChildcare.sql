USE [MinistryPlatform]
GO

DECLARE @subpage_view int = 120

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Sub_Page_Views] WHERE [Sub_Page_View_ID] = @subpage_view)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON
	INSERT INTO [dbo].[dp_Sub_Page_Views]
			   ([Sub_Page_View_ID]
			   ,[View_Title]
			   ,[Sub_Page_ID]
			   ,[Description]
			   ,[Field_List]
			   ,[View_Clause])
		 VALUES
			   (@subpage_view
			   ,N'Registered'
			   ,508
			   ,N'Show only registered users'
			   ,N'Participant_ID_Table.[Participant_ID], Participant_ID_Table_Contact_ID_Table.[Contact_ID], Participant_ID_Table_Contact_ID_Table.Display_Name,Participant_ID_Table_Contact_ID_Table.Nickname, Participant_ID_Table_Contact_ID_Table.[Email_Address], Participation_Status_ID_Table.Participation_Status, Event_Participants.Time_In, Group_Participant_ID_Table_Group_ID_Table.Group_Name, Group_Participant_ID_Table_Group_Role_ID_Table.Role_Title'
			   ,N'Event_Participants.Participation_Status_ID = 2')
	SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] OFF
END
ELSE 
BEGIN
	UPDATE [dbo].[dp_Sub_Page_Views] 
	SET [Field_List] = N'Participant_ID_Table.[Participant_ID], Participant_ID_Table_Contact_ID_Table.[Contact_ID], Participant_ID_Table_Contact_ID_Table.Display_Name,Participant_ID_Table_Contact_ID_Table.Nickname, Participant_ID_Table_Contact_ID_Table.[Email_Address], Participation_Status_ID_Table.Participation_Status, Event_Participants.Time_In, Group_Participant_ID_Table_Group_ID_Table.Group_Name, Group_Participant_ID_Table_Group_Role_ID_Table.Role_Title'
	WHERE [Sub_Page_View_ID] = 120
END

