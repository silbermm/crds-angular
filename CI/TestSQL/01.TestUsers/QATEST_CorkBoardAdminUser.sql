--Registered Account - Corkboard Admin account
USE [MinistryPlatform]
GO

--Registered Account
--mpcrds+cbadmin@gmail.com contact record
DECLARE @contactID as int
set @contactID = (select contact_id from contacts where Email_Address = 'mpcrds+cbadmin@gmail.com' and Last_Name = 'CorkboardAdmin');

DECLARE @userID as int
set @userID =(select user_id from dp_users where contact_id = @contactID)
--Add corkboard admin user role to user account.
INSERT INTO [dbo].Dp_User_Roles (User_ID,Role_ID,Domain_ID) 
VALUES                         (@userID ,67     ,1);
GO