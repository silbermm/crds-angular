USE MinistryPlatform
GO

ALTER TABLE dp_Publications
	ADD 
		Third_Party_Publication_ID VARCHAR(100),
		Last_Successful_Sync DATETIME CONSTRAINT df_Last_Successful_Sync DEFAULT GETDATE() NOT NULL
GO

ALTER TABLE dp_Contact_Publications
	ADD
		Third_Party_Contact_ID VARCHAR(255) NULL
GO


-- Create Mapping table for Publications to Page_Views
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Publication_Page_Views]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Publication_Page_Views](
	[Publication_Page_View_ID] [int] IDENTITY(1,1) NOT NULL,
	[Publication_ID] [int] NOT NULL,
	[Page_View_ID] [int] NOT NULL,
 CONSTRAINT [PK_Publication_Page_Views] PRIMARY KEY CLUSTERED 
(
	[Publication_Page_View_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Publication_Page_Views_dp_Page_Views]') AND parent_object_id = OBJECT_ID(N'[dbo].[Publication_Page_Views]'))
ALTER TABLE [dbo].[Publication_Page_Views]  WITH CHECK ADD  CONSTRAINT [FK_Publication_Page_Views_dp_Page_Views] FOREIGN KEY([Page_View_ID])
REFERENCES [dbo].[dp_Page_Views] ([Page_View_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Publication_Page_Views_dp_Page_Views]') AND parent_object_id = OBJECT_ID(N'[dbo].[Publication_Page_Views]'))
ALTER TABLE [dbo].[Publication_Page_Views] CHECK CONSTRAINT [FK_Publication_Page_Views_dp_Page_Views]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Publication_Page_Views_dp_Publications]') AND parent_object_id = OBJECT_ID(N'[dbo].[Publication_Page_Views]'))
ALTER TABLE [dbo].[Publication_Page_Views]  WITH CHECK ADD  CONSTRAINT [FK_Publication_Page_Views_dp_Publications] FOREIGN KEY([Publication_ID])
REFERENCES [dbo].[dp_Publications] ([Publication_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Publication_Page_Views_dp_Publications]') AND parent_object_id = OBJECT_ID(N'[dbo].[Publication_Page_Views]'))
ALTER TABLE [dbo].[Publication_Page_Views] CHECK CONSTRAINT [FK_Publication_Page_Views_dp_Publications]
GO
