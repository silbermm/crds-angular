USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] ON 
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
           (509
           ,'Non-Cash Asset Types'
           ,'Non-Cash Asset Type'
           ,'This Crossroads specific table contains all possible values associated with a non-cash donation'
           ,90
           ,'Donation_Non_Cash_Assets'
           ,'Non_Cash_Asset_Type_ID'          
           ,'Non_Cash_Asset_Type'
           ,'Non_Cash_Asset_Type'           
           ,0)
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO
