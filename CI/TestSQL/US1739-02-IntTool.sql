USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Tools]
   SET [Launch_Page] = 'http://int.crossroads.net/mptools/tripParticipants'
 WHERE Tool_ID = 44
GO
