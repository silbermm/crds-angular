USE [MinistryPlatform]
GO

DELETE FROM [dbo].[Form_Fields] WHERE (Form_ID = 16 OR Form_ID = 17)
GO

INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (20,'Last Name',1,'NULL',1,16,1,0,310)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (10,'First Name',1,'NULL',1,16,1,0,311)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (30,'Middle Initial',1,'NULL',0,16,1,0,312)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (90,'When did you first start attending Crossroads?',3,'NULL',1,16,1,0,314)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (70,'Gender',7,'Male
Female',1,16,1,0,315)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (60,'Date of Birth',3,'NULL',1,16,1,0,317)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (50,'Nickname for Name Tag',1,'NULL',1,16,1,0,318)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (110,'Street Address',1,'NULL',1,16,1,0,319)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (120,'City',1,'NULL',1,16,1,0,320)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (130,'State',1,'NULL',1,16,1,0,321)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (140,'Zip',1,'NULL',1,16,1,0,322)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (150,'Cell Phone',1,'NULL',0,16,1,0,323)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (160,'Home Phone',1,'NULL',0,16,1,0,324)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (40,'Email',1,'NULL',0,16,1,0,325)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (170,'School',1,'NULL',1,16,1,0,326)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (180,'Grade',1,'NULL',1,16,1,0,327)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (190,'How would you define your spiritual life?',8,'Searching for answers.
I have received Jesus.
Focusing on obedience- I''ve quit negotiating with God.
I''m actively replicating my faith in others.',1,16,1,0,330)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (240,'Please Explain where you are',2,'NULL',1,16,1,0,331)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (250,'Why do you want to serve?',2,'NULL',1,16,1,0,332)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (260,'Any special talents, abilities, skills or experien',2,'NULL',1,16,1,0,335)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (290,'Serve Site and Service Time',1,'NULL',1,16,1,0,340)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (300,'What age kids do you prefer to serve with?',8,'NULL',1,16,1,0,344)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (340,'Reference 1: Name',1,'NULL',1,16,1,0,346)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (350,'Reference 1: Length of Time Known',1,'NULL',1,16,1,0,347)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (360,'Reference 1: Home Phone',1,'NULL',0,16,1,0,349)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (370,'Reference 1: Mobile Phone',1,'NULL',0,16,1,0,350)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (380,'Reference 1: Work Phone',1,'NULL',0,16,1,0,351)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (390,'Reference 1: Email ',1,'NULL',0,16,1,0,352)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (400,'Reference 1: Nature of Association',1,'NULL',1,16,1,0,353)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (410,'Reference 1: Occupation 1',1,'NULL',1,16,1,0,354)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (420,'Reference 2: Name',1,'NULL',1,16,1,0,355)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (430,'Reference 2: Length of Time Known',1,'NULL',1,16,1,0,356)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (440,'Reference 2: Home',1,'NULL',0,16,1,0,358)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (450,'Reference 2:  Mobile',1,'NULL',0,16,1,0,359)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (460,'Reference 2: Work',1,'NULL',0,16,1,0,360)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (470,'Reference 2: Email',1,'NULL',1,16,1,0,361)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (480,'Reference 2: Nature of Association',1,'NULL',1,16,1,0,362)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (490,'Reference 2: Occupation',1,'NULL',1,16,1,0,363)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (500,'Parent/Guaridan Last Name',1,'NULL',1,16,1,0,366)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (510,'Parent/Guaridan First Name',1,'NULL',1,16,1,0,367)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (520,'Parent/Guaridan Home Phone',1,'NULL',1,16,1,0,368)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (530,'Parent/Guaridan Cell Phone',1,'NULL',1,16,1,0,369)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (540,'Parent/Guaridan Email',1,'NULL',1,16,1,0,370)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (550,'Parent/Guaridan Consent',7,'Yes
No',1,16,1,0,371)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (560,'Parent/Guaridan Date',3,'NULL',1,16,1,0,372)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (570,'Student Agreement to Commitment',7,'Yes
No',1,16,1,0,373)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (580,'Student Date',3,'NULL',1,16,1,0,374)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (20,'Last Name',1,'NULL',1,17,1,0,375)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (10,'First Name',1,'NULL',1,17,1,0,376)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (30,'Middle Initial',1,'NULL',1,17,1,0,377)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (120,'When did you start attending Crossroads?',3,'NULL',1,17,1,0,378)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (130,'Site/Service Time Where you Attend ',5,'Oakley - Sat - 4:30pm
Oakley - Sat - 6:15pm
Oakley - Sun - 8:30am
Oakley - Sun - 10:05am
Oakley - Sun - 11:55am
Mason - Sun - 8:15am
Mason - Sun - 9:45am
Mason - Sun - 11:30am
Florence - Sat - 5:30pm
Florence - Sun - 9:30am
Florence - Sun - 11:30am
West Side - Sun - 8:15am
West Side - Sun - 10:00am
West Side - Sun 11:45am
Uptown - Sun - 7:00pm',1,17,1,0,379)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (80,'Gender',7,'Male
Female',1,17,1,0,380)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (70,'Date of Birth',3,'NULL',1,17,1,0,382)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (40,'Previous Name / Maiden Name',1,'NULL',1,17,1,0,383)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (60,'Name for Name Tag',1,'NULL',1,17,1,0,384)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (140,'Street Address',1,'NULL',1,17,1,0,385)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (150,'City',1,'NULL',1,17,1,0,386)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (160,'State',1,'NULL',1,17,1,0,387)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (170,'Zip',1,'NULL',1,17,1,0,388)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (180,'Cell Phone',1,'NULL',0,17,1,0,389)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (190,'Home Phone',1,'NULL',0,17,1,0,390)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (50,'Email',1,'NULL',1,17,1,0,391)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (200,'Company Name',1,'NULL',1,17,1,0,393)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (210,'Position',1,'NULL',1,17,1,0,394)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (220,'Work Phone (If you can receive calls at this #)',1,'NULL',0,17,1,0,395)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (90,'Marital Status',7,'Single
Divorced
Separated
Widowed
Married',1,17,1,0,396)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (100,'Spouse''s Name',1,'NULL',0,17,1,0,397)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (110,'Spouse''s Gender',7,'Male
Female',0,17,1,0,398)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (230,'Child 1: Name',1,'NULL',0,17,1,0,400)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (240,'Child 1: Birthday',3,'NULL',0,17,1,0,401)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (250,'Child 2: Name',1,'NULL',0,17,1,0,402)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (260,'Child 2: Birthday',3,'NULL',0,17,1,0,403)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (270,'Child 3: Name',1,'NULL',0,17,1,0,404)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (280,'Child 3: Birthday',3,'NULL',0,17,1,0,405)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (290,'Child 4: Name',1,'NULL',0,17,1,0,406)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (300,'Child 4: Birthday',3,'NULL',0,17,1,0,407)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (310,'Have you ever been arrested, convicted of or plead',7,'Yes
No',1,17,1,0,409)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (320,'Have you ever been concerned that you may have an ',7,'Yes
No',1,17,1,0,411)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (340,'Have you ever been treated for or diagnosed with a',7,'Yes
No',1,17,1,0,415)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (350,'Currently, or in the past year, have you been sexu',7,'Yes
No',1,17,1,0,417)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (360,'How would you define your spiritual life?',8,'Searching for answers.
I have received Jesus.
Focusing on obedience-I''ve quit negotiating with God.
I''m actively replicating my faith in others.',1,17,1,0,421)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (370,'Please Explain',2,'NULL',1,17,1,0,422)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (380,'What prompted you to fill out an application and c',2,'NULL',1,17,1,0,424)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (390,'Any special talents, abilities, skills or experien',2,'NULL',1,17,1,0,426)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (400,'Serve During the Week',8,'NULL',0,17,1,0,428)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (410,'Serve During Weekend Services',8,'NULL',0,17,1,0,429)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (470,'Serve Site and Service Time',1,'NULL',0,17,1,0,432)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (590,'Reference 1:  Name',1,'NULL',1,17,1,0,440)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (600,'Reference 1: Length of Time Known',1,'NULL',1,17,1,0,441)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (610,'Reference 1: Home Phone',1,'NULL',0,17,1,0,442)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (620,'Reference 1: Mobile Phone',1,'NULL',0,17,1,0,443)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (630,'Reference 1: Work Phpne',1,'NULL',0,17,1,0,444)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (640,'Reference 1: Email',1,'NULL',1,17,1,0,445)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (650,'Reference 1: Nature of Association',1,'NULL',1,17,1,0,446)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (660,'Reference 1: Occupation',1,'NULL',1,17,1,0,447)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (670,'Reference 2: Name',1,'NULL',1,17,1,0,448)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (680,'Reference 2: Length of Time Known',1,'NULL',1,17,1,0,449)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (690,'Reference 2: Home Phone',1,'NULL',0,17,1,0,450)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (700,'Reference 2: Mobile Phone',1,'NULL',0,17,1,0,451)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (710,'Reference 2: Work Phone',1,'NULL',0,17,1,0,452)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (720,'Reference 2: Email',1,'NULL',1,17,1,0,453)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (730,'Reference 2: Nature of Association',1,'NULL',1,17,1,0,454)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (740,'Reference 2: Occupation',1,'NULL',1,17,1,0,455)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (750,'Reference 3: Reference Name',1,'NULL',1,17,1,0,456)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (760,'Reference 3: Length of time Known',1,'NULL',1,17,1,0,457)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (770,'Reference 3: Home Phone',1,'NULL',0,17,1,0,458)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (780,'Reference 3: Mobile Phone',1,'NULL',0,17,1,0,459)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (790,'Reference 3: Work Phone',1,'NULL',0,17,1,0,460)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (800,'Reference 3: Email',1,'NULL',1,17,1,0,461)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (810,'Reference 3: Nature of Association',1,'NULL',1,17,1,0,462)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (820,'Reference 3: Occupation',1,'NULL',1,17,1,0,463)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (830,'Applicant Agreement',7,'Yes
No',1,17,1,0,465)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (100,'Site/Service Time where you attend',1,'NULL',1,16,1,0,474)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (310,'1 to 2 Year Olds',8,'NULL',0,16,1,0,481)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (320,'3 Year Olds - Pre K',8,'NULL',0,16,1,0,482)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (330,'Kinder - 5th Grade',8,'NULL',0,16,1,0,483)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (270,'During the Week',8,'NULL',0,16,1,0,484)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (280,'Weekend Services',8,'NULL',0,16,1,0,485)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (480,'Serving in a Classroom',8,'NULL',0,17,1,0,491)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (490,'Welcoming New Families and showing them to their r',8,'NULL',0,17,1,0,492)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (500,'One-on-one helper with a kid who has special needs',8,'NULL',0,17,1,0,493)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (510,'Tech (Provide tech and systems support)',8,'NULL',0,17,1,0,494)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (520,'Prepping the rooms',8,'NULL',0,17,1,0,495)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (530,'Administrative tasks',8,'NULL',0,17,1,0,496)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (540,'Shopping for supplies',8,'NULL',0,17,1,0,497)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (550,'Creating the weekend experience',8,'NULL',0,17,1,0,498)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (560,'Birth to Two',8,'NULL',0,17,1,0,499)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (570,'Three to Pre K',8,'NULL',0,17,1,0,500)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (580,'K to 5th Grade',8,'NULL',0,17,1,0,501)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (840,'Agree Date',1,'NULL',0,17,1,0,502)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (330,'Neglecting Child',8,'NULL',0,17,1,0,503)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (361,'Searching For Answers',8,'NULL',0,17,1,0,505)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (362,'I have received Jesus',8,'NULL',0,17,1,0,506)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (363,'Focusing on obedience',8,'NULL',0,17,1,0,508)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (364,'I''m Actively replicating my faith',8,'NULL',0,17,1,0,509)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (200,'Searching For Answers',8,'NULL',0,16,1,0,510)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (210,'I have received Jesus',8,'NULL',0,16,1,0,511)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (220,'Focusing on obedience',8,'NULL',0,16,1,0,512)
INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES (230,'I''m actively replicating my faith',8,'NULL',0,16,1,0,513)

GO
