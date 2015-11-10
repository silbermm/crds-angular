USE [MinistryPlatform]
GO

--Address
SET IDENTITY_INSERT [dbo].[Addresses] ON;

DECLARE @addressId as int
set @addressId = IDENT_CURRENT('Addresses')+1;

INSERT INTO [dbo].Addresses 
(Address_ID,Address_Line_1        ,Address_Line_2,City  ,[State/Region],Postal_Code,Foreign_Country,Country_Code,Domain_ID,Carrier_Route,Lot_Number,Delivery_Point_Code,Delivery_Point_Check_Digit,Latitude,Longitude,Altitude,Time_Zone,Bar_Code,Area_Code,Last_Validation_Attempt,County,Validated,Do_Not_Validate,Last_GeoCode_Attempt,__ExternalAddressID) VALUES
(@addressID,'123 Midgar Section 6',null          ,'CITY','OH'          ,'45209'    ,'United States','USA'       ,1        ,null         ,null      ,null               ,null                      ,null    ,null     ,null    ,null     ,null    ,null     ,null                   ,null  ,null     ,null           ,null                ,null               );

SET IDENTITY_INSERT [dbo].[Addresses] OFF;

--Household
SET IDENTITY_INSERT [dbo].[Households] ON;

DECLARE @houseHoldID as int
set @houseHoldID = IDENT_CURRENT('Households')+1;

INSERT INTO [dbo].Households (Household_ID,  Household_Name,Address_ID,    Home_Phone,Domain_ID,Congregation_ID,Care_Person,Household_Source_ID,Family_Call_Number,Household_Preferences,Home_Phone_Unlisted,Home_Address_Unlisted,Bulk_Mail_Opt_Out,_Last_Donation,_Last_Activity,__ExternalHouseholdID,__ExternalBusinessID) 
VALUES 						 (@houseHoldID,'Strife'        ,@addressId,'123-765-4323',1        ,6              ,null       ,38                 ,null              ,null                 ,null               ,null                 ,0                ,null          ,null          ,null                 ,null);

SET IDENTITY_INSERT [dbo].[Households] OFF;

--Cloud Strife Contact
SET IDENTITY_INSERT [dbo].[Contacts] ON;

--Store the current identity value so we can reset it.
DECLARE @currentContactId as int
set @currentContactId = IDENT_CURRENT('Contacts');

DECLARE @contactID as int
set @contactID = 100000000;

INSERT INTO [dbo].Contacts 
(Contact_ID,Company,Company_Name,Display_Name   ,Prefix_ID,First_Name,Middle_Name,Last_Name   ,Suffix_ID,Nickname ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address                  ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone ,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID ,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@contactID,0      ,null        ,'Strife, Cloud',1        ,'Cloud'   ,'S'        ,'Strife'    ,null     ,'Cloud'  ,{d '1975-01-01'},1        ,2                ,1                ,@houseHoldID,1                    ,null              ,null        ,'mpcrds+CloudStrife@gmail.com' ,null          ,0                 ,0               ,'513-654-8745',null          ,null                 ,'555-365-4125',null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()      ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );
SET IDENTITY_INSERT [dbo].[Contacts] OFF;

--This command resets the identity value so that if someone adds contacts a big ID. 
DBCC CHECKIDENT (Contacts, reseed, @currentContactId);

--Cloud Strife Participant Record
SET IDENTITY_INSERT [dbo].[Participants] ON;

DECLARE @partID as int
set @partID = IDENT_CURRENT('Participants')+1;

INSERT INTO [dbo].Participants 
(Participant_ID,Contact_ID,Participant_Type_ID,Attend_Start_Date,Participant_Start_Date    ,Participant_End_Date,Notes                       ,Domain_ID,__ExternalPersonID,_First_Attendance_Ever,_Second_Attendance_Ever,_Third_Attendance_Ever,_Last_Attendance_Ever) VALUES
(@partID       ,@contactID,4                  ,null             ,{ts '2015-07-02 08:15:05'},null                ,'Created by Add Family Tool',1        ,null              ,null                  ,null                   ,null                  ,null                 );

SET IDENTITY_INSERT [dbo].[Participants] OFF;

-- Cloud Strife Donor RECORD
SET IDENTITY_INSERT [dbo].[Donors] ON;

DECLARE @donor_id as int
set @donor_id =IDENT_CURRENT('Donors')+1;

INSERT INTO [dbo].Donors 
( Donor_ID,Contact_ID,Statement_Frequency_ID,Statement_Type_ID,Statement_Method_ID,Setup_Date                ,Envelope_No,Cancel_Envelopes,Notes,First_Contact_Made,Domain_ID,__ExternalPersonID,_First_Donation_Date,_Last_Donation_Date,Processor_ID) VALUES
(@donor_id,@contactID,3                     ,1                ,4                  ,{ts '2015-07-06 12:03:37'},null       ,0               ,null ,null              ,1        ,null              ,null                ,null               ,null);

SET IDENTITY_INSERT [dbo].[Donors] OFF;

--Cloud Strife User Record
SET IDENTITY_INSERT [dbo].[Dp_Users] ON;

DECLARE @userID as int
set @userID = IDENT_CURRENT('Dp_Users')+1;

INSERT INTO [dbo].dp_users 
(User_ID,User_Name                      ,User_Email                     ,Display_Name,Password                     ,Admin,Domain_ID,Publications_Manager,Contact_ID,Supervisor,User_GUID,Can_Impersonate,In_Recovery,Time_Zone,Locale,Theme,Setup_Admin,__ExternalPersonID,__ExternalUserID,Data_Service_Permissions,Read_Permitted,Create_Permitted,Update_Permitted,Delete_Permitted) VALUES
(@userID,'mpcrds+CloudStrife@gmail.com' ,'mpcrds+CloudStrife@gmail.com' ,'Cloud'     ,CAST('welcome' as binary(16)),0    ,1        ,0                   ,@contactID,null      ,NEWID()  ,null           ,null       ,null     ,null  ,null ,0          ,null              ,null            ,null                    ,0             ,0               ,0               ,0               );

SET IDENTITY_INSERT [dbo].[Dp_Users] OFF;

--Cloud Strife User ROLE
SET IDENTITY_INSERT [dbo].[Dp_User_Roles] ON;

DECLARE @user_role_id as int
set @user_role_id =IDENT_CURRENT('Dp_User_Roles')+1;

INSERT INTO [dbo].Dp_User_Roles (User_Role_ID,User_ID,Role_ID,Domain_ID) 
VALUES                         (@user_role_id,@userID,39    ,1);

SET IDENTITY_INSERT [dbo].[Dp_User_Roles] OFF;

--Cloud Strife Updates
update [dbo].Contacts set Participant_Record = @partID where Contact_ID = @contactID;
update [dbo].Contacts set User_Account = @userID where Contact_ID = @contactID;
update [dbo].Contacts set Donor_Record = @donor_id where Contact_ID = @contactID;
GO

--Household for Tifa Lockhart
DECLARE @houseHoldID as int
set @houseHoldID = (select houseHold_ID from Households where Household_Name = 'Strife');

SET IDENTITY_INSERT [dbo].[Contacts] ON;

--Store the current identity value so we can reset it.
DECLARE @currentContactId as int
set @currentContactId = IDENT_CURRENT('Contacts');

DECLARE @contactID as int
set @contactID = 100000001;

--Tifa Lockhart contact record
INSERT INTO [dbo].Contacts 
(Contact_ID,Company,Company_Name,Display_Name    ,Prefix_ID,First_Name,Middle_Name,Last_Name ,Suffix_ID,Nickname,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address                  ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@contactID,0      ,null        ,'Lockhart, Tifa',2        ,'Tifa'    ,'A'        ,'Lockhart',null     ,'Tifa'  ,{d '1975-01-01'},2        ,2                ,1                ,@houseHoldID,1                    ,null              ,null        ,'mpcrds+tifalockhart@gmail.com',null          ,0                 ,0               ,'321-444-8184',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET IDENTITY_INSERT [dbo].[Contacts] OFF;

--This command resets the identity value so that if someone adds contacts a big ID. 
DBCC CHECKIDENT (Contacts, reseed, @currentContactId);

--Tifa Lockhart Participant record
SET IDENTITY_INSERT [dbo].[Participants] ON;

DECLARE @partID as int
set @partID = IDENT_CURRENT('Participants')+1;

INSERT INTO [dbo].Participants 
(Participant_ID,Contact_ID,Participant_Type_ID,Attend_Start_Date,Participant_Start_Date    ,Participant_End_Date,Notes                       ,Domain_ID,__ExternalPersonID,_First_Attendance_Ever,_Second_Attendance_Ever,_Third_Attendance_Ever,_Last_Attendance_Ever) VALUES
(@partID       ,@contactID,4                  ,null             ,{ts '2015-07-02 08:15:06'},null                ,'Created by Add Family Tool',1        ,null              ,null                  ,null                   ,null                  ,null                 );

SET IDENTITY_INSERT [dbo].[Participants] OFF;

--Tifa Lockhart User Record
SET IDENTITY_INSERT [dbo].[Dp_Users] ON;

DECLARE @userID as int
set @userID = IDENT_CURRENT('Dp_Users')+1;

INSERT INTO [dbo].dp_users 
(User_ID,User_Name                       ,User_Email                     ,Display_Name,Password                     ,Admin,Domain_ID,Publications_Manager,Contact_ID,Supervisor,User_GUID,Can_Impersonate,In_Recovery,Time_Zone,Locale,Theme,Setup_Admin,__ExternalPersonID,__ExternalUserID,Data_Service_Permissions,Read_Permitted,Create_Permitted,Update_Permitted,Delete_Permitted) VALUES
(@userID,'mpcrds+tifalockhart@gmail.com' ,'mpcrds+tifalockhart@gmail.com','Tifa'      ,CAST('welcome' as binary(16)),0    ,1        ,0                   ,@contactID,null      ,NEWID()  ,null           ,null       ,null     ,null  ,null ,0          ,null              ,null            ,null                    ,0             ,0               ,0               ,0               );

SET IDENTITY_INSERT [dbo].[Dp_Users] OFF;

--Tifa Lockhart User ROLE
SET IDENTITY_INSERT [dbo].[Dp_User_Roles] ON;

DECLARE @user_role_id as int
set @user_role_id =IDENT_CURRENT('Dp_User_Roles')+1;

INSERT INTO [dbo].Dp_User_Roles (User_Role_ID,User_ID,Role_ID,Domain_ID) 
VALUES                         (@user_role_id,@userID,39    ,1);

SET IDENTITY_INSERT [dbo].[Dp_User_Roles] OFF;

-- Tifa Lockhart Donor RECORD
SET IDENTITY_INSERT [dbo].[Donors] ON;

DECLARE @donor_id as int
set @donor_id =IDENT_CURRENT('Donors')+1;

INSERT INTO [dbo].Donors 
( Donor_ID,Contact_ID,Statement_Frequency_ID,Statement_Type_ID,Statement_Method_ID,Setup_Date                ,Envelope_No,Cancel_Envelopes,Notes,First_Contact_Made,Domain_ID,__ExternalPersonID,_First_Donation_Date,_Last_Donation_Date,Processor_ID) VALUES
(@donor_id,@contactID,3                     ,1                ,4                  ,{ts '2015-07-06 12:03:37'},null       ,0               ,null ,null              ,1        ,null              ,null                ,null               ,null);

SET IDENTITY_INSERT [dbo].[Donors] OFF;

--Tifa Lockhart Updates
update [dbo].Contacts set Participant_Record = @partID where Contact_ID = @contactID;
update [dbo].Contacts set User_Account = @userID where Contact_ID = @contactID;
update [dbo].Contacts set Donor_Record = @donor_id where Contact_ID = @contactID;
GO

--Marlene Wallace (age 14)
DECLARE @houseHoldID as int
set @houseHoldID = (select houseHold_ID from Households where Household_Name = 'Strife');

--Marlene Wallace Contact
SET IDENTITY_INSERT [dbo].[Contacts] ON;

--Store the current identity value so we can reset it.
DECLARE @currentContactId as int
set @currentContactId = IDENT_CURRENT('Contacts');

DECLARE @contactID as int
set @contactID = 100000002;

INSERT INTO [dbo].Contacts 
(Contact_ID,Company,Company_Name,Display_Name      ,Prefix_ID,First_Name,Middle_Name,Last_Name,Suffix_ID,Nickname ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address                    ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@contactID,0      ,null        ,'Wallace, Marlene',null     ,'Marlene' ,null       ,'Wallace',null     ,'Marlene',{d '2001-01-01'},2        ,1                ,1                ,@houseHoldID,2                    ,null              ,null        ,'mpcrds+marlenewallace@gmail.com',null          ,0                 ,0               ,'123-548-4232',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET IDENTITY_INSERT [dbo].[Contacts] OFF;

--This command resets the identity value so that if someone adds contacts a big ID. 
DBCC CHECKIDENT (Contacts, reseed, @currentContactId);

--Marlene Wallace Participant Record
SET IDENTITY_INSERT [dbo].[Participants] ON;

DECLARE @partID as int
set @partID = IDENT_CURRENT('Participants')+1;

INSERT INTO [dbo].Participants 
(Participant_ID,Contact_ID,Participant_Type_ID,Attend_Start_Date,Participant_Start_Date    ,Participant_End_Date,Notes                       ,Domain_ID,__ExternalPersonID,_First_Attendance_Ever,_Second_Attendance_Ever,_Third_Attendance_Ever,_Last_Attendance_Ever) VALUES
(@partID       ,@contactID,4                  ,null             ,{ts '2015-07-02 08:15:06'},null                ,'Created by Add Family Tool',1        ,null              ,null                  ,null                   ,null                  ,null                 );

SET IDENTITY_INSERT [dbo].[Participants] OFF;

--Marlene Wallace USER
SET IDENTITY_INSERT [dbo].[Dp_Users] ON;

DECLARE @userID as int
set @userID = IDENT_CURRENT('Dp_Users')+1;

INSERT INTO [dbo].dp_users 
(User_ID,User_Name                        ,User_Email                       ,Display_Name,Password                     ,Admin,Domain_ID,Publications_Manager,Contact_ID,Supervisor,User_GUID,Can_Impersonate,In_Recovery,Time_Zone,Locale,Theme,Setup_Admin,__ExternalPersonID,__ExternalUserID,Data_Service_Permissions,Read_Permitted,Create_Permitted,Update_Permitted,Delete_Permitted) VALUES
(@userID,'mpcrds+marlenewallace@gmail.com','mpcrds+marlenewallace@gmail.com','Marlene'   ,CAST('welcome' as binary(16)),0    ,1        ,0                   ,@contactID,null      ,NEWID()  ,null           ,null       ,null     ,null  ,null ,0          ,null              ,null            ,null                    ,0             ,0               ,0               ,0           );

SET IDENTITY_INSERT [dbo].[Dp_Users] OFF;

--Marlene Wallace User ROLE
SET IDENTITY_INSERT [dbo].[Dp_User_Roles] ON;

DECLARE @user_role_id as int
set @user_role_id =IDENT_CURRENT('Dp_User_Roles')+1;

INSERT INTO [dbo].Dp_User_Roles (User_Role_ID,User_ID,Role_ID,Domain_ID) 
VALUES                         (@user_role_id,@userID,39    ,1);

SET IDENTITY_INSERT [dbo].[Dp_User_Roles] OFF;

-- Marlene Wallace Donor RECORD
SET IDENTITY_INSERT [dbo].[Donors] ON;

DECLARE @donor_id as int
set @donor_id =IDENT_CURRENT('Donors')+1;

INSERT INTO [dbo].Donors 
( Donor_ID,Contact_ID,Statement_Frequency_ID,Statement_Type_ID,Statement_Method_ID,Setup_Date                ,Envelope_No,Cancel_Envelopes,Notes,First_Contact_Made,Domain_ID,__ExternalPersonID,_First_Donation_Date,_Last_Donation_Date,Processor_ID) VALUES
(@donor_id,@contactID,3                     ,1                ,4                  ,{ts '2015-07-06 12:03:37'},null       ,0               ,null ,null              ,1        ,null              ,null                ,null               ,null);

SET IDENTITY_INSERT [dbo].[Donors] OFF;

--Marlene Wallace Updates
update [dbo].Contacts set Participant_Record = @partID where Contact_ID = @contactID;
update [dbo].Contacts set Donor_Record = @donor_id where Contact_ID = @contactID;
update [dbo].Contacts set User_Account = @userID where Contact_id = @contactID;
GO

--Cloud Strife's Request to Join Kids' Club Response Record
SET IDENTITY_INSERT [dbo].[Responses] ON;

DECLARE @respID as int
set @respID = IDENT_CURRENT('Responses')+1;

DECLARE @partID as int
set @partID = (select Participant_Record from Contacts where email_address = 'mpcrds+CloudStrife@gmail.com');

INSERT INTO [dbo].Responses 
(Response_ID,Response_Date             ,Opportunity_ID,Participant_ID,Comments                        ,Website_Submission,First_Name,Last_Name,Email,Phone,Follow_up_Information,Response_Result_ID,Closed,Domain_ID,Event_ID,__ExternalCESTPID) VALUES
(@respID    ,{ts '2015-07-02 08:17:17'},115           ,@partID       ,'Request on 7/2/2015 8:17:17 AM',null              ,null      ,null     ,null ,null ,null                 ,null              ,0     ,1        ,null    ,null             );

SET IDENTITY_INSERT [dbo].[Responses] OFF;
GO

--Marlene Wallace's Request to Join Kids' Club Response Record
SET IDENTITY_INSERT [dbo].[Responses] ON;

DECLARE @respID as int
set @respID = IDENT_CURRENT('Responses')+1;

DECLARE @partID as int
set @partID = (select Participant_Record from Contacts where email_address = 'mpcrds+marlenewallace@gmail.com');

INSERT INTO [dbo].Responses 
(Response_ID,Response_Date             ,Opportunity_ID,Participant_ID,Comments                        ,Website_Submission,First_Name,Last_Name,Email,Phone,Follow_up_Information,Response_Result_ID,Closed,Domain_ID,Event_ID,__ExternalCESTPID) VALUES
(@respID    ,{ts '2015-07-02 08:20:21'},115           ,@partID       ,'Request on 7/2/2015 8:20:21 AM',null              ,null      ,null     ,null ,null ,null                 ,null              ,0 ,1        ,null    ,null             )

SET IDENTITY_INSERT [dbo].[Responses] OFF;
GO

--Family Relationships
DECLARE @cloudContact as int
set @cloudContact = (select Contact_ID from Contacts where email_address = 'mpcrds+CloudStrife@gmail.com');

DECLARE @tifaContact as int
set @tifaContact = (select Contact_ID from Contacts where email_address = 'mpcrds+tifalockhart@gmail.com');

DECLARE @marleneContact as int
set @marleneContact = (select Contact_ID from Contacts where email_address = 'mpcrds+marlenewallace@gmail.com');


--Cloud married to Tifa
INSERT INTO [dbo].Contact_Relationships 
(Contact_ID ,Relationship_ID,Related_Contact_ID,Start_Date                ,End_Date,Domain_ID,Notes                       ,_Triggered_By) VALUES
(@cloudContact,1              ,@tifaContact       ,{ts '2015-07-02 08:15:06'},null    ,1        ,'Created by Add Family Tool',null      );

--Cloud Foster Parent of Marlene Parent of Kids
INSERT INTO [dbo].Contact_Relationships 
(Contact_ID   ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@cloudContact,21           ,@marleneContact     ,null      ,null    ,1        ,null ,null      );


--Tifa foster Parent Of Marlene
INSERT INTO [dbo].Contact_Relationships 
(Contact_ID  ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@tifaContact,21             ,@marleneContact   ,null      ,null    ,1        ,null ,null      );

SET IDENTITY_INSERT [dbo].[Contact_Relationships] OFF;
GO

--Some Groups for the family so sign up to serve is functional
--Cloud Signs up for KC Florence Nursery!
INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                   ,Participant_ID                                                                                 ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) KC Florence Nursery') ,(select Participant_record from contacts where email_address = 'mpcrds+CloudStrife@gmail.com') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

--Tifa signs up for FI Florence Parking and the Nursery!
INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                   ,Participant_ID                                                                                  ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) KC Florence Nursery') ,(select Participant_record from contacts where email_address = 'mpcrds+tifalockhart@gmail.com') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                         ,Participant_ID                                                                                  ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select top 1 group_id FROM groups where group_name = '(d) FI Florence Parking') ,(select Participant_record from contacts where email_address = 'mpcrds+tifalockhart@gmail.com') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

--Marlene signs up for  FI florence coffee
INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                  ,Participant_ID                                                                                    ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) FI Florence Coffee') ,(select Participant_record from contacts where email_address = 'mpcrds+marlenewallace@gmail.com') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

--Everyone signs up for Oakley Coffee
INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                ,Participant_ID                                                                                 ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) FI Oakley Coffee') ,(select Participant_record from contacts where email_address = 'mpcrds+CloudStrife@gmail.com') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                ,Participant_ID                                                                                  ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) FI Oakley Coffee') ,(select Participant_record from contacts where email_address = 'mpcrds+tifalockhart@gmail.com') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                ,Participant_ID                                                                                    ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) FI Oakley Coffee') ,(select Participant_record from contacts where email_address = 'mpcrds+marlenewallace@gmail.com') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );
GO