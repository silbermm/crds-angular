USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
INSERT INTO [dbo].[dp_Page_Views]
			([Page_View_ID]
			,[View_Title]
			,[Page_ID]
			,[Field_List]
			,[View_Clause])
		VALUES
			(2186
			,'TripPledgesByContactId'
			,363
			,'Donor_ID_Table_Contact_ID_Table.[Contact_ID]
				, Donor_ID_Table.[Donor_ID]
				, Pledge_Campaign_ID_Table_Event_ID_Table.[Event_ID]
				, Pledge_Campaign_ID_Table_Event_ID_Table.[Event_Title]
				, Pledge_Campaign_ID_Table_Event_ID_Table_Event_Type_ID_Table.[Event_Type_ID]
				, Pledge_Campaign_ID_Table_Event_ID_Table_Event_Type_ID_Table.[Event_Type]
				, Pledge_Campaign_ID_Table_Program_ID_Table.[Program_Name]
				, Pledge_Campaign_ID_Table_Program_ID_Table.[Program_ID]
				, Pledge_Campaign_ID_Table_Program_ID_Table.[Allow_Online_Giving]
				, Pledge_Campaign_ID_Table_Event_ID_Table.[Event_Start_Date]
				, Pledge_Campaign_ID_Table_Event_ID_Table.[Event_End_Date]
				, Pledge_Campaign_ID_Table_Pledge_Campaign_Type_ID_Table.[Campaign_Type]
				, Pledges.[Total_Pledge]
				, Pledge_Status_ID_Table.[Pledge_Status_ID]
				, Pledge_Status_ID_Table.[Pledge_Status]
				, Pledge_Campaign_ID_Table.[Start_Date] AS [Pledge_Campaign_Start_Date]
				, Pledge_Campaign_ID_Table.[End_Date] AS [Pledge_Campaign_End_Date]'
			,'Pledge_Campaign_ID_Table_Event_ID_Table_Event_Type_ID_Table.[Event_Type_ID] = 6
			  AND Pledge_Status_ID_Table.[Pledge_Status_ID] != 3')

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
