USE [MinistryPlatform]
GO

DELETE FROM [dbo].[Batch_Entry_Types]
      WHERE Batch_Entry_Type_ID BETWEEN 4 AND 7
GO

UPDATE [dbo].[Batch_Entry_Types]
   SET Batch_Entry_Type  = 'Payment Processor'
 WHERE Batch_Entry_Type_ID = 10
GO
