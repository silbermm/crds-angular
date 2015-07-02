USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           ('Florence Equipment'
           ,301
           ,'Equipment for the Florence location only'
           ,'Room_ID_Table_Building_ID_Table.[Building_Name], Equipment.[Equipment_Name], Equipment_Type_ID_Table.[Equipment_Type], Equipment.[Bookable], Equipment_Coordinator_Table.[User_Name] AS [Equipment Coordinator]'
           ,'Room_ID_Table_Building_ID_Table.[Building_ID] = 5')
GO