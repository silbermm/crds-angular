USE [MinistryPlatform]
GO

declare @ToolName varchar(30) = 'GP Deposit Export'
declare @LaunchPage varchar(100) = 'http://int.crossroads.net/mptools/gpExport'
declare @Description varchar(50) = 'Generate the GP Deposit Export tab delimited file.'
declare @ToolId int = 367
declare @Toolids table (id int);

declare @PageToolAppearsOn int = 294 -- Deposit Page
declare @Role int = 2 -- Administrator

-- Integration Environment
IF EXISTS (
  SELECT 1
  FROM [dbo].[dp_Tools]
  WHERE [dbo].[dp_Tools].[Tool_Name] = @ToolName
  )
  BEGIN

    -- Update the dp_Tools table with the new tool data
    UPDATE [dbo].[dp_Tools]
    SET  [Description] = @Description
      ,[Launch_Page] = @LaunchPage
    OUTPUT INSERTED.Tool_ID INTO @Toolids
    WHERE [Tool_Name] = @ToolName
    select top 1 @ToolId = id from @Toolids
  END
ELSE
  BEGIN
    INSERT INTO [dbo].[dp_Tools]
         ([Tool_Name]
         ,[Description]
         ,[Launch_Page])
     VALUES
         (@ToolName
         ,@Description
         ,@LaunchPage)
    set @ToolId = SCOPE_IDENTITY()
  END

IF NOT EXISTS (
  SELECT 1
  FROM [dbo].[dp_Tool_Pages]
  WHERE [dbo].[dp_Tool_Pages].[Tool_ID] = @ToolId
  AND [dbo].[dp_Tool_Pages].[Page_ID] = @PageToolAppearsOn
)
BEGIN
  INSERT INTO [dbo].[dp_Tool_Pages]
  ([Tool_ID],
   [Page_ID])
   VALUES (@ToolId, @PageToolAppearsOn)
END

-- Give a role permission to access the tool
IF NOT EXISTS ( SELECT 1 FROM [dbo].[dp_Role_Tools] WHERE [Role_ID] = @Role AND [Tool_ID] = @ToolId )
BEGIN
  INSERT INTO [dbo].[dp_Role_Tools] ([Tool_ID], [Role_ID], [Domain_ID])
  VALUES (@ToolId, @Role, 1)
END

GO



USE [MinistryPlatform]
GO

declare @ToolName varchar(30) = 'Check Batch Processor'
declare @LaunchPage varchar(100) = 'http://int.crossroads.net/mptools/checkBatchProcessor'
declare @Description varchar(50) = 'Processor for EZ Scan checks'
declare @ToolId int
declare @Toolids table (id int);

declare @PageToolAppearsOn int = 297 -- Donation Page
declare @Role int = 2 -- Administrator

-- Integration Environment
IF EXISTS (
  SELECT 1
  FROM [dbo].[dp_Tools]
  WHERE [dbo].[dp_Tools].[Tool_Name] = @ToolName
  )
  BEGIN

    -- Update the dp_Tools table with the new tool data
    UPDATE [dbo].[dp_Tools]
    SET  [Description] = @Description
      ,[Launch_Page] = @LaunchPage
    OUTPUT INSERTED.Tool_ID INTO @Toolids
    WHERE [Tool_Name] = @ToolName
    select top 1 @ToolId = id from @Toolids
  END
ELSE
  BEGIN
    INSERT INTO [dbo].[dp_Tools]
         ([Tool_Name]
         ,[Description]
         ,[Launch_Page])
     VALUES
         (@ToolName
         ,@Description
         ,@LaunchPage)
    set @ToolId = SCOPE_IDENTITY()
  END

IF NOT EXISTS (
  SELECT 1
  FROM [dbo].[dp_Tool_Pages]
  WHERE [dbo].[dp_Tool_Pages].[Tool_ID] = @ToolId
  AND [dbo].[dp_Tool_Pages].[Page_ID] = @PageToolAppearsOn
)
BEGIN
  INSERT INTO [dbo].[dp_Tool_Pages]
  ([Tool_ID],
   [Page_ID])
   VALUES (@ToolId, @PageToolAppearsOn)
END