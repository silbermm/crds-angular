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
           ,'Donation Status'
           ,'Donation Status'
           ,'This Crossroads custom table contains the valid statuses for any donation'
           ,90
           ,'Donation_Status'
           ,'Donation_Status_ID'           
           ,'Donation_Status,Donation_Status.Display_On_Statements,Donation_Status.Display_On_Giving_History'
           ,'Donation_Status'       
           ,0)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO
