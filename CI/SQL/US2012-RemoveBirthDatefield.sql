USE [MinistryPlatform]
GO


DELETE FROM [dbo].[Form_Response_Answers]
      WHERE [Form_Field_ID] = 1470
GO

DELETE FROM [dbo].[Form_Fields]
      WHERE [Form_Field_ID] = 1470
GO


