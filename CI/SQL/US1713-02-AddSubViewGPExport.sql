USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
           ,[View_Title]
           ,[Page_ID]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (92198
           ,'GP Export'
           ,296
           ,'Donation_ID_Table.[Donation_ID] AS [Donation ID]
, Donation_ID_Table_Batch_ID_Table.[Batch_Name] AS [Batch Name]
, Donation_ID_Table.[Donation_Date] AS [Donation Date]
, Donation_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_Date] AS [Deposit Date]
, Donation_ID_Table.[Donation_Amount] AS [Donation Amount]
, Donation_Distributions.[Amount] AS [Amount]
, Program_ID_Table.[Program_ID] AS [Program ID]
, Donation_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Exported] AS [Exported]'
           ,'Donation_ID_Table.[Donation_ID] IS NOT NULL
 AND Donation_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Exported] = 0')

GO
SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] OFF
GO
