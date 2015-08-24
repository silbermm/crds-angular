USE [MinistryPlatform]
GO

declare @ToolName varchar(30) = 'Check Batch Processor'
declare @LaunchPage varchar(100) = 'http://demo.crossroads.net/mptools/checkBatchProcessor'
declare @Description varchar(50) = 'Processor for EZ Scan checks'
declare @ToolId int

declare @PageToolAppearsOn int = 297 -- Donation Page
declare @Role int = 2 -- Administrator

/* Integration Environment */
IF EXISTS ( SELECT 1 FROM [dbo].[dp_Tools] WHERE [Tool_Name] = @ToolName )
BEGIN
  SELECT @ToolId = [Tool_ID] FROM [dbo].[dp_Tools] WHERE [Tool_Name] = @ToolName
END
ELSE
BEGIN
  INSERT INTO [dbo].[dp_Tools] ([Tool_Name], [Description], [Launch_Page])
  VALUES (@ToolName, @Description, @LaunchPage)

  SELECT @ToolId = [Tool_ID] FROM [dbo].[dp_Tools] WHERE [Tool_Name] = @ToolName
END

IF NOT EXISTS ( SELECT 1 FROM [dbo].[dp_Tool_Pages] WHERE [Tool_ID] = @ToolId AND [Page_ID] = @PageToolAppearsOn )
BEGIN
  INSERT INTO [dbo].[dp_Tool_Pages] ([Tool_ID], [Page_ID])
  VALUES (@ToolId, @PageToolAppearsOn)
END

GO
