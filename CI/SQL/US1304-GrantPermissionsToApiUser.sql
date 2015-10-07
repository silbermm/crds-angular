USE [MinistryPlatform]
GO

-- Grant "Edit" access on "Donor Accounts" Page to "unauthenticatedCreate"
INSERT [dbo].[dp_Role_Pages]
	([Role_ID],
	 [Page_ID],
	 [Access_Level],
	 [Scope_All],
	 [Approver],
	 [File_Attacher],
	 [Data_Importer],
	 [Data_Exporter],
	 [Secure_Records],
	 [Allow_Comments],
	 [Quick_Add])
VALUES
	(62,
	 298,
	 1,
	 0,
	 0,
	 0,
	 0,
	 0,
	 0,
	 0,
	 0);
GO

-- Grant "Edit" access on "Addresses" Page to "unauthenticatedCreate"
INSERT [dbo].[dp_Role_Pages]
	([Role_ID],
	 [Page_ID],
	 [Access_Level],
	 [Scope_All],
	 [Approver],
	 [File_Attacher],
	 [Data_Importer],
	 [Data_Exporter],
	 [Secure_Records],
	 [Allow_Comments],
	 [Quick_Add])
VALUES
	(62,
	 271,
	 1,
	 0,
	 0,
	 0,
	 0,
	 0,
	 0,
	 0,
	 0);
GO
