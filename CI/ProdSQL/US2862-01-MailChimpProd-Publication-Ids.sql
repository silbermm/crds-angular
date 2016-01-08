-- Set List ID values for Prod account for Mailchimp
USE MinistryPlatform
GO

IF EXISTS (SELECT * FROM [dbo].[dp_Publications] WHERE [Publication_ID] = 1)
BEGIN
	UPDATE [dbo].[dp_Publications]
	SET
		[Third_Party_Publication_ID] = 'f5e8422ab9'
	WHERE
		[Publication_ID] = 1
END

IF EXISTS (SELECT * FROM [dbo].[dp_Publications] WHERE [Publication_ID] = 2)
BEGIN

	UPDATE [dbo].[dp_Publications]
	SET
		[Third_Party_Publication_ID] = '3d32a23ad9'
	WHERE
		[Publication_ID] = 2
END

IF EXISTS (SELECT * FROM [dbo].[dp_Publications] WHERE [Publication_ID] = 3)
BEGIN

	UPDATE [dbo].[dp_Publications]
	SET
		[Third_Party_Publication_ID] = 'c76a137657'
	WHERE
		[Publication_ID] = 3
END

GO