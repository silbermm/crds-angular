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

DECLARE @users_temp TABLE(user_id INT, contact_id INT, user_name VARCHAR(MAX), passwordresettoken VARCHAR(MAX))

-- This block is here for testing. Substitute your user id in the appropriate spot and run. This will insert a communication and send an email.
INSERT INTO @users_temp
SELECT TOP(1) u.User_ID, u.Contact_ID, u.User_Name, u.PasswordResetToken FROM dp_Users u inner join contacts c ON u.Contact_ID = c.Contact_ID 
WHERE c.Contact_Status_ID = 1 
AND (u.User_Name = u.User_Email)
AND (u.User_Name = c.Email_Address)
AND (u.User_Email = c.Email_Address)
AND u.User_Email LIKE '%_@__%.__%'
AND u.User_Id=<YOUR USER ID AS AN INT HERE>
AND u.PasswordResetToken IS NOT NULL

-- This is the "live" code and is commented out, along with the TOP(0) line in order to avoid accidentally running this code. 
-- DO NOT UNCOMMENT THIS CODE AND RUN THIS QUERY EXCEPT AT GO-LIVE NIGHT
--INSERT INTO @users_temp
--SELECT TOP(0) u.User_ID, u.Contact_ID, u.User_Name, u.PasswordResetToken FROM dp_Users u inner join contacts c ON u.Contact_ID = c.Contact_ID 
--WHERE c.Contact_Status_ID = 1 
--AND (u.User_Name = u.User_Email)
--AND (u.User_Name = c.Email_Address)
--AND (u.User_Email = c.Email_Address)
--AND u.User_Email LIKE '%_@__%.__%'
--AND u.PasswordResetToken IS NOT NULL

SELECT * FROM @users_temp

DECLARE @contact_id AS INT
DECLARE @user_name AS VARCHAR(MAX)
DECLARE @passwordresettoken AS VARCHAR(MAX)

DECLARE users_curs CURSOR FOR SELECT contact_id, user_name, passwordresettoken FROM @users_temp

OPEN users_curs

FETCH NEXT FROM users_curs into @contact_id, @user_name, @passwordresettoken

WHILE @@FETCH_STATUS = 0 BEGIN

	-- check to see if the user has an existing reset message already send to them
	 IF (SELECT COUNT(*) FROM dp_communication_messages WHERE contact_id = @contact_id AND [subject]='New System Password Reset Email') = 0
	 BEGIN

		DECLARE @row_ids TABLE (id INT);

		---- The subject and body on these inserts needs to change once we have final direction from OCM on the verbiage
		---- UNCOMMENT THE FOLLOWING LINES TO RUN THE INSERT
		--INSERT INTO dp_Communications 
		--	(Author_User_ID, Subject, Body, Domain_ID, Start_Date, Expire_Date, Communication_Status_ID, From_Contact, Reply_to_Contact, _Sent_From_Task, Selection_ID, Template, Active,
		--	To_Contact, Time_Zone, Locale)
		--	OUTPUT inserted.Communication_ID INTO @row_ids
		--	VALUES
		--	(5, 'New System Password Reset Email', 'This is a System Test. If you have received this email in error, please disregard. ', 1, '2016-01-04 19:49:42.610', NULL, 3, 1519180, 1519180, NULL, NULL,0, 0, NULL, NULL, NULL)
	
		--INSERT INTO dp_Communication_Messages
		--	(Communication_ID, Action_Status_ID, Action_Status_Time, Action_Text, Contact_ID, [From], [To], Reply_To, Subject, Body, Domain_ID, Deleted)
		--	VALUES
		--	((SELECT TOP(1) * FROM @row_ids), 2, '2016-01-05 00:50:01.107', NULL, @contact_id, 'updates@crossroads.net', @user_name, 'updates@crossroads.net', 'New System Password Reset Email',
		--	'This is a System Test. If you have received this email in error, please disregard. Reset link: https://int.crossroads.net/reset-password?token=' + @passwordresettoken + '123', 1, 0)	

	 END

FETCH NEXT FROM users_curs into @contact_id, @user_name, @passwordresettoken

END
CLOSE users_curs
DEALLOCATE users_curs
GO
