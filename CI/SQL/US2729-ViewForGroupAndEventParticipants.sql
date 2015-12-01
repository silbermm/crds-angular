USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 2200)
BEGIN
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (2200
		   ,'Participants By Group And Event'
           ,305
           ,'Get contact information for participants in an event for a particular group'
           ,'Group_ID_Table.[Group_ID], Event_ID_Table.[Event_ID]
, Participant_ID_Table_Contact_ID_Table.[Contact_ID]
, Participant_ID_Table_Contact_ID_Table.[Nickname]
, Participant_ID_Table_Contact_ID_Table.[Last_Name]'
           ,'1=1')
SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
GO


