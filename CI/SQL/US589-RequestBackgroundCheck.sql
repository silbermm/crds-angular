USE [MinistryPlatform]
GO

DECLARE @TEMPLATEID int = 11269;

IF EXISTS (Select 1 FROM [dbo].[dp_Communications] WHERE Communication_ID = @TEMPLATEID)
BEGIN
	UPDATE [dbo].[dp_Communications] 
	SET [Body] = N'Hello there, <br /><br /> Thank you for your continued interest in serving. As part of our policy, we are requesting that you fill out a background check.<div><br /></div><div><a href=''https://www.employmentreports.net/ApplicationAlpha/Subject/DisclosureForm?cra=525VER&username=CrossroadsPDF&application=CrossroadsDisclosure.pdf&externalId=[Contact_ID]&redirectUrl=https%3A%2F%2www.crossroads.net%2F%23%2Fbackgroundcheck-thanks%2F'' target=''_self''>Please fill out the background check and required information</a>.<br /><br /> Please note, our background check is handled by <a href=''http://veridataservices.com'' target=''_self''>VeriData</a>  This information will not be public or used in crossroads.net.<br /><br /> Thank you,</div>'
	WHERE Communication_ID = @TEMPLATEID
END 
ELSE
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Communications] ON;
	INSERT INTO [dbo].[dp_Communications]
			   ([Author_User_ID]
			   ,[Subject]
			   ,[Body]
			   ,[Domain_ID]
			   ,[Start_Date]
			   ,[Expire_Date]
			   ,[Communication_Status_ID]
			   ,[From_Contact]
			   ,[Reply_to_Contact]
			   ,[_Sent_From_Task]
			   ,[Selection_ID]
			   ,[Template]
			   ,[Active]
			   ,[To_Contact])
		 VALUES
			   (5
			   ,'Background Check Request'
			   ,N'Hello there, <br /><br /> Thank you for your continued interest in serving. As part of our policy, we are requesting that you fill out a background check.<div><br /></div><div><a href=''https://www.employmentreports.net/ApplicationAlpha/Subject/DisclosureForm?cra=525VER&username=CrossroadsPDF&application=CrossroadsDisclosure.pdf&externalId=[Contact_ID]&redirectUrl=https%3A%2F%2www.crossroads.net%2F%23%2Fbackgroundcheck-thanks%2F'' target=''_self''>Please fill out the background check and required information</a>.<br /><br /> Please note, our background check is handled by <a href=''http://veridataservices.com'' target=''_self''>VeriData</a>  This information will not be public or used in crossroads.net.<br /><br /> Thank you,</div>'
			   ,1
			   ,GETDATE()
			   ,NULL
			   ,NULL
			   ,7
			   ,7
			   ,NULL
			   ,NULL
			   ,1
			   ,1
			   ,NULL)
		SET IDENTITY_INSERT [dbo].[dp_Communications] OFF;
END

