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
           (92188
		   ,'Group Participants By Id'
		   ,316
           ,NULL
		   ,'Group_Participants.[Group_Participant_ID], Group_ID_Table.[Group_ID], Participant_ID_Table.[Participant_ID]'
           ,'Group_Participants.[Group_Participant_ID] IS NOT NULL')

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO