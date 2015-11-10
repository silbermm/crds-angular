USE [MinistryPlatform]
GO

IF EXISTS (SELECT * FROM [dbo].[dp_Pages] WHERE [Page_ID] = 376)
BEGIN

UPDATE [dbo].[dp_Pages]
SET 

   [Display_Name] = 'Publications',
   [Singular_Name] = 'Publication',
   [Description] = 'Publications',
   [View_Order] = 3,
   [Table_Name] = 'dp_Publications',
   [Primary_Key] = 'Publication_ID',
   [Display_Search] = 1,
   [Default_Field_List] = 'dp_Publications.Publication_ID AS [ID],
		dp_Publications.Title,
		dp_Publications.Description,
		dp_Publications.Online_Sort_Order,
		dp_Publications.Available_Online', 
   [Selected_Record_Expression] = 'dp_Publications.Title',                      
   [Display_Copy] = 1

WHERE [Page_ID] = 376
		   
END

ELSE

BEGIN
SET IDENTITY_INSERT [dbo].[dp_Pages] ON 

INSERT INTO [dbo].[dp_Pages]
           ([Page_ID]
           ,[Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
		   ,[Display_Search]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]                    
           ,[Display_Copy])
     VALUES
           (376
           ,'Publications'
           ,'Publication'
           ,'Publications'
           ,3
           ,'dp_Publications'
           ,'Publication_ID'
			,1
           ,'dp_Publications.Publication_ID AS [ID]
			,dp_Publications.Title
			,dp_Publications.Description
			,dp_Publications.Online_Sort_Order
			,dp_Publications.Available_Online'
           ,'dp_Publications.Title'           
           ,1)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
END
GO