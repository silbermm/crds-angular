USE [MinistryPlatform]
GO

IF EXISTS (SELECT * FROM [dbo].[dp_Communications] WHERE [Communication_ID] = 13356)
BEGIN

UPDATE [dbo].[dp_Communications]
SET 

   [Author_User_ID] = 1,
   [Subject] = 'Password Reset Link',
   [Body] = '<p>Someone requested that the password be reset for the user with this email address on crossroads.net.</p><div><p>If this was a mistake, just ignore this email and nothing will happen.</p></div><div><p>Use this <a href="[resetlink]">link</a> to reset your password</p></div>',
   [Domain_ID] = 1,
   [Start_Date] = '2015-11-03 00:00:00.000',
   [Expire_Date] = NULL,
   [Communication_Status_ID] = 1,
   [From_Contact] = 7,
   [Reply_to_Contact] = 7,                      
   [Template] = 1,
   [Active] = 1

WHERE [Communication_ID] = 13356
		   
END

ELSE

BEGIN
SET IDENTITY_INSERT [dbo].[dp_Communications] ON 

INSERT INTO [dbo].[dp_Communications]
           ([Communication_ID],
            [Author_User_ID],
            [Subject],
            [Body],
            [Domain_ID],
            [Start_Date],
            [Expire_Date],
		    [Communication_Status_ID],
            [From_Contact],
            [Reply_to_Contact],                    
            [Template],
			[Active])
     VALUES
           (13356,
		   1,
		   'Password Reset Link',
		   '<p>Someone requested that the password be reset for the user with this email address on crossroads.net.</p><div><p>If this was a mistake, just ignore this email and nothing will happen.</p></div><div><p>Use this <a href="[resetlink]">link</a> to reset your password.</p></div>',
		   1,
		   '2015-11-03 00:00:00.000',
		   NULL,
		   1,
		   7,
		   7,
		   1,
		   1)

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
END
GO