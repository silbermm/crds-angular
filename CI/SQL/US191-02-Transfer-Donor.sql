USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[Contacts] ON

INSERT INTO [dbo].[Contacts]
           ([Contact_ID]
		   ,[Company]
           ,[Display_Name]
           ,[Contact_Status_ID]
           ,[Bulk_Email_Opt_Out]
           ,[Bulk_SMS_Opt_Out]
           ,[Contact_GUID]
           ,[Domain_ID])
     VALUES
           (3717417
		   ,0
           ,'Transfer'
           ,1
           ,0
           ,0
           ,'4D70F390-F00D-478E-875E-EA36ECE317C1'
           ,1)

SET IDENTITY_INSERT [dbo].[Contacts] OFF
GO

SET IDENTITY_INSERT [dbo].[Donors] ON

INSERT INTO [MinistryPlatform].[dbo].[Donors]
           ([Donor_ID]
		         ,[Contact_ID]
           ,[Statement_Frequency_ID]
           ,[Statement_Type_ID]
           ,[Statement_Method_ID]
           ,[Setup_Date]
           ,[Cancel_Envelopes]
           ,[Notes]
           ,[Domain_ID])
     VALUES
           (3950265
		   ,3717417
           ,3
           ,1
           ,4
           ,'2015-09-08 15:00:00.000'
           ,0
           ,'Donor record used for internal transfers'
           ,1)

SET IDENTITY_INSERT [dbo].[Donors] OFF

UPDATE [dbo].[Contacts] SET [Donor_Record] = 3950265 WHERE Contact_ID = 3717417
