USE [MinistryPlatform]
GO


UPDATE [dbo].[dp_Sub_Page_Views]
   SET 
      [Field_List] = N'Event_ID_Table.[Event_ID], Event_ID_Table.[Event_Title], Event_ID_Table_Congregation_ID_Table.[Congregation_Name], Event_ID_Table.[Event_Start_Date],Event_ID_Table.[Event_End_Date]'
      
 WHERE Sub_Page_View_ID = 111
GO


