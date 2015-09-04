USE [MinistryPlatform]
GO

DECLARE @PAGE_VIEW_ID int = 92193;
DECLARE @PLEDGE_CAMPAIGN_PAGE_ID int = 361;
DECLARE @PAGE_ROLE_ID int = 10405;
DECLARE @API_ROLE_ID int = 62;
DECLARE @ACCESS_LEVEL int = 0;

IF EXISTS (Select 1 FROM [dbo].[dp_Role_Pages] WHERE [dbo].[dp_Role_Pages].[Role_Page_ID] = @PAGE_ROLE_ID)
	BEGIN
		UPDATE [dbo].[dp_Role_Pages]
		SET  Role_ID = @API_ROLE_ID
			,Page_ID = @PLEDGE_CAMPAIGN_PAGE_ID
			,Access_Level = @ACCESS_LEVEL
		WHERE Role_Page_ID = @PAGE_ROLE_ID
	END
ELSE
	BEGIN
		SET IDENTITY_INSERT [dbo].[dp_Role_Pages] ON
		INSERT INTO [dbo].[dp_Role_Pages]
           ([Role_Page_ID]
		   ,[Role_ID]
           ,[Page_ID]
           ,[Access_Level]
           ,[Scope_All]
           ,[Approver]
           ,[File_Attacher]
           ,[Data_Importer]
           ,[Data_Exporter]
           ,[Secure_Records]
           ,[Allow_Comments]
           ,[Quick_Add])
     VALUES
           (@PAGE_ROLE_ID
		    ,@API_ROLE_ID
		    ,@PLEDGE_CAMPAIGN_PAGE_ID
			,@ACCESS_LEVEL
			,0
			,0
			,0
			,0
			,0
			,0
			,0
			,0)
			SET IDENTITY_INSERT [dbo].[dp_Role_Pages] OFF
	END

IF EXISTS (Select 1 FROM [dbo].[dp_Page_Views] WHERE [dbo].[dp_Page_Views].[Page_View_ID] = @PAGE_VIEW_ID)
	BEGIN
		UPDATE [dbo].[dp_Page_Views] SET
			 View_Title = 'GO Trips with Forms'
			,Page_ID = @PLEDGE_CAMPAIGN_PAGE_ID
			,Field_List = 'Pledge_Campaigns.[Pledge_Campaign_ID]
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
			,View_Clause = 'Pledge_Campaign_Type_ID_Table.[Campaign_Type] = ''Mission Project/Trip''
				AND Registration_Form_Table.[Form_ID] IS NOT NULL
				AND Event_ID_Table.[Event_Start_Date] >= GetDate()
		'
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
