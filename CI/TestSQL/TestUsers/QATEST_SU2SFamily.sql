


USE [MinistryPlatform]
GO

--SignUp to Serve Family

DECLARE @fatherContactId AS INT
DECLARE @fatherEmail AS VARCHAR
DECLARE @fatherDOB DATE

DECLARE @motherContactId AS INT
DECLARE @child19ContactId AS INT
DECLARE @child4ContactId AS INT

DECLARE @currentContactId AS INT


DECLARE @motherEmail AS VARCHAR
DECLARE @child19Email AS VARCHAR
DECLARE @child4Email AS VARCHAR


DECLARE @motherDOB DATE
DECLARE @child19DOB DATE
DECLARE @child4DOB DATE

DECLARE @fatherParticipantId AS INT
DECLARE @motherParticipantId AS INT
DECLARE @child19ParticipantId AS INT
DECLARE @child4ParticipantId AS INT

DECLARE @currentParticipantId AS INT

SET IDENTITY_INSERT [dbo].[Addresses] ON;

DECLARE @addressId AS INT
set @addressId = IDENT_CURRENT('Addresses')+1;

INSERT INTO Addresses 
(Address_ID,Address_Line_1     ,Address_Line_2,City    ,[State/Region],Postal_Code,Foreign_Country,Country_Code,Domain_ID,Carrier_Route,Lot_Number,Delivery_Point_Code,Delivery_Point_Check_Digit,Latitude,Longitude,Altitude,Time_Zone,Bar_Code,Area_Code,Last_Validation_Attempt,County,Validated,Do_Not_Validate,Last_GeoCode_Attempt,__ExternalAddressID) VALUES
(@addressID,'1234 Testing Lane',null          ,'Oakley','OH'          ,'45067'    ,'United States','USA'       ,1        ,null         ,null      ,null               ,null                      ,null    ,null     ,null    ,null     ,null    ,null     ,null                   ,null  ,null     ,null           ,null                ,null               );

SET IDENTITY_INSERT [dbo].[Addresses] OFF;



SET IDENTITY_INSERT [dbo].[Households] ON;

DECLARE @householdID AS INT
DECLARE @currenthouseholdId AS INT
SET @householdId = ((SELECT MAX(Household_ID) FROM Households) + 1);
SET @currentHouseholdId = IDENT_CURRENT('Households');

INSERT INTO Households 
(Household_ID,Household_Name  ,Address_ID  ,Home_Phone      ,Domain_ID,Congregation_ID  ,Care_Person, Household_Source_ID ,Family_Call_Number, Household_Preferences     ,Home_Phone_Unlisted   , Home_Address_Unlisted, Bulk_Mail_Opt_Out, _Last_Donation, _Last_Activity, __ExternalHouseholdID, __ExternalBusinessID) VALUES
(@householdId,'SU2SHousehold' ,@addressId        ,'513-410-3540'  ,1        ,6                ,null       , null                ,null              , null                      ,null                  , null                 , 0                ,null           ,null           ,null                  , null);

SET IDENTITY_INSERT [dbo].[Households] OFF;
DBCC CHECKIDENT (Households, reseed, @currentHouseholdId);


SET @fatherContactId = (SELECT Contact_ID FROM Contacts WHERE Email_Address = 'mpcrds+SU2SFather@gmail.com' AND Last_Name = 'Head1');
SET @fatherDOB = DATEADD(year, -40, GETDATE());
SET @fatherEmail = 'mpcrds+SU2SF_'+ CAST(@fatherContactId AS varchar(10))+'@gmail.com' ;

UPDATE [dbo].Contacts 
SET Display_Name = 'SU2SFather'+ CAST(@fatherContactId AS varchar(10)), Prefix_ID = 1, Middle_Name = null, Nickname = 'SU2SFather', Date_of_Birth = @fatherDOB , Gender_ID = 1, Marital_Status_ID = 2, Household_ID = @householdId, Household_Position_ID = 1, Email_Address = @fatherEmail, Mobile_Phone = '513-410-3540', Company_Phone = null
WHERE Contact_ID = @fatherContactID;


UPDATE [dbo].dp_Users 
SET User_Name = @fatherEmail, User_Email = @fatherEmail
WHERE Contact_ID = @fatherContactID;


SET IDENTITY_INSERT [dbo].[Contacts] ON;



SET @currentContactId = IDENT_CURRENT('Contacts');

SET @motherContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @motherEmail = 'mpcrds+SU2SM_'+ (CAST(@motherContactId AS VARCHAR(10)))+'@gmail.com' ;
SET @motherDOB = DATEADD(year, -39, GETDATE());
INSERT INTO Contacts 
(Contact_ID      ,Company,Company_Name,Display_Name                                       ,Prefix_ID,First_Name  ,Middle_Name,Last_Name ,Suffix_ID,Nickname     ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address  ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@motherContactId,0      ,null        ,'SU2SMother'+ CAST(@motherContactId AS VARCHAR(10)),2        ,'SU2SMother' ,null      ,'Head2'   ,null     ,'SU2SHead2'  ,@motherDOB      ,1        ,2                ,1                ,@householdId,2                    ,null              ,null        ,@motherEmail   ,null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET @child19ContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @child19Email = 'mpcrds+SU2SC19_'+ (CAST(@child19ContactId AS VARCHAR(10)))+'@gmail.com' ;
SET @child19DOB = DATEADD(year, -19, GETDATE());
INSERT INTO Contacts 
(Contact_ID      ,Company,Company_Name,Display_Name                                         ,Prefix_ID,First_Name   ,Middle_Name,Last_Name ,Suffix_ID,Nickname      ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address   ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@child19ContactId,0      ,null        ,'SU2SChild19'+ CAST(@child19ContactId AS VARCHAR(10)),2        ,'SU2SChild19',null      ,'Adult '  ,null     ,'SU2SChild19' ,@child19DOB     ,2        ,1                ,1                ,@householdId,3                    ,null              ,null        ,@child19Email   ,null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET @child4ContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @child4Email = 'mpcrds+SU2SC4_'+ (CAST(@child4ContactId AS VARCHAR(10)))+'@gmail.com' ;
SET @child4DOB = DATEADD(year, -4, GETDATE());
INSERT INTO Contacts 
(Contact_ID      ,Company,Company_Name,Display_Name                                       ,Prefix_ID,First_Name  ,Middle_Name,Last_Name ,Suffix_ID,Nickname     ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address  ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@child4ContactId,0      ,null        ,'SU2SChild4'+ CAST(@child4ContactId AS VARCHAR(10)),2        ,'SU2SChild4',null       ,'Minor '  ,null     ,'SU2SChild4' ,@child4DOB      ,1        ,1                ,1                ,@householdId,3                    ,null              ,null        ,@child4Email   ,null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET IDENTITY_INSERT [dbo].[Contacts] OFF;
DBCC CHECKIDENT (Contacts, reseed, @currentContactId);



SET IDENTITY_INSERT [dbo].[Participants] ON;

SET @currentParticipantId = IDENT_CURRENT('Participants');

SET @fatherParticipantId = ((SELECT MAX(Participant_ID) FROM Participants) +1);
INSERT INTO Participants 
( Participant_ID         , Contact_ID      , Participant_Type_ID, Attend_Start_Date  , Participant_Start_Date, Participant_End_Date, Notes, Domain_ID, __ExternalPersonID, _First_Attendance_Ever, _Second_Attendance_Ever, _Third_Attendance_Ever, _Last_Attendance_Ever) VALUES
( @fatherParticipantId   , @fatherContactId, 1                  , null                , '01/01/2015'          , null                , null , 1        , null              , null                  ,  null                  ,  null                 ,null                  );

SET @motherParticipantId = ((SELECT MAX(Participant_ID) FROM Participants) +1);
INSERT INTO Participants 
( Participant_ID          , Contact_ID      , Participant_Type_ID, Attend_Start_Date  , Participant_Start_Date, Participant_End_Date, Notes, Domain_ID, __ExternalPersonID, _First_Attendance_Ever, _Second_Attendance_Ever, _Third_Attendance_Ever, _Last_Attendance_Ever) VALUES
( @motherParticipantId    , @motherContactId, 1                  , null                , '01/01/2015'          , null                , null , 1        , null              , null                  ,  null                  ,  null                 ,null                  );

SET @child19ParticipantId = ((SELECT MAX(Participant_ID) FROM Participants) +1);
INSERT INTO Participants 
( Participant_ID          , Contact_ID       , Participant_Type_ID, Attend_Start_Date  , Participant_Start_Date, Participant_End_Date, Notes, Domain_ID, __ExternalPersonID, _First_Attendance_Ever, _Second_Attendance_Ever, _Third_Attendance_Ever, _Last_Attendance_Ever) VALUES
( @child19ParticipantId   , @child19ContactId, 1                  , null                , '01/01/2015'          , null                , null , 1        , null              , null                  ,  null                  ,  null                 ,null                  );

SET @child4ParticipantId = ((SELECT MAX(Participant_ID) FROM Participants) +1);
INSERT INTO Participants 
( Participant_ID         , Contact_ID      , Participant_Type_ID, Attend_Start_Date  , Participant_Start_Date, Participant_End_Date, Notes, Domain_ID, __ExternalPersonID, _First_Attendance_Ever, _Second_Attendance_Ever, _Third_Attendance_Ever, _Last_Attendance_Ever) VALUES
( @child4ParticipantId   , @child4ContactId, 1                  , null                , '01/01/2015'          , null                , null , 1        , null              , null                  ,  null                  ,  null                 ,null                  );

SET IDENTITY_INSERT [dbo].[Participants] OFF;
DBCC CHECKIDENT (Participants, reseed, @currentParticipantId);



UPDATE [dbo].Contacts 
SET Participant_Record = @fatherParticipantId
WHERE Contact_ID = @fatherContactID;


UPDATE [dbo].Contacts 
SET Participant_Record = @motherParticipantId
WHERE Contact_ID = @motherContactId ;


UPDATE [dbo].Contacts 
SET Participant_Record = @child19ParticipantId
WHERE Contact_ID = @child19ContactId;


UPDATE [dbo].Contacts 
SET Participant_Record = @child4ParticipantId
WHERE Contact_ID = @child4ContactId;



SET IDENTITY_INSERT [dbo].[Contact_Relationships] OFF;

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID,Start_Date  ,End_Date,Domain_ID,Notes                       ,_Triggered_By) VALUES
(@fatherContactId,1              ,@motherContactId  ,null        ,null    ,1        ,'Created by Add Family Tool',null      );


INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@fatherContactId,6              ,@child19ContactId ,null      ,null    ,1        ,null ,null      );

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@fatherContactId,6              ,@child4ContactId  ,null      ,null    ,1        ,null ,null      );


INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@motherContactId,6              ,@child19ContactId ,null      ,null    ,1        ,null ,null      );

INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@motherContactId,6              ,@child4ContactId  ,null      ,null    ,1        ,null ,null      );


INSERT INTO [dbo].Contact_Relationships 
(Contact_ID      ,Relationship_ID,Related_Contact_ID,Start_Date,End_Date,Domain_ID,Notes,_Triggered_By) VALUES
(@child19ContactId,2              ,@child4ContactId  ,null      ,null    ,1        ,null ,null      );

SET IDENTITY_INSERT [dbo].[Contact_Relationships] OFF;