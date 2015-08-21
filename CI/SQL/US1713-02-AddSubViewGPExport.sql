USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON
GO

INSERT INTO [dbo].[dp_Sub_Page_Views]
           ([Sub_Page_View_ID]
           ,[View_Title]
           ,[Sub_Page_ID]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (101
           ,'GP Export'
           ,272
           ,'Donation_ID_Table.[Donation_ID] AS [Donation ID]
, Donation_ID_Table_Batch_ID_Table.[Batch_Name] AS [Batch Name]
, Donation_ID_Table_Batch_ID_Table.[Batch_Name] AS [Batch Name]
, Donation_ID_Table.[Donation_Date] AS [Donation Date]
, Donation_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_Date] AS [Deposit Date]
, Donation_ID_Table.[Donation_Amount] AS [Donation Amount]
, Donation_Distributions.[Amount] AS [Amount]
, Program_ID_Table.[Program_ID] AS [Program ID]'
           ,'Donation_ID_Table.[Donation_ID] IS NOT NULL')

GO
SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] OFF
GO
