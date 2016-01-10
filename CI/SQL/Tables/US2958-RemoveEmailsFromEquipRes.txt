USE [MinistryPlatform]
GO
	
	
DELETE FROM [dbo].[dp_Process_Steps]
       WHERE [Process_ID] = 15 AND [Process_Step_ID] IN (52, 53, 54)
GO
