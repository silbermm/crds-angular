USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] ON

USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Pages]
           ([Page_ID]
           ,[Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Display_Copy])
     VALUES
           (506
           ,'Donation Statuses'
           ,'Donation Status'
           ,'This Crossroads custom table contains the valid statuses for any donation'
           ,90
           ,'Donation_Statuses'
           ,'Donation_Status_ID'
           ,'Donation_Status,Display_On_Statements,Display_On_Giving_History'
           ,'Donation_Status'
           ,0)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO

INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID]
           ,[User_ID])
     VALUES
           (506
           ,4
           ,NULL)
GO
