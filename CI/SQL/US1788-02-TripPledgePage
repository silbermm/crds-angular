USE [MinistryPlatform]
GO
/*
SET IDENTITY_INSERT [dbo].[dp_Pages] ON
GO

INSERT INTO [dbo].[dp_Pages]
			([Page_ID]
           ,[Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Display_Search]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
					 ,[Filter_Clause]
           ,[Display_Copy])
     VALUES
			(514,
           'Trip Pledge Campaigns',
           'Trip Pledge Campaign',
           'Efforts by the church to raise funds for GO trips',
           60,
           'Pledge_Campaigns',
           'Pledge_Campaign_ID',
           1,
           'Pledge_Campaigns.Campaign_Name, Pledge_Campaign_Type_ID_Table.Campaign_Type, Pledge_Campaigns.Start_Date, Pledge_Campaigns.End_Date, Pledge_Campaigns.Campaign_Goal, Event_ID_Table.Event_Title ,Program_ID_Table.Program_Name',
           'Campaign_Name',
					 'Pledge_Campaign_Type_ID_Table.[Pledge_Campaign_Type_ID] = 2',
           1)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO
*/

INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
     VALUES
           (514, 9)
GO

INSERT INTO [dbo].[dp_Role_Pages]
           ([Role_ID]
           ,[Page_ID]
           ,[Access_Level]
           ,[Scope_All]
           ,[Approver]
           ,[File_Attacher]
           ,[Data_Importer]
           ,[Data_Exporter]
           ,[Secure_Records]
           ,[Allow_Comments]
           ,[Quick_Add])
     VALUES
           (62,514,0,0,0,0,0,0,0,0,0)
GO

INSERT INTO [dbo].[dp_Role_Pages]
           ([Role_ID]
           ,[Page_ID]
           ,[Access_Level]
           ,[Scope_All]
           ,[Approver]
           ,[File_Attacher]
           ,[Data_Importer]
           ,[Data_Exporter]
           ,[Secure_Records]
           ,[Allow_Comments]
           ,[Quick_Add])
     VALUES
           (2,514,3,0,0,0,0,0,0,0,0)
GO
