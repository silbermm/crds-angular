USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO
DELETE FROM [dbo].[dp_Page_Views]
WHERE Page_View_ID = 92155
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
           ,[View_Title]
           ,[Page_ID]
           ,[Field_List]
           ,[View_Clause]
           ,[Order_By]
           ,[User_ID])
     VALUES
           (92155
           ,'GO Trip Participants'
           ,305
           ,'Event_Participants.[Event_Participant_ID], Event_ID_Table.[Event_ID], Event_ID_Table.[Event_Title], Participant_ID_Table_Contact_ID_Table.[Nickname], Participant_ID_Table_Contact_ID_Table.[Last_Name], Participant_ID_Table_Contact_ID_Table.[Email_Address], Event_ID_Table.[Event_Start_Date], Event_ID_Table.[Event_End_Date], Event_ID_Table_Event_Type_ID_Table.[Event_Type], Participant_ID_Table.[Participant_ID], Participant_ID_Table_Contact_ID_Table.[Nickname] + '' '' + Participant_ID_Table_Contact_ID_Table.[Last_Name] AS [Full_Name], Event_ID_Table_Program_ID_Table.Program_ID, Event_ID_Table_Program_ID_Table.Program_Name,Participant_ID_Table_Contact_ID_Table.Contact_ID,
          Event_ID_Table_Program_ID_Table_Pledge_Campaign_ID_Table.[Pledge_Campaign_ID] AS [Campaign_ID]
          , Event_ID_Table_Program_ID_Table_Pledge_Campaign_ID_Table.[Campaign_Name] AS [Campaign_Name],Participant_ID_Table_Contact_ID_Table_Donor_Record_Table.[Donor_ID] AS [Donor_ID]'
           ,'Event_ID_Table_Event_Type_ID_Table.[Event_Type_ID] = 6 AND (DATEADD(day, 90, Event_ID_Table.[Event_End_Date]) >= GETDATE())'
           ,null
           ,null)
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO
