USE [MinistryPlatform]
GO

IF EXISTS (SELECT 1 FROM [dbo].[Statement_Headers] WHERE [Statement_Header] = 'Benevolence')
BEGIN
	DELETE FROM [dbo].[Statement_Headers] WHERE [Statement_Header] = 'Benevolence';
END

IF EXISTS (SELECT 1 FROM [dbo].[Statement_Headers] WHERE [Statement_Header] = 'Special Offerings')
BEGIN
	DELETE FROM [dbo].[Statement_Headers] WHERE [Statement_Header] = 'Special Offerings';
END

IF EXISTS (SELECT 1 FROM [dbo].[Statement_Headers] WHERE [Statement_Header] = 'Ministry')
BEGIN
	UPDATE [dbo].[Statement_Headers] SET [Header_Sort] = 1 WHERE [Statement_Header] = 'Ministry';
END

IF EXISTS (SELECT 1 FROM [dbo].[Statement_Headers] WHERE [Statement_Header] LIKE 'Trip%Giving')
BEGIN
	UPDATE [dbo].[Statement_Headers] SET [Statement_Header] = 'Trips Giving', [Header_Sort] = 2 WHERE [Statement_Header] LIKE 'Trip%Giving';
END

IF EXISTS (SELECT 1 FROM [dbo].[Statement_Headers] WHERE [Statement_Header] = 'Campaigns')
BEGIN
	UPDATE [dbo].[Statement_Headers] SET [Header_Sort] = 3 WHERE [Statement_Header] = 'Campaigns';
END

IF EXISTS (SELECT 1 FROM [dbo].[Statement_Headers] WHERE [Statement_Header] = 'Other')
BEGIN
	UPDATE [dbo].[Statement_Headers] SET [Header_Sort] = 4 WHERE [Statement_Header] = 'Other';
END

-- If we ever need to re-populate default data from MP...
--SET IDENTITY_INSERT [dbo].[Statement_Headers] ON;

--INSERT INTO [dbo].[Statement_Headers] (
--	 [Statement_Header_ID]
--	,[Statement_Header]
--	,[Header_Sort]
--	,[Domain_ID]
--) VALUES (
--	 4
--	,'Benevolence'
--	,4
--	,1
--);

--INSERT INTO [dbo].[Statement_Headers] (
--	 [Statement_Header_ID]
--	,[Statement_Header]
--	,[Header_Sort]
--	,[Domain_ID]
--) VALUES (
--	 5
--	,'Special Offerings'
--	,5
--	,1
--);

--SET IDENTITY_INSERT [dbo].[Statement_Headers] OFF;
