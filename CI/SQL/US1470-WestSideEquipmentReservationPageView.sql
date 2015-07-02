USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           ('West Side Equipment Reservations'
           ,302
           ,'Equipment Reservations for the West Side location only'
           ,'Equipment_ID_Table_Room_ID_Table_Building_ID_Table.[Building_Name], Equipment_ID_Table.[Equipment_Name], Equipment_ID_Table_Equipment_Type_ID_Table.[Equipment_Type], Equipment_ID_Table.[Bookable], Event_ID_Table.[Event_Title], Event_ID_Table.[Event_Start_Date], Equipment_ID_Table_Equipment_Coordinator_Table.[User_Name] AS [Equipment Coordinator]'
           ,'Room_ID_Table_Building_ID_Table.[Building_ID] = 6')
GO
