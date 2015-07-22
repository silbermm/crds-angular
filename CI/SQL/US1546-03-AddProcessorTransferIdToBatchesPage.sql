USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
SET [Default_Field_List] =
	'Setup_Date
	,Batches.Batch_Name
	,Batches.Batch_ID AS BatchID
	,Batches.Batch_Total
	,Finalize_Date
	,Batches.Deposit_ID
	,Congregation_ID_Table.Congregation_Name AS Congregation
	,Batch_Entry_Type_ID_Table.Batch_Entry_Type
	,Batches.Processor_Transfer_ID
	,Batches.Currency
	,Batches.Default_Payment_Type'
WHERE [Page_ID] = 282;
