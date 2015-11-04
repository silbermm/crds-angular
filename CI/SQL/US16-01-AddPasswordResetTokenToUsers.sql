USE [MinistryPlatform]
GO

IF NOT EXISTS(
	SELECT * FROM sys.columns
	WHERE Name = N'__PasswordResetToken' and OBJECT_ID = OBJECT_ID(N'dp_Users'))
BEGIN

ALTER TABLE dp_Users
ADD __PasswordResetToken VARCHAR(MAX) NULL

END
GO
				