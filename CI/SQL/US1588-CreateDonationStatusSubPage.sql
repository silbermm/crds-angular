USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Sub_Pages]
           ([Display_Name]
           ,[Singular_Name]
           ,[Page_ID]
           ,[View_Order]           
           ,[Select_To_Page_ID]
           ,[Select_From_Field_Name]
           ,[Primary_Table]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]           
           ,[Display_Copy])
     VALUES
           ('Donation Status'
           ,'Donation Status'
           ,532
           ,140           
           ,297
           ,'Donation_ID'
           ,'Donation_Status_Mapping'
           ,'Donation_Status_Mapping_ID'
           ,'Donation_Status_ID_Table.Donation_Status,Donation_Status_Date,Donation_Status_Notes'
           ,'Donation_Status_Mapping_ID'
           ,'Donation_ID'
           ,3           
           ,1)
GO