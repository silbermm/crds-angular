USE [MinistryPlatform]
GO


-- PROGRAM --

SET IDENTITY_INSERT [dbo].[Programs] ON
GO

IF EXISTS (Select 1 FROM [dbo].[Programs] WHERE [dbo].[Programs].[Program_ID] = 139)
	BEGIN
		UPDATE [dbo].[Programs]
   SET [Program_Name] = '2015 December GO South Africa'
      ,[Congregation_ID] = 1
      ,[Ministry_ID] = 20
      ,[Start_Date] = '2015-5-20'
      ,[End_Date] = '2015-12-28'
      ,[Program_Type_ID] = 3
      --,[Leadership_Team] = <Leadership_Team, int,>
      ,[Primary_Contact] = 768379
      --,[Priority_ID] = <Priority_ID, int,>
      ,[On_Connection_Card] = 1
      --,[Stewardship_Information] = <Stewardship_Information, [dbo].[dp_Separator],>
      --,[Tax_Deductible_Donations] = <Tax_Deductible_Donations, bit,>
      ,[Statement_Title] = '2015 December GO South Africa'
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
 WHERE [dbo].Programs.Program_ID = 139
	END
ELSE
	BEGIN


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
				   (139
				   ,'2015 December GO South Africa'
				   ,1
				   ,20
				   ,'2015-5-20'
				   ,'2015-12-28'
				   ,3
				   ,768379
				   ,'2015 December GO South Africa'
				   ,2
				   ,1
				   ,1
				   ,1)
	END

SET IDENTITY_INSERT [dbo].[Programs] OFF
GO


-- PLEDGE CAMPAIGNS --

SET IDENTITY_INSERT [dbo].[Pledge_Campaigns] ON
GO

IF EXISTS (Select 1 FROM [dbo].[Pledge_Campaigns] WHERE [dbo].[Pledge_Campaigns].[Pledge_Campaign_ID] = 178)
	BEGIN
		UPDATE [dbo].[Pledge_Campaigns]
		SET [Campaign_Name] = '2015 December GO South Africa'
			,[Nickname] = 'South Africa December 2015'
			,[Pledge_Campaign_Type_ID] = 2
			,[Description] = 'Going to South Africa to help those in need'
			,[Campaign_Goal] = 300000
			,[Start_Date] = '12/21/15'
			,[End_Date] = '2015-12-28'
			,[Domain_ID] = 1
			-- ,[Event_ID] = <Event_ID, int,>
			,[Program_ID] = 139
			,[Registration_Start] = '2015-5-20'
			,[Registration_End] = '2015-12-21'
			,[Maximum_Registrants] = 300
			,[Youngest_Age_Allowed] = 13
			,[Registration_Deposit] = 200
			,[Fundraising_Goal] = 1000
			,[Registration_Form] = 20
			,[Allow_Online_Pledge] = 1
			,[Show_On_My_Pledges] = 1
		WHERE [dbo].[Pledge_Campaigns].[Pledge_Campaign_ID] = 178

	END
ELSE
	BEGIN
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
				   ,[Event_ID]
				   ,[Registration_Start]
				   ,[Registration_End]
				   ,[Maximum_Registrants]
				   ,[Youngest_Age_Allowed]
				   ,[Registration_Deposit]
				   ,[Fundraising_Goal]
				   ,[Allow_Online_Pledge]
				   ,[Show_On_My_Pledges])
			 VALUES
				   (178,
				   'South Africa December 2015'
				   ,'South Africa December 2015'
				   ,2
				   ,'Going to South Africa to help those in need'
				   ,300000
				   ,'2015-12-21'
				   ,'2015-12-28'
				   ,1
				   ,139
				   ,'2015-5-20'
				   ,'2015-12-21'
				   ,300
				   ,13
				   ,200
				   ,1000
				   ,1
				   ,1)
	END


SET IDENTITY_INSERT [dbo].[Pledge_Campaigns] OFF
GO
