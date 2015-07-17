USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Page_Views]
	([View_Title]
	,[Page_ID]
	,[Description]
	,[Field_List]
	,[View_Clause]
	,[Order_By]
	,[User_ID]
	,[User_Group_ID])
VALUES
	('Declined Donations (All)'
	,297
	,'View all Donations that were Declined'
	,'Donations.[Donation_Date] AS [Donation Date]
		, Donor_ID_Table_Contact_ID_Table.[Display_Name] AS [Donor Display Name]
		, Donor_ID_Table_Contact_ID_Table.[First_Name] AS [Donor First Name]
		, Donor_ID_Table_Contact_ID_Table.[Last_Name] AS [Donor Last Name]
		, Donor_ID_Table_Contact_ID_Table.[Email_Address] AS [Donor Email Address]
		, Donations.[Donation_Amount] AS [Donation Amount]
		, Payment_Type_ID_Table.[Payment_Type] AS [Payment Type]
		, Donations.[Donation_Status_Date] AS [Decline Date]
		, Donations.[Donation_Status_Notes] AS [Status Reason]
		, Donations.[Transaction_Code] AS [Transaction Code]
		, Donor_ID_Table.[Processor_ID] AS [Processor ID]'
	,'Donation_Status_ID_Table.[Donation_Status] = ''Declined'''
	,'Donations.[Donation_Status_Date] DESC'
	,NULL
	,NULL);
GO

INSERT INTO [dbo].[dp_Page_Views]
	([View_Title]
	,[Page_ID]
	,[Description]
	,[Field_List]
	,[View_Clause]
	,[Order_By]
	,[User_ID]
	,[User_Group_ID])
VALUES
	('Declined Donations (Last 18 Months)'
	,297
	,'View all Donations that were Declined in the past 18 months'
	,'Donations.[Donation_Date] AS [Donation Date]
		, Donor_ID_Table_Contact_ID_Table.[Display_Name] AS [Donor Display Name]
		, Donor_ID_Table_Contact_ID_Table.[First_Name] AS [Donor First Name]
		, Donor_ID_Table_Contact_ID_Table.[Last_Name] AS [Donor Last Name]
		, Donor_ID_Table_Contact_ID_Table.[Email_Address] AS [Donor Email Address]
		, Donations.[Donation_Amount] AS [Donation Amount]
		, Payment_Type_ID_Table.[Payment_Type] AS [Payment Type]
		, Donations.[Donation_Status_Date] AS [Decline Date]
		, Donations.[Donation_Status_Notes] AS [Status Reason]
		, Donations.[Transaction_Code] AS [Transaction Code]
		, Donor_ID_Table.[Processor_ID] AS [Processor ID]'
	,'Donation_Status_ID_Table.[Donation_Status] = ''Declined''
		AND Donations.[Donation_Date] >= DATEADD(month, -18, GETDATE())'
	,'Donations.[Donation_Status_Date] DESC'
	,NULL
	,NULL);
GO
