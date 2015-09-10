USE [MinistryPlatform]
GO

DECLARE @PAGE_VIEW_ID int = 92219;;
DECLARE @PAGE_ROLE_ID int = 10405;
DECLARE @API_ROLE_ID int = 62;
DECLARE @ACCESS_LEVEL int = 0;
DECLARE @EVENT_PAGE_ID int = 308;

IF EXISTS (Select 1 FROM [dbo].[dp_Page_Views] WHERE [dbo].[dp_Page_Views].[Page_View_ID] = @PAGE_VIEW_ID)
BEGIN
		UPDATE [dbo].[dp_Page_Views] SET
			 View_Title = 'Current Missions Trips Events'
			,Page_ID = @EVENT_PAGE_ID
			,Field_List = 'Events.[Event_ID]
				, Events.[Event_Title]
				, Event_Type_ID_Table.[Event_Type]
				, Program_ID_Table.[Program_Name]
				, Program_ID_Table_Pledge_Campaign_ID_Table.[Pledge_Campaign_ID]
				, Program_ID_Table_Pledge_Campaign_ID_Table.[Campaign_Name]'
			,View_Clause = 'Event_Type_ID_Table.[Event_Type] = ''Missions Trips''
				AND Events.[Event_Start_Date] >= GetDate()'
		WHERE dp_Page_Views.Page_View_ID = @PAGE_VIEW_ID
	END
ELSE
BEGIN
		SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
		INSERT INTO [dbo].[dp_Page_Views]
				   ([Page_View_ID]
				   ,[View_Title]
				   ,[Page_ID]
				   ,[Field_List]
				   ,[View_Clause])
			 VALUES
				   (@PAGE_VIEW_ID
				   ,'GO Trips with Forms'
				   ,@PLEDGE_CAMPAIGN_PAGE_ID
				   ,'Pledge_Campaigns.[Pledge_Campaign_ID]
		 				, Pledge_Campaigns.[Campaign_Name]
		 				, Pledge_Campaign_Type_ID_Table.[Campaign_Type]
		 				, Pledge_Campaigns.[Start_Date]
		 				, Pledge_Campaigns.[End_Date]
		 				, Pledge_Campaigns.[Campaign_Goal]
		 				, Registration_Form_Table.[Form_ID]
		 				, Registration_Form_Table.[Form_Title]
		 				, Pledge_Campaigns.[Registration_Start]
		 				, Pledge_Campaigns.[Registration_End]
		 				, Pledge_Campaigns.[Youngest_Age_Allowed]
		 				, Event_ID_Table.[Event_Start_Date]'
				   ,'Pledge_Campaign_Type_ID_Table.[Campaign_Type] = ''Mission Project/Trip''
		 				AND Registration_Form_Table.[Form_ID] IS NOT NULL
		 				AND Event_ID_Table.[Event_Start_Date] >= GetDate()
		')
		SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
	END