USE [MinistryPlatform]
GO

-- SOUTH AFRICA TRIP
DECLARE @PROGRAM_ID_SOUTH_AFRICA int = 139;
DECLARE @PLEDGE_CAMPAIGN_ID_SOUTH_AFRICA int = 178;
DECLARE @EVENT_ID_SOUTH_AFRICA int = 1599781;
DECLARE @TITLE_SOUTH_AFRICA varchar(29) = '2015 December GO South Africa';
DECLARE @SOUTH_AFRICA_FORM_ID int = 20;

-- NOLA TRIP --
DECLARE @PROGRAM_ID_NOLA int = 140;
DECLARE @PLEDGE_CAMPAIGN_ID_NOLA int = 179;
DECLARE @EVENT_ID_NOLA int = 1599782;
DECLARE @TITLE_NOLA varchar(254) = '2015 December GO NOLA';
DECLARE @NOLA_FORM_ID int = 21;

-- INDIA Trip --
DECLARE @PROGRAM_ID_INDIA int = 141;
DECLARE @PLEDGE_CAMPAIGN_ID_INDIA int = 180;
DECLARE @EVENT_ID_INDIA int = 1599783;
DECLARE @TITLE_INDIA varchar(254) = '2015 December GO INDIA';
DECLARE @INDIA_FORM_ID int = 23;

-- Nicaragua Trip --
DECLARE @PROGRAM_ID_NICARAGUA int = 142;
DECLARE @PLEDGE_CAMPAIGN_ID_NICARAGUA int = 181;
DECLARE @EVENT_ID_NICARAGUA int = 1599784;
DECLARE @TITLE_NICARAGUA varchar(254) = '2015 December GO NICARAGUA';
DECLARE @NICARAGUA_FORM_ID int = 22;



-- COMMON AMONG ALL TRIPS --
DECLARE @PROGRAM_START_DATE datetime = '2015-01-01';
DECLARE @START_DATE datetime = '2015-12-21';
DECLARE @END_DATE datetime = '2015-12-28';
DECLARE @PRIMARY_CONTACT_ID int = 768379;

------------------------
----- SOUTH AFRICA -----
------------------------
-- PROGRAM SOUTH AFRICA--
IF EXISTS (Select 1 FROM [dbo].[Programs] WHERE [dbo].[Programs].[Program_ID] = @PROGRAM_ID_SOUTH_AFRICA)
	BEGIN
		UPDATE [dbo].[Programs]
		   SET [Program_Name] = @TITLE_SOUTH_AFRICA
			  ,[Congregation_ID] = 1
			  ,[Ministry_ID] = 20
			  ,[Start_Date] = @PROGRAM_START_DATE
			  ,[End_Date] = @END_DATE
			  ,[Program_Type_ID] = 3
			  --,[Leadership_Team] = <Leadership_Team, int,>
			  ,[Primary_Contact] = 768379
			  --,[Priority_ID] = <Priority_ID, int,>
			  ,[On_Connection_Card] = 1
			  --,[Stewardship_Information] = <Stewardship_Information, [dbo].[dp_Separator],>
			  --,[Tax_Deductible_Donations] = <Tax_Deductible_Donations, bit,>
			  ,[Statement_Title] = @TITLE_SOUTH_AFRICA
			  ,[Statement_Header_ID] = 2
			  ,[Allow_Online_Giving] = 1
			  --,[Online_Sort_Order] = <Online_Sort_Order, smallint,>
			  --,[Pledge_Campaign_ID] = <Pledge_Campaign_ID, int,>
			  --,[Account_Number] = <Account_Number, nvarchar(25),>
			  --,[Default_Target_Event] = <Default_Target_Event, int,>
			  ,[On_Donation_Batch_Tool] = 1
			  ,[Domain_ID] = 1
			  --,[Available_Online] = <Available_Online, bit,>
			  --,[__ExternalFundID] = <__ExternalFundID, int,>
			  --,[Communication_ID] = <Communication_ID, int,>
		 WHERE [dbo].Programs.Program_ID = @PROGRAM_ID_SOUTH_AFRICA
	END
ELSE
	BEGIN
		SET IDENTITY_INSERT [dbo].[Programs] ON
		INSERT INTO [dbo].[Programs]
				   ( [Program_ID]
				   ,[Program_Name]
				   ,[Congregation_ID]
				   ,[Ministry_ID]
				   ,[Start_Date]
				   ,[End_Date]
				   ,[Program_Type_ID]
				   ,[Primary_Contact]
				   ,[Statement_Title]
				   ,[Statement_Header_ID]
				   ,[Allow_Online_Giving]
				   ,[On_Donation_Batch_Tool]
				   ,[Domain_ID])
			 VALUES
				   (@PROGRAM_ID_SOUTH_AFRICA
				   ,@TITLE_SOUTH_AFRICA
				   ,1
				   ,20
				   ,@PROGRAM_START_DATE
				   ,@END_DATE
				   ,3
				   ,@PRIMARY_CONTACT_ID
				   ,@TITLE_SOUTH_AFRICA
				   ,2
				   ,1
				   ,1
				   ,1)
			SET IDENTITY_INSERT [dbo].[Programs] OFF
	END

-- EVENT SOUTH AFRICA --
IF EXISTS (SELECT 1 FROM [dbo].[Events] WHERE [dbo].[Events].[Event_ID] = @EVENT_ID_SOUTH_AFRICA)
	BEGIN
		Update [dbo].[Events] SET 
			 Event_Title = @TITLE_SOUTH_AFRICA 
			,Event_Type_ID = 6
			,Congregation_ID = 5
			,Location_ID = 4
			,Program_ID = @PROGRAM_ID_SOUTH_AFRICA
			,Primary_Contact = @PRIMARY_CONTACT_ID
			,Minutes_for_Setup = 0
			,Event_Start_Date = @START_DATE
			,Event_End_Date = @END_DATE
			,Minutes_for_Cleanup = 0
			,Cancelled = 0
			,Visibility_Level_ID = 4
			,Featured_On_Calendar = 0
			,[Allow_Check-In] = 1
			,Ignore_Program_Groups = 0
			,Prohibit_Guests = 0
			,Domain_ID = 1
			,On_Donation_Batch_Tool = 1
			,Send_Reminder = 0
		WHERE Event_ID = @EVENT_ID_SOUTH_AFRICA
	END
ELSE
	BEGIN
	 SET IDENTITY_INSERT [dbo].[Events] ON
	 INSERT INTO [dbo].[Events]
           ([Event_ID]
		   ,[Event_Title]
           ,[Event_Type_ID]
           ,[Congregation_ID]
           ,[Location_ID]
           ,[Program_ID]
           ,[Primary_Contact]
           ,[Minutes_for_Setup]
           ,[Event_Start_Date]
           ,[Event_End_Date]
           ,[Minutes_for_Cleanup]
           ,[Cancelled]
           ,[Visibility_Level_ID]
           ,[Featured_On_Calendar]
           ,[Allow_Check-in]
           ,[Ignore_Program_Groups]
           ,[Prohibit_Guests]
           ,[Domain_ID]
           ,[On_Donation_Batch_Tool]
		   ,[Send_Reminder])
     VALUES
           (@Event_ID_SOUTH_AFRICA
		   ,@TITLE_SOUTH_AFRICA
           ,6
           ,5
           ,4
           ,@PROGRAM_ID_SOUTH_AFRICA
           ,@PRIMARY_CONTACT_ID
           ,0
           ,@START_DATE
           ,@END_DATE
           ,0
           ,0
           ,4
           ,0
           ,1
           ,0
           ,0
           ,1
		   ,1
		   ,1)
		SET IDENTITY_INSERT [dbo].[Events] OFF
	END

-- PLEDGE CAMPAIGNS SOUTH AFRICA --
IF EXISTS (Select 1 FROM [dbo].[Pledge_Campaigns] WHERE [dbo].[Pledge_Campaigns].[Pledge_Campaign_ID] = @PLEDGE_CAMPAIGN_ID_SOUTH_AFRICA)
	BEGIN
		UPDATE [dbo].[Pledge_Campaigns]
		SET [Campaign_Name] = @TITLE_SOUTH_AFRICA
			,[Nickname] = @TITLE_SOUTH_AFRICA
			,[Pledge_Campaign_Type_ID] = 2
			,[Description] = 'Going to South Africa to help those in need'
			,[Campaign_Goal] = 300000
			,[Start_Date] = @START_DATE
			,[End_Date] = @END_DATE
			,[Domain_ID] = 1
			-- ,[Event_ID] = <Event_ID, int,>
			,[Program_ID] = @PROGRAM_ID_SOUTH_AFRICA
			,[Registration_Start] = '2015-5-20'
			,[Registration_End] = '2015-12-21'
			,[Maximum_Registrants] = 300
			,[Youngest_Age_Allowed] = 13
			,[Registration_Deposit] = 200
			,[Fundraising_Goal] = 1000
			,[Registration_Form] = @SOUTH_AFRICA_FORM_ID
			,[Allow_Online_Pledge] = 1
			,[Show_On_My_Pledges] = 1
		WHERE [dbo].[Pledge_Campaigns].[Pledge_Campaign_ID] = @PLEDGE_CAMPAIGN_ID_SOUTH_AFRICA

	END
ELSE
	BEGIN
		SET IDENTITY_INSERT [dbo].[Pledge_Campaigns] ON
		INSERT INTO [dbo].[Pledge_Campaigns]
				   ([Pledge_Campaign_ID]
				   ,[Campaign_Name]
				   ,[Nickname]
				   ,[Pledge_Campaign_Type_ID]
				   ,[Description]
				   ,[Campaign_Goal]
				   ,[Start_Date]
				   ,[End_Date]
				   ,[Domain_ID]
				   ,[Program_ID]
				   ,[Registration_Start]
				   ,[Registration_End]
				   ,[Maximum_Registrants]
				   ,[Youngest_Age_Allowed]
				   ,[Registration_Deposit]
				   ,[Fundraising_Goal]
				   ,[Allow_Online_Pledge]
				   ,[Show_On_My_Pledges])
			 VALUES
				   (@PLEDGE_CAMPAIGN_ID_SOUTH_AFRICA
				   ,@TITLE_SOUTH_AFRICA
				   ,@TITLE_SOUTH_AFRICA
				   ,2
				   ,'Going to South Africa to help those in need'
				   ,300000
				   ,@START_DATE
				   ,@END_DATE
				   ,1
				   ,@PROGRAM_ID_SOUTH_AFRICA
				   ,'2015-5-20'
				   ,@START_DATE
				   ,300
				   ,13
				   ,200
				   ,1000
				   ,1
				   ,1)
				SET IDENTITY_INSERT [dbo].[Pledge_Campaigns] OFF
	END

----------------
----- NOLA -----
----------------
-- PROGRAM NOLA --
IF EXISTS (Select 1 FROM [dbo].[Programs] WHERE [dbo].[Programs].[Program_ID] = @PROGRAM_ID_NOLA)
	BEGIN
		UPDATE [dbo].[Programs]
		   SET [Program_Name] = @TITLE_NOLA
			  ,[Congregation_ID] = 1
			  ,[Ministry_ID] = 20
			  ,[Start_Date] = @PROGRAM_START_DATE
			  ,[End_Date] = @END_DATE
			  ,[Program_Type_ID] = 3
			  --,[Leadership_Team] = <Leadership_Team, int,>
			  ,[Primary_Contact] = @PRIMARY_CONTACT_ID
			  --,[Priority_ID] = <Priority_ID, int,>
			  ,[On_Connection_Card] = 1
			  --,[Stewardship_Information] = <Stewardship_Information, [dbo].[dp_Separator],>
			  --,[Tax_Deductible_Donations] = <Tax_Deductible_Donations, bit,>
			  ,[Statement_Title] = @TITLE_NOLA
			  ,[Statement_Header_ID] = 2
			  ,[Allow_Online_Giving] = 1
			  --,[Online_Sort_Order] = <Online_Sort_Order, smallint,>
			  --,[Pledge_Campaign_ID] = <Pledge_Campaign_ID, int,>
			  --,[Account_Number] = <Account_Number, nvarchar(25),>
			  --,[Default_Target_Event] = <Default_Target_Event, int,>
			  ,[On_Donation_Batch_Tool] = 1
			  ,[Domain_ID] = 1
			  --,[Available_Online] = <Available_Online, bit,>
			  --,[__ExternalFundID] = <__ExternalFundID, int,>
			  --,[Communication_ID] = <Communication_ID, int,>
		 WHERE [dbo].Programs.Program_ID = @PROGRAM_ID_NOLA
	END
ELSE
	BEGIN
		SET IDENTITY_INSERT [dbo].[Programs] ON
		INSERT INTO [dbo].[Programs]
				   ( [Program_ID]
				   ,[Program_Name]
				   ,[Congregation_ID]
				   ,[Ministry_ID]
				   ,[Start_Date]
				   ,[End_Date]
				   ,[Program_Type_ID]
				   ,[Primary_Contact]
				   ,[Statement_Title]
				   ,[Statement_Header_ID]
				   ,[Allow_Online_Giving]
				   ,[On_Donation_Batch_Tool]
				   ,[Domain_ID])
			 VALUES
				   (@PROGRAM_ID_NOLA
				   ,@TITLE_NOLA
				   ,1
				   ,20
				   ,@PROGRAM_START_DATE
				   ,@END_DATE
				   ,3
				   ,@PRIMARY_CONTACT_ID
				   ,@TITLE_NOLA
				   ,2
				   ,1
				   ,1
				   ,1)
			SET IDENTITY_INSERT [dbo].[Programs] OFF
	END

-- EVENT NOLA --
IF EXISTS (SELECT 1 FROM [dbo].[Events] WHERE [dbo].[Events].[Event_ID] = @EVENT_ID_NOLA)
	BEGIN
		Update [dbo].[Events] SET 
			 Event_Title = @TITLE_NOLA 
			,Event_Type_ID = 6
			,Congregation_ID = 5
			,Location_ID = 4
			,Program_ID = @PROGRAM_ID_NOLA
			,Primary_Contact = @PRIMARY_CONTACT_ID
			,Minutes_for_Setup = 0
			,Event_Start_Date = @START_DATE
			,Event_End_Date = @END_DATE
			,Minutes_for_Cleanup = 0
			,Cancelled = 0
			,Visibility_Level_ID = 4
			,Featured_On_Calendar = 0
			,[Allow_Check-In] = 1
			,Ignore_Program_Groups = 0
			,Prohibit_Guests = 0
			,Domain_ID = 1
			,On_Donation_Batch_Tool = 1
			,Send_Reminder = 0
		WHERE Event_ID = @EVENT_ID_NOLA
	END
ELSE
	BEGIN
	 SET IDENTITY_INSERT [dbo].[Events] ON
	 INSERT INTO [dbo].[Events]
           ([Event_ID]
		   ,[Event_Title]
           ,[Event_Type_ID]
           ,[Congregation_ID]
           ,[Location_ID]
           ,[Program_ID]
           ,[Primary_Contact]
           ,[Minutes_for_Setup]
           ,[Event_Start_Date]
           ,[Event_End_Date]
           ,[Minutes_for_Cleanup]
           ,[Cancelled]
           ,[Visibility_Level_ID]
           ,[Featured_On_Calendar]
           ,[Allow_Check-in]
           ,[Ignore_Program_Groups]
           ,[Prohibit_Guests]
           ,[Domain_ID]
           ,[On_Donation_Batch_Tool]
		   ,[Send_Reminder])
     VALUES
           (@Event_ID_NOLA
		   ,@TITLE_SOUTH_AFRICA
           ,6
           ,5
           ,4
           ,@PROGRAM_ID_NOLA
           ,@PRIMARY_CONTACT_ID
           ,0
           ,@START_DATE
           ,@END_DATE
           ,0
           ,0
           ,4
           ,0
           ,1
           ,0
           ,0
           ,1
		   ,1
		   ,1)
		SET IDENTITY_INSERT [dbo].[Events] OFF
	END

	-- PLEDGE CAMPAIGNS NOLA --
IF EXISTS (Select 1 FROM [dbo].[Pledge_Campaigns] WHERE [dbo].[Pledge_Campaigns].[Pledge_Campaign_ID] = @PLEDGE_CAMPAIGN_ID_NOLA)
	BEGIN
		UPDATE [dbo].[Pledge_Campaigns]
		SET [Campaign_Name] = @TITLE_NOLA
			,[Nickname] = @TITLE_NOLA
			,[Pledge_Campaign_Type_ID] = 2
			,[Description] = 'Going to NOLA to help those in need'
			,[Campaign_Goal] = 300000
			,[Start_Date] = @START_DATE
			,[End_Date] = @END_DATE
			,[Domain_ID] = 1
			-- ,[Event_ID] = <Event_ID, int,>
			,[Program_ID] = @PROGRAM_ID_NOLA
			,[Registration_Start] = '2015-5-20'
			,[Registration_End] = '2015-12-21'
			,[Maximum_Registrants] = 300
			,[Youngest_Age_Allowed] = 13
			,[Registration_Deposit] = 200
			,[Fundraising_Goal] = 1000
			,[Registration_Form] = @NOLA_FORM_ID
			,[Allow_Online_Pledge] = 1
			,[Show_On_My_Pledges] = 1
		WHERE [dbo].[Pledge_Campaigns].[Pledge_Campaign_ID] = @PLEDGE_CAMPAIGN_ID_NOLA

	END
ELSE
	BEGIN
		SET IDENTITY_INSERT [dbo].[Pledge_Campaigns] ON
		INSERT INTO [dbo].[Pledge_Campaigns]
				   ([Pledge_Campaign_ID]
				   ,[Campaign_Name]
				   ,[Nickname]
				   ,[Pledge_Campaign_Type_ID]
				   ,[Description]
				   ,[Campaign_Goal]
				   ,[Start_Date]
				   ,[End_Date]
				   ,[Domain_ID]
				   ,[Program_ID]
				   ,[Registration_Start]
				   ,[Registration_End]
				   ,[Maximum_Registrants]
				   ,[Youngest_Age_Allowed]
				   ,[Registration_Deposit]
				   ,[Fundraising_Goal]
				   ,[Allow_Online_Pledge]
				   ,[Show_On_My_Pledges])
			 VALUES
				   (@PLEDGE_CAMPAIGN_ID_NOLA
				   ,@TITLE_NOLA
				   ,@TITLE_NOLA
				   ,2
				   ,'Going to NOLA to help those in need'
				   ,300000
				   ,@START_DATE
				   ,@END_DATE
				   ,1
				   ,@PROGRAM_ID_NOLA
				   ,'2015-5-20'
				   ,@START_DATE
				   ,300
				   ,13
				   ,200
				   ,1000
				   ,1
				   ,1)
				SET IDENTITY_INSERT [dbo].[Pledge_Campaigns] OFF
	END

----------------
----- INDIA -----
----------------
-- PROGRAM INDIA --
IF EXISTS (Select 1 FROM [dbo].[Programs] WHERE [dbo].[Programs].[Program_ID] = @PROGRAM_ID_INDIA)
	BEGIN
		UPDATE [dbo].[Programs]
		   SET [Program_Name] = @TITLE_INDIA
			  ,[Congregation_ID] = 1
			  ,[Ministry_ID] = 20
			  ,[Start_Date] = @PROGRAM_START_DATE
			  ,[End_Date] = @END_DATE
			  ,[Program_Type_ID] = 3
			  --,[Leadership_Team] = <Leadership_Team, int,>
			  ,[Primary_Contact] = @PRIMARY_CONTACT_ID
			  --,[Priority_ID] = <Priority_ID, int,>
			  ,[On_Connection_Card] = 1
			  --,[Stewardship_Information] = <Stewardship_Information, [dbo].[dp_Separator],>
			  --,[Tax_Deductible_Donations] = <Tax_Deductible_Donations, bit,>
			  ,[Statement_Title] = @TITLE_INDIA
			  ,[Statement_Header_ID] = 2
			  ,[Allow_Online_Giving] = 1
			  --,[Online_Sort_Order] = <Online_Sort_Order, smallint,>
			  --,[Pledge_Campaign_ID] = <Pledge_Campaign_ID, int,>
			  --,[Account_Number] = <Account_Number, nvarchar(25),>
			  --,[Default_Target_Event] = <Default_Target_Event, int,>
			  ,[On_Donation_Batch_Tool] = 1
			  ,[Domain_ID] = 1
			  --,[Available_Online] = <Available_Online, bit,>
			  --,[__ExternalFundID] = <__ExternalFundID, int,>
			  --,[Communication_ID] = <Communication_ID, int,>
		 WHERE [dbo].Programs.Program_ID = @PROGRAM_ID_INDIA
	END
ELSE
	BEGIN
		SET IDENTITY_INSERT [dbo].[Programs] ON
		INSERT INTO [dbo].[Programs]
				   ( [Program_ID]
				   ,[Program_Name]
				   ,[Congregation_ID]
				   ,[Ministry_ID]
				   ,[Start_Date]
				   ,[End_Date]
				   ,[Program_Type_ID]
				   ,[Primary_Contact]
				   ,[Statement_Title]
				   ,[Statement_Header_ID]
				   ,[Allow_Online_Giving]
				   ,[On_Donation_Batch_Tool]
				   ,[Domain_ID])
			 VALUES
				   (@PROGRAM_ID_INDIA
				   ,@TITLE_INDIA
				   ,1
				   ,20
				   ,@PROGRAM_START_DATE
				   ,@END_DATE
				   ,3
				   ,@PRIMARY_CONTACT_ID
				   ,@TITLE_INDIA
				   ,2
				   ,1
				   ,1
				   ,1)
			SET IDENTITY_INSERT [dbo].[Programs] OFF
	END

-- EVENT INDIA --
IF EXISTS (SELECT 1 FROM [dbo].[Events] WHERE [dbo].[Events].[Event_ID] = @EVENT_ID_INDIA)
	BEGIN
		Update [dbo].[Events] SET 
			 Event_Title = @TITLE_INDIA 
			,Event_Type_ID = 6
			,Congregation_ID = 5
			,Location_ID = 4
			,Program_ID = @PROGRAM_ID_INDIA
			,Primary_Contact = @PRIMARY_CONTACT_ID
			,Minutes_for_Setup = 0
			,Event_Start_Date = @START_DATE
			,Event_End_Date = @END_DATE
			,Minutes_for_Cleanup = 0
			,Cancelled = 0
			,Visibility_Level_ID = 4
			,Featured_On_Calendar = 0
			,[Allow_Check-In] = 1
			,Ignore_Program_Groups = 0
			,Prohibit_Guests = 0
			,Domain_ID = 1
			,On_Donation_Batch_Tool = 1
			,Send_Reminder = 0
		WHERE Event_ID = @EVENT_ID_INDIA
	END
ELSE
	BEGIN
	 SET IDENTITY_INSERT [dbo].[Events] ON
	 INSERT INTO [dbo].[Events]
           ([Event_ID]
		   ,[Event_Title]
           ,[Event_Type_ID]
           ,[Congregation_ID]
           ,[Location_ID]
           ,[Program_ID]
           ,[Primary_Contact]
           ,[Minutes_for_Setup]
           ,[Event_Start_Date]
           ,[Event_End_Date]
           ,[Minutes_for_Cleanup]
           ,[Cancelled]
           ,[Visibility_Level_ID]
           ,[Featured_On_Calendar]
           ,[Allow_Check-in]
           ,[Ignore_Program_Groups]
           ,[Prohibit_Guests]
           ,[Domain_ID]
           ,[On_Donation_Batch_Tool]
		   ,[Send_Reminder])
     VALUES
           (@Event_ID_INDIA
		   ,@TITLE_SOUTH_AFRICA
           ,6
           ,5
           ,4
           ,@PROGRAM_ID_INDIA
           ,@PRIMARY_CONTACT_ID
           ,0
           ,@START_DATE
           ,@END_DATE
           ,0
           ,0
           ,4
           ,0
           ,1
           ,0
           ,0
           ,1
		   ,1
		   ,1)
		SET IDENTITY_INSERT [dbo].[Events] OFF
	END

	-- PLEDGE CAMPAIGNS INDIA --
IF EXISTS (Select 1 FROM [dbo].[Pledge_Campaigns] WHERE [dbo].[Pledge_Campaigns].[Pledge_Campaign_ID] = @PLEDGE_CAMPAIGN_ID_INDIA)
	BEGIN
		UPDATE [dbo].[Pledge_Campaigns]
		SET [Campaign_Name] = @TITLE_INDIA
			,[Nickname] = @TITLE_INDIA
			,[Pledge_Campaign_Type_ID] = 2
			,[Description] = 'Going to INDIA to help those in need'
			,[Campaign_Goal] = 300000
			,[Start_Date] = @START_DATE
			,[End_Date] = @END_DATE
			,[Domain_ID] = 1
			-- ,[Event_ID] = <Event_ID, int,>
			,[Program_ID] = @PROGRAM_ID_INDIA
			,[Registration_Start] = '2015-5-20'
			,[Registration_End] = '2015-12-21'
			,[Maximum_Registrants] = 300
			,[Youngest_Age_Allowed] = 13
			,[Registration_Deposit] = 200
			,[Fundraising_Goal] = 1000
			,[Registration_Form] = @INDIA_FORM_ID
			,[Allow_Online_Pledge] = 1
			,[Show_On_My_Pledges] = 1
		WHERE [dbo].[Pledge_Campaigns].[Pledge_Campaign_ID] = @PLEDGE_CAMPAIGN_ID_INDIA

	END
ELSE
	BEGIN
		SET IDENTITY_INSERT [dbo].[Pledge_Campaigns] ON
		INSERT INTO [dbo].[Pledge_Campaigns]
				   ([Pledge_Campaign_ID]
				   ,[Campaign_Name]
				   ,[Nickname]
				   ,[Pledge_Campaign_Type_ID]
				   ,[Description]
				   ,[Campaign_Goal]
				   ,[Start_Date]
				   ,[End_Date]
				   ,[Domain_ID]
				   ,[Program_ID]
				   ,[Registration_Start]
				   ,[Registration_End]
				   ,[Maximum_Registrants]
				   ,[Youngest_Age_Allowed]
				   ,[Registration_Deposit]
				   ,[Fundraising_Goal]
				   ,[Allow_Online_Pledge]
				   ,[Show_On_My_Pledges])
			 VALUES
				   (@PLEDGE_CAMPAIGN_ID_INDIA
				   ,@TITLE_INDIA
				   ,@TITLE_INDIA
				   ,2
				   ,'Going to INDIA to help those in need'
				   ,300000
				   ,@START_DATE
				   ,@END_DATE
				   ,1
				   ,@PROGRAM_ID_INDIA
				   ,'2015-5-20'
				   ,@START_DATE
				   ,300
				   ,13
				   ,200
				   ,1000
				   ,1
				   ,1)
				SET IDENTITY_INSERT [dbo].[Pledge_Campaigns] OFF
	END


----------------
----- NICARAGUA -----
----------------
-- PROGRAM NICARAGUA --
IF EXISTS (Select 1 FROM [dbo].[Programs] WHERE [dbo].[Programs].[Program_ID] = @PROGRAM_ID_NICARAGUA)
	BEGIN
		UPDATE [dbo].[Programs]
		   SET [Program_Name] = @TITLE_NICARAGUA
			  ,[Congregation_ID] = 1
			  ,[Ministry_ID] = 20
			  ,[Start_Date] = @PROGRAM_START_DATE
			  ,[End_Date] = @END_DATE
			  ,[Program_Type_ID] = 3
			  --,[Leadership_Team] = <Leadership_Team, int,>
			  ,[Primary_Contact] = @PRIMARY_CONTACT_ID
			  --,[Priority_ID] = <Priority_ID, int,>
			  ,[On_Connection_Card] = 1
			  --,[Stewardship_Information] = <Stewardship_Information, [dbo].[dp_Separator],>
			  --,[Tax_Deductible_Donations] = <Tax_Deductible_Donations, bit,>
			  ,[Statement_Title] = @TITLE_NICARAGUA
			  ,[Statement_Header_ID] = 2
			  ,[Allow_Online_Giving] = 1
			  --,[Online_Sort_Order] = <Online_Sort_Order, smallint,>
			  --,[Pledge_Campaign_ID] = <Pledge_Campaign_ID, int,>
			  --,[Account_Number] = <Account_Number, nvarchar(25),>
			  --,[Default_Target_Event] = <Default_Target_Event, int,>
			  ,[On_Donation_Batch_Tool] = 1
			  ,[Domain_ID] = 1
			  --,[Available_Online] = <Available_Online, bit,>
			  --,[__ExternalFundID] = <__ExternalFundID, int,>
			  --,[Communication_ID] = <Communication_ID, int,>
		 WHERE [dbo].Programs.Program_ID = @PROGRAM_ID_NICARAGUA
	END
ELSE
	BEGIN
		SET IDENTITY_INSERT [dbo].[Programs] ON
		INSERT INTO [dbo].[Programs]
				   ( [Program_ID]
				   ,[Program_Name]
				   ,[Congregation_ID]
				   ,[Ministry_ID]
				   ,[Start_Date]
				   ,[End_Date]
				   ,[Program_Type_ID]
				   ,[Primary_Contact]
				   ,[Statement_Title]
				   ,[Statement_Header_ID]
				   ,[Allow_Online_Giving]
				   ,[On_Donation_Batch_Tool]
				   ,[Domain_ID])
			 VALUES
				   (@PROGRAM_ID_NICARAGUA
				   ,@TITLE_NICARAGUA
				   ,1
				   ,20
				   ,@PROGRAM_START_DATE
				   ,@END_DATE
				   ,3
				   ,@PRIMARY_CONTACT_ID
				   ,@TITLE_NICARAGUA
				   ,2
				   ,1
				   ,1
				   ,1)
			SET IDENTITY_INSERT [dbo].[Programs] OFF
	END

-- EVENT NICARAGUA --
IF EXISTS (SELECT 1 FROM [dbo].[Events] WHERE [dbo].[Events].[Event_ID] = @EVENT_ID_NICARAGUA)
	BEGIN
		Update [dbo].[Events] SET 
			 Event_Title = @TITLE_NICARAGUA 
			,Event_Type_ID = 6
			,Congregation_ID = 5
			,Location_ID = 4
			,Program_ID = @PROGRAM_ID_NICARAGUA
			,Primary_Contact = @PRIMARY_CONTACT_ID
			,Minutes_for_Setup = 0
			,Event_Start_Date = @START_DATE
			,Event_End_Date = @END_DATE
			,Minutes_for_Cleanup = 0
			,Cancelled = 0
			,Visibility_Level_ID = 4
			,Featured_On_Calendar = 0
			,[Allow_Check-In] = 1
			,Ignore_Program_Groups = 0
			,Prohibit_Guests = 0
			,Domain_ID = 1
			,On_Donation_Batch_Tool = 1
			,Send_Reminder = 0
		WHERE Event_ID = @EVENT_ID_NICARAGUA
	END
ELSE
	BEGIN
	 SET IDENTITY_INSERT [dbo].[Events] ON
	 INSERT INTO [dbo].[Events]
           ([Event_ID]
		   ,[Event_Title]
           ,[Event_Type_ID]
           ,[Congregation_ID]
           ,[Location_ID]
           ,[Program_ID]
           ,[Primary_Contact]
           ,[Minutes_for_Setup]
           ,[Event_Start_Date]
           ,[Event_End_Date]
           ,[Minutes_for_Cleanup]
           ,[Cancelled]
           ,[Visibility_Level_ID]
           ,[Featured_On_Calendar]
           ,[Allow_Check-in]
           ,[Ignore_Program_Groups]
           ,[Prohibit_Guests]
           ,[Domain_ID]
           ,[On_Donation_Batch_Tool]
		   ,[Send_Reminder])
     VALUES
           (@Event_ID_NICARAGUA
		   ,@TITLE_SOUTH_AFRICA
           ,6
           ,5
           ,4
           ,@PROGRAM_ID_NICARAGUA
           ,@PRIMARY_CONTACT_ID
           ,0
           ,@START_DATE
           ,@END_DATE
           ,0
           ,0
           ,4
           ,0
           ,1
           ,0
           ,0
           ,1
		   ,1
		   ,1)
		SET IDENTITY_INSERT [dbo].[Events] OFF
	END

	-- PLEDGE CAMPAIGNS NICARAGUA --
IF EXISTS (Select 1 FROM [dbo].[Pledge_Campaigns] WHERE [dbo].[Pledge_Campaigns].[Pledge_Campaign_ID] = @PLEDGE_CAMPAIGN_ID_NICARAGUA)
	BEGIN
		UPDATE [dbo].[Pledge_Campaigns]
		SET [Campaign_Name] = @TITLE_NICARAGUA
			,[Nickname] = @TITLE_NICARAGUA
			,[Pledge_Campaign_Type_ID] = 2
			,[Description] = 'Going to NICARAGUA to help those in need'
			,[Campaign_Goal] = 300000
			,[Start_Date] = @START_DATE
			,[End_Date] = @END_DATE
			,[Domain_ID] = 1
			-- ,[Event_ID] = <Event_ID, int,>
			,[Program_ID] = @PROGRAM_ID_NICARAGUA
			,[Registration_Start] = '2015-5-20'
			,[Registration_End] = '2015-12-21'
			,[Maximum_Registrants] = 300
			,[Youngest_Age_Allowed] = 13
			,[Registration_Deposit] = 200
			,[Fundraising_Goal] = 1000
			,[Registration_Form] = @NICARAGUA_FORM_ID
			,[Allow_Online_Pledge] = 1
			,[Show_On_My_Pledges] = 1
		WHERE [dbo].[Pledge_Campaigns].[Pledge_Campaign_ID] = @PLEDGE_CAMPAIGN_ID_NICARAGUA

	END
ELSE
	BEGIN
		SET IDENTITY_INSERT [dbo].[Pledge_Campaigns] ON
		INSERT INTO [dbo].[Pledge_Campaigns]
				   ([Pledge_Campaign_ID]
				   ,[Campaign_Name]
				   ,[Nickname]
				   ,[Pledge_Campaign_Type_ID]
				   ,[Description]
				   ,[Campaign_Goal]
				   ,[Start_Date]
				   ,[End_Date]
				   ,[Domain_ID]
				   ,[Program_ID]
				   ,[Registration_Start]
				   ,[Registration_End]
				   ,[Maximum_Registrants]
				   ,[Youngest_Age_Allowed]
				   ,[Registration_Deposit]
				   ,[Fundraising_Goal]
				   ,Registration_Form
				   ,[Allow_Online_Pledge]
				   ,[Show_On_My_Pledges])
			 VALUES
				   (@PLEDGE_CAMPAIGN_ID_NICARAGUA
				   ,@TITLE_NICARAGUA
				   ,@TITLE_NICARAGUA
				   ,2
				   ,'Going to NICARAGUA to help those in need'
				   ,300000
				   ,@START_DATE
				   ,@END_DATE
				   ,1
				   ,@PROGRAM_ID_NICARAGUA
				   ,'2015-5-20'
				   ,@START_DATE
				   ,300
				   ,13
				   ,200
				   ,1000
				   ,@NICARAGUA_FORM_ID
				   ,1
				   ,1)
				SET IDENTITY_INSERT [dbo].[Pledge_Campaigns] OFF
	END
