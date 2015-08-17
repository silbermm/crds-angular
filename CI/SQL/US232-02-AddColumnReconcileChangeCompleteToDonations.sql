USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[Donations]    Script Date: 8/12/2015 2:07:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[Donations] ADD Reconcile_Change_Complete bit;

GO
