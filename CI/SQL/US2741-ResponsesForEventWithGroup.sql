USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 2201)
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
           (2201
		   ,'Responses by Event and Group'
           ,382
           ,'Used to find all group members who have responded to an event.'
           ,'Opportunity_ID_Table_Add_to_Group_Table.[Group_ID], Event_ID_Table.[Event_ID]
, Participant_ID_Table.[Participant_ID], Participant_ID_Table_Contact_ID_Table.[Contact_ID]
, Response_Result_ID_Table.[Response_Result_ID], Responses.[Response_Date]'
           ,'1=1')
SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
GO