USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
		   ,[Field_List]
           ,[View_Clause])
     VALUES
           (92189
		   ,'Trip Applicant Form Responses'
		   ,424
           ,NULL
		   ,'Form_Responses.[Response_Date] AS [Response Date], Form_ID_Table.[Form_Title] AS [Form Title], Contact_ID_Table.[Display_Name] AS [Display Name], Pledge_Campaign_ID_Table.[Campaign_Name] AS [Campaign Name]'
           ,'Pledge_Campaign_ID_Table_Program_ID_Table_Program_Type_ID_Table.[Program_Type_ID] = 3')

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO