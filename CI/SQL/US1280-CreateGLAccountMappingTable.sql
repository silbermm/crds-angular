USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.GL_Account_Mapping', 'U') IS NULL
BEGIN
	CREATE TABLE [dbo].[GL_Account_Mapping](
		[Domain_ID] [int] NOT NULL CONSTRAINT [DF_GL_Account_Mapping_Domain_ID]  DEFAULT ((1)),
		[Program_ID] [int] NOT NULL,
		[Congregation_ID] [int] NOT NULL,
		[GL_Account] [nvarchar](50) NOT NULL,
		[GL_Account_Mapping_ID] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_GL_Account_Mapping] PRIMARY KEY CLUSTERED 
	(
		[GL_Account_Mapping_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	)ON [PRIMARY];

	ALTER TABLE [dbo].[GL_Account_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_GL_Account_Mapping_Congregations] FOREIGN KEY([Congregation_ID])
	REFERENCES [dbo].[Congregations] ([Congregation_ID]);

	ALTER TABLE [dbo].[GL_Account_Mapping] CHECK CONSTRAINT [FK_GL_Account_Mapping_Congregations];

	ALTER TABLE [dbo].[GL_Account_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_GL_Account_Mapping_dp_Domains] FOREIGN KEY([Domain_ID])
	REFERENCES [dbo].[dp_Domains] ([Domain_ID]);

	ALTER TABLE [dbo].[GL_Account_Mapping] CHECK CONSTRAINT [FK_GL_Account_Mapping_dp_Domains];

	ALTER TABLE [dbo].[GL_Account_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_GL_Account_Mapping_Programs] FOREIGN KEY([Program_ID])
	REFERENCES [dbo].[Programs] ([Program_ID]);

	ALTER TABLE [dbo].[GL_Account_Mapping] CHECK CONSTRAINT [FK_GL_Account_Mapping_Programs];
END