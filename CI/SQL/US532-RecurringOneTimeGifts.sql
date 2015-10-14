USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
	SET [Default_Field_List] = 'Donations.Donation_Date
		,Donor_ID_Table_Contact_ID_Table.Display_Name
		,Donor_ID_Table_Contact_ID_Table.Nickname
		,Donor_ID_Table_Contact_ID_Table.First_Name
		,Donations.Donation_Amount
		,Payment_Type_ID_Table.Payment_Type
		,Item_Number
		,Transaction_Code
		,Subscription_Code
		,Batch_ID_Table.Batch_ID
		,Batch_ID_Table.Setup_Date
		,Donations.Registered_Donor
		,Processor_Fee_Amount
		,Donor_ID_Table.Donor_ID
		,Donation_Status_Notes,Donations.Check_Scanner_Batch,Is_Recurring_Gift'
	WHERE [Page_ID] = 297

	UPDATE [dbo].[dp_Page_Views]
	SET [Field_List] = 'Donations.Donation_Date
		,Donor_ID_Table_Contact_ID_Table.Display_Name
		,Donor_ID_Table_Contact_ID_Table.Nickname
		,Donor_ID_Table_Contact_ID_Table.First_Name
		,Donations.Donation_Amount
		,ISNULL((SELECT Sum(Amount) FROM Donation_Distributions DD WHERE DD.Donation_ID = Donations.Donation_ID),0) AS Distribution_Amount
		,Payment_Type_ID_Table.Payment_Type
		,Item_Number
		,Donations.Transaction_Code
		,Donations.Subscription_Code
		,Batch_ID_Table.Batch_ID
		,Batch_ID_Table.Setup_Date
		,Donations.Registered_Donor,Donations.Check_Scanner_Batch,Is_Recurring_Gift'
	WHERE [Page_View_ID] = 357

	USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
	SET [Field_List] = 'Donations.[Donation_Date]
		,Donor_ID_Table_Contact_ID_Table.[Display_Name]
		,Donor_ID_Table_Contact_ID_Table.[First_Name] 
		,Donor_ID_Table_Contact_ID_Table.[Last_Name]
		,Donor_ID_Table_Contact_ID_Table.[Email_Address] AS [Donor Email Address]
		,Donations.[Donation_Amount] 
		,Payment_Type_ID_Table.[Payment_Type]
		,Donations.[Donation_Status_Date] AS [Decline Date]
		,Donations.[Donation_Status_Notes] AS [Status Reason]
		,Donations.[Transaction_Code] 
		,Donor_ID_Table.[Processor_ID],Is_Recurring_Gift'
	WHERE [Page_View_ID] = 92174

UPDATE [dbo].[dp_Page_Views]
	SET [Field_List] = 'Donations.[Donation_Date]
		,Donor_ID_Table_Contact_ID_Table.[Display_Name] AS [Donor Display Name]
		,Donor_ID_Table_Contact_ID_Table.[First_Name] AS [Donor First Name]
		,Donor_ID_Table_Contact_ID_Table.[Last_Name] AS [Donor Last Name]
		,Donor_ID_Table_Contact_ID_Table.[Email_Address] AS [Donor Email Address]
		,Donations.[Donation_Amount]
		,Payment_Type_ID_Table.[Payment_Type]
		,Donations.[Donation_Status_Date] AS [Decline Date]
		,Donations.[Donation_Status_Notes] AS [Status Reason]
		,Donations.[Transaction_Code] 
		,Donor_ID_Table.[Processor_ID],Is_Recurring_Gift'
	WHERE [Page_View_ID] = 92175

UPDATE [dbo].[dp_Page_Views]
	SET [Field_List] = 'Donation_Date,Donor_ID_Table_Contact_ID_Table.Display_Name,
			 Donor_ID_Table_Contact_ID_Table.Nickname,
			 Donor_ID_Table_Contact_ID_Table.Email_Address,
			 Donation_Amount,
			 Payment_Type_ID_Table.Payment_Type,
		     Donation_Status_ID_Table.Donation_Status,
			 Donations.Donation_Status_Date,Donations.Check_Scanner_Batch,Is_Recurring_Gift'
	WHERE [Page_View_ID] = 92199

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]          
           ,[View_Clause] )
     VALUES
           (2189
		   ,'Recurring Gifts'
		   ,297
           ,'This view will display donations associated with a recurring gift'
           ,'Is_Recurring_Gift = 1')
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO


SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]          
           ,[View_Clause] )
     VALUES
           (2190
		   ,'One-Time Gifts'
		   ,297
           ,'This view will display donations associated with a one-time gift'
           ,'Is_Recurring_Gift = 0')
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO


