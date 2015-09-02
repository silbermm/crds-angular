USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Page_Views]
  SET [Field_List] = 'Donation_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_ID] AS [Deposit ID]
, Donation_ID_Table_Batch_ID_Table.[Batch_ID] AS [Batch ID]
, Program_ID_Table.[Program_ID] AS [Program ID]
, Congregation_ID_Table.[Congregation_ID] AS [Congregation ID]
, (select Document_Type from GL_Account_Mapping G where G.Program_ID=Program_ID_Table.Program_ID and G.Congregation_ID = Congregation_ID_Table.Congregation_ID) AS [Document Type]
, Donation_ID_Table.[Donation_ID] AS [Donation ID]
, Donation_ID_Table_Batch_ID_Table.[Batch_Name] AS [Batch Name]
, Donation_ID_Table.[Donation_Date] AS [Donation Date]
, Donation_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Deposit_Date] AS [Deposit Date]
, (select Customer_ID from GL_Account_Mapping G where G.Program_ID=Program_ID_Table.Program_ID and G.Congregation_ID = Congregation_ID_Table.Congregation_ID) AS [Customer ID]
, Donation_ID_Table.[Donation_Amount] AS [Donation Amount]
, (select Checkbook_ID from GL_Account_Mapping G where G.Program_ID=Program_ID_Table.Program_ID and G.Congregation_ID = Congregation_ID_Table.Congregation_ID) AS [Checkbook ID]
, (select Cash_Account from GL_Account_Mapping G where G.Program_ID=Program_ID_Table.Program_ID and G.Congregation_ID = Congregation_ID_Table.Congregation_ID) AS [Cash Account]
, (select Receivable_Account from GL_Account_Mapping G where G.Program_ID=Program_ID_Table.Program_ID and G.Congregation_ID = Congregation_ID_Table.Congregation_ID) AS [Receivable Account]
, (select Distribution_Account from GL_Account_Mapping G where G.Program_ID=Program_ID_Table.Program_ID and G.Congregation_ID = Congregation_ID_Table.Congregation_ID) AS [Distribution Account]
, Donation_Distributions.[Amount] AS [Amount]
, Donation_ID_Table_Batch_ID_Table_Deposit_ID_Table.[Exported] AS [Exported]'
WHERE [Page_View_ID] = 92198

GO
