USE [MinistryPlatform]
GO

IF EXISTS (SELECT * FROM [dbo].[dp_Processes] WHERE [Process_ID] = 29)
BEGIN

	UPDATE [dbo].[dp_Processes]
	   SET [Active] = 0     
	 WHERE Process_ID = 29
END



