/****** Script for SelectTopNRows command from SSMS  ******/
--/*
SELECT c.First_Name, c.Middle_Name, c.Last_Name, c.Maiden_Name, c.Nickname, c.Email_Address, 
c.Date_of_Birth,
ms.Marital_Status,
g.Gender,
c.Employer_Name, p._First_Attendance_Ever, c.Mobile_Phone,
a.Address_Line_1, a.Address_Line_2, a.City, a.[State/Region], a.Postal_Code, a.County, a.Foreign_Country, h.Home_Phone,
cn.Congregation_Name
--*/
--SELECT a.*
FROM [MinistryPlatform].[dbo].[Contacts] c
INNER JOIN MinistryPlatform.dbo.Households h on c.Household_ID = h.Household_ID
INNER JOIN MinistryPlatform.dbo.Addresses a on h.Address_ID = a.Address_ID
INNER JOIN MinistryPlatform.dbo.Participants p on c.Participant_Record = p.Participant_ID
INNER JOIN MinistryPlatform.dbo.Congregations cn on h.Congregation_ID = cn.Congregation_ID
INNER JOIN MinistryPlatform.dbo.Genders g on c.Gender_ID = g.Gender_ID
INNER JOIN MinistryPlatform.dbo.Marital_Statuses ms on c.Marital_Status_ID = ms.Marital_Status_ID
where c.Contact_ID = 768379

--select * from MinistryPlatform.dbo.Contact_Households where Household_ID = 627743

--SELECT * FROM [MinistryPlatform].[dbo].[Households] where Household_ID = 627743
--SELECT * FROM [MinistryPlatform].[dbo].[Addresses]