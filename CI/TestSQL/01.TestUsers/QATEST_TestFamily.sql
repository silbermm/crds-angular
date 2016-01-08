USE [MinistryPlatform]
GO

--SignUp to Serve Family

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

SET IDENTITY_INSERT [dbo].[Addresses] ON;

DECLARE @addressId AS INT
set @addressId = IDENT_CURRENT('Addresses')+1;

INSERT INTO Addresses 
(Address_ID,Address_Line_1         ,Address_Line_2,City    ,[State/Region],Postal_Code,Foreign_Country,Country_Code,Domain_ID,Carrier_Route,Lot_Number,Delivery_Point_Code,Delivery_Point_Check_Digit,Latitude,Longitude,Altitude,Time_Zone,Bar_Code,Area_Code,Last_Validation_Attempt,County,Validated,Do_Not_Validate,Last_GeoCode_Attempt,__ExternalAddressID) VALUES
(@addressID,'123 Towne Commons Way',null          ,'Oakley','OH'          ,'45067'    ,'United States','USA'       ,1        ,null         ,null      ,null               ,null                      ,null    ,null     ,null    ,null     ,null    ,null     ,null                   ,null  ,null     ,null           ,null                ,null               );

SET IDENTITY_INSERT [dbo].[Addresses] OFF;



SET IDENTITY_INSERT [dbo].[Households] ON;

DECLARE @householdID AS INT
DECLARE @currenthouseholdId AS INT
SET @householdId = ((SELECT MAX(Household_ID) FROM Households) + 1);
SET @currentHouseholdId = IDENT_CURRENT('Households');

INSERT INTO Households 
(Household_ID,Household_Name,Address_ID  ,Home_Phone      ,Domain_ID,Congregation_ID  ,Care_Person, Household_Source_ID ,Family_Call_Number, Household_Preferences     ,Home_Phone_Unlisted   , Home_Address_Unlisted, Bulk_Mail_Opt_Out, _Last_Donation, _Last_Activity, __ExternalHouseholdID, __ExternalBusinessID) VALUES
(@householdId,'Tremplay'    ,@addressId  ,'513-410-3540'  ,1        ,6                ,null       , null                ,null              , null                      ,null                  , null                 , 0                ,null           ,null           ,null                  , null);

SET IDENTITY_INSERT [dbo].[Households] OFF;
DBCC CHECKIDENT (Households, reseed, @currentHouseholdId);


SET @fatherContactId = (SELECT Contact_ID FROM Contacts WHERE Email_Address = 'richard.tremplay@gmail.com' AND Last_Name = 'Tremplay');
SET @fatherDOB = DATEADD(year, -40, GETDATE());

UPDATE [dbo].Contacts 
SET Display_Name = 'Richard', Prefix_ID = 1, Middle_Name = null, Nickname = 'Richard', Date_of_Birth = @fatherDOB , Gender_ID = 1, Marital_Status_ID = 2, Household_ID = @householdId, Household_Position_ID = 1, Mobile_Phone = '513-410-3540', Company_Phone = null
WHERE Contact_ID = @fatherContactID;


SET IDENTITY_INSERT [dbo].[Contacts] ON;

SET @currentContactId = IDENT_CURRENT('Contacts');

SET @motherContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @motherDOB = DATEADD(year, -39, GETDATE());
INSERT INTO Contacts 
(Contact_ID      ,Company,Company_Name,Display_Name,Prefix_ID,First_Name  ,Middle_Name,Last_Name ,Suffix_ID,Nickname,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address         ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@motherContactId,0      ,null        ,'Mary'      ,2        ,'Mary'      ,null       ,'Riley'   ,null     ,'Mary'  ,@motherDOB      ,1        ,2                ,1                ,@householdId,2                    ,null              ,null        ,'mary.riley@gmail.com',null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET @child18ContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @child18DOB = DATEADD(year, -18, GETDATE());
INSERT INTO Contacts 
(Contact_ID      ,Company,Company_Name,Display_Name,Prefix_ID,First_Name,Middle_Name,Last_Name ,Suffix_ID,Nickname ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address         ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@child18ContactId,0      ,null       ,'Johan'     ,2        ,'Johan'   ,null       ,'Tremplay',null     ,'Johan'  ,@child18DOB     ,2        ,1                ,1                ,@householdId,3                    ,null              ,null        ,'mary.riley@gmail.com',null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET @child4ContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @child4DOB = DATEADD(year, -4, GETDATE());
INSERT INTO Contacts 
(Contact_ID      ,Company,Company_Name,Display_Name,Prefix_ID,First_Name  ,Middle_Name,Last_Name ,Suffix_ID,Nickname ,Date_of_Birth   ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address                   ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@child4ContactId,0      ,null        ,'Josina'    ,2        ,'Josina'    ,null       ,'Tremplay',null     ,'Josina' ,@child4DOB      ,1        ,1                ,1                ,@householdId,3                    ,null              ,null        ,'Josina.Tremplay@gmail.com'   ,null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET @childFoster1ContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @childFoster1DOB = DATEADD(year, -9, GETDATE());
INSERT INTO Contacts 
(Contact_ID           ,Company,Company_Name,Display_Name,Prefix_ID,First_Name ,Middle_Name,Last_Name ,Suffix_ID,Nickname ,Date_of_Birth        ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address            ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@childFoster1ContactId,0      ,null        ,'Vincent'   ,2        ,'Vincent'  ,null       ,'Brown'   ,null     ,'Vincent',@childFoster1DOB      ,1        ,1                ,1                ,@householdId,3                    ,null              ,null        ,'vincent.brown@gmail.com',null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET @childFoster2ContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @childFoster2DOB = DATEADD(year, -9, GETDATE());
INSERT INTO Contacts 
(Contact_ID           ,Company,Company_Name,Display_Name,Prefix_ID,First_Name ,Middle_Name,Last_Name ,Suffix_ID,Nickname ,Date_of_Birth        ,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address            ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@childFoster2ContactId,0      ,null        ,'Sophia'    ,2        ,'Sophia'   ,null       ,'Bemmen'  ,null     ,'Sophia' ,@childFoster2DOB      ,1        ,1                ,1                ,@householdId,3                    ,null              ,null        ,'sophia.bemmen@gmail.com',null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET @legalWard1ContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @legalWard1DOB = DATEADD(year, -60, GETDATE());
INSERT INTO Contacts 
(Contact_ID         ,Company,Company_Name,Display_Name ,Prefix_ID,First_Name ,Middle_Name,Last_Name    ,Suffix_ID,Nickname ,Date_of_Birth,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address              ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@legalWard1ContactId,0      ,null        ,'George'     ,2        ,'George'   ,null       ,'Tremplay'   ,null     ,'George' ,@legalWard1DOB,1        ,1                ,1                ,@householdId,3                    ,null              ,null        ,'george.tremplay@gmail.com',null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET @legalWard2ContactId = ((SELECT MAX(Contact_ID) FROM Contacts)+1);
SET @legalWard2DOB = DATEADD(year, -58, GETDATE());
INSERT INTO Contacts 
(Contact_ID         ,Company,Company_Name,Display_Name ,Prefix_ID,First_Name ,Middle_Name,Last_Name    ,Suffix_ID,Nickname   ,Date_of_Birth,Gender_ID,Marital_Status_ID,Contact_Status_ID,Household_ID,Household_Position_ID,Participant_Record,Donor_Record,Email_Address           ,Email_Unlisted,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Mobile_Phone  ,Mobile_Carrier,Mobile_Phone_Unlisted,Company_Phone,Pager_Phone,Fax_Phone,User_Account,Web_Page,Remove_From_Directory,Industry_ID,Occupation_ID,Employer_Name,[SSN/EIN],Anniversary_Date,HS_Graduation_Year,Current_School,Contact_GUID,ID_Card,Domain_ID,__ShelbyID,__ExternalHouseholdID,__ExternalPersonID,__ExternalUserID,__ExternalBusinessID,Maiden_Name,__LastLegacyLogin,__LegacyUserName,__LegacyUserID,__LegacyEmailAddress) VALUES
(@legalWard2ContactId,0      ,null        ,'Margaret'   ,2        ,'Margaret' ,null       ,'Ray'        ,null     ,'Margaret' ,@legalWard2DOB,1        ,1                ,1                ,@householdId,3                    ,null              ,null        ,'margaret.ray@gmail.com',null          ,0                 ,0               ,'513-410-3540',null          ,null                 ,null         ,null       ,null     ,null        ,null    ,null                 ,null       ,null         ,null         ,null     ,null            ,null              ,null          ,NEWID()     ,null   ,1        ,null      ,null                 ,null              ,null            ,null                ,null       ,null             ,null            ,null          ,null                );

SET IDENTITY_INSERT [dbo].[Contacts] OFF;
DBCC CHECKIDENT (Contacts, reseed, @currentContactId);



SET IDENTITY_INSERT [dbo].[Participants] ON;

SET @currentParticipantId = IDENT_CURRENT('Participants');

SET @motherParticipantId = ((SELECT MAX(Participant_ID) FROM Participants) +1);
INSERT INTO Participants 
( Participant_ID          , Contact_ID      , Participant_Type_ID, Participant_Start_Date, Participant_End_Date, Notes, Domain_ID, __ExternalPersonID, _First_Attendance_Ever, _Second_Attendance_Ever, _Third_Attendance_Ever, _Last_Attendance_Ever) VALUES
( @motherParticipantId    , @motherContactId, 1                  , '01/01/2015'          , null                , null , 1        , null              , null                  ,  null                  ,  null                 ,null                  );

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
DBCC CHECKIDENT (Participants, reseed, @currentParticipantId);



UPDATE [dbo].Contacts 
SET Participant_Record = @motherParticipantId
WHERE Contact_ID = @motherContactId ;


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