USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Programs] ON
GO

INSERT INTO [dbo].[Programs]
	([Program_ID]
	,[Program_Name]
	,[Congregation_ID]
	,[Ministry_ID]
	,[Start_Date]
	,[End_Date]
	,[Program_Type_ID]
	,[Leadership_Team]
	,[Primary_Contact]
	,[Priority_ID]
	,[On_Connection_Card]
	,[Stewardship_Information]
	,[Tax_Deductible_Donations]
	,[Statement_Title]
	,[Statement_Header_ID]
	,[Allow_Online_Giving]
	,[Online_Sort_Order]
	,[Pledge_Campaign_ID]
	,[Account_Number]
	,[Default_Target_Event]
	,[On_Donation_Batch_Tool]
	,[Domain_ID]
	,[Available_Online]
	,[__ExternalFundID]
	,[Communication_ID])
VALUES
	(127
	,'Payment Processor Fees'
	,5
	,3
	,CAST('20150716 00:00:00.000' as DATETIME)
	,NULL
	,1
	,NULL
	,2760496
	,NULL
	,NULL
	,NULL
	,0
	,'Payment Processor Fees'
	,6
	,0
	,NULL
	,NULL
	,NULL
	,NULL
	,0
	,1
	,NULL
	,NULL
	,NULL);

SET IDENTITY_INSERT [dbo].[Programs] OFF
GO

SET IDENTITY_INSERT [dbo].[GL_Account_Mapping] ON
GO

INSERT INTO [dbo].[GL_Account_Mapping]
	([Domain_ID]
	,[Program_ID]
	,[Congregation_ID]
	,[GL_Account]
	,[GL_Account_Mapping_ID])
VALUES
	(1
	,127
	,5
	,'70303-030-01'
	,57);

SET IDENTITY_INSERT [dbo].[GL_Account_Mapping] OFF
GO
