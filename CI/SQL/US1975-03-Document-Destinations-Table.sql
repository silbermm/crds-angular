USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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

SET IDENTITY_INSERT [dbo].[cr_Document_Destinations] ON

GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (7, 1, 2, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (8, 2, 2, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (9, 3, 2, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (10, 4, 2, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (11, 5, 2, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (12, 6, 2, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (13, 7, 2, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (14, 8, 2, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (15, 10, 2, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (16, 12, 2, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (17, 13, 2, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (18, 16, 2, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (19, 17, 2, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (20, 10, 3, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (21, 12, 3, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (22, 13, 3, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (23, 16, 3, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (24, 12, 1, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (25, 13, 1, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (26, 16, 1, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (27, 9, 4, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (28, 10, 4, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (29, 11, 4, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (30, 12, 4, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (31, 13, 4, 1)
GO
INSERT [dbo].[cr_Document_Destinations] ([Document_Destination_ID], [Document_ID], [Destination_ID], [Domain_ID]) VALUES (32, 16, 4, 1)
GO

SET IDENTITY_INSERT [dbo].[cr_Document_Destinations] OFF
GO

IF NOT EXISTS (SELECT * 
           FROM sys.foreign_keys 
           WHERE object_id = OBJECT_ID(N'[dbo].[FK_Document_Destinations_Destinations]') 
             AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Document_Destinations]'))
BEGIN
   ALTER TABLE [dbo].[cr_Document_Destinations]  WITH CHECK ADD  CONSTRAINT [FK_Document_Destinations_Destinations] FOREIGN KEY([Destination_ID])
REFERENCES [dbo].[cr_Destinations] ([Destination_ID])
END


ALTER TABLE [dbo].[cr_Document_Destinations] CHECK CONSTRAINT [FK_Document_Destinations_Destinations]
GO	

IF NOT EXISTS (SELECT * 
           FROM sys.foreign_keys 
           WHERE object_id = OBJECT_ID(N'[dbo].[FK_Document_Destinations_Documents]') 
             AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Document_Destinations]'))
BEGIN
	ALTER TABLE [dbo].[cr_Document_Destinations]  WITH CHECK ADD  CONSTRAINT [FK_Document_Destinations_Documents] FOREIGN KEY([Document_ID])
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
