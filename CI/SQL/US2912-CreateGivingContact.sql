USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Contacts] WHERE Contact_ID = 5396574)
BEGIN
	SET IDENTITY_INSERT [dbo].[Contacts] ON;
	INSERT INTO [dbo].[Contacts]
           ([Contact_ID]
		   ,[Company]
           ,[Display_Name]
		   ,[First_Name]
		   ,[Last_Name]
		   ,[Nickname]
		   ,[Gender_ID]
		   ,[Marital_Status_ID]
		   ,[Contact_Status_ID]
		   ,[Email_Address]
		   ,[Email_Unlisted]
		   ,[Bulk_Email_Opt_Out]
		   ,[Bulk_SMS_Opt_Out]
		   ,[Contact_GUID]
		   ,[Domain_ID]
		   )
     VALUES
           (5396574
		   ,0
		   ,N'Giving'
		   ,N'Giving'
		   ,N'Communications'
		   ,N'Giving'
		   ,2
		   ,2
		   ,1
		   ,N'giving@crossroads.net'
		   ,0
		   ,0
		   ,0
		   ,NEWID()
		   ,1
		   )
	SET IDENTITY_INSERT [dbo].[Contacts] ON;
END