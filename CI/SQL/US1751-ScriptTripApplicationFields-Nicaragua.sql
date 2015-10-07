USE [MinistryPlatform]
GO

DECLARE @NicaraguaForm int;
SET @NicaraguaForm = 22; 

IF NOT EXISTS (SELECT * FROM [dbo].[Forms] WHERE Form_ID = @NicaraguaForm)
BEGIN
SET IDENTITY_INSERT [dbo].[Forms] ON

INSERT INTO [dbo].[Forms]
([Form_ID], [Form_Title], [Instructions], [Get_Contact_Info], [Get_Address_Info], [Domain_ID], [Form_GUID])
VALUES
(@NicaraguaForm, 'GO Nicaragua Application', 'Fill this out this form to be considered for a GO Nicaragua trip', 1, 1, 1, NEWID())

SET IDENTITY_INSERT [dbo].[Forms] OFF

DELETE FROM [dbo].[Form_Fields] WHERE (Form_ID = @NicaraguaForm)

INSERT INTO [dbo].[Form_Fields] 
([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) 
VALUES 
(10, 'Guardian First Name', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 800),
(20, 'Guardian Last Name', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 801),
(30, 'T-Shirt Size', 5, 'Adult XS
Adult S
Adult M
Adult L
Adult XL
Adult XXL
Adult XXXL
Child S
Child M
Child L', 1, @NicaraguaForm, 1, 0, 802),
(40, 'Vegetarian', 7, 'Yes
No', 1, @NicaraguaForm, 1, 0, 803),
(50, 'Food Allergies', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 804),
(60, 'Medical Conditions', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 805),
(70, 'Home Church', 5, 'None
Uptown
Florence
I do not attend Crossroads
Mason
Not Site Specific
Oakley
West Side', 1, @NicaraguaForm, 1, 0, 806),
(80, 'How did you hear about this trip?', 5, 'Program
Info Session
Friend/Family
Website
Other', 1, @NicaraguaForm, 1, 0, 807),
(90, 'Why do you want to go on this trip?', 1, 'NULL', 1, @NicaraguaForm, 1, 0, 808),
(100, 'Searching for answers', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 809),
(110, 'I have received Jesus', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 810),
(120, 'Focusing on obedience', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 811),
(130, 'I''m activtely replicating my faith', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 812),
(140, 'Emergency Contact First Name', 1, 'NULL', 1, @NicaraguaForm, 1, 0, 813),
(150, 'Emergency Contact Last Name', 1, 'NULL', 1, @NicaraguaForm, 1, 0, 814),
(160, 'Emergency Contact Email', 1, 'NULL', 1, @NicaraguaForm, 1, 0, 815),
(170, 'Emergency Contact Primary Phone', 1, 'NULL', 1, @NicaraguaForm, 1, 0, 816),
(180, 'Emergency Contact Secondary Phone', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 817),
(190, 'Lottery', 4, 'As long as I am selected, I will go on the trip
If my spouse or any member of my family is not selected, none of us will go
If my spouse or any member of my family is not selected, all others will go
If my spouse or any member of my family OR any member of our larger group of friends is not selected, none of us will go
If my spouse or any member of my family OR any member of our larger group of friends is not selected, my spouse or family will still go', 1, @NicaraguaForm, 1, 0, 818),
(200, 'Common Name For Group', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 819),
(210, 'Roommate First Choice', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 820),
(220, 'Roommate Second Choice', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 821),
(230, 'Support Person Email', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 822),
(240, 'Insterested In Being A GO Group Leader?', 7, 'Yes
No', 1, @NicaraguaForm, 1, 0, 823),
(250, 'Why would you like to be a GO Group Leader', 1, 'NULL', 1, @NicaraguaForm, 1, 0, 824),
(260, 'Sponsored a child through Crossroads in Nicaragua?', 7, 'Yes
No', 1, @NicaraguaForm, 1, 0, 825),
(270, 'Sponsored Child''s First Name', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 826),
(280, 'Sponsored Child''s Last Name', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 827),
(290, 'Sponsored Child''s Number', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 828),
(300, 'Previous Trip Experience', 1, 'NULL', 1, @NicaraguaForm, 1, 0, 829),
(310, 'Business Professional', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 830),
(320, 'Construction Professional', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 831),
(330, 'Dental Professional', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 832),
(340, 'Education Professional', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 833),
(350, 'IT Professional', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 834),
(360, 'Media / Journalism Professional', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 835),
(370, 'Medical Professional', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 836),
(380, 'Music Professional', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 837),
(390, 'Other Professional', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 838),
(400, 'Photography Professional', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 839),
(410, 'Social Worker / Counselor', 8, 'NULL', 0, @NicaraguaForm, 1, 0, 840),
(420, 'Student',8, 'NULL', 0, @NicaraguaForm, 1, 0, 841),
(430, 'Valid Passport', 7, 'Yes
No', 1, @NicaraguaForm, 1, 0, 842),
(440, 'Passport Expiration Date', 3, 'NULL', 1, @NicaraguaForm, 1, 0, 843),
(450, 'Passport First Name', 1, 'NULL', 1, @NicaraguaForm, 1, 0, 844),
(460, 'Passport Middle Name', 1, 'NULL', 1, @NicaraguaForm, 1, 0, 845),
(470, 'Passport Last Name', 1, 'NULL', 1, @NicaraguaForm, 1, 0, 846),
(480, 'Passport Country', 1, 'NULL', 1, @NicaraguaForm, 1, 0, 847),
(490, 'Passport Birthday', 1, 'NULL', 1, @NicaraguaForm, 1, 0, 848),
(500, 'Delta Frequent Flyer Number', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 849),
(510, 'United Frequent Flyer Number', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 850),
(520, 'US Airways Frequent Flyer Number', 1, 'NULL', 0, @NicaraguaForm, 1, 0, 851),
(530, 'International Travel Experience', 5, 'I take three or more international trips a year.
I take less than three international trips per year.
I have not traveled internationally.', 1, @NicaraguaForm, 1, 0, 852),
(540, 'Experience Abroad Outside GO Trips', 7, 'Yes
No', 1, @NicaraguaForm, 1, 0, 853),
(550, 'Describe Experience Abroad', 1, 'NULL', 1, @NicaraguaForm, 1, 0, 854)
END