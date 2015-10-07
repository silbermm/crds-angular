USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
SET [Default_Field_List] =
	'Deposits.Deposit_Date
	,Deposits.Deposit_Name
	,Deposits.Deposit_ID AS DepositID
	,Deposits.Deposit_Total
	,Deposits.Batch_Count
	,Deposits.Exported
	,Deposits.Processor_Transfer_ID'
WHERE [Page_ID] = 294;
