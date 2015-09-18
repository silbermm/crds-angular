USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 2180)
BEGIN
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (2180
		   ,'Household Profile'
           ,327
           ,'Household view used by profile page'
           ,'Address_ID_Table.[Address_Line_1], Address_ID_Table.[Address_Line_2], Address_ID_Table.[City], 
		   Address_ID_Table.[State/Region], Address_ID_Table.[Postal_Code], Address_ID_Table.[Foreign_Country], 
		   Households.[Home_Phone], Address_ID_Table.[County], Congregation_ID_Table.[Congregation_ID], Households.[Household_ID]'
           ,'1=1')
SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
GO