USE [MinistryPlatform]
GO

DECLARE @SouthAfricaForm int;
SET @SouthAfricaForm = 20; 

IF NOT EXISTS (SELECT * FROM [dbo].[Forms] WHERE Form_ID = @SouthAfricaForm)
BEGIN
SET IDENTITY_INSERT [dbo].[Forms] ON

INSERT INTO [dbo].[Forms]
([Form_ID], [Form_Title], [Instructions], [Get_Contact_Info], [Get_Address_Info], [Domain_ID], [Form_GUID])
VALUES
(@SouthAfricaForm, 'GO South Africa Application', 'Fill this out this form to be considered for a GO South Africa trip', 1, 1, 1, NEWID())

SET IDENTITY_INSERT [dbo].[Forms] OFF

DELETE FROM [dbo].[Form_Fields] WHERE (Form_ID = @SouthAfricaForm)

INSERT INTO [dbo].[Form_Fields] 
([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) 
VALUES 
(10, 'Guardian First Name', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 600),
(20, 'Guardian Last Name', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 601),
(30, 'T-Shirt Size', 5, 'Adult XS
Adult S
Adult M
Adult L
Adult XL
Adult XXL
Adult XXXL
Child S
Child M
Child L', 1, @SouthAfricaForm, 1, 0, 602),
(40, 'Scrub Size', 5, 'Adult XS
Adult S
Adult M
Adult L
Adult XL
Adult XXL
Adult XXXL', 0, @SouthAfricaForm, 1, 0, 603),
(50, 'Vegetarian', 7, 'Yes
No', 1, @SouthAfricaForm, 1, 0, 604),
(60, 'Food Allergies', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 605),
(70, 'Medical Conditions', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 606),
(80, 'Home Church', 5, 'None
Uptown
Florence
I do not attend Crossroads
Mason
Not Site Specific
Oakley
West Side', 1, @SouthAfricaForm, 1, 0, 607),
(90, 'How did you hear about this trip?', 5, 'Program
Info Session
Friend/Family
Website
Other', 1, @SouthAfricaForm, 1, 0, 608),
(100, 'Why do you want to go on this trip?', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 609),
(110, 'Searching for answers', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 610),
(120, 'I have received Jesus', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 611),
(130, 'Focusing on obedience', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 612),
(140, 'I''m activtely replicating my faith', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 613),
(150, 'Emergency Contact First Name', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 614),
(160, 'Emergency Contact Last Name', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 615),
(170, 'Emergency Contact Email', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 616),
(180, 'Emergency Contact Primary Phone', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 617),
(190, 'Emergency Contact Secondary Phone', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 618),
(200, 'Lottery', 4, 'As long as I am selected, I will go on the trip
If my spouse or any member of my family is not selected, none of us will go
If my spouse or any member of my family is not selected, all others will go
If my spouse or any member of my family OR any member of our larger group of friends is not selected, none of us will go
If my spouse or any member of my family OR any member of our larger group of friends is not selected, my spouse or family will still go', 1, @SouthAfricaForm, 1, 0, 619),
(210, 'Common Name For Group', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 620),
(220, 'Roommate First Choice', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 621),
(230, 'Roommate Second Choice', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 622),
(240, 'Support Person Email', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 623),
(250, 'Insterested In Being A GO Group Leader?', 7, 'Yes
No', 1, @SouthAfricaForm, 1, 0, 624),
(260, 'Why would you like to be a GO Group Leader', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 625),
(270, 'Previous Trip Experience', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 626),
(280, 'Business Professional', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 627),
(290, 'Construction Professional', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 628),
(300, 'Dental Professional', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 629),
(310, 'Education Professional', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 630),
(320, 'IT Professional', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 631),
(330, 'Media / Journalism Professional', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 632),
(340, 'Medical Professional', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 633),
(350, 'Music Professional', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 634),
(360, 'Other Professional', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 635),
(370, 'Photography Professional', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 636),
(380, 'Social Worker / Counselor', 8, 'NULL', 0, @SouthAfricaForm, 1, 0, 637),
(390, 'Student',8, 'NULL', 0, @SouthAfricaForm, 1, 0, 638),
(400, 'Valid Passport', 7, 'Yes
No', 1, @SouthAfricaForm, 1, 0, 639),
(410, 'Passport Expiration Date', 3, 'NULL', 1, @SouthAfricaForm, 1, 0, 640),
(420, 'Passport First Name', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 641),
(430, 'Passport Middle Name', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 642),
(440, 'Passport Last Name', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 643),
(450, 'Passport Country', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 644),
(460, 'Passport Birthday', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 645),
(470, 'Delta Frequent Flyer Number', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 646),
(480, 'South African Airlines Frequent Flyer Number', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 647),
(490, 'United Frequent Flyer Number', 1, 'NULL', 0, @SouthAfricaForm, 1, 0, 648),
(500, 'International Travel Experience', 5, 'I take three or more international trips a year.
I take less than three international trips per year.
I have not traveled internationally.', 1, @SouthAfricaForm, 1, 0, 649),
(510, 'Experience Abroad Outside GO Trips', 7, 'Yes
No', 1, @SouthAfricaForm, 1, 0, 650),
(520, 'Describe Experience Abroad', 1, 'NULL', 1, @SouthAfricaForm, 1, 0, 651)
END