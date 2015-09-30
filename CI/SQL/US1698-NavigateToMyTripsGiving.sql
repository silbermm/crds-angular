USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Page_Views] WHERE [View_Title] = 'MyTripsGiving')
INSERT INTO [dbo].[dp_Page_Views]
           ([View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           ('MyTripsGiving'
           ,296
           ,'View Used for MyTrips page'
           ,'Pledge_ID_Table_Donor_ID_Table_Contact_ID_Table.[Contact_ID] AS [Contact ID]
				, Pledge_ID_Table_Pledge_Campaign_ID_Table_Event_ID_Table_Event_Type_ID_Table.[Event_Type_ID] AS [Event Type ID]
				, Pledge_ID_Table_Pledge_Campaign_ID_Table_Event_ID_Table.[Event_ID] AS [Event ID]
				, Pledge_ID_Table_Pledge_Campaign_ID_Table_Event_ID_Table.[Event_Title] AS [Event Title]
				, Pledge_ID_Table_Pledge_Campaign_ID_Table_Event_ID_Table.[Event_Start_Date] AS [Event Start Date]
				, Pledge_ID_Table_Pledge_Campaign_ID_Table_Event_ID_Table.[Event_End_Date] AS [Event End Date]
				, Pledge_ID_Table.[Total_Pledge] AS [Total Pledge]
				, Pledge_ID_Table_Pledge_Campaign_ID_Table.[Start_Date] AS [Start Date]
				, Pledge_ID_Table_Pledge_Campaign_ID_Table.[End_Date] AS [End Date]
				, Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.[Nickname] AS [Nickname]
				, Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.[First_Name] AS [First Name]
				, Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.[Last_Name] AS [Last Name]
				, Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.[Email_Address] AS [Email Address]
				, Donation_ID_Table.[Donation_Date] AS [Donation Date]
				, Donation_Distributions.[Amount] AS [Amount]
				, Donation_ID_Table.[Anonymous] AS [Anonymous]
				, Donation_ID_Table.[Registered_Donor] AS [Registered Donor]'
		    ,'(Pledge_ID_Table_Pledge_Campaign_ID_Table_Event_ID_Table_Event_Type_ID_Table.[Event_Type_ID] = 6) AND (Donation_ID_Table_Donation_Status_ID_Table.[Display_On_MyTrips] = 1) AND (DATEADD(day, 90, Pledge_ID_Table_Pledge_Campaign_ID_Table_Event_ID_Table.[Event_End_Date]) >= GETDATE())')
GO