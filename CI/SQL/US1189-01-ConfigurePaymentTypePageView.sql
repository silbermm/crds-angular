USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[View_Clause])
     VALUES
           (92148
		   ,'Current Payment Types'
		   ,358
           ,'This is the list of payment types the finance team is able to select from.'
           ,'Payment_Types.Payment_Type_ID != 3 and Payment_Types.Payment_Type_ID != 8')

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO
