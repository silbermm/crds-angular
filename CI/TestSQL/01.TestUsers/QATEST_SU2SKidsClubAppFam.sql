USE [MinistryPlatform]
GO

--KidsClubDad Contact
DECLARE @contactID as int
set @contactID = (select contact_id from contacts where Email_Address = 'kidsclubdad@gmail.com' and Last_Name = 'KidsClubAppFam');

-- KidsClubAppFam Address
SET IDENTITY_INSERT [dbo].[Addresses] ON;

DECLARE @addressId as int
set @addressId = IDENT_CURRENT('Addresses')+1;

INSERT INTO [dbo].Addresses 
(Address_ID,Address_Line_1     ,Address_Line_2,City  ,[State/Region],Postal_Code,Foreign_Country,Country_Code,Domain_ID,Carrier_Route,Lot_Number,Delivery_Point_Code,Delivery_Point_Check_Digit,Latitude,Longitude,Altitude,Time_Zone,Bar_Code,Area_Code,Last_Validation_Attempt,County,Validated,Do_Not_Validate,Last_GeoCode_Attempt,__ExternalAddressID) VALUES
(@addressID,'1234 Testing Lane',null          ,'CITY','OH'          ,'45067'    ,'United States','USA'       ,1        ,null         ,null      ,null               ,null                      ,null    ,null     ,null    ,null     ,null    ,null     ,null                   ,null  ,null     ,null           ,null                ,null               );

SET IDENTITY_INSERT [dbo].[Addresses] OFF;

--KidsClubAppFam Household
DECLARE @houseHoldID as int
set @houseHoldID = (select Household_ID from contacts where contact_id = @contactID);

UPDATE [dbo].Households 
SET Address_ID = @addressID, Home_Phone = '123-867-5309', Congregation_ID = 6, Household_Source_ID = 38                        
WHERE houseHold_ID = @houseHoldID;

--KidsClubDad Contact updates
UPDATE [dbo].Contacts 
SET Prefix_ID = 1, Middle_Name = 'Jokes', Nickname = 'Dadjokes', Date_of_Birth = {d '1964-01-01'}, Gender_ID = 1, Marital_Status_ID = 2, Household_Position_ID = 1, Mobile_Phone = '123-654-8745', Company_Phone = '555-365-4125'
WHERE Contact_ID = @contactID;
GO

--Contact for KidsclubMom@gmail.com
DECLARE @contactID as int
set @contactID = (select contact_id from contacts where Email_Address = 'kidsclubmom@gmail.com' and Last_Name = 'KidsClubAppFam');

--Household for kids club mom
DECLARE @houseHoldID as int
set @houseHoldID = (select HouseHold_ID from Contacts where email_address = 'kidsclubdad@gmail.com' and last_name = 'KidsClubAppFam');

--KidsClubMom Contact updates
UPDATE [dbo].Contacts 
SET Prefix_ID = 2, Middle_Name = 'A', Nickname = 'Momma', Date_of_Birth = {d '1965-01-01'}, Gender_ID = 2, Marital_Status_ID = 2, HouseHold_ID = @houseHoldID, Household_Position_ID = 1, Mobile_Phone = '321-645-8184'
WHERE contact_id = @contactID;
GO

--Kidsclubkid14@gmail.com contact 
DECLARE @contactID as int
set @contactID = (select contact_id from contacts where Email_Address = 'kidsclubkid14@gmail.com' and Last_Name = 'KidsClubAppFam');

--Household for kidsclubkid14
DECLARE @houseHoldID as int
set @houseHoldID = (select HouseHold_ID from Contacts where email_address = 'kidsclubdad@gmail.com' and last_name = 'KidsClubAppFam');

--KidsClubKid14 Contact Updates
UPDATE [dbo].Contacts 
SET Nickname = 'Billy the Kid', Date_of_Birth = {d '2001-01-01'}, Gender_ID = 2, Marital_Status_ID = 1, HouseHold_ID = @houseHoldID, Household_Position_ID = 2, Mobile_Phone = '321-548-6154'
WHERE Contact_ID = @contactID;
GO

--Contact for kidsclubkid17@gmail.com
DECLARE @contactID as int
set @contactID = (select contact_id from contacts where Email_Address = 'kidsclubkid17@gmail.com' and Last_Name = 'KidsClubAppFam');

--Household for kidsclubkid17
DECLARE @houseHoldID as int
set @houseHoldID = (select HouseHold_ID from Contacts where email_address = 'kidsclubdad@gmail.com' and last_name = 'KidsClubAppFam');

UPDATE [dbo].Contacts 
SET Middle_Name = 'D', Date_of_Birth = {d '1998-01-01'}, Marital_Status_ID = 1, HouseHold_ID = @houseHoldID, Household_Position_ID = 4, Mobile_Phone = '654-818-1425'
WHERE Contact_ID = @contactID;
GO

--KidsClubDad Request to Join Kids' Club Response Record
SET IDENTITY_INSERT [dbo].[Responses] ON;

DECLARE @respID as int
set @respID = IDENT_CURRENT('Responses')+1;

DECLARE @partID as int
set @partID = (select Participant_Record from Contacts where Email_Address = 'kidsclubdad@gmail.com' and Last_Name = 'KidsClubAppFam');

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
set @partID = (select Participant_Record from Contacts where Email_Address = 'kidsclubkid14@gmail.com' and Last_Name = 'KidsClubAppFam');

INSERT INTO [dbo].Responses 
(Response_ID,Response_Date             ,Opportunity_ID,Participant_ID,Comments                        ,Website_Submission,First_Name,Last_Name,Email,Phone,Follow_up_Information,Response_Result_ID,Closed,Domain_ID,Event_ID,__ExternalCESTPID) VALUES
(@respID    ,{ts '2015-07-02 08:20:21'},115           ,@partID       ,'Request on 7/2/2015 8:20:21 AM',null              ,null      ,null     ,null ,null ,null                 ,null              ,0 ,1        ,null    ,null             )

SET IDENTITY_INSERT [dbo].[Responses] OFF;
GO

--Family Relationships
DECLARE @dadContact as int
set @dadContact = (select Contact_ID from Contacts where Email_Address = 'kidsclubdad@gmail.com' and Last_Name = 'KidsClubAppFam');

DECLARE @momContact as int
set @momContact = (select Contact_ID from Contacts where Email_Address = 'kidsclubmom@gmail.com');

DECLARE @kid14Contact as int
set @kid14Contact = (select Contact_ID from Contacts where email_address = 'kidsclubkid14@gmail.com');

DECLARE @kid17Contact as int
set @kid17Contact = (select Contact_ID from Contacts where email_address = 'kidsclubkid17@gmail.com');

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