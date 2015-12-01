USE [MinistryPlatform]
GO

-- Cleanup old Page_Views from US2615 story
DELETE FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] IN (2193)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 2197)
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
           (2197
		   ,'Segmentation: Base'
           ,417
           ,'Mailchimp Base Segmentation -- Basic information for contacts > 12 years old'
           ,'dp_Contact_Publications.Contact_Publication_ID, Publication_ID_Table.Publication_ID, Publication_ID_Table.Title, Publication_ID_Table.Third_Party_Publication_ID, Publication_ID_Table.Last_Successful_Sync, dp_Contact_Publications.Unsubscribed, dp_Contact_Publications.Third_Party_Contact_ID, Contact_ID_Table.Contact_ID, Contact_ID_Table.Email_Address, Contact_ID_Table.Nickname, Contact_ID_Table.Last_Name, Contact_ID_Table_Gender_ID_Table.Gender, Contact_ID_Table_Marital_Status_ID_Table.Marital_Status'
           ,'Contact_ID_Table.Email_Address IS NOT NULL AND (Contact_ID_Table.__Age>12 OR Contact_ID_Table.__Age is null)')

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF

END

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 2198)
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
           (2198
		   ,'Segmentation: Has Child of Age'
           ,417
           ,'Mailchimp Segmentation -- Preschool Children of an Age'
           ,'dp_Contact_Publications.Contact_Publication_ID, Contact_ID_Table.Contact_ID 
,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contact_ID_Table.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age < 1) AS Has_Infant
,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contact_ID_Table.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 1) AS Has_1_Year_Old
,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contact_ID_Table.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 2) AS Has_2_Year_Old
,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contact_ID_Table.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 3) AS Has_3_Year_Old
,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contact_ID_Table.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 4 AND C2.HS_Graduation_Year is NULL) AS Has_PreK_4_Year_Old
,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contact_ID_Table.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 5 AND C2.HS_Graduation_Year is NULL) AS Has_PreK_5_Year_Old'		   
           ,'Contact_ID_Table.Email_Address IS NOT NULL AND (Contact_ID_Table.__Age>12 OR Contact_ID_Table.__Age is null)')

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
GO

TRUNCATE TABLE dbo.Publication_Page_Views

-- Associate Kids Club with Segmentation: Has Child of Age
INSERT INTO dbo.Publication_Page_Views
	(
		[Publication_ID],
		[Page_View_ID]
	)
	VALUES
	(
		3,
		2198
	)
GO
