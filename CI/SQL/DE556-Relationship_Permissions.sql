USE [MinistryPlatform]
GO

DECLARE @API_USER int = 62;
DECLARE @SUB_PAGE int = 265;
DECLARE @ACCESS int = 1;

IF NOT EXISTS (SELECT 1 FROM dbo.dp_Role_Sub_Pages where Role_ID = @API_USER and Sub_Page_ID = @SUB_PAGE)
BEGIN
	INSERT INTO [dbo].[dp_Role_Sub_Pages]
			   ([Role_ID]
			   ,[Sub_Page_ID]
			   ,[Access_Level])
		 VALUES
			   (@API_USER
			   ,@SUB_PAGE
			   ,@ACCESS)
END


