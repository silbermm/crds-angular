USE MinistryPlatform
GO

-- Update names of columns returned to match up with MailChimp names which are limited in length
UPDATE dbo.dp_Page_Views 
SET
	Field_List = 'dp_Contact_Publications.Contact_Publication_ID, Publication_ID_Table.Publication_ID, Publication_ID_Table.Title, Publication_ID_Table.Third_Party_Publication_ID, Publication_ID_Table.Last_Successful_Sync, dp_Contact_Publications.Unsubscribed, dp_Contact_Publications.Third_Party_Contact_ID, Contact_ID_Table.Contact_ID, Contact_ID_Table.Email_Address, Contact_ID_Table.Nickname AS FNAME, Contact_ID_Table.Last_Name AS LNAME, Contact_ID_Table_Gender_ID_Table.Gender AS GENDER, Contact_ID_Table_Marital_Status_ID_Table.Marital_Status as MARITAL'
WHERE Page_View_ID = 2197


UPDATE dbo.dp_Page_Views 
SET
	Field_List = 'dp_Contact_Publications.Contact_Publication_ID, dp_Contact_Publications.Publication_ID, Contact_ID_Table.Contact_ID 
,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contact_ID_Table.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age < 1) AS HAS_INFANT
,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contact_ID_Table.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 1) AS HAS_1_YEAR
,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contact_ID_Table.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 2) AS HAS_2_YEAR
,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contact_ID_Table.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 3) AS HAS_3_YEAR
,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contact_ID_Table.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 4 AND C2.HS_Graduation_Year is NULL) AS HAS_PREK_4
,(Select Count(*) FROM Contacts C2, Contact_Relationships R WHERE Contact_ID_Table.Contact_ID = R.Contact_ID AND C2.Contact_ID = R.Related_Contact_ID AND (R.End_Date is null OR R.End_Date>getDate()) AND C2.__Age = 5 AND C2.HS_Graduation_Year is NULL) AS HAS_PREK_5'
WHERE Page_View_ID = 2198

GO