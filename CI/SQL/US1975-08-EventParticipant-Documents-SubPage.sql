USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON
GO

INSERT INTO [dbo].[dp_Sub_Pages]
           ([Sub_Page_ID], [Display_Name]
           ,[Singular_Name]
           ,[Page_ID]
           ,[View_Order]
           ,[Primary_Table]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Filter_Key]
           ,[Relation_Type_ID]
           ,[Display_Copy])
     VALUES
           (536, 'Documents'
           ,'Document'
           ,305
           ,9
           ,'cr_EventParticipant_Documents'
           ,'EventParticipant_Document_ID'
           ,'Document_ID_Table.Document, cr_EventParticipant_Documents.Received, cr_EventParticipant_Documents.Notes'
           ,'Document_ID_Table.Document'
           ,'Event_Participant_ID'
           ,1
           ,0)
GO

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF
GO

INSERT INTO [dbo].[dp_Role_Sub_Pages]
           ([Role_ID]
           ,[Sub_Page_ID]
           ,[Access_Level])
     VALUES
           (2
           ,536
           ,3)
GO

INSERT INTO [dbo].[dp_Role_Sub_Pages]
           ([Role_ID]
           ,[Sub_Page_ID]
           ,[Access_Level])
     VALUES
           (62
           ,536
           ,3)
GO
