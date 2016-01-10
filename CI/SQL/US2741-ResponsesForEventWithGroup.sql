USE [MinistryPlatform]
GO

DECLARE @PAGE_VIEW_ID int = 2201;
DECLARE @FIELD_LIST nvarchar(4000) = N'
	Opportunity_ID_Table_Add_to_Group_Table.[Group_ID], Event_ID_Table.[Event_ID]
, Participant_ID_Table.[Participant_ID], Participant_ID_Table_Contact_ID_Table.[Contact_ID]
, Response_Result_ID_Table.[Response_Result_ID], Responses.[Response_Date], Event_ID_Table.[Event_Start_Date]
';
DECLARE @VIEW_CLAUSE nvarchar(4000) = N'Event_ID_Table.[Event_ID] IS NOT NULL';

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = @PAGE_VIEW_ID)
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
           (@PAGE_VIEW_ID
		   ,'Responses by Event and Group'
           ,382
           ,'Used to find all group members who have responded to an event.'
           ,@FIELD_LIST
           ,@VIEW_CLAUSE)
SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
ELSE 
BEGIN
	UPDATE [dbo].[dp_Page_Views]
	SET [Field_List] = @FIELD_LIST, [View_Clause] = @VIEW_CLAUSE
	WHERE Page_View_ID = @PAGE_VIEW_ID
END
GO