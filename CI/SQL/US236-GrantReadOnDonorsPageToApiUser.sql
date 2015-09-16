USE [MinistryPlatform]
GO

-- Add domain_id, needed to provide access to the table via security roles
ALTER TABLE [dbo].[Donation_Statuses] ADD [Domain_ID] [int] NULL;
GO

ALTER TABLE [dbo].[Donation_Statuses] WITH CHECK ADD  CONSTRAINT [FK_Donation_Statuses_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

-- Set Domain_ID to default domain (use 1=1 where clause to avoid errors running script)
UPDATE [dbo].[Donation_Statuses] SET [Domain_ID] = 1 WHERE 1 = 1;
GO

-- Grant "Read" access on "DonationStatuses" Page to "unauthenticatedCreate"
INSERT [dbo].[dp_Role_Pages]
	([Role_ID],
	 [Page_ID],
	 [Access_Level])
VALUES
	(62,
	 506,
	 0);
GO
