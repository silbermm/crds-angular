USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_NAME = '[dbo].[cr_Destinations]'))
BEGIN
	CREATE TABLE [dbo].[cr_Destinations](
		[Destination_ID] [int] IDENTITY(1,1) NOT NULL,
		[Destination] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_cr_Destination] PRIMARY KEY CLUSTERED
	(
		[Destination_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END


IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_NAME = 'cr_Document_Destinations'))
BEGIN

	CREATE TABLE [dbo].[cr_Document_Destinations](
		[Document_Destination_ID] [int] IDENTITY(1,1) NOT NULL,
		[Document_ID] [int] NOT NULL,
		[Destination_ID] [int] NOT NULL,
		[Domain_ID] [int] NOT NULL,
	 CONSTRAINT [PK_Document_Destinations] PRIMARY KEY CLUSTERED
	(
		[Document_Destination_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END


IF NOT EXISTS (SELECT * 
           FROM sys.foreign_keys 
           WHERE object_id = OBJECT_ID(N'[dbo].[FK_Document_Destinations_Destinations]') 
             AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Document_Destinations]'))
BEGIN
   ALTER TABLE [dbo].[cr_Document_Destinations]  WITH CHECK ADD  CONSTRAINT [FK_Document_Destinations_Destinations] FOREIGN KEY([Destination_ID])
REFERENCES [dbo].[cr_Destinations] ([Destination_ID])
ALTER TABLE [dbo].[cr_Document_Destinations] CHECK CONSTRAINT [FK_Document_Destinations_Destinations]
END
	

IF NOT EXISTS (SELECT * 
           FROM sys.foreign_keys 
           WHERE object_id = OBJECT_ID(N'[dbo].[FK_Document_Destinations_Documents]') 
             AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Document_Destinations]'))
BEGIN
	ALTER TABLE [dbo].[cr_Document_Destinations]  WITH CHECK ADD CONSTRAINT [FK_Document_Destinations_Documents] FOREIGN KEY([Document_ID])
	REFERENCES [dbo].[cr_Documents] ([Document_ID])
END 

ALTER TABLE [dbo].[cr_Document_Destinations] CHECK CONSTRAINT [FK_Document_Destinations_Documents]
GO

IF NOT EXISTS (SELECT * 
           FROM sys.foreign_keys 
           WHERE object_id = OBJECT_ID(N'[dbo].[FK_Document_Destinations_dp_Domains]') 
             AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Document_Destinations]'))
BEGIN
	ALTER TABLE [dbo].[cr_Document_Destinations]  WITH CHECK ADD  CONSTRAINT [FK_Document_Destinations_dp_Domains] FOREIGN KEY([Domain_ID])
	REFERENCES [dbo].[dp_Domains] ([Domain_ID])
END

ALTER TABLE [dbo].[cr_Document_Destinations] CHECK CONSTRAINT [FK_Document_Destinations_dp_Domains]
GO
