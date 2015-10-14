USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (2187
		   ,'DI - Fix Too Many Heads'
           ,327
           ,'Data Integrity View - Households should have no more than 2 heads. Please review and correct'
           ,NULL
           ,'EXISTS (SELECT 1 FROM Contacts C WHERE C.Household_ID = Households.Household_ID AND Household_Position_ID = 1 AND C.Contact_Status_ID = 1 GROUP BY Household_ID, C.Contact_Status_ID HAVING Count(*)>2)')

GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO



SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON
GO


INSERT INTO [dbo].[dp_Sub_Page_Views]
           ([Sub_Page_view_ID]
           ,[View_Title]
           ,[Sub_Page_ID]           
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (104
          ,'Heads'
          ,307         
          ,'Household_Position_ID_Table.[Household_Position],Contact_Status_ID_Table.[Contact_Status],
          Contacts.[Display_Name]'
          ,'Household_Position_ID_Table.[Household_Position] = ''Head of Household''
          AND Contact_Status_ID_Table.[Contact_Status_ID] = ''1''')
GO

SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] OFF
GO

