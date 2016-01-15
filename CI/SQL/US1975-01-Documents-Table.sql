USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'cr_Documents'))
BEGIN
	
	CREATE TABLE [dbo].[cr_Documents](
		[Document_ID] [int] IDENTITY(1,1) NOT NULL,
		[Document] [nvarchar](100) NOT NULL,
		[Description] [nvarchar](255) NULL,
		[Domain_ID] [int] NOT NULL DEFAULT (1)
	 CONSTRAINT [PK_cr_Document] PRIMARY KEY CLUSTERED
	(
		[Document_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Domain_ID' AND Object_ID = Object_ID(N'cr_Documents'))
BEGIN
ALTER TABLE [dbo].[cr_Documents] ADD
	[Domain_ID] INT NOT NULL DEFAULT (1)
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Documents_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Documents]'))
ALTER TABLE [dbo].[cr_Documents]  WITH CHECK ADD  CONSTRAINT [FK_cr_Documents_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Documents_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Documents]'))
ALTER TABLE [dbo].[cr_Documents] CHECK CONSTRAINT [FK_cr_Documents_dp_Domains]
GO
