USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Sub_Pages] WHERE [Sub_Page_ID] = 376)
BEGIN
SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON 

INSERT INTO [dbo].[dp_Pages]
           ([Sub_Page_ID]
           ,[Display_Name]
           ,[Singular_Name]
		   ,[Page_ID]
           ,[View_Order]
		   ,[Link_To_Page_ID]
		   ,[Link_From_Field_Name]
		   ,[Select_To_Page_ID]
		   ,[Select_From_Field_Name]
		   ,[Primary_Table]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
		   ,[Filter_Key]
		   ,[Relation_Type_ID]
		   ,[On_Quick_Add]
		   ,[Contact_ID_Field]
           ,[Default_View])
     VALUES
           (424
           ,'Subscriptions'
           ,'Subscription'
           ,455
           ,5
           ,417
           ,'Contact_Publication_ID'
			,376
           ,'dp_Contact_Publications.Publication_ID'
           ,'dp_Contact_Publications'           
           ,'Contact_Publication_ID',
		   ,'Publication_ID_Table.Publication_ID
			,Publication_ID_Table.Title AS Publication_Title
			,dp_Contact_Publications.Unsubscribed'
			,'Publication_ID_Table.Title'
			,'Contact_ID'
			,2
			,0
			,'dp_Contact_Publications.Contact_ID'
			,59
)

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF
END
GO