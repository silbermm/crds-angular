USE [MinistryPlatform]
GO

DECLARE @SouthAfricaForm int;
SET @SouthAfricaForm = 20; 

INSERT INTO [dbo].[Form_Fields] 
	([Field_Order], 
	[Field_Label], 
	[Field_Type_ID], 
	[Field_Values],
	[Required], 
	[Form_ID], 
	[Domain_ID], 
	[Placement_Required], 
	[CrossroadsId]) 
VALUES 
	(10, 'GuardianFirstName', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 600),
	(20, 'GuardianLastName', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 601),
	(30, 'TShirtSize', 5, 'Adult XS
Adult S
Adult M
Adult L
Adult XL
Adult XXL
Adult XXXL
Child S
Child M
Child L', 1, @SouthAfricaForm, 1, 0, 602),
(40, 'ScrubSize', 5, 'Adult XS
Adult S
Adult M
Adult L
Adult XL
Adult XXL
Adult XXXL', 0, @SouthAfricaForm, 1, 0, 603),
(50, 'Vegetarian', 7, 'Yes
No', 1, @SouthAfricaForm, 1, 0, 604),
(60, 'FoodAllergies', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 605),
(70, 'MedicalConditions', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 606),
(80, 'HomeChurch', 5, 'None
Clifton
Florence
I do not attend Crossroads
Mason
Not Site Specific
Oakley
West Side', 1, @SouthAfricaForm, 1, 0, 607),
(90, 'HowDidYouHearAboutThisTrip', 5, 'Program
Info Session
Friend/Family
Website
Other', 1, @SouthAfricaForm, 1, 0, 608),
(100, 'WhyDoYouWantToGo', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 609),
(110, 'SpiritualLife', 5, 'Searching for answers
I have received Jesus
Focusing on obedience - I''ve quit negotiating with God.
I''m activtely replicating my faith in others.', 1, @SouthAfricaForm, 1, 0, 610),
(120, 'EmergencyContactFirstName', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 611),
(130, 'EmergencyContactLastName', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 612),
(140, 'EmergencyContactEmail', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 613),
(150, 'EmergencyContactPrimaryPhone', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 614),
(160, 'EmergencyContactSecondaryPhone', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 615),
(170, 'Lottery', 5, 'As long as I am selected, I will go on the trip
If my spouse or any member of my family is not selected, none of us will go
If my spouse or any member of my family is not selected, all others will go
If my spouse or any member of my family OR any member of our larger group of friends is not selected, none of us will go
If my spouse or any member of my family OR any member of our larger group of friends is not selected, my spouse or family will still go', 1, @SouthAfricaForm, 1, 0, 616),
(180, 'CommonName', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 617),
(190, 'RoommateFirstChoice', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 618),
(200, 'RoommateSecondChoice', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 619),
(210, 'SupportPersonName', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 620),
(220, 'SupportPersonEmail', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 621),
(230, 'InsterestedGOGroupLeader', 7, 'Yes
No', 1, @SouthAfricaForm, 1, 0, 622),
(240, 'WhyGOGroupLeader', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 623),
(250, 'PreviousTripExperience', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 624),
(260, 'Professional', 1, 'Business
Construction
Dental
Education
IT
Media / Journalism
Medical
Music
Other
Photography
Social Worker / Counselor
Student', 1, @SouthAfricaForm, 1, 0, 625),
(270, 'ValidPassport', 7, 'Yes
No', 1, @SouthAfricaForm, 1, 0, 626),
(280, 'PassportExpiration', 3, 'NULL', 1, @SouthAfricaForm, 1, 0, 627),
(290, 'PassportFirstName', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 628),
(300, 'PassportMiddleName', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 629),
(310, 'PassportLastName', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 630),
(320, 'PassportCountry', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 631),
(330, 'PassportBirthday', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 632),
(340, 'DeltaNumber', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 633),
(350, 'SouthAfricanAirlinesNumber', 0, 'NULL', 1, @SouthAfricaForm, 1, 0, 634),
(360, 'UnitedNumber', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 635),
(370, 'InternationalTravel', 5, 'I take three or more international trips a year.
I take less than three international trips per year.
I have not traveled internationally.', 1, @SouthAfricaForm, 1, 0, 636),
(380, 'ExperienceAbroadOutsideGOTrips', 7, 'Yes
No', 1, @SouthAfricaForm, 1, 0, 637),
(390, 'DescribeExperienceAbroad', 0, 'NULL', 1, @SouthAfricaForm, 1, 0, 638)