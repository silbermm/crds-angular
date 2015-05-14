USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON
GO
DELETE FROM [dbo].[dp_Sub_Page_Views]
WHERE Sub_Page_View_ID = 88
GO

INSERT INTO [dbo].[dp_Sub_Page_Views]
           ([Sub_Page_View_ID]
           ,[View_Title]
           ,[Sub_Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause]
           ,[Order_By]
           ,[User_ID])
     VALUES
           (88
           ,'All with ContactId'
           ,298
           ,'All group participants with contact id and participant id'
           ,'Participant_ID_Table_Contact_ID_Table.[Contact_ID] AS [Contact_ID] , Participant_ID_Table_Contact_ID_Table.[First_Name] AS [First_Name] , Participant_ID_Table_Contact_ID_Table.[Last_Name] AS [Last_Name] , Participant_ID_Table_Contact_ID_Table.[Nickname] AS [Nickname] , Group_Role_ID_Table.[Group_Role_ID] AS [Group_Role_ID] , Group_Role_ID_Table.[Role_Title] AS [Role_Title] , Participant_ID_Table.[Participant_ID] AS [Participant_ID]'
           ,'GetDate() BETWEEN CONVERT(DATE,Group_Participants.Start_Date) AND ISNULL(Group_Participants.End_Date,GetDate())'
           ,null
           ,null)
GO

SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] OFF
GO
