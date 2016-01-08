USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =========================================================================
-- Author:		John Cleaver
-- Create date: 01/07/2016
-- Description:	This script is a one-time run for go-live to reset user password tokens.
-- Uncomment the update line in order to fully run the script. This is commented out on purpose
-- to avoid accidentally running this script.
--
-- DO NOT RUN THIS SCRIPT AFTER GO-LIVE NIGHT.
-- =========================================================================

DECLARE @users_temp TABLE(user_id INT)

INSERT INTO @users_temp

SELECT User_ID FROM dp_Users u inner join contacts c ON u.Contact_ID = c.Contact_ID 
WHERE c.Contact_Status_ID = 1 
and (u.User_Name = u.User_Email)
and (u.User_Name = c.Email_Address)
and (u.User_Email = c.Email_Address)
and u.User_Email LIKE '%_@__%.__%'

DECLARE @user_id AS INT
DECLARE @token as VARCHAR(MAX)

DECLARE users_curs CURSOR FOR SELECT * FROM @users_temp

OPEN users_curs

FETCH NEXT FROM users_curs INTO @user_id

WHILE @@FETCH_STATUS = 0 BEGIN

	SELECT @token = ( SELECT TOP 64 SUBSTRING(tblSource.vsSource, tblValue.number + 1, 1) 
	FROM (SELECT 'abcdefhkmnpqrstuvwxyzABCDEFHKMNPQRSTUVWXYZ23456789' AS vsSource) 
	AS tblSource JOIN master..spt_values AS tblValue ON tblValue.number < len(tblSource.vsSource) 
	WHERE tblValue.type = 'P' ORDER BY NEWID() FOR XML PATH ('') )

	-- Uncomment this line in order to run the script fully
    -- UPDATE dp_users SET passwordresettoken = @token WHERE user_id = @user_id

FETCH NEXT FROM users_curs into @user_id

END
CLOSE users_curs
DEALLOCATE users_curs
GO
