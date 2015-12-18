USE [MinistryPlatform]
GO

DECLARE @ServingGroup int = 9
DECLARE @KidsClubMinistry int = 2
DECLARE @KidsClubDeadlinePassed int = 58

UPDATE [dbo].[Groups]
SET Deadline_Passed_Message_ID = @KidsClubDeadlinePassed
WHERE Group_Type_ID = @ServingGroup AND Ministry_ID = @KidsClubMinistry