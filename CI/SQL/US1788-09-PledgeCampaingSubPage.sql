USE [MinistryPlatform]
GO

DELETE FROM [dbo].[dp_Page_Views]
      WHERE Page_View_ID = 92193
GO


SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dbo].[dp_Page_Views]
			([Page_View_ID]
			,[View_Title]
			,[Page_ID]
			,[Field_List]
			,[View_Clause])
		VALUES
			(92193
			,'GO Trips with Forms'
			,361
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
			,'Pledge_Campaign_Type_ID_Table.[Campaign_Type] = 'Mission Project/Trip'
				AND Registration_Form_Table.[Form_ID] IS NOT NULL
				AND Event_ID_Table.[Event_Start_Date] >= GetDate()
			')

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
