USE [MinistryPlatform]
GO

DECLARE @SUB_PAGE int = 544;

IF EXISTS (SELECT 1 FROM dbo.dp_Role_Sub_Pages WHERE Sub_Page_ID = @SUB_PAGE)
BEGIN
	DELETE FROM [dbo].[dp_Role_Sub_Pages]
    WHERE Sub_Page_ID = @SUB_PAGE
END
GO

IF EXISTS(SELECT * FROM [dbo].[dp_Sub_Pages] WHERE [Sub_Page_ID] = 544)
BEGIN
	DELETE FROM [dbo].[dp_Sub_Pages]
    WHERE Sub_Page_ID = 544
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Staff_Only_Feedback_Contacts]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Staff_Only_Feedback]'))
ALTER TABLE [dbo].[cr_Staff_Only_Feedback] DROP CONSTRAINT [FK_cr_Staff_Only_Feedback_Contacts]
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Staff_Only_Feedback_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Staff_Only_Feedback]'))
ALTER TABLE [dbo].[cr_Staff_Only_Feedback] DROP CONSTRAINT [FK_cr_Staff_Only_Feedback_dp_Domains]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Staff_Only_Feedback]') AND type in (N'U'))
DROP TABLE [dbo].[cr_Staff_Only_Feedback]
GO