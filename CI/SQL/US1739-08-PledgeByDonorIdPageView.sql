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
           (92187
		   ,'Pledge By Donor Id'
		   ,363
           ,NULL
		   ,'Pledges.[Pledge_ID], Pledge_Campaign_ID_Table.[Pledge_Campaign_ID], Donor_ID_Table.[Donor_ID], Pledge_Status_ID_Table.[Pledge_Status_ID]'
           ,'Pledges.[Pledge_ID] IS NOT NULL')

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO