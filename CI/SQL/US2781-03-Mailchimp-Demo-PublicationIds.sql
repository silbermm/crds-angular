-- Set List ID values for Int account for Mailchimp
USE MinistryPlatform
GO

IF EXISTS (SELECT * FROM [dbo].[dp_Publications] WHERE [Publication_ID] = 1)
BEGIN

UPDATE [dbo].[dp_Publications]

SET
	[Third_Party_Publication_ID] = 'f64274f568'
WHERE
	[Publication_ID] = 1
END

IF EXISTS (SELECT * FROM [dbo].[dp_Publications] WHERE [Publication_ID] = 2)
BEGIN

UPDATE [dbo].[dp_Publications]

SET
	[Third_Party_Publication_ID] = '32e46a3131'
WHERE
	[Publication_ID] = 2
END

IF EXISTS (SELECT * FROM [dbo].[dp_Publications] WHERE [Publication_ID] = 3)
BEGIN

UPDATE [dbo].[dp_Publications]

SET
	[Third_Party_Publication_ID] = 'c743eb873b'
WHERE
	[Publication_ID] = 3
END