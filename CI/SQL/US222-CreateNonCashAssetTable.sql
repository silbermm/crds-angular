USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT *
                 FROM INFORMATION_SCHEMA.TABLES
                 WHERE TABLE_CATALOG = 'MinistryPlatform'
				 AND TABLE_SCHEMA = 'dbo'
                 AND  TABLE_NAME = 'Donation_Non_Cash_Assets'))
BEGIN
	CREATE TABLE [dbo].[Donation_Non_Cash_Assets](
		[Non_Cash_Asset_Type_ID] [int] IDENTITY(1,1) NOT NULL,
		[Non_Cash_Asset_Type] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_Donation_Non_Cash_Assets] PRIMARY KEY CLUSTERED
	(
		[Non_Cash_Asset_Type_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY];
END;

IF(NOT EXISTS (SELECT COLUMN_NAME
               FROM INFORMATION_SCHEMA.COLUMNS
               WHERE TABLE_CATALOG = 'MinistryPlatform'
			   AND TABLE_SCHEMA = 'dbo'
               AND TABLE_NAME = 'Donation_Non_Cash_Assets'
               AND COLUMN_NAME = 'Domain_ID'))
BEGIN
	ALTER TABLE [dbo].[Donation_Non_Cash_Assets]
	ADD [Domain_ID] [int] NOT NULL DEFAULT (1);
	
	ALTER TABLE [dbo].[Donation_Non_Cash_Assets]  WITH CHECK ADD  CONSTRAINT [FK_Donation_Non_Cash_Assets_dp_Domains] FOREIGN KEY([Domain_ID])
    REFERENCES [dbo].[dp_Domains] ([Domain_ID]);
END;

GO