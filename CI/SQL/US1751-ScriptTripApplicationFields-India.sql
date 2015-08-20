USE [MinistryPlatform]
GO

DECLARE @IndiaForm int;
SET @IndiaForm = 23; 

IF NOT EXISTS (SELECT * FROM [dbo].[Forms] WHERE Form_ID = @IndiaForm)
BEGIN
SET IDENTITY_INSERT [dbo].[Forms] ON

INSERT INTO [dbo].[Forms]
([Form_ID], [Form_Title], [Instructions], [Get_Contact_Info], [Get_Address_Info], [Domain_ID], [Form_GUID])
VALUES
(@IndiaForm, 'GO India Application', 'Fill this out this form to be considered for a GO India trip', 1, 1, 1, NEWID())

SET IDENTITY_INSERT [dbo].[Forms] OFF

DELETE FROM [dbo].[Form_Fields] WHERE (Form_ID = @IndiaForm)

INSERT INTO [dbo].[Form_Fields] 
([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) 
VALUES 
(10, 'Guardian First Name', 1, 'NULL', 0, @IndiaForm, 1, 0, 900),
(20, 'Guardian Last Name', 1, 'NULL', 0, @IndiaForm, 1, 0, 901),
(30, 'T-Shirt Size', 5, 'Adult XS
Adult S
Adult M
Adult L
Adult XL
Adult XXL
Adult XXXL
Child S
Child M
Child L', 1, @IndiaForm, 1, 0, 902),
(40, 'Vegetarian', 7, 'Yes
No', 1, @IndiaForm, 1, 0, 903),
(50, 'Food Allergies', 1, 'NULL', 0, @IndiaForm, 1, 0, 904),
(60, 'Medical Conditions', 1, 'NULL', 0, @IndiaForm, 1, 0, 905),
(70, 'Home Church', 5, 'None
Uptown
Florence
I do not attend Crossroads
Mason
Not Site Specific
Oakley
West Side', 1, @IndiaForm, 1, 0, 906),
(80, 'How did you hear about this trip?', 5, 'Program
Info Session
Friend/Family
Website
Other', 1, @IndiaForm, 1, 0, 907),
(90, 'Why do you want to go on this trip?', 1, 'NULL', 1, @IndiaForm, 1, 0, 908),
(100, 'Searching for answers', 8, 'NULL', 0, @IndiaForm, 1, 0, 909),
(110, 'I have received Jesus', 8, 'NULL', 0, @IndiaForm, 1, 0, 910),
(120, 'Focusing on obedience', 8, 'NULL', 0, @IndiaForm, 1, 0, 911),
(130, 'I''m activtely replicating my faith', 8, 'NULL', 0, @IndiaForm, 1, 0, 912),
(140, 'Emergency Contact First Name', 1, 'NULL', 1, @IndiaForm, 1, 0, 913),
(150, 'Emergency Contact Last Name', 1, 'NULL', 1, @IndiaForm, 1, 0, 914),
(160, 'Emergency Contact Email', 1, 'NULL', 1, @IndiaForm, 1, 0, 915),
(170, 'Emergency Contact Primary Phone', 1, 'NULL', 1, @IndiaForm, 1, 0, 916),
(180, 'Emergency Contact Secondary Phone', 1, 'NULL', 0, @IndiaForm, 1, 0, 917),
(190, 'Roommate First Choice', 1, 'NULL', 0, @IndiaForm, 1, 0, 918),
(200, 'Roommate Second Choice', 1, 'NULL', 0, @IndiaForm, 1, 0, 919),
(210, 'Support Person Email', 1, 'NULL', 0, @IndiaForm, 1, 0, 920),
(220, 'Insterested In Being A GO Group Leader?', 7, 'Yes
No', 1, @IndiaForm, 1, 0, 921),
(230, 'Why would you like to be a GO Group Leader', 1, 'NULL', 1, @IndiaForm, 1, 0, 922),
(240, 'Previous Trip Experience', 1, 'NULL', 1, @IndiaForm, 1, 0, 923),
(250, 'Business Professional', 8, 'NULL', 0, @IndiaForm, 1, 0, 924),
(260, 'Construction Professional', 8, 'NULL', 0, @IndiaForm, 1, 0, 925),
(270, 'Dental Professional', 8, 'NULL', 0, @IndiaForm, 1, 0, 926),
(280, 'Education Professional', 8, 'NULL', 0, @IndiaForm, 1, 0, 927),
(290, 'IT Professional', 8, 'NULL', 0, @IndiaForm, 1, 0, 928),
(300, 'Media / Journalism Professional', 8, 'NULL', 0, @IndiaForm, 1, 0, 929),
(310, 'Medical Professional', 8, 'NULL', 0, @IndiaForm, 1, 0, 930),
(320, 'Music Professional', 8, 'NULL', 0, @IndiaForm, 1, 0, 931),
(330, 'Other Professional', 8, 'NULL', 0, @IndiaForm, 1, 0, 932),
(340, 'Photography Professional', 8, 'NULL', 0, @IndiaForm, 1, 0, 933),
(350, 'Social Worker / Counselor', 8, 'NULL', 0, @IndiaForm, 1, 0, 934),
(360, 'Student',8, 'NULL', 0, @IndiaForm, 1, 0, 935),
(370, 'Valid Passport', 7, 'Yes
No', 1, @IndiaForm, 1, 0, 936),
(380, 'Passport Expiration Date', 3, 'NULL', 1, @IndiaForm, 1, 0, 937),
(390, 'Passport First Name', 1, 'NULL', 1, @IndiaForm, 1, 0, 938),
(400, 'Passport Middle Name', 1, 'NULL', 1, @IndiaForm, 1, 0, 939),
(410, 'Passport Last Name', 1, 'NULL', 1, @IndiaForm, 1, 0, 940),
(420, 'Passport Country', 1, 'NULL', 1, @IndiaForm, 1, 0, 941),
(430, 'Passport Birthday', 1, 'NULL', 1, @IndiaForm, 1, 0, 942),
(440, 'Delta Frequent Flyer Number', 1, 'NULL', 0, @IndiaForm, 1, 0, 943),
(450, 'United Frequent Flyer Number', 1, 'NULL', 0, @IndiaForm, 1, 0, 944),
(460, 'International Travel Experience', 5, 'I take three or more international trips a year.
I take less than three international trips per year.
I have not traveled internationally.', 1, @IndiaForm, 1, 0, 945),
(470, 'Experience Abroad Outside GO Trips', 7, 'Yes
No', 1, @IndiaForm, 1, 0, 946),
(480, 'Describe Experience Abroad', 1, 'NULL', 1, @IndiaForm, 1, 0, 947),
(490, 'Past Abuse History', 7, 'Yes
No', 1, @IndiaForm, 1, 0, 948)
END