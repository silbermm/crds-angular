USE [MinistryPlatform]
GO

--Address
SET IDENTITY_INSERT [dbo].[Addresses] ON;

DECLARE @addressId as int
set @addressId = IDENT_CURRENT('Addresses')+1;

INSERT INTO [dbo].Addresses (Address_ID,Address_Line_1,Address_Line_2,City,[State/Region],Postal_Code,Foreign_Country,Country_Code,Domain_ID,Carrier_Route,Lot_Number,Delivery_Point_Code,Delivery_Point_Check_Digit,Latitude,Longitude,Altitude,Time_Zone,Bar_Code,Area_Code,Last_Validation_Attempt,County,Validated,Do_Not_Validate,Last_GeoCode_Attempt,__ExternalAddressID) 
VALUES (@addressID,'1234 Rich Testing Lane',null,'CITY','OH','45209','United States','USA',1,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null);

SET IDENTITY_INSERT [dbo].[Addresses] OFF;

--Household
SET IDENTITY_INSERT [dbo].[Households] ON;

DECLARE @houseHoldID as int
set @houseHoldID = IDENT_CURRENT('Households')+1;

INSERT INTO [dbo].Households (Household_ID,  Household_Name,Address_ID,    Home_Phone,Domain_ID,Congregation_ID,Care_Person,Household_Source_ID,Family_Call_Number,Household_Preferences,Home_Phone_Unlisted,Home_Address_Unlisted,Bulk_Mail_Opt_Out,_Last_Donation,_Last_Activity,__ExternalHouseholdID,__ExternalBusinessID) 
VALUES 						 (@houseHoldID,'Theil-Test'    ,@addressId,'123-867-5309',1        ,6              ,null       ,38                 ,null              ,null                 ,null               ,null                 ,0                ,null          ,null          ,null                 ,null);

SET IDENTITY_INSERT [dbo].[Households] OFF;

--Rich Theil-Test Contact
SET IDENTITY_INSERT [dbo].[Contacts] ON;

DECLARE @contactID as int
set @contactID = IDENT_CURRENT('Contacts')+1;

INSERT INTO [dbo].Contacts 
(Contact_ID,Company,Company_Name,Display_Name      ,Prefix_ID,First_Name,Middle_Name,Last_Name   ,Suffix_ID,Nickname ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address                  ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone ,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID ,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@contactID,0      ,null        ,'Theil-Test, Rich',1        ,'Rich'   ,'Brian'    ,'Theil-Test',null     ,'Rich'   ,{d '1975-01-01'},1        ,2                ,1                ,@houseHoldID,1                    ,null              ,null        ,'rtheil+Testing@crossroads.net',null          ,0                 ,0               ,'513-654-8745',null          ,null                 ,'555-365-4125',null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()      ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );
SET IDENTITY_INSERT [dbo].[Contacts] OFF;

--Rich Theil-Test Participant Record
SET IDENTITY_INSERT [dbo].[Participants] ON;

DECLARE @partID as int
set @partID = IDENT_CURRENT('Participants')+1;

INSERT INTO [dbo].Participants 
(Participant_ID,Contact_ID,Participant_Type_ID,Attend_Start_Date,Participant_Start_Date    ,Participant_End_Date,Notes                       ,Domain_ID,__ExternalPersonID,_First_Attendance_Ever,_Second_Attendance_Ever,_Third_Attendance_Ever,_Last_Attendance_Ever) VALUES
(@partID       ,@contactID,4                  ,null             ,{ts '2015-07-02 08:15:05'},null                ,'Created by Add Family Tool',1        ,null              ,null                  ,null                   ,null                  ,null                 );

SET IDENTITY_INSERT [dbo].[Participants] OFF;

--Rich Theil-Test User Record
SET IDENTITY_INSERT [dbo].[Dp_Users] ON;

DECLARE @userID as int
set @userID = IDENT_CURRENT('Dp_Users')+1;

INSERT INTO [dbo].dp_users 
(User_ID,User_Name                      ,User_Email                     ,Display_Name,Password                     ,Admin,Domain_ID,Publications_Manager,Contact_ID,Supervisor,User_GUID,Can_Impersonate,In_Recovery,Time_Zone,Locale,Theme,Setup_Admin,__ExternalPersonID,__ExternalUserID,Data_Service_Permissions,Read_Permitted,Create_Permitted,Update_Permitted,Delete_Permitted) VALUES
(@userID,'rtheil+Testing@crossroads.net','rtheil+Testing@crossroads.net','Rich'      ,CAST('welcome' as binary(16)),0    ,1        ,0                   ,@contactID,null      ,NEWID()  ,null           ,null       ,null     ,null  ,null ,0          ,null              ,null            ,null                    ,0             ,0               ,0               ,0               );

SET IDENTITY_INSERT [dbo].[Dp_Users] OFF;

--Rich Theil-Test User ROLE
SET IDENTITY_INSERT [dbo].[Dp_User_Roles] ON;

DECLARE @user_role_id as int
set @user_role_id =IDENT_CURRENT('Dp_User_Roles')+1;

INSERT INTO [dbo].Dp_User_Roles (User_Role_ID,User_ID,Role_ID,Domain_ID) 
VALUES                         (@user_role_id,@userID,39    ,1);

SET IDENTITY_INSERT [dbo].[Dp_User_Roles] OFF;

--Rich Theil-Test Updates
update [dbo].Contacts set Participant_Record = @partID where Contact_ID = @contactID;
update [dbo].Contacts set User_Account = @userID where Contact_ID = @contactID;
GO

--Household for Wife Theil-Test
DECLARE @houseHoldID as int
set @houseHoldID = (select houseHold_ID from Households where Household_Name = 'Theil-Test');

SET IDENTITY_INSERT [dbo].[Contacts] ON;

DECLARE @contactID as int
set @contactID = IDENT_CURRENT('Contacts')+1;

INSERT INTO [dbo].Contacts 
(Contact_ID,Company,Company_Name,Display_Name      ,Prefix_ID,First_Name,Middle_Name,Last_Name   ,Suffix_ID,Nickname,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address               ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@contactID,0      ,null        ,'Theil-Test, Wife',2        ,'Wife'    ,'A'        ,'Theil-Test',null     ,'Wife'  ,{d '1975-01-01'},2        ,2                ,1                ,@houseHoldID,1                    ,null              ,null        ,'rtheil+Wife@crossroads.net',null          ,0                 ,0               ,'321-654-8184',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET IDENTITY_INSERT [dbo].[Contacts] OFF;

--Wife Theil-Test Participant record
SET IDENTITY_INSERT [dbo].[Participants] ON;

DECLARE @partID as int
set @partID = IDENT_CURRENT('Participants')+1;

INSERT INTO [dbo].Participants 
(Participant_ID,Contact_ID,Participant_Type_ID,Attend_Start_Date,Participant_Start_Date    ,Participant_End_Date,Notes                       ,Domain_ID,__ExternalPersonID,_First_Attendance_Ever,_Second_Attendance_Ever,_Third_Attendance_Ever,_Last_Attendance_Ever) VALUES
(@partID       ,@contactID,4                  ,null             ,{ts '2015-07-02 08:15:06'},null                ,'Created by Add Family Tool',1        ,null              ,null                  ,null                   ,null                  ,null                 );

SET IDENTITY_INSERT [dbo].[Participants] OFF;

--Wife Theil-Test Updates
update [dbo].Contacts set Participant_Record = @partID where Contact_ID = @contactID;
GO

--Household for Kid14 Theil-Test
DECLARE @houseHoldID as int
set @houseHoldID = (select houseHold_ID from Households where Household_Name = 'Theil-Test');

--Kid14 Theil-Test Contact
SET IDENTITY_INSERT [dbo].[Contacts] ON;

DECLARE @contactID as int
set @contactID = IDENT_CURRENT('Contacts')+1;

INSERT INTO [dbo].Contacts 
(Contact_ID,Company,Company_Name,Display_Name       ,Prefix_ID,First_Name,Middle_Name,Last_Name   ,Suffix_ID,Nickname,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address                ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@contactID,0      ,null        ,'Theil-Test, Kid14',null     ,'Kid14'   ,null       ,'Theil-Test',null     ,'Kid14' ,{d '2001-01-01'},2        ,1                ,1                ,@houseHoldID,2                    ,null              ,null        ,'rtheil+kid14@crossroads.net',null          ,0                 ,0               ,'321-548-6154',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET IDENTITY_INSERT [dbo].[Contacts] OFF;

--Kid14 Theil-Test Participant Record
SET IDENTITY_INSERT [dbo].[Participants] ON;

DECLARE @partID as int
set @partID = IDENT_CURRENT('Participants')+1;

INSERT INTO [dbo].Participants 
(Participant_ID,Contact_ID,Participant_Type_ID,Attend_Start_Date,Participant_Start_Date    ,Participant_End_Date,Notes                       ,Domain_ID,__ExternalPersonID,_First_Attendance_Ever,_Second_Attendance_Ever,_Third_Attendance_Ever,_Last_Attendance_Ever) VALUES
(@partID       ,@contactID,4                  ,null             ,{ts '2015-07-02 08:15:06'},null                ,'Created by Add Family Tool',1        ,null              ,null                  ,null                   ,null                  ,null                 );

SET IDENTITY_INSERT [dbo].[Participants] OFF;

--Kid14 Theil-Test USER
SET IDENTITY_INSERT [dbo].[Dp_Users] ON;

DECLARE @userID as int
set @userID = IDENT_CURRENT('Dp_Users')+1;

INSERT INTO [dbo].dp_users 
(User_ID,User_Name                    ,User_Email                   ,Display_Name,Password                     ,Admin,Domain_ID,Publications_Manager,Contact_ID,Supervisor,User_GUID,Can_Impersonate,In_Recovery,Time_Zone,Locale,Theme,Setup_Admin,__ExternalPersonID,__ExternalUserID,Data_Service_Permissions,Read_Permitted,Create_Permitted,Update_Permitted,Delete_Permitted) VALUES
(@userID,'rtheil+kid14@crossroads.net','rtheil+kid14@crossroads.net','Kid14'     ,CAST('welcome' as binary(16)),0    ,1        ,0                   ,@contactID,null      ,NEWID()  ,null           ,null       ,null     ,null  ,null ,0          ,null              ,null            ,null                    ,0             ,0               ,0               ,0           );

SET IDENTITY_INSERT [dbo].[Dp_Users] OFF;

--Kid14 Theil-Test User ROLE
SET IDENTITY_INSERT [dbo].[Dp_User_Roles] ON;

DECLARE @user_role_id as int
set @user_role_id =IDENT_CURRENT('Dp_User_Roles')+1;

INSERT INTO [dbo].Dp_User_Roles (User_Role_ID,User_ID,Role_ID,Domain_ID) 
VALUES                         (@user_role_id,@userID,39    ,1);

SET IDENTITY_INSERT [dbo].[Dp_User_Roles] OFF;

--Kid14 Theil-Test Updates
update [dbo].Contacts set Participant_Record = @partID where Contact_ID = @contactID;
update [dbo].Contacts set User_Account = @userID where Contact_id = @contactID;
GO

--Household for Kid17 Theil-Test
DECLARE @houseHoldID as int
set @houseHoldID = (select houseHold_ID from Households where Household_Name = 'Theil-Test');

--Kid17 Theil-Test Contact
SET IDENTITY_INSERT [dbo].[Contacts] ON;

DECLARE @contactID as int
set @contactID = IDENT_CURRENT('Contacts')+1;

INSERT INTO [dbo].Contacts 
(Contact_ID,Company,Company_Name,Display_Name       ,Prefix_ID,First_Name  ,Middle_Name,Last_Name   ,Suffix_ID,Nickname,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address                ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@contactID,0      ,null        ,'Theil-Test, Kid17',null     ,'Kid17'     ,'D'        ,'Theil-Test',null     ,'Kid17' ,{d '1998-01-01'},null     ,1                ,1                ,@houseHoldID,4                    ,null              ,null        ,'rtheil+Kid17@crossroads.net',null          ,0                 ,0               ,'654-818-1425',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET IDENTITY_INSERT [dbo].[Contacts] OFF;

--KidsClubKid17 Participant Record
SET IDENTITY_INSERT [dbo].[Participants] ON;

DECLARE @partID as int
set @partID = IDENT_CURRENT('Participants')+1;

INSERT INTO [dbo].Participants 
(Participant_ID,Contact_ID,Participant_Type_ID,Attend_Start_Date,Participant_Start_Date    ,Participant_End_Date,Notes                       ,Domain_ID,__ExternalPersonID,_First_Attendance_Ever,_Second_Attendance_Ever,_Third_Attendance_Ever,_Last_Attendance_Ever) VALUES
(@partID       ,@contactID,4                  ,null             ,{ts '2015-07-02 08:15:06'},null                ,'Created by Add Family Tool',1        ,null              ,null                  ,null                   ,null                  ,null                 );

SET IDENTITY_INSERT [dbo].[Participants] OFF;

--KidsClubKid17 Updates
update [dbo].Contacts set Participant_Record = @partID where Contact_ID = @contactID;
GO

--Rich Theil-Test's Request to Join Kids' Club Response Record
SET IDENTITY_INSERT [dbo].[Responses] ON;

DECLARE @respID as int
set @respID = IDENT_CURRENT('Responses')+1;

DECLARE @partID as int
set @partID = (select Participant_Record from Contacts where display_name = 'Theil-Test, Rich');

INSERT INTO [dbo].Responses 
(Response_ID,Response_Date             ,Opportunity_ID,Participant_ID,Comments                        ,Website_Submission,First_Name,Last_Name,Email,Phone,Follow_up_Information,Response_Result_ID,Closed,Domain_ID,Event_ID,__ExternalCESTPID) VALUES
(@respID    ,{ts '2015-07-02 08:17:17'},115           ,@partID       ,'Request on 7/2/2015 8:17:17 AM',null              ,null      ,null     ,null ,null ,null                 ,null              ,0     ,1        ,null    ,null             );

SET IDENTITY_INSERT [dbo].[Responses] OFF;
GO

--Kid14 Theil-Test's Request to Join Kids' Club Response Record
SET IDENTITY_INSERT [dbo].[Responses] ON;

DECLARE @respID as int
set @respID = IDENT_CURRENT('Responses')+1;

DECLARE @partID as int
set @partID = (select Participant_Record from Contacts where display_name = 'Theil-Test, Kid14');

INSERT INTO [dbo].Responses 
(Response_ID,Response_Date             ,Opportunity_ID,Participant_ID,Comments                        ,Website_Submission,First_Name,Last_Name,Email,Phone,Follow_up_Information,Response_Result_ID,Closed,Domain_ID,Event_ID,__ExternalCESTPID) VALUES
(@respID    ,{ts '2015-07-02 08:20:21'},115           ,@partID       ,'Request on 7/2/2015 8:20:21 AM',null              ,null      ,null     ,null ,null ,null                 ,null              ,0 ,1        ,null    ,null             )

SET IDENTITY_INSERT [dbo].[Responses] OFF;
GO

--Family Relationships
DECLARE @dadContact as int
set @dadContact = (select Contact_ID from Contacts where display_name = 'Theil-Test, Rich');

DECLARE @momContact as int
set @momContact = (select Contact_ID from Contacts where display_name = 'Theil-Test, Wife');

DECLARE @kid14Contact as int
set @kid14Contact = (select Contact_ID from Contacts where display_name = 'Theil-Test, Kid14');

DECLARE @kid17Contact as int
set @kid17Contact = (select Contact_ID from Contacts where display_name = 'Theil-Test, Kid17');

--Dad Married to Mom
INSERT INTO [dbo].Contact_Relationships 
(Contact_ID ,Relationship_ID,Related_Contact_ID,Start_Date                ,End_Date,Domain_ID,Notes                       ,_Triggered_By) VALUES
(@dadContact,1              ,@momContact       ,{ts '2015-07-02 08:15:06'},null    ,1        ,'Created by Add Family Tool',null      );

--Dad Parent of Kids
INSERT INTO [dbo].Contact_Relationships 
(Contact_ID,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@dadContact   ,6              ,@kid14Contact ,null      ,null    ,1        ,null ,null      );

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@dadContact,6              ,@kid17Contact     ,null      ,null    ,1        ,null ,null      );

--Mom Parent Of kids
INSERT INTO [dbo].Contact_Relationships 
(Contact_ID ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@momContact,6              ,@kid14Contact     ,null      ,null    ,1        ,null ,null      );

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@momContact,6              ,@kid17Contact     ,null      ,null    ,1        ,null ,null      );

--Kids Siblings
INSERT INTO [dbo].Contact_Relationships 
(Contact_ID   ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@kid14Contact,2              ,@kid17Contact     ,null      ,null    ,1        ,null ,null      );

SET IDENTITY_INSERT [dbo].[Contact_Relationships] OFF;
GO

--Some Groups for the family so sign up to serve is functional

--Rich Signs up for KC Florence Nursery!
INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                   ,Participant_ID                                                                    ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) KC Florence Nursery') ,(select Participant_record from contacts where Display_Name = 'Theil-Test, Rich') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

--Test Wife signs up for FI Florence Parking and the Nursery!
INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                   ,Participant_ID                                                                    ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) KC Florence Nursery') ,(select Participant_record from contacts where Display_Name = 'Theil-Test, Wife') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                   ,Participant_ID                                                                    ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) FI Florence Parking') ,(select Participant_record from contacts where Display_Name = 'Theil-Test, Wife') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

--Kid14 and Kid17 sign up for 
INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                   ,Participant_ID                                                                    ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) FI Florence Coffee') ,(select Participant_record from contacts where Display_Name = 'Theil-Test, Kid14') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                   ,Participant_ID                                                                    ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) FI Florence Coffee') ,(select Participant_record from contacts where Display_Name = 'Theil-Test, Kid17') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

--Everyone signs up for Oakley Coffee
INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                   ,Participant_ID                                                                 ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) FI Oakley Coffee') ,(select Participant_record from contacts where Display_Name = 'Theil-Test, Rich') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                   ,Participant_ID                                                                 ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) FI Oakley Coffee') ,(select Participant_record from contacts where Display_Name = 'Theil-Test, Wife') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                   ,Participant_ID                                                                  ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) FI Oakley Coffee') ,(select Participant_record from contacts where Display_Name = 'Theil-Test, Kid14') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                   ,Participant_ID                                                                  ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups where group_name = '(d) FI Oakley Coffee') ,(select Participant_record from contacts where Display_Name = 'Theil-Test, Kid17') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );
GO