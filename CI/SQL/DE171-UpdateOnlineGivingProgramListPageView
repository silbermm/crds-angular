USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO
DELETE FROM [dbo].[dp_Page_Views]
WHERE Page_View_ID = 1038
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
           ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause]
           ,[Order_By]
           ,[User_ID])
     VALUES
           (1038
           ,'Online Giving Program List'
           ,375
           ,''
           ,'Programs.[Program_ID] AS [Program_ID] , Programs.[Program_Name] AS [Program_Name] , Program_Type_ID_Table.[Program_Type] AS [Program_Type] , Program_Type_ID_Table.[Program_Type_ID] AS [Program_Type_ID], Programs.[Online_Sort_Order] AS [Online_Sort_Order]'
           ,'Programs.Allow_Online_Giving = 1 AND GetDate() BETWEEN Programs.Start_Date AND ISNULL(Programs.End_Date, GetDate())'
           ,'Programs.Online_Sort_Order,Programs.Statement_Title'
           ,null)
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO
