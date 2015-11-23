USE MinistryPlatform
GO

SET IDENTITY_INSERT dbo.dp_Page_Views ON

INSERT INTO dbo.dp_Page_Views
	(
		Page_View_ID,
		View_Title,
		Page_ID,
		Description,
		Field_List,
		View_Clause,
		Order_By
	)
	VALUES
	(
		2199,
		'Bulk Email Publications',
		376,
		'Publications to be Synchronized by the CRDS Bulk Email Sync process',
		'dp_Publications.Publication_ID, dp_Publications.Title, dp_Publications.Description, dp_Publications.Third_Party_Publication_ID, dp_Publications.Last_Successful_Sync',
		'dp_Publications.Available_Online = 1',
		'dp_Publications.Online_Sort_Order'
	)

SET IDENTITY_INSERT dbo.dp_Page_Views OFF
GO
