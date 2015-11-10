USE [MinistryPlatform]
GO

IF NOT EXISTS(
	SELECT * FROM sys.columns
	WHERE Name = N'PasswordResetToken' and OBJECT_ID = OBJECT_ID(N'dp_Users'))
BEGIN

ALTER TABLE dp_Users
ADD PasswordResetToken VARCHAR(MAX) NULL

END
GO
				