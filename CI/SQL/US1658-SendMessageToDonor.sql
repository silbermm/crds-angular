USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[Donation_Distributions]    Script Date: 9/28/2015 12:50:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Message_Sent' AND Object_ID = Object_ID(N'dbo.Donation_Distributions'))
BEGIN
    ALTER TABLE [dbo].[Donation_Distributions] ADD [Message_Sent] bit null
END


UPDATE [dbo].[dp_Page_Views]
   SET 
      [Field_List] = 'Pledge_ID_Table_Donor_ID_Table_Contact_ID_Table.[Contact_ID] AS [Contact ID]
, Pledge_ID_Table_Pledge_Campaign_ID_Table_Event_ID_Table_Event_Type_ID_Table.[Event_Type_ID] AS [Event Type ID]
, Pledge_ID_Table_Pledge_Campaign_ID_Table_Event_ID_Table.[Event_ID] AS [Event ID]
, Pledge_ID_Table_Pledge_Campaign_ID_Table_Event_ID_Table.[Event_Title] AS [Event Title]
, Pledge_ID_Table_Pledge_Campaign_ID_Table_Event_ID_Table.[Event_Start_Date] AS [Event Start Date]
, Pledge_ID_Table_Pledge_Campaign_ID_Table_Event_ID_Table.[Event_End_Date] AS [Event End Date]
, Pledge_ID_Table.[Total_Pledge] AS [Total Pledge]
, Pledge_ID_Table_Pledge_Campaign_ID_Table.[Start_Date] AS [Start Date]
, Pledge_ID_Table_Pledge_Campaign_ID_Table.[End_Date] AS [End Date]
, Donation_ID_Table_Donor_ID_Table.[Donor_ID] AS [Donor ID]
, Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.[Nickname] AS [Nickname]
, Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.[First_Name] AS [First Name]
, Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.[Last_Name] AS [Last Name]
, Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.[Email_Address] AS [Email Address]
, Donation_ID_Table.[Donation_Date] AS [Donation Date]
, Donation_Distributions.[Amount] AS [Amount]
, Donation_ID_Table.[Anonymous] AS [Anonymous]
, Donation_ID_Table.[Registered_Donor] AS [Registered Donor]
, Donation_Distributions.[Message_Sent] AS [Message Sent]
, Donation_ID_Table_Payment_Type_ID_Table.[Payment_Type_ID] AS [Payment Type ID]'
 WHERE [Page_View_ID] = 92179
GO