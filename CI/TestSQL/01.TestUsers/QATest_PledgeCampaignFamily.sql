 USE [MinistryPlatform]
GO

DECLARE @HOH1ContactId AS INT
DECLARE @HOH2ContactId AS INT

DECLARE @HOH1DOB AS DATE
DECLARE @HOH2DOB AS DATE


SET IDENTITY_INSERT [dbo].[Addresses] ON;

DECLARE @addressId AS INT
set @addressId = IDENT_CURRENT('Addresses')+1;

INSERT INTO Addresses 
(Address_ID,Address_Line_1             ,Address_Line_2,City    ,[State/Region],Postal_Code,Foreign_Country,Country_Code,Domain_ID,Carrier_Route,Lot_Number,Delivery_Point_Code,Delivery_Point_Check_Digit,Latitude,Longitude,Altitude,Time_Zone,Bar_Code,Area_Code,Last_Validation_Attempt,County,Validated,Do_Not_Validate,Last_GeoCode_Attempt,__ExternalAddressID) VALUES
(@addressID,'1234 Pledge Campaign Lane',null          ,'Oakley','OH'          ,'45067'    ,'United States','USA'       ,1        ,null         ,null      ,null               ,null                      ,null    ,null     ,null    ,null     ,null    ,null     ,null                   ,null  ,null     ,null           ,null                ,null               );

SET IDENTITY_INSERT [dbo].[Addresses] OFF;


SET IDENTITY_INSERT [dbo].[Households] ON;

DECLARE @householdID AS INT
DECLARE @currenthouseholdId AS INT
SET @householdId = ((SELECT MAX(Household_ID) FROM Households) + 1);
SET @currentHouseholdId = IDENT_CURRENT('Households');

INSERT INTO Households 
(Household_ID,Household_Name,Address_ID  ,Home_Phone      ,Domain_ID,Congregation_ID  ,Care_Person, Household_Source_ID ,Family_Call_Number, Household_Preferences     ,Home_Phone_Unlisted   , Home_Address_Unlisted, Bulk_Mail_Opt_Out, _Last_Donation, _Last_Activity, __ExternalHouseholdID, __ExternalBusinessID) VALUES
(@householdId,'PCHousehold' ,@addressId  ,'513-410-3540'  ,1        ,6                ,null       , null                ,null              , null                      ,null                  , null                 , 0                ,null           ,null           ,null                  , null);

SET IDENTITY_INSERT [dbo].[Households] OFF;
DBCC CHECKIDENT (Households, reseed, @currentHouseholdId);



SET @HOH1ContactId = (SELECT Contact_ID FROM Contacts WHERE Email_Address = 'mpcrds+PCHead1@gmail.com' AND Last_Name = 'HeadofHousehold1');
SET @HOH1DOB = DATEADD(year, -40, GETDATE());

INSERT INTO Donors
(Contact_ID    , Statement_Frequency_ID, Statement_Type_ID, Statement_Method_ID, Setup_Date                   , Envelope_No, Cancel_Envelopes, Notes, First_Contact_Made, Domain_ID, __ExternalPersonID, _First_Donation_Date, _Last_Donation_Date, Processor_ID )  VALUES
(@HOH1ContactId, 2                     , 2                , 2                  , DATEADD(day, -10, GETDATE()) , null       , 0               , null , 1                 , 1        , null              , null                ,null                , null  );

UPDATE [dbo].Contacts 
SET Display_Name = 'PCHead1', Prefix_ID = 1, Middle_Name = null, Nickname = 'PCHead1', Date_of_Birth = @HOH1DOB , Gender_ID = 1, Marital_Status_ID = 2, Household_ID = @householdId, Household_Position_ID = 1, Mobile_Phone = '513-410-3540', Company_Phone = null
WHERE Contact_ID = @HOH1ContactID;

SET @HOH2ContactId = (SELECT Contact_ID FROM Contacts WHERE Email_Address = 'mpcrds+PCHead2@gmail.com' AND Last_Name = 'HeadofHousehold2');
SET @HOH2DOB = DATEADD(year, -39, GETDATE());

INSERT INTO Donors
(Contact_ID    , Statement_Frequency_ID, Statement_Type_ID, Statement_Method_ID, Setup_Date                   , Envelope_No, Cancel_Envelopes, Notes, First_Contact_Made, Domain_ID, __ExternalPersonID, _First_Donation_Date, _Last_Donation_Date, Processor_ID )  VALUES
(@HOH2ContactId, 2                     , 2                , 2                  , DATEADD(day, -10, GETDATE()) , null       , 0               , null , 1                 , 1        , null              , null                ,null                , null  );

UPDATE [dbo].Contacts 
SET Display_Name = 'PCHead2', Prefix_ID = 1, Middle_Name = null, Nickname = 'PCHead2', Date_of_Birth = @HOH2DOB , Gender_ID = 2, Marital_Status_ID = 2, Household_ID = @householdId, Household_Position_ID = 1, Mobile_Phone = '513-410-3540', Company_Phone = null
WHERE Contact_ID = @HOH2ContactID;


