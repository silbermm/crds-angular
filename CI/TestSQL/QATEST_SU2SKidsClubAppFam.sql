USE [MinistryPlatform]
GO

--Address
SET IDENTITY_INSERT [dbo].[Addresses] ON;

DECLARE @addressId as int
set @addressId = IDENT_CURRENT('Addresses')+1;

INSERT INTO [dbo].Addresses (Address_ID,Address_Line_1,Address_Line_2,City,[State/Region],Postal_Code,Foreign_Country,Country_Code,Domain_ID,Carrier_Route,Lot_Number,Delivery_Point_Code,Delivery_Point_Check_Digit,Latitude,Longitude,Altitude,Time_Zone,Bar_Code,Area_Code,Last_Validation_Attempt,County,Validated,Do_Not_Validate,Last_GeoCode_Attempt,__ExternalAddressID) 
VALUES (@addressID,'1234 Testing Lane',null,'CITY','OH','45067','United States','USA',1,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null);

SET IDENTITY_INSERT [dbo].[Addresses] OFF;

--Household
SET IDENTITY_INSERT [dbo].[Households] ON;

DECLARE @houseHoldID as int
set @houseHoldID = IDENT_CURRENT('Households')+1;

INSERT INTO [dbo].Households (Household_ID,  Household_Name,Address_ID,    Home_Phone,Domain_ID,Congregation_ID,Care_Person,Household_Source_ID,Family_Call_Number,Household_Preferences,Home_Phone_Unlisted,Home_Address_Unlisted,Bulk_Mail_Opt_Out,_Last_Donation,_Last_Activity,__ExternalHouseholdID,__ExternalBusinessID) 
VALUES 						 (@houseHoldID,'KidsClubAppFam',@addressId,'123-867-5309',1        ,6              ,null       ,38                 ,null              ,null                 ,null               ,null                 ,0                ,null          ,null          ,null                 ,null);

SET IDENTITY_INSERT [dbo].[Households] OFF;

--KidsClubDad Contact
SET IDENTITY_INSERT [dbo].[Contacts] ON;

DECLARE @contactID as int
set @contactID = IDENT_CURRENT('Contacts')+1;

INSERT INTO [dbo].Contacts 
(Contact_ID,Company,Company_Name,Display_Name              ,Prefix_ID,First_Name,Middle_Name,Last_Name       ,Suffix_ID,Nickname  ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address          ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone ,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID ,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@contactID,0      ,null        ,'KidsClubAppFam, DadJokes',1        ,'Dad'     ,'Jokes'    ,'KidsClubAppFam',null     ,'DadJokes',{d '1964-01-01'},1        ,2                ,1                ,@houseHoldID,1                    ,null              ,null        ,'kidsclubdad@gmail.com',null          ,0                 ,0               ,'123-654-8745',null          ,null                 ,'555-365-4125',null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()      ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET IDENTITY_INSERT [dbo].[Contacts] OFF;

--Kids Club Dad Participant
SET IDENTITY_INSERT [dbo].[Participants] ON;

DECLARE @partID as int
set @partID = IDENT_CURRENT('Participants')+1;

INSERT INTO [dbo].Participants 
(Participant_ID,Contact_ID,Participant_Type_ID,Attend_Start_Date,Participant_Start_Date    ,Participant_End_Date,Notes                       ,Domain_ID,__ExternalPersonID,_First_Attendance_Ever,_Second_Attendance_Ever,_Third_Attendance_Ever,_Last_Attendance_Ever) VALUES
(@partID       ,@contactID,4                  ,null             ,{ts '2015-07-02 08:15:05'},null                ,'Created by Add Family Tool',1        ,null              ,null                  ,null                   ,null                  ,null                 );

SET IDENTITY_INSERT [dbo].[Participants] OFF;

--KidsClubDad USER
SET IDENTITY_INSERT [dbo].[Dp_Users] ON;

DECLARE @userID as int
set @userID = IDENT_CURRENT('Dp_Users')+1;

INSERT INTO [dbo].dp_users 
(User_ID,User_Name              ,User_Email             ,Display_Name,Password                     ,Admin,Domain_ID,Publications_Manager,Contact_ID,Supervisor,User_GUID  ,Can_Impersonate,In_Recovery,Time_Zone,Locale,Theme,Setup_Admin,__ExternalPersonID,__ExternalUserID,Data_Service_Permissions,Read_Permitted,Create_Permitted,Update_Permitted,Delete_Permitted) VALUES
(@userID,'kidsclubdad@gmail.com','kidsclubdad@gmail.com','KCDad'     ,CAST('welcome' as binary(16)),0    ,1        ,0                   ,@contactID   ,null      ,NEWID() ,null           ,null       ,null     ,null  ,null ,0          ,null              ,null            ,null                    ,0             ,0               ,0               ,0               );

SET IDENTITY_INSERT [dbo].[Dp_Users] OFF;

--KidsClubDad User ROLE
SET IDENTITY_INSERT [dbo].[Dp_User_Roles] ON;

DECLARE @user_role_id as int
set @user_role_id =IDENT_CURRENT('Dp_User_Roles')+1;

INSERT INTO [dbo].Dp_User_Roles (User_Role_ID,User_ID,Role_ID,Domain_ID) 
VALUES                         (@user_role_id,@userID,39    ,1);

SET IDENTITY_INSERT [dbo].[Dp_User_Roles] OFF;

--KidsClubDad Updates
update [dbo].Contacts set Participant_Record = @partID where Contact_ID = @contactID;
update [dbo].Contacts set User_Account = @userID where Contact_ID = @contactID;
GO

--Household for kids club mom
DECLARE @houseHoldID as int
set @houseHoldID = (select houseHold_ID from Households where Household_Name = 'KidsClubAppFam');

--KidsClubMom Contact
SET IDENTITY_INSERT [dbo].[Contacts] ON;

DECLARE @contactID as int
set @contactID = IDENT_CURRENT('Contacts')+1;

INSERT INTO [dbo].Contacts 
(Contact_ID,Company,Company_Name,Display_Name           ,Prefix_ID,First_Name,Middle_Name,Last_Name       ,Suffix_ID,Nickname,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address          ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@contactID,0      ,null        ,'KidsClubAppFam, Momma',2        ,'Mom'     ,'A'        ,'KidsClubAppFam',null     ,'Momma' ,{d '1965-01-01'},2        ,2                ,1                ,@houseHoldID,1                    ,null              ,null        ,'kidsclubmom@gmail.com',null          ,0                 ,0               ,'321-654-8184',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET IDENTITY_INSERT [dbo].[Contacts] OFF;

--KidsclubMom Participant record
SET IDENTITY_INSERT [dbo].[Participants] ON;

DECLARE @partID as int
set @partID = IDENT_CURRENT('Participants')+1;

INSERT INTO [dbo].Participants 
(Participant_ID,Contact_ID,Participant_Type_ID,Attend_Start_Date,Participant_Start_Date    ,Participant_End_Date,Notes                       ,Domain_ID,__ExternalPersonID,_First_Attendance_Ever,_Second_Attendance_Ever,_Third_Attendance_Ever,_Last_Attendance_Ever) VALUES
(@partID       ,@contactID,4                  ,null             ,{ts '2015-07-02 08:15:06'},null                ,'Created by Add Family Tool',1        ,null              ,null                  ,null                   ,null                  ,null                 );

SET IDENTITY_INSERT [dbo].[Participants] OFF;

--KidsClubMom Updates
update [dbo].Contacts set Participant_Record = @partID where Contact_ID = @contactID;
GO

--Household for kidsclubkid14
DECLARE @houseHoldID as int
set @houseHoldID = (select houseHold_ID from Households where Household_Name = 'KidsClubAppFam');

--KidsClubKid14 Contact
SET IDENTITY_INSERT [dbo].[Contacts] ON;

DECLARE @contactID as int
set @contactID = IDENT_CURRENT('Contacts')+1;

INSERT INTO [dbo].Contacts 
(Contact_ID,Company,Company_Name,Display_Name                   ,Prefix_ID,First_Name     ,Middle_Name,Last_Name       ,Suffix_ID,Nickname       ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address            ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@contactID,0      ,null        ,'KidsClubAppFam, Billy the Kid',null     ,'KidsClubKid14',null       ,'KidsClubAppFam',null     ,'Billy the Kid',{d '2001-01-01'},2        ,1                ,1                ,@houseHoldID,2                    ,null              ,null        ,'kidsclubkid14@gmail.com',null          ,0                 ,0               ,'321-548-6154',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET IDENTITY_INSERT [dbo].[Contacts] OFF;

--KidsClubKid14 USER
SET IDENTITY_INSERT [dbo].[Dp_Users] ON;

DECLARE @userID as int
set @userID = IDENT_CURRENT('Dp_Users')+1;

INSERT INTO [dbo].dp_users 
(User_ID,User_Name                ,User_Email               ,Display_Name,Password                     ,Admin,Domain_ID,Publications_Manager,Contact_ID,Supervisor,User_GUID,Can_Impersonate,In_Recovery,Time_Zone,Locale,Theme,Setup_Admin,__ExternalPersonID,__ExternalUserID,Data_Service_Permissions,Read_Permitted,Create_Permitted,Update_Permitted,Delete_Permitted) VALUES
(@userID,'kidsclubkid14@gmail.com','kidsclubkid14@gmail.com','KCKid14'   ,CAST('welcome' as binary(16)),0    ,1        ,0                   ,@contactID,null      ,NEWID()  ,null           ,null       ,null     ,null  ,null ,0          ,null              ,null            ,null                    ,0             ,0               ,0               ,0           );

SET IDENTITY_INSERT [dbo].[Dp_Users] OFF;

--KidsClubKid14 User ROLE
SET IDENTITY_INSERT [dbo].[Dp_User_Roles] ON;

DECLARE @user_role_id as int
set @user_role_id =IDENT_CURRENT('Dp_User_Roles')+1;

INSERT INTO [dbo].Dp_User_Roles (User_Role_ID,User_ID,Role_ID,Domain_ID) 
VALUES                         (@user_role_id,@userID,39    ,1);

SET IDENTITY_INSERT [dbo].[Dp_User_Roles] OFF;

--KidsClubKid14 Participant Record
SET IDENTITY_INSERT [dbo].[Participants] ON;

DECLARE @partID as int
set @partID = IDENT_CURRENT('Participants')+1;

INSERT INTO [dbo].Participants 
(Participant_ID,Contact_ID,Participant_Type_ID,Attend_Start_Date,Participant_Start_Date    ,Participant_End_Date,Notes                       ,Domain_ID,__ExternalPersonID,_First_Attendance_Ever,_Second_Attendance_Ever,_Third_Attendance_Ever,_Last_Attendance_Ever) VALUES
(@partID       ,@contactID,4                  ,null             ,{ts '2015-07-02 08:15:06'},null                ,'Created by Add Family Tool',1        ,null              ,null                  ,null                   ,null                  ,null                 );

SET IDENTITY_INSERT [dbo].[Participants] OFF;

--KidsClubKid14 Updates
update [dbo].Contacts set Participant_Record = @partID where Contact_ID = @contactID;
update [dbo].Contacts set User_Account = @userID  where Contact_ID = @contactID;
GO

--Household for kidsclubkid17
DECLARE @houseHoldID as int
set @houseHoldID = (select houseHold_ID from Households where Household_Name = 'KidsClubAppFam');

--KidsClubKid17 Contact
SET IDENTITY_INSERT [dbo].[Contacts] ON;

DECLARE @contactID as int
set @contactID = IDENT_CURRENT('Contacts')+1;

INSERT INTO [dbo].Contacts 
(Contact_ID,Company,Company_Name,Display_Name                ,Prefix_ID,First_Name  ,Middle_Name,Last_Name       ,Suffix_ID,Nickname    ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address            ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@contactID,0      ,null        ,'KidsClubAppFam, kidsclub17',null     ,'kidsclub17','D'        ,'KidsClubAppFam',null     ,'kidsclub17',{d '1998-01-01'},null     ,1                ,1                ,@houseHoldID,4                    ,null              ,null        ,'kidsclubkid17@gmail.com',null          ,0                 ,0               ,'654-818-1425',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

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

--KidsClubDad Request to Join Kids' Club Response Record
SET IDENTITY_INSERT [dbo].[Responses] ON;

DECLARE @respID as int
set @respID = IDENT_CURRENT('Responses')+1;

DECLARE @partID as int
set @partID = (select Participant_Record from Contacts where display_name = 'KidsClubAppFam, DadJokes');

INSERT INTO [dbo].Responses 
(Response_ID,Response_Date             ,Opportunity_ID,Participant_ID,Comments                        ,Website_Submission,First_Name,Last_Name,Email,Phone,Follow_up_Information,Response_Result_ID,Closed,Domain_ID,Event_ID,__ExternalCESTPID) VALUES
(@respID    ,{ts '2015-07-02 08:17:17'},115           ,@partID       ,'Request on 7/2/2015 8:17:17 AM',null              ,null      ,null     ,null ,null ,null                 ,null              ,0     ,1        ,null    ,null             );

SET IDENTITY_INSERT [dbo].[Responses] OFF;
GO

--KidsClubKid14 Request to join Kids' Club Response Record
SET IDENTITY_INSERT [dbo].[Responses] ON;

DECLARE @respID as int
set @respID = IDENT_CURRENT('Responses')+1;

DECLARE @partID as int
set @partID = (select Participant_Record from Contacts where display_name = 'KidsClubAppFam, Billy the Kid');

INSERT INTO [dbo].Responses 
(Response_ID,Response_Date             ,Opportunity_ID,Participant_ID,Comments                        ,Website_Submission,First_Name,Last_Name,Email,Phone,Follow_up_Information,Response_Result_ID,Closed,Domain_ID,Event_ID,__ExternalCESTPID) VALUES
(@respID    ,{ts '2015-07-02 08:20:21'},115           ,@partID       ,'Request on 7/2/2015 8:20:21 AM',null              ,null      ,null     ,null ,null ,null                 ,null              ,0 ,1        ,null    ,null             )

SET IDENTITY_INSERT [dbo].[Responses] OFF;
GO

--Family Relationships
DECLARE @dadContact as int
set @dadContact = (select Contact_ID from Contacts where display_name = 'KidsClubAppFam, DadJokes');

DECLARE @momContact as int
set @momContact = (select Contact_ID from Contacts where display_name = 'KidsClubAppFam, Momma');

DECLARE @kid14Contact as int
set @kid14Contact = (select Contact_ID from Contacts where display_name = 'KidsClubAppFam, Billy the Kid');

DECLARE @kid17Contact as int
set @kid17Contact = (select Contact_ID from Contacts where display_name = 'KidsClubAppFam, kidsclub17');

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