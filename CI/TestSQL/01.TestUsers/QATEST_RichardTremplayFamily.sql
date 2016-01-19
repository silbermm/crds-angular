USE [MinistryPlatform]
GO


DECLARE @fatherContactId AS INT
DECLARE @fatherDOB DATE

DECLARE @motherContactId AS INT
DECLARE @child18ContactId AS INT
DECLARE @child4ContactId AS INT
DECLARE @childFoster1ContactId AS INT
DECLARE @legalWard1ContactId AS INT
DECLARE @childFoster2ContactId AS INT
DECLARE @legalWard2ContactId AS INT

DECLARE @currentContactId AS INT

DECLARE @motherDOB DATE
DECLARE @child18DOB DATE
DECLARE @child4DOB DATE
DECLARE @childfoster1DOB DATE
DECLARE @legalWard1DOB DATE
DECLARE @childfoster2DOB DATE
DECLARE @legalWard2DOB DATE


DECLARE @fatherParticipantId AS INT
DECLARE @motherParticipantId AS INT
DECLARE @child18ParticipantId AS INT
DECLARE @child4ParticipantId AS INT
DECLARE @childf1ParticipantId AS INT
DECLARE @legalWard1ParticipantId AS INT
DECLARE @childf2ParticipantId AS INT
DECLARE @legalWard2ParticipantId AS INT

DECLARE @currentParticipantId AS INT

DECLARE @fContactId AS INT
DECLARE @mContactId AS INT
-------------------------------------------------------------------------------------------------------------------------------------------
--Set Current contact record to 100000020--
SET @currentContactId = IDENT_CURRENT('Contacts');

SET IDENTITY_INSERT [dbo].[Contacts] ON;

SET @fContactId = 100000020;

INSERT INTO Contacts 
(Contact_ID,Company,Company_Name,Display_Name,Prefix_ID,First_Name,Middle_Name,Last_Name,Suffix_ID,Nickname ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@fContactId,0      ,null        ,'temp'      ,2        ,'temp'    ,null       ,'Temp'   ,null     ,'temp'   ,{d '1975-01-01'},1        ,1                ,1                ,null        ,3                    ,null              ,null        ,null         ,null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET @mContactId = 100000021;

INSERT INTO Contacts 
(Contact_ID,Company,Company_Name,Display_Name,Prefix_ID,First_Name,Middle_Name,Last_Name,Suffix_ID,Nickname ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@mContactId,0      ,null        ,'temp'      ,2        ,'temp'    ,null       ,'Temp'   ,null     ,'temp'   ,{d '1975-01-01'},1        ,1                ,1                ,null        ,3                    ,null              ,null        ,null         ,null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

DBCC CHECKIDENT (Contacts, reseed, @currentContactId);
SET IDENTITY_INSERT [dbo].[Contacts] OFF;
DBCC CHECKIDENT (Contacts, reseed, @currentContactId);


--------------------------------------------------------------------------------------------------------------------------------------------
--Get the required data to add to our contact. 

SET @fatherContactId = (SELECT Contact_ID FROM Contacts WHERE Email_Address = 'mpcrds+tremplay.richard@gmail.com' and Last_Name = 'Tremplay');
SET @motherContactId = (SELECT Contact_ID FROM Contacts WHERE Email_Address = 'mpcrds+tremplay.mary@gmail.com' and Last_Name = 'Tremplay');

DECLARE @fatheHouseholdId AS INT
SET @fatheHouseholdId = (SELECT Household_ID FROM Contacts WHERE Contact_ID = @fatherContactId);

SET @fatherParticipantId = (SELECT Participant_Record FROM Contacts WHERE Contact_ID = @fatherContactId);
SET @motherParticipantId = (SELECT Participant_Record FROM Contacts WHERE Contact_ID = @motherContactId);

DECLARE @fatherUserAccount AS INT
SET @fatherUserAccount = (SELECT User_Account FROM Contacts WHERE Contact_ID = @fatherContactId);
DECLARE @motherUserAccount AS INT
SET @motherUserAccount = (SELECT User_Account FROM Contacts WHERE Contact_ID = @motherContactId);


--Update old contact record so we can delete it. 
UPDATE [dbo].Contacts
SET Household_ID = null, Participant_Record = null, User_Account = null
WHERE email_address = 'mpcrds+tremplay.richard@gmail.com' and last_name = 'Templay';
UPDATE [dbo].Contacts
SET Household_ID = null, Participant_Record = null, User_Account = null
WHERE email_address = 'mpcrds+tremplay.mary@gmail.com' and last_name = 'Templay';

--Temporarily update the participant and user account records - Please don't fail.
UPDATE [dbo].Participants 
SET Contact_ID = @fContactId WHERE Participant_ID = @fatherParticipantId;
UPDATE [dbo].Participants 
SET Contact_ID = @mContactId WHERE Participant_ID = @motherParticipantId;

UPDATE [dbo].dp_users
SET Contact_ID = @fContactId WHERE USER_ID = @fatherUserAccount;
UPDATE [dbo].dp_users
SET Contact_ID = @mContactId WHERE USER_ID = @motherUserAccount;

--Just get rid of this so we can delete Cloud's old contact record
DELETE FROM Contact_Households WHERE Contact_ID = @fatherContactId;
DELETE FROM Contact_Households WHERE Contact_ID = @motherContactId;

DECLARE @fatherCommunicationId AS INT
SET @fatherCommunicationId = (SELECT Communication_ID FROM dp_Communications WHERE TO_CONTACT = @fatherContactId);
DECLARE @motherCommunicationId AS INT
SET @motherCommunicationId = (SELECT Communication_ID FROM dp_Communications WHERE TO_CONTACT = @motherContactId);

DELETE FROM [dbo].dp_commands WHERE Communication_ID = @fatherCommunicationId;
DELETE FROM [dbo].dp_Contact_Publications WHERE Contact_ID = @fatherContactId;
DELETE FROM [dbo].dp_communication_messages WHERE Communication_ID = @fatherCommunicationId;
DELETE FROM [dbo].dp_Communications WHERE Communication_ID = @fatherCommunicationId;
DELETE FROM [dbo].Activity_Log WHERE Contact_iD = @fatherContactId;
DELETE FROM [dbo].dp_commands WHERE Communication_ID = @motherCommunicationId;
DELETE FROM [dbo].dp_Contact_Publications WHERE Contact_ID = @motherContactId;
DELETE FROM [dbo].dp_communication_messages WHERE Communication_ID = @motherCommunicationId;
DELETE FROM [dbo].dp_Communications WHERE Communication_ID = @motherCommunicationId;
DELETE FROM [dbo].Activity_Log WHERE Contact_iD = @motherContactId;

--Delete the old contact record for cloud
DELETE FROM [dbo].Contacts where Contact_ID = @fatherContactId;
DELETE FROM [dbo].Contacts where Contact_ID = @motherContactId;
-----------------------------------------------------------------------------------------------------------------------------

--Insert New Address
DECLARE @currentAddressId AS INT
SET @currentAddressId = IDENT_CURRENT('Addresses');

SET IDENTITY_INSERT [dbo].[Addresses] ON;

DECLARE @addressId AS INT
SET @addressId = 100000020;

INSERT INTO Addresses 
(Address_ID,Address_Line_1         ,Address_Line_2,City    ,[State/Region],Postal_Code,Foreign_Country,Country_Code,Domain_ID,Carrier_Route,Lot_Number,Delivery_Point_Code,Delivery_Point_Check_Digit,Latitude,Longitude,Altitude,Time_Zone,Bar_Code,Area_Code,Last_Validation_Attempt,County,Validated,Do_Not_Validate,Last_GeoCode_Attempt,__ExternalAddressID) VALUES
(@addressID,'123 Towne Commons Way',null          ,'Oakley','OH'          ,'45067'    ,'United States','USA'       ,1        ,null         ,null      ,null               ,null                      ,null    ,null     ,null    ,null     ,null    ,null     ,null                   ,null  ,null     ,null           ,null                ,null               );

DBCC CHECKIDENT (Addresses, reseed, @currentAddressId);
SET IDENTITY_INSERT [dbo].[Addresses] OFF;
DBCC CHECKIDENT (Addresses, reseed, @currentAddressId);
-------------------------------------------------------------------------------------------------------------------------------------------

--Insert new Household

DECLARE @currenthouseholdId AS INT
SET @currentHouseholdId = IDENT_CURRENT('Households');

SET IDENTITY_INSERT [dbo].[Households] ON;

DECLARE @householdID AS INT
SET @householdId = 100000020;

INSERT INTO Households 
(Household_ID,Household_Name,Address_ID  ,Home_Phone      ,Domain_ID,Congregation_ID  ,Care_Person, Household_Source_ID ,Family_Call_Number, Household_Preferences     ,Home_Phone_Unlisted   , Home_Address_Unlisted, Bulk_Mail_Opt_Out, _Last_Donation, _Last_Activity, __ExternalHouseholdID, __ExternalBusinessID) VALUES
(@householdId,'Tremplay'    ,@addressId  ,'513-410-3540'  ,1        ,6                ,null       , null                ,null              , null                      ,null                  , null                 , 0                ,null           ,null           ,null                  , null);

DBCC CHECKIDENT (Households, reseed, @currentHouseholdId);
SET IDENTITY_INSERT [dbo].[Households] OFF;
DBCC CHECKIDENT (Households, reseed, @currentHouseholdId);
-------------------------------------------------------------------------------------------------------------------------------------------
--Update father Contact Record

SET @fatherContactId = @fContactId;
SET @fatherDOB = DATEADD(year, -40, GETDATE());

UPDATE [dbo].Contacts 
SET Display_Name = 'Tremplay,Richard', Prefix_ID = 1,First_Name = 'Richard',  Middle_Name = null, Last_Name = 'Tremplay', Nickname = 'Richard', Date_of_Birth = @fatherDOB , Gender_ID = 1, Marital_Status_ID = 2, Household_ID = @householdId, Household_Position_ID = 1,Participant_Record = @fatherParticipantId, Email_Address = 'mpcrds+tremplay.richard@gmail.com', Mobile_Phone = '513-410-3540', Company_Phone = null
WHERE Contact_ID = @fatherContactID;

SET @motherContactId = @mContactId;
SET @motherDOB = DATEADD(year, -38, GETDATE());

UPDATE [dbo].Contacts 
SET Display_Name = 'Tremplay,Mary', Prefix_ID = 2,First_Name = 'Mary',  Middle_Name = null, Last_Name = 'Tremplay', Nickname = 'Richard', Date_of_Birth = @motherDOB , Gender_ID = 2, Marital_Status_ID = 2, Household_ID = @householdId, Household_Position_ID = 1,Participant_Record = @motherParticipantId, Email_Address = 'mpcrds+tremplay.mary@gmail.com', Mobile_Phone = '513-410-3540', Company_Phone = null
WHERE Contact_ID = @motherContactID;


--Insert new Family members

SET @currentContactId = IDENT_CURRENT('Contacts');

SET IDENTITY_INSERT [dbo].[Contacts] ON;

SET @child18ContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @child18DOB = DATEADD(year, -18, GETDATE());
INSERT INTO Contacts 
(Contact_ID      ,Company,Company_Name,Display_Name    ,Prefix_ID,First_Name,Middle_Name,Last_Name ,Suffix_ID,Nickname ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address                    ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@child18ContactId,0      ,null       ,'Tremplay,Johan',2        ,'Johan'   ,null       ,'Tremplay',null     ,'Johan'  ,@child18DOB     ,2        ,1                ,1                ,@householdId,3                    ,null              ,null        ,'mpcrds+tremplay.johan@gmail.com',null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET @child4ContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @child4DOB = DATEADD(year, -4, GETDATE());
INSERT INTO Contacts 
(Contact_ID      ,Company,Company_Name,Display_Name         ,Prefix_ID,First_Name  ,Middle_Name,Last_Name ,Suffix_ID,Nickname ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address                     ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@child4ContactId,0      ,null        ,'Tremplay,Josina'    ,2        ,'Josina'    ,null       ,'Tremplay',null     ,'Josina' ,@child4DOB      ,1        ,1                ,1                ,@householdId,3                    ,null              ,null        ,'mpcrds+tremplay.Josina@gmail.com',null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET @childFoster1ContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @childFoster1DOB = DATEADD(year, -9, GETDATE());
INSERT INTO Contacts 
(Contact_ID            ,Company,Company_Name,Display_Name      ,Prefix_ID,First_Name ,Middle_Name,Last_Name ,Suffix_ID,Nickname ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address                      ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@childFoster1ContactId,0      ,null        ,'Brown,Vincent'   ,2        ,'Vincent'  ,null       ,'Brown'   ,null     ,'Vincent',@childFoster1DOB,1        ,1                ,1                ,@householdId,3                    ,null              ,null        ,'mpcrds+tremplay.vincent@gmail.com',null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET @childFoster2ContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @childFoster2DOB = DATEADD(year, -9, GETDATE());
INSERT INTO Contacts 
(Contact_ID            ,Company,Company_Name,Display_Name       ,Prefix_ID,First_Name ,Middle_Name,Last_Name ,Suffix_ID,Nickname ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address                     ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@childFoster2ContactId,0      ,null        ,'Bemmen,Sophia'    ,2        ,'Sophia'   ,null       ,'Bemmen'  ,null     ,'Sophia' ,@childFoster2DOB,1        ,1                ,1                ,@householdId,3                    ,null              ,null        ,'mpcrds+tremplay.sophia@gmail.com',null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET @legalWard1ContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @legalWard1DOB = DATEADD(year, -60, GETDATE());
INSERT INTO Contacts 
(Contact_ID          ,Company,Company_Name,Display_Name          ,Prefix_ID,First_Name ,Middle_Name,Last_Name    ,Suffix_ID,Nickname ,Date_of_Birth,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address                     ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@legalWard1ContactId,0      ,null        ,'Tremplay,George'     ,2        ,'George'   ,null       ,'Tremplay'   ,null     ,'George' ,@legalWard1DOB,1        ,1                ,1               ,@householdId,3                    ,null              ,null        ,'mpcrds+tremplay.george@gmail.com',null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET @legalWard2ContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @legalWard2DOB = DATEADD(year, -58, GETDATE());
INSERT INTO Contacts 
(Contact_ID          ,Company,Company_Name,Display_Name          ,Prefix_ID,First_Name ,Middle_Name,Last_Name    ,Suffix_ID,Nickname   ,Date_of_Birth,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address                       ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@legalWard2ContactId,0      ,null        ,'Tremplay,Margaret'   ,2        ,'Margaret' ,null       ,'Ray'        ,null     ,'Margaret' ,@legalWard2DOB,1        ,1                ,1                ,@householdId,3                    ,null              ,null       ,'mpcrds+tremplay.margaret@gmail.com',null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

DBCC CHECKIDENT (Contacts, reseed, @currentContactId);
SET IDENTITY_INSERT [dbo].[Contacts] OFF;
DBCC CHECKIDENT (Contacts, reseed, @currentContactId);

-------------------------------------------------------------------------------------------------------------------------------------------
--Insert Participant records

SET IDENTITY_INSERT [dbo].[Participants] ON;


SET @child18ParticipantId = ((SELECT MAX(Participant_ID) FROM Participants) +1);
INSERT INTO Participants 
( Participant_ID          , Contact_ID       , Participant_Type_ID,Participant_Start_Date, Participant_End_Date, Notes, Domain_ID, __ExternalPersonID, _First_Attendance_Ever, _Second_Attendance_Ever, _Third_Attendance_Ever, _Last_Attendance_Ever) VALUES
( @child18ParticipantId   , @child18ContactId, 1                  ,'01/01/2015'          , null                , null , 1        , null              , null                  ,  null                  ,  null                 ,null                  );

SET @child4ParticipantId = ((SELECT MAX(Participant_ID) FROM Participants) +1);
INSERT INTO Participants 
( Participant_ID         , Contact_ID      , Participant_Type_ID, Participant_Start_Date, Participant_End_Date, Notes, Domain_ID, __ExternalPersonID, _First_Attendance_Ever, _Second_Attendance_Ever, _Third_Attendance_Ever, _Last_Attendance_Ever) VALUES
( @child4ParticipantId   , @child4ContactId, 1                  , '01/01/2015'          , null                , null , 1        , null              , null                  ,  null                  ,  null                 ,null                  );

SET @childf1ParticipantId = ((SELECT MAX(Participant_ID) FROM Participants) +1);
INSERT INTO Participants 
( Participant_ID      , Contact_ID           , Participant_Type_ID, Participant_Start_Date, Participant_End_Date, Notes, Domain_ID, __ExternalPersonID, _First_Attendance_Ever, _Second_Attendance_Ever, _Third_Attendance_Ever, _Last_Attendance_Ever) VALUES
( @childf1ParticipantId, @childFoster1ContactId, 1                  , '01/01/2015'          , null                , null , 1        , null              , null                  ,  null                  ,  null                 ,null                  );

SET @legalWard1ParticipantId = ((SELECT MAX(Participant_ID) FROM Participants) +1);
INSERT INTO Participants 
( Participant_ID         , Contact_ID      , Participant_Type_ID, Participant_Start_Date, Participant_End_Date, Notes, Domain_ID, __ExternalPersonID, _First_Attendance_Ever, _Second_Attendance_Ever, _Third_Attendance_Ever, _Last_Attendance_Ever) VALUES
( @legalWard1ParticipantId, @legalWard1ContactId, 1                  , '01/01/2015'          , null                , null , 1        , null              , null                  ,  null                  ,  null                 ,null                  );

SET @childf2ParticipantId = ((SELECT MAX(Participant_ID) FROM Participants) +1);
INSERT INTO Participants 
( Participant_ID      , Contact_ID           , Participant_Type_ID, Participant_Start_Date, Participant_End_Date, Notes, Domain_ID, __ExternalPersonID, _First_Attendance_Ever, _Second_Attendance_Ever, _Third_Attendance_Ever, _Last_Attendance_Ever) VALUES
( @childf2ParticipantId, @childFoster2ContactId, 1                  , '01/01/2015'          , null                , null , 1        , null              , null                  ,  null                  ,  null                 ,null                  );

SET @legalWard2ParticipantId = ((SELECT MAX(Participant_ID) FROM Participants) +1);
INSERT INTO Participants 
( Participant_ID         , Contact_ID      , Participant_Type_ID, Participant_Start_Date, Participant_End_Date, Notes, Domain_ID, __ExternalPersonID, _First_Attendance_Ever, _Second_Attendance_Ever, _Third_Attendance_Ever, _Last_Attendance_Ever) VALUES
( @legalWard2ParticipantId, @legalWard2ContactId, 1                  , '01/01/2015'          , null                , null , 1        , null              , null                  ,  null                  ,  null                 ,null                  );

SET IDENTITY_INSERT [dbo].[Participants] OFF;

--Update Contact record with participant records


UPDATE [dbo].Contacts 
SET Participant_Record = @child18ParticipantId
WHERE Contact_ID = @child18ContactId;


UPDATE [dbo].Contacts 
SET Participant_Record = @child4ParticipantId
WHERE Contact_ID = @child4ContactId;

UPDATE [dbo].Contacts 
SET Participant_Record = @childf1ParticipantId
WHERE Contact_ID = @childFoster1ContactId;

UPDATE [dbo].Contacts 
SET Participant_Record = @legalWard1ParticipantId
WHERE Contact_ID = @legalWard1ContactId;

UPDATE [dbo].Contacts 
SET Participant_Record = @childf2ParticipantId
WHERE Contact_ID = @childFoster2ContactId;

UPDATE [dbo].Contacts 
SET Participant_Record = @legalWard2ParticipantId
WHERE Contact_ID = @legalWard2ContactId;

-------------------------------------------------------------------------------------------------------------------------------------------

--Update the relationships between family members

SET IDENTITY_INSERT [dbo].[Contact_Relationships] OFF;

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID,Start_Date  ,End_Date,Domain_ID,Notes                       ,_Triggered_By) VALUES
(@fatherContactId,1              ,@motherContactId  ,null        ,null    ,1        ,'Created by Add Family Tool',null      );

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID   ,Start_Date  ,End_Date,Domain_ID,Notes                       ,_Triggered_By) VALUES
(@fatherContactId,21             ,@childFoster1ContactId,null        ,null    ,1        ,'Created by Add Family Tool',null      );

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID   ,Start_Date  ,End_Date,Domain_ID,Notes                       ,_Triggered_By) VALUES
(@fatherContactId,29             ,@legalWard1ContactId  ,null        ,null    ,1        ,'Created by Add Family Tool',null      );

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID   ,Start_Date  ,End_Date,Domain_ID,Notes                       ,_Triggered_By) VALUES
(@fatherContactId,21             ,@childFoster2ContactId,null        ,null    ,1        ,'Created by Add Family Tool',null      );

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID   ,Start_Date  ,End_Date,Domain_ID,Notes                       ,_Triggered_By) VALUES
(@fatherContactId,29             ,@legalWard2ContactId  ,null        ,null    ,1        ,'Created by Add Family Tool',null      );

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@fatherContactId,6              ,@child18ContactId ,null      ,null    ,1        ,null ,null      );

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@fatherContactId,6              ,@child4ContactId  ,null      ,null    ,1        ,null ,null      );

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@motherContactId,6              ,@child18ContactId ,null      ,null    ,1        ,null ,null      );

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@motherContactId,6              ,@child4ContactId  ,null      ,null    ,1        ,null ,null      );

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@child18ContactId,2              ,@child4ContactId  ,null      ,null    ,1        ,null ,null      );

SET IDENTITY_INSERT [dbo].[Contact_Relationships] OFF;
-------------------------------------------------------------------------------------------------------------------------------------------

--Create donor records for family

INSERT INTO [dbo].Donors 
(Contact_ID      ,Statement_Frequency_ID,Statement_Type_ID,Statement_Method_ID,Setup_Date                ,Envelope_No,Cancel_Envelopes,Notes,First_Contact_Made,Domain_ID,__ExternalPersonID,_First_Donation_Date,_Last_Donation_Date,Processor_ID) VALUES
(@fatherContactId,3                     ,1                ,4                  ,{ts '2015-07-06 12:03:37'},null       ,0               ,null ,null              ,1        ,null              ,null                ,null               ,null);

INSERT INTO [dbo].Donors 
(Contact_ID      ,Statement_Frequency_ID,Statement_Type_ID,Statement_Method_ID,Setup_Date                ,Envelope_No,Cancel_Envelopes,Notes,First_Contact_Made,Domain_ID,__ExternalPersonID,_First_Donation_Date,_Last_Donation_Date,Processor_ID) VALUES
(@motherContactId,3                     ,1                ,4                  ,{ts '2015-07-06 12:03:37'},null       ,0               ,null ,null              ,1        ,null              ,null                ,null               ,null);


-------------------------------------------------------------------------------------------------------------------------------------------