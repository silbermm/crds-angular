USE [MinistryPlatform]
GO

declare @ToolId int = 1

DELETE FROM [dbo].[dp_Role_Tools] WHERE Tool_ID = @ToolId
DELETE FROM [dbo].[dp_Tool_Pages] WHERE Tool_ID = @ToolId
DELETE FROM [dbo].[dp_Tools] WHERE Tool_ID = @ToolId

GO
