USE [MinistryPlatform]
GO

declare @ToolName varchar(30) = 'Check Batch Processor'
declare @LaunchPage varchar(100) = 'http://int.crossroads.net/mptools/checkBatchProcessor'
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

/* DEVELOPMENT TOOL */
declare @ToolNameDev varchar(30) = 'Check Batch Processor - Dev'
declare @LaunchPageDev varchar(100) = 'http://localhost:8080/mptools/checkBatchProcessor'
declare @DescriptionDev varchar(50) = 'Development Processor for EZ Scan checks'
declare @ToolIdDev int

IF EXISTS ( SELECT 1 FROM [dbo].[dp_Tools] WHERE [Tool_Name] = @ToolNameDev )
BEGIN
  SELECT @ToolIdDev = [Tool_ID] FROM [dbo].[dp_Tools] WHERE [Tool_Name] = @ToolNameDev
END
ELSE
BEGIN
  INSERT INTO [dbo].[dp_Tools] ([Tool_Name], [Description], [Launch_Page])
  VALUES (@ToolNameDev, @DescriptionDev, @LaunchPageDev)

  SELECT @ToolIdDev = [Tool_ID] FROM [dbo].[dp_Tools] WHERE [Tool_Name] = @ToolNameDev
END

IF NOT EXISTS ( SELECT 1 FROM [dbo].[dp_Tool_Pages] WHERE [Tool_ID] = @ToolIdDev AND [Page_ID] = @PageToolAppearsOn )
BEGIN
  INSERT INTO [dbo].[dp_Tool_Pages] ([Tool_ID], [Page_ID])
  VALUES (@ToolIdDev, @PageToolAppearsOn)
END

-- Give a role permission to access the tool
IF NOT EXISTS ( SELECT 1 FROM [dbo].[dp_Role_Tools] WHERE [Role_ID] = @Role AND [Tool_ID] = @ToolId )
BEGIN
  INSERT INTO [dbo].[dp_Role_Tools] ([Tool_ID], [Role_ID], [Domain_ID])
  VALUES (@ToolId, @Role, 1)
END

-- Give a role permission to access the dev tool
IF NOT EXISTS ( SELECT 1 FROM [dbo].[dp_Role_Tools] WHERE [Role_ID] = @Role AND [Tool_ID] = @ToolIdDev )
BEGIN
  INSERT INTO [dbo].[dp_Role_Tools] ([Tool_ID], [Role_ID], [Domain_ID])
  VALUES (@ToolIdDev, @Role, 1)
END

GO
