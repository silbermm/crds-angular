USE [MinistryPlatform]
GO

-- Grant "Read" access on "Donors: Donations" SubPage to "unauthenticatedCreate"
INSERT [dbo].[dp_Role_Sub_Pages]
	([Role_ID],
	 [Sub_Page_ID],
	 [Access_Level])
VALUES
	(62,
	 274,
	 0);
GO

-- Grant "Read" access on "Donors: Soft Credit Donations" SubPage to "unauthenticatedCreate"
INSERT [dbo].[dp_Role_Sub_Pages]
	([Role_ID],
	 [Sub_Page_ID],
	 [Access_Level])
VALUES
	(62,
	 277,
	 0);
GO
