USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Page_Views] WHERE [Page_View_ID] = 2193)
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
           (2193
		   ,'Segmentation: Has Child of Age'
           ,292
           ,'Mailchimp Segmentation -- Preschool Children of n Age'
           ,'Contacts.Contact_ID, Contacts.Nickname, Contacts.Last_Name, Contacts.Email_Address
			,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contacts.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age < 1) AS Has_Infant
			,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contacts.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 1) AS Has_1_Year_Old
			,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contacts.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 2) AS Has_2_Year_Old
			,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contacts.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 3) AS Has_3_Year_Old
			,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contacts.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 4 AND C2.HS_Graduation_Year is NULL) AS Has_PreK_4_Year_Old
			,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contacts.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 5 AND C2.HS_Graduation_Year is NULL) AS Has_PreK_5_Year_Old'
           ,'Contacts.Email_Address is not null AND Contacts.__Age>12')
SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END
GO