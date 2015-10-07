USE [MinistryPlatform]
GO

-- Grant "Read" access on "Security Roles" Page to "unauthenticatedCreate"
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
	 387, 
	 0, 
	 0, 
	 0, 
	 0, 
	 0, 
	 0, 
	 0, 
	 0, 
	 0);
GO

-- Grant "Read" access on "Security Roles/Users" SubPage to "unauthenticatedCreate"
INSERT [dbo].[dp_Role_Sub_Pages] 
	([Role_ID], 
	 [Sub_Page_ID], 
	 [Access_Level]) 
VALUES 
	(62, 
	 363, 
	 0);
GO