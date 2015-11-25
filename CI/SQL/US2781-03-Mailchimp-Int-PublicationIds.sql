-- Set List ID values for Int account for Mailchimp
USE MinistryPlatform
GO

IF EXISTS (SELECT * FROM [dbo].[dp_Publications] WHERE [Title] = 'The Daily')
BEGIN

UPDATE [dbo].[dp_Publications]

SET
	[Third_Party_Publication_ID] = '864ac15bc2'
WHERE
	[Title] = 'The Daily'
END

IF EXISTS (SELECT * FROM [dbo].[dp_Publications] WHERE [Title] = 'General Crossroads Communication')
BEGIN

UPDATE [dbo].[dp_Publications]

SET
	[Third_Party_Publication_ID] = 'ff1d70b3e6'
WHERE
	[Title] = 'General Crossroads Communication'
END

IF EXISTS (SELECT * FROM [dbo].[dp_Publications] WHERE [Title] = 'Kids'' Club & Student Ministry')
BEGIN

UPDATE [dbo].[dp_Publications]

SET
	[Third_Party_Publication_ID] = 'fcf72bfb27'
WHERE
	[Title] = 'Kids'' Club & Student Ministry'
END