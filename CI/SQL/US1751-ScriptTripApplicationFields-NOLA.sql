USE [MinistryPlatform]
GO

DECLARE @NOLAForm int;
SET @NOLAForm = 21; 

IF NOT EXISTS (SELECT * FROM [dbo].[Forms] WHERE Form_ID = @NOLAForm)
BEGIN
SET IDENTITY_INSERT [dbo].[Forms] ON

INSERT INTO [dbo].[Forms]
([Form_ID], [Form_Title], [Instructions], [Get_Contact_Info], [Get_Address_Info], [Domain_ID], [Form_GUID])
VALUES
(@NOLAForm, 'GO NOLA Application', 'Fill this out this form to be considered for a GO NOLA trip', 1, 1, 1, NEWID())

SET IDENTITY_INSERT [dbo].[Forms] OFF

DELETE FROM [dbo].[Form_Fields] WHERE (Form_ID = @NOLAForm)

INSERT INTO [dbo].[Form_Fields] 
([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) 
VALUES 
(10, 'Guardian First Name', 1, 'NULL', 0, @NOLAForm, 1, 0, 700),
(20, 'Guardian Last Name', 1, 'NULL', 0, @NOLAForm, 1, 0, 701),
(30, 'T-Shirt Size', 5, 'Adult XS
Adult S
Adult M
Adult L
Adult XL
Adult XXL
Adult XXXL
Child S
Child M
Child L', 1, @NOLAForm, 1, 0, 702),
(40, 'Vegetarian', 7, 'Yes
No', 1, @NOLAForm, 1, 0, 703),
(50, 'Food Allergies', 1, 'NULL', 0, @NOLAForm, 1, 0, 704),
(60, 'Medical Conditions', 1, 'NULL', 0, @NOLAForm, 1, 0, 705),
(70, 'Home Church', 5, 'None
Uptown
Florence
I do not attend Crossroads
Mason
Not Site Specific
Oakley
West Side', 1, @NOLAForm, 1, 0, 706),
(80, 'How did you hear about this trip?', 5, 'Program
Info Session
Friend/Family
Website
Other', 1, @NOLAForm, 1, 0, 707),
(90, 'Why do you want to go on this trip?', 1, 'NULL', 1, @NOLAForm, 1, 0, 708),
(100, 'Searching for answers', 8, 'NULL', 0, @NOLAForm, 1, 0, 709),
(110, 'I have received Jesus', 8, 'NULL', 0, @NOLAForm, 1, 0, 710),
(120, 'Focusing on obedience', 8, 'NULL', 0, @NOLAForm, 1, 0, 711),
(130, 'I''m activtely replicating my faith', 8, 'NULL', 0, @NOLAForm, 1, 0, 712),
(140, 'Emergency Contact First Name', 1, 'NULL', 1, @NOLAForm, 1, 0, 713),
(150, 'Emergency Contact Last Name', 1, 'NULL', 1, @NOLAForm, 1, 0, 714),
(160, 'Emergency Contact Email', 1, 'NULL', 1, @NOLAForm, 1, 0, 715),
(170, 'Emergency Contact Primary Phone', 1, 'NULL', 1, @NOLAForm, 1, 0, 716),
(180, 'Emergency Contact Secondary Phone', 1, 'NULL', 0, @NOLAForm, 1, 0, 717),
(190, 'Lottery', 4, 'As long as I am selected, I will go on the trip
If my spouse or any member of my family is not selected, none of us will go
If my spouse or any member of my family is not selected, all others will go
If my spouse or any member of my family OR any member of our larger group of friends is not selected, none of us will go
If my spouse or any member of my family OR any member of our larger group of friends is not selected, my spouse or family will still go', 1, @NOLAForm, 1, 0, 718),
(200, 'Common Name For Group', 1, 'NULL', 0, @NOLAForm, 1, 0, 719),
(210, 'Roommate First Choice', 1, 'NULL', 0, @NOLAForm, 1, 0, 720),
(220, 'Roommate Second Choice', 1, 'NULL', 0, @NOLAForm, 1, 0, 721),
(230, 'Support Person Email', 1, 'NULL', 0, @NOLAForm, 1, 0, 722),
(240, 'Insterested In Being A GO Group Leader?', 7, 'Yes
No', 1, @NOLAForm, 1, 0, 723),
(250, 'Why would you like to be a GO Group Leader', 1, 'NULL', 1, @NOLAForm, 1, 0, 724),
(260, 'First Choice Work Team', 5, 'Habitat (must be 16 or older)
Hands on - Neighborhood Restoration
Vacation Bible Camp', 1, @NOLAForm, 1, 0, 725),
(270, 'Experience with First Choice', 1, 'NULL', 1, @NOLAForm, 1, 0, 726),
(280, 'Second Choice Work Team', 5, 'Habitat (must be 16 or older)
Hands on - Neighborhood Restoration
Vacation Bible Camp', 1, @NOLAForm, 1, 0, 727),
(290, 'Previous Trip Experience', 1, 'NULL', 1, @NOLAForm, 1, 0, 728),
(300, 'Business Professional', 8, 'NULL', 0, @NOLAForm, 1, 0, 729),
(310, 'Construction Professional', 8, 'NULL', 0, @NOLAForm, 1, 0, 730),
(320, 'Dental Professional', 8, 'NULL', 0, @NOLAForm, 1, 0, 731),
(330, 'Education Professional', 8, 'NULL', 0, @NOLAForm, 1, 0, 732),
(340, 'IT Professional', 8, 'NULL', 0, @NOLAForm, 1, 0, 733),
(350, 'Media / Journalism Professional', 8, 'NULL', 0, @NOLAForm, 1, 0, 734),
(360, 'Medical Professional', 8, 'NULL', 0, @NOLAForm, 1, 0, 735),
(370, 'Music Professional', 8, 'NULL', 0, @NOLAForm, 1, 0, 736),
(380, 'Other Professional', 8, 'NULL', 0, @NOLAForm, 1, 0, 737),
(390, 'Photography Professional', 8, 'NULL', 0, @NOLAForm, 1, 0, 738),
(400, 'Social Worker / Counselor', 8, 'NULL', 0, @NOLAForm, 1, 0, 739),
(410, 'Student',8, 'NULL', 0, @NOLAForm, 1, 0, 740)
END