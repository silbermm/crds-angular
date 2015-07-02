USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON

INSERT INTO [dbo].[dp_Sub_Pages]
           ([Sub_Page_ID]
           ,[Display_Name]
           ,[Singular_Name]
           ,[Page_ID]
           ,[View_Order]
           ,[Link_To_Page_ID] 
           ,[Select_From_Field_Name]
           ,[Primary_Table]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Filter_Key]
           ,[Relation_Type_ID])
     VALUES
           (532
           ,'Donation Status'
           ,'Donation Status'
           ,297
           ,140           
           ,506
           ,'Donation_ID'
           ,'Donation_Status_Mapping'
           ,'Donation_Status_Mapping_ID'
           ,'Donation_Status_ID_Table.Donation_Status,Donation_Status_Date,Donation_Status_Notes'
           ,'Donation_Status_Mapping_ID'
           ,'Donation_ID'
           ,3)

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF
GO