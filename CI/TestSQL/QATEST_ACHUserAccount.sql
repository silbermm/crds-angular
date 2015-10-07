--Registered Account - This is a user that has ACH saved as payment data in stripe and should not change to credit card. 
USE [MinistryPlatform]
GO

--Registered Account
--Household
SET IDENTITY_INSERT [dbo].[Households] ON;

DECLARE @householdID AS int
set @householdID = (IDENT_CURRENT('households')+1);

INSERT INTO [dbo].Households 
(Household_ID,Household_Name,Address_ID,Home_Phone,Domain_ID,Congregation_ID,Care_Person,Household_Source_ID,Family_Call_Number,Household_Preferences,Home_Phone_Unlisted,Home_Address_Unlisted,Bulk_Mail_Opt_Out,_Last_Donation,_Last_Activity,__ExternalHouseholdID,__ExternalBusinessID) VALUES 
(@householdID,'Fry'         ,null      ,null      ,1        ,5              ,null       ,30                 ,null              ,null                 ,null               ,null                 ,0                ,null          ,null          ,null                 ,null);

SET IDENTITY_INSERT [dbo].[Households] OFF;

--Mpcrds+32@gmail.com contact record
SET IDENTITY_INSERT [dbo].[Contacts] ON;
DECLARE @contactID as int
set @contactID = IDENT_CURRENT('Contacts')+1;

INSERT INTO [dbo].Contacts 
(Contact_ID,Company,Company_Name,Display_Name  ,Prefix_ID,First_Name,Middle_Name,Last_Name,Suffix_ID,Nickname ,Date_of_Birth,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address        ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@contactID,0      ,null        ,'Fry, Phillip',null     ,'Phillip' ,'J'        ,'Fry'    ,null     ,'Phillip',null         ,null     ,null             ,1                ,@householdID,1                    ,null              ,null        ,'mpcrds+32@gmail.com',null          ,0                 ,0               ,null        ,null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

--User Record
SET IDENTITY_INSERT [dbo].[Contacts] OFF;
SET IDENTITY_INSERT [dbo].[Dp_Users] ON;
DECLARE @user_id as int
set @user_id = IDENT_CURRENT('Dp_Users')+1;

INSERT INTO [dbo].Dp_Users 
(User_ID ,User_Name            ,User_Email           ,Display_Name,Password                     ,Admin,Domain_ID,Publications_Manager,Contact_ID,Supervisor,User_GUID,Can_Impersonate,In_Recovery,Time_Zone,Locale,Theme,Setup_Admin,__ExternalPersonID,__ExternalUserID,Data_Service_Permissions,Read_Permitted,Create_Permitted,Update_Permitted,Delete_Permitted) VALUES
(@user_id,'mpcrds+32@gmail.com','mpcrds+32@gmail.com','Phillip'   ,CAST('welcome' as binary(16)),0    ,1        ,0                   ,@contactID,null      ,NEWID()  ,null           ,null       ,null     ,null  ,null ,0          ,null              ,null            ,null                    ,0         ,0           ,0           ,0           );

--Participant RECORD
SET IDENTITY_INSERT [dbo].[Dp_Users] OFF;
SET IDENTITY_INSERT [dbo].[Participants] ON;

DECLARE @part_id as int
set @part_id = IDENT_CURRENT('Participants')+1;

INSERT INTO [dbo].Participants 
(Participant_ID,Contact_ID,Participant_Type_ID,Attend_Start_Date,Participant_Start_Date    ,Participant_End_Date,Notes,Domain_ID,__ExternalPersonID,_First_Attendance_Ever,_Second_Attendance_Ever,_Third_Attendance_Ever,_Last_Attendance_Ever) VALUES
(@part_id      ,@contactID,4                  ,null             ,{ts '2015-07-15 16:18:32'},null                ,null ,1        ,null              ,null                  ,null                   ,null                  ,null                 );

--Donor RECORD
SET IDENTITY_INSERT [dbo].[Participants] OFF;
SET IDENTITY_INSERT [dbo].[Donors] ON;

DECLARE @donor_id as int
set @donor_id =IDENT_CURRENT('Donors')+1;

INSERT INTO [dbo].donors 
(Donor_ID ,Contact_ID,Statement_Frequency_ID,Statement_Type_ID,Statement_Method_ID,Setup_Date                ,Envelope_No,Cancel_Envelopes,Notes,First_Contact_Made,Domain_ID,__ExternalPersonID,_First_Donation_Date,_Last_Donation_Date,Processor_ID        ) VALUES
(@donor_id,@contactID,1                     ,1                ,2                  ,{ts '2015-07-15 16:19:22'},null       ,0               ,null ,null              ,1        ,null              ,null                ,null               ,'cus_6cAhwmmKyaw9D5');

--User ROLE
SET IDENTITY_INSERT [dbo].[Donors] OFF;
SET IDENTITY_INSERT [dbo].[Dp_User_Roles] ON;

DECLARE @user_role_id as int
set @user_role_id =IDENT_CURRENT('Dp_User_Roles')+1;

INSERT INTO [dbo].Dp_User_Roles (User_Role_ID,User_ID,Role_ID,Domain_ID) 
VALUES                         (@user_role_id,@user_id,39    ,1);

SET IDENTITY_INSERT [dbo].[Dp_User_Roles] OFF;

--Update Contact Record
Update [dbo].Contacts set User_account = @user_id where contact_id = @contactID;
update [dbo].Contacts set Participant_Record = @part_id where contact_id = @contactID;
update [dbo].Contacts set Donor_Record = @donor_id where contact_id = @contactID;
GO
