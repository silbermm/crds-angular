USE [MinistryPlatform]
GO

--Sign Up to Serve Data set up
DECLARE @groupId AS INT
DECLARE @eventTypeId1 AS INT
DECLARE @eventTypeId2 AS INT
DECLARE @opportunityId1 AS INT
DECLARE @opportunityId2 AS INT
DECLARE @opportunityId3 AS INT
DECLARE @opportunityId4 AS INT
DECLARE @eventId AS INT
DECLARE @eventDate AS DATE
DECLARE @eventStartDate1 AS DATETIME
DECLARE @eventEndDate1 AS DATETIME
DECLARE @eventStartDate2 AS DATETIME
DECLARE @eventEndDate2 AS DATETIME


--Create a new Sign Up to Serve Group
SET IDENTITY_INSERT [dbo].[Groups] ON;

SET @groupId = (SELECT MAX(Group_ID) FROM Groups) + 1 ;

INSERT INTO Groups
(Group_ID, Group_Name               , Group_Type_ID, Ministry_ID, Congregation_ID, Primary_Contact, Description, Start_Date      , End_Date, Target_Size, Parent_Group, Priority_ID, Enable_Waiting_List, Small_Group_Information, Offsite_Meeting_Address, Group_Is_Full, Available_Online, Life_Stage_ID, Group_Focus_ID, Meeting_Time, Meeting_Day_ID, Descended_From, Reason_Ended, Domain_ID, Check_in_Information, [Secure_Check-in], Suppress_Care_Note, On_Classroom_Manager, Promotion_Information, Promote_to_Group, Age_in_Months_to_Promote , Promote_Weekly , __ExternalGroupID , __ExternalParentGroupID , __IsPublic , __ISBlogEnabled , __ISWebEnabled , Group_Notes , Sign_Up_To_Serve , Deadline_Passed_Message_ID , Notifications , Send_Attendance_Notification , Send_Service_Notification , Child_Care_Available , Meeting_Frequency_ID , Meeting_Duration_ID , Required_Book ) VALUES
(@groupId, '(t) SU2S KC Mason Group', 9            , 2          ,  6             , 2562428        , null       , {d '2015-11-01'}, null    , null       , null        , null       , null               , null                   , null                   , 0            , 1               , null         , null          , null        , null          , null          , null        , 1        , null                , null             , null              , null                , null                 , null            , null                     , null           , null              , null                    , null       , null            , null           , null        , null             , 58                         , null          , 0                            , 0                         , 0                    , null                 , null                , null) ;

SET IDENTITY_INSERT [dbo].[Groups] OFF;

--Create new Sign Up to Serve Evet Types
SET IDENTITY_INSERT [dbo].[Event_Types] ON;

SET @eventTypeId1 = (SELECT MAX(Event_Type_ID) FROM Event_Types) + 1 ;

INSERT INTO Event_Types
(Event_Type_ID, Event_Type                              , Description, Domain_ID , __ExternalEventTypeID , __ExternalSiteServiceTimeID ) VALUES
(@eventTypeId1 , '(t) Serve - SU2S KC Mason Weekly 10:00', null       , 1          ,  null                , null) ;

SET @eventTypeId2 = (SELECT MAX(Event_Type_ID) FROM Event_Types) + 1 ;

INSERT INTO Event_Types
(Event_Type_ID, Event_Type                              , Description, Domain_ID  , __ExternalEventTypeID , __ExternalSiteServiceTimeID ) VALUES
(@eventTypeId2 , '(t) Serve - SU2S KC Mason Weekly 3:00' , null       , 1          ,  null                 , null) ;

SET IDENTITY_INSERT [dbo].[Event_Types] OFF;

--Create new Sign Up to Serve Opportunities

SET IDENTITY_INSERT [dbo].[Opportunities] ON;

SET @opportunityId1 = (SELECT MAX(Opportunity_ID) FROM Opportunities) + 1 ;

INSERT INTO Opportunities
(Opportunity_ID, Opportunity_Title ,Description, Group_Role_ID, Program_ID, Visibility_Level_ID, Contact_Person, Publish_Date    , Opportunity_Date, Duration_in_Hours, Add_to_Group, Add_to_Event, Required_Gender, Minimum_Age, Minimum_Needed, Maximum_Needed, Domain_ID, Letter_Body, Letter_From, On_Connection_Card, Shift_Start       , Shift_End         , Custom_Form, Event_Type_ID, Sign_Up_Deadline_ID, Room, __ExternalCalendarEventServingTimeID, __ExternalRecurrenceSubsetCode) VALUES
(@opportunityId1, '(t)SU2S KC Mason',null       , 22           , 109       , 4                  , 2562428       , {d '2015-11-01'}, null            , null             , @groupId   , null        , null           , null       , 1             , 1             , 1        , null       , null       , null              , '09:45:00.1234567', '11:15:00.1234567', null       , @eventTypeId1, 3                  , null, null                                , null     );                             

SET @opportunityId2 = (SELECT MAX(Opportunity_ID) FROM Opportunities) + 1 ;

INSERT INTO Opportunities
(Opportunity_ID, Opportunity_Title ,Description, Group_Role_ID, Program_ID, Visibility_Level_ID, Contact_Person, Publish_Date    , Opportunity_Date, Duration_in_Hours, Add_to_Group, Add_to_Event, Required_Gender, Minimum_Age, Minimum_Needed, Maximum_Needed, Domain_ID, Letter_Body, Letter_From, On_Connection_Card, Shift_Start       , Shift_End         , Custom_Form, Event_Type_ID, Sign_Up_Deadline_ID, Room, __ExternalCalendarEventServingTimeID, __ExternalRecurrenceSubsetCode) VALUES
(@opportunityId2, '(t)SU2S KC Mason',null       , 16           , 109       , 4                  , 2562428       , {d '2015-11-01'}, null            , null             , @groupId   , null        , null           , null       , 5             , 10             , 1        , null       , null       , null              , '09:45:00.1234567', '11:15:00.1234567', null       , @eventTypeId1, 3                  , null, null                                , null     );                             

SET @opportunityId3 = (SELECT MAX(Opportunity_ID) FROM Opportunities) + 1 ;

INSERT INTO Opportunities
(Opportunity_ID , Opportunity_Title ,Description, Group_Role_ID, Program_ID, Visibility_Level_ID, Contact_Person, Publish_Date    , Opportunity_Date, Duration_in_Hours, Add_to_Group, Add_to_Event, Required_Gender, Minimum_Age, Minimum_Needed, Maximum_Needed, Domain_ID, Letter_Body, Letter_From, On_Connection_Card, Shift_Start       , Shift_End         , Custom_Form, Event_Type_ID, Sign_Up_Deadline_ID, Room, __ExternalCalendarEventServingTimeID, __ExternalRecurrenceSubsetCode) VALUES
(@opportunityId3, '(t)SU2S KC Mason',null       , 22           , 109       , 4                  , 2562428       , {d '2015-11-01'}, null            , null             , @groupId    , null        , null           , null       , 1            , 1             , 1        , null       , null       , null              , '14:45:00.1234567', '16:15:00.1234567', null       , @eventTypeId2, 3                  , null, null                                , null     );                             

SET @opportunityId4 = (SELECT MAX(Opportunity_ID) FROM Opportunities) + 1 ;

INSERT INTO Opportunities
(Opportunity_ID , Opportunity_Title ,Description, Group_Role_ID, Program_ID, Visibility_Level_ID, Contact_Person, Publish_Date    , Opportunity_Date, Duration_in_Hours, Add_to_Group, Add_to_Event, Required_Gender, Minimum_Age, Minimum_Needed, Maximum_Needed, Domain_ID, Letter_Body, Letter_From, On_Connection_Card, Shift_Start       , Shift_End         , Custom_Form, Event_Type_ID, Sign_Up_Deadline_ID, Room, __ExternalCalendarEventServingTimeID, __ExternalRecurrenceSubsetCode) VALUES
(@opportunityId4, '(t)SU2S KC Mason',null       , 16           , 109       , 4                  , 2562428       , {d '2015-11-01'}, null            , null             , @groupId    , null        , null           , null       , 5             , 10            , 1        , null       , null       , null              , '14:45:00.1234567', '16:15:00.1234567', null       , @eventTypeId2, 3                  , null, null                                , null     );                             

SET IDENTITY_INSERT [dbo].[Opportunities] OFF;

--Create new Sign Up to Serve Events

SET IDENTITY_INSERT [dbo].[Events] ON;

SET @eventDate = DATEADD(DAY, 7,GETDATE());
SET @eventId = (SELECT MAX(Event_ID) FROM Events) + 1 ;
SET @eventStartDate1 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('10:00' AS TIME)) ;
SET @eventEndDate1 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('11:00' AS TIME));

INSERT INTO Events
(Event_ID , Event_Title                  , Event_Type_ID, Congregation_ID, Location_ID, Meeting_Instructions, Description, Program_ID, Primary_Contact, Participants_Expected, Minutes_for_Setup, Event_Start_Date           , Event_End_Date, Minutes_for_Cleanup, Cancelled, _Approved, Public_Website_Settings, Visibility_Level_ID, Featured_On_Calendar, Online_Registration_Product, Registration_Form, Registration_Start, Registration_End, Registration_Active, _Web_Approved, [Check-in_Information], [Allow_Check-in], Ignore_Program_Groups, Prohibit_Guests, [Early_Check-in_Period], [Late_Check-in_Period], Other_Event_Information, Parent_Event_ID, Priority_ID, Domain_ID, On_Connection_Card, External_Registration_URL, On_Donation_Batch_Tool, __ExternalEventID, __ExternalOrganizerUserID, __ExternalGroupID, __ExternalRoomID, __ExternalContactUserID, Project_Code, Participant_Reminder_Settings, Send_Reminder, Reminder_Sent, Reminder_Days_Prior_ID, __ExternalTripID, __ExternalTripLegID ) VALUES
( @eventId, '(t) SU2S Mason Weekly 10:00', @eventTypeId1, 6              , 5          , null                , null       , 109       , 2562428        , null                 , 0                , @eventStartDate1           , @eventEndDate1, 0                  , 0        , 1        , null                   , 4                  , 0                   , null                       , null             , null              , null            , null               , 1            , null                  , 0               , 0                    , 0              , null                   , null                  , null                   , null           , null       , 1        , null              , null                     , 0                     , null             , null                     , null             , null            , null                   , null        , null                         , 1            , null         , 2                     , null            , null );   

INSERT INTO Event_Groups
(Event_ID, Group_ID, Room_ID, Domain_ID, [__Secure_Check-in], Closed) VALUES
(@eventId, @groupId, null   , 1        , null               , null  )


SET @eventId = (SELECT MAX(Event_ID) FROM Events) + 1 ;
SET @eventStartDate2 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('15:00' AS TIME)) ;
SET @eventEndDate2 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('16:00' AS TIME)) ;

INSERT INTO Events
(Event_ID, Event_Title                  , Event_Type_ID, Congregation_ID, Location_ID, Meeting_Instructions, Description, Program_ID, Primary_Contact, Participants_Expected, Minutes_for_Setup, Event_Start_Date           , Event_End_Date, Minutes_for_Cleanup, Cancelled, _Approved, Public_Website_Settings, Visibility_Level_ID, Featured_On_Calendar, Online_Registration_Product, Registration_Form, Registration_Start, Registration_End, Registration_Active, _Web_Approved, [Check-in_Information], [Allow_Check-in], Ignore_Program_Groups, Prohibit_Guests,[Early_Check-in_Period], [Late_Check-in_Period] , Other_Event_Information, Parent_Event_ID, Priority_ID, Domain_ID, On_Connection_Card, External_Registration_URL, On_Donation_Batch_Tool, __ExternalEventID, __ExternalOrganizerUserID, __ExternalGroupID, __ExternalRoomID, __ExternalContactUserID, Project_Code, Participant_Reminder_Settings, Send_Reminder, Reminder_Sent, Reminder_Days_Prior_ID, __ExternalTripID, __ExternalTripLegID ) VALUES
( @eventId, '(t) SU2S Mason Weekly 3:00' , @eventTypeId2, 6              , 5          , null                , null       , 109       , 2562428        , null                 , 0                , @eventStartDate2           , @eventEndDate2, 0                  , 0        , 1        , null                   , 4                  , 0                   , null                       , null             , null              , null            , null               , 1            , null                  , 0               , 0                    , 0              , null                 , null                   , null                   , null           , null       , 1        , null              , null                     , 0                     , null             , null                     , null             , null            , null                   , null        , null                         , 1            , null         , 2                     , null            , null );   
 
INSERT INTO Event_Groups
(Event_ID, Group_ID, Room_ID, Domain_ID, [__Secure_Check-in], Closed) VALUES
(@eventId, @groupId, null   , 1        , null               , null  );

SET @eventDate = DATEADD(DAY, 7, @eventDate);
SET @eventId = (SELECT MAX(Event_ID) FROM Events) + 1 ;
SET @eventStartDate1 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('10:00' AS TIME)) ;
SET @eventEndDate1 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('11:00' AS TIME));

INSERT INTO Events
(Event_ID, Event_Title                  , Event_Type_ID, Congregation_ID, Location_ID, Meeting_Instructions, Description, Program_ID, Primary_Contact, Participants_Expected, Minutes_for_Setup, Event_Start_Date           , Event_End_Date, Minutes_for_Cleanup, Cancelled, _Approved, Public_Website_Settings, Visibility_Level_ID, Featured_On_Calendar, Online_Registration_Product, Registration_Form, Registration_Start, Registration_End, Registration_Active, _Web_Approved, [Check-in_Information], [Allow_Check-in], Ignore_Program_Groups, Prohibit_Guests, [Early_Check-in_Period], [Late_Check-in_Period], Other_Event_Information, Parent_Event_ID, Priority_ID, Domain_ID, On_Connection_Card, External_Registration_URL, On_Donation_Batch_Tool, __ExternalEventID, __ExternalOrganizerUserID, __ExternalGroupID, __ExternalRoomID, __ExternalContactUserID, Project_Code, Participant_Reminder_Settings, Send_Reminder, Reminder_Sent, Reminder_Days_Prior_ID, __ExternalTripID, __ExternalTripLegID ) VALUES
(@eventId, '(t) SU2S Mason Weekly 10:00', @eventTypeId1, 6              , 5          , null                , null       , 109       , 2562428        , null                 , 0                , @eventStartDate1           , @eventEndDate1, 0                  , 0        , 1        , null                   , 4                  , 2                   , null                       , null             , null              , null            , null               , 1            , null                  , 0               , 0                    , 0              , null                   , null                  , null                   , null           , null       , 1        , null              , null                     , 0                     , null             , null                     , null             , null            , null                   , null        , null                         , 1            , null         , 2                     , null            , null );   

INSERT INTO Event_Groups
(Event_ID, Group_ID, Room_ID, Domain_ID, [__Secure_Check-in], Closed) VALUES
(@eventId, @groupId, null   , 1        , null               , null  )

SET @eventId = (SELECT MAX(Event_ID) FROM Events) + 1 ;
SET @eventStartDate2 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('15:00' AS TIME)) ;
SET @eventEndDate2 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('16:00' AS TIME)) ;

INSERT INTO Events
(Event_ID, Event_Title                  , Event_Type_ID, Congregation_ID, Location_ID, Meeting_Instructions, Description, Program_ID, Primary_Contact, Participants_Expected, Minutes_for_Setup, Event_Start_Date           , Event_End_Date, Minutes_for_Cleanup, Cancelled, _Approved, Public_Website_Settings, Visibility_Level_ID, Featured_On_Calendar, Online_Registration_Product, Registration_Form, Registration_Start, Registration_End, Registration_Active, _Web_Approved, [Check-in_Information], [Allow_Check-in], Ignore_Program_Groups, Prohibit_Guests, [Early_Check-in_Period], [Late_Check-in_Period], Other_Event_Information, Parent_Event_ID, Priority_ID, Domain_ID, On_Connection_Card, External_Registration_URL, On_Donation_Batch_Tool, __ExternalEventID, __ExternalOrganizerUserID, __ExternalGroupID, __ExternalRoomID, __ExternalContactUserID, Project_Code, Participant_Reminder_Settings, Send_Reminder, Reminder_Sent, Reminder_Days_Prior_ID, __ExternalTripID, __ExternalTripLegID ) VALUES
(@eventId, '(t) SU2S Mason Weekly 3:00' , @eventTypeId2, 6              , 5          , null                , null       , 109       , 2562428        , null                 , 0                , @eventStartDate2           , @eventEndDate2, 0                  , 0        , 1        , null                   , 4                  , 0                   , null                       , null             , null              , null            , null               , 1            , null                  , 0               , 0                    , 0              , null                   , null                  , null                   , null           , null       , 1        , null              , null                     , 0                     , null             , null                     , null             , null            , null                   , null        , null                         , 1            , null         , 2                     , null            , null );   

INSERT INTO Event_Groups
(Event_ID, Group_ID, Room_ID, Domain_ID, [__Secure_Check-in], Closed) VALUES
(@eventId, @groupId, null   , 1        , null               , null  );

SET @eventDate = DATEADD(DAY, 7, @eventDate);
SET @eventId = (SELECT MAX(Event_ID) FROM Events) + 1 ;
SET @eventStartDate1 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('10:00' AS TIME)) ;
SET @eventEndDate1 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('11:00' AS TIME));

INSERT INTO Events
(Event_ID, Event_Title                  , Event_Type_ID, Congregation_ID, Location_ID, Meeting_Instructions, Description, Program_ID, Primary_Contact, Participants_Expected, Minutes_for_Setup, Event_Start_Date           , Event_End_Date, Minutes_for_Cleanup, Cancelled, _Approved, Public_Website_Settings, Visibility_Level_ID, Featured_On_Calendar, Online_Registration_Product, Registration_Form, Registration_Start, Registration_End, Registration_Active, _Web_Approved, [Check-in_Information], [Allow_Check-in], Ignore_Program_Groups, Prohibit_Guests, [Early_Check-in_Period], [Late_Check-in_Period], Other_Event_Information, Parent_Event_ID, Priority_ID, Domain_ID, On_Connection_Card, External_Registration_URL, On_Donation_Batch_Tool, __ExternalEventID, __ExternalOrganizerUserID, __ExternalGroupID, __ExternalRoomID, __ExternalContactUserID, Project_Code, Participant_Reminder_Settings, Send_Reminder, Reminder_Sent, Reminder_Days_Prior_ID, __ExternalTripID, __ExternalTripLegID ) VALUES
(@eventId, '(t) SU2S Mason Weekly 10:00', @eventTypeId1, 6              , 5          , null                , null       , 109       , 2562428        , null                 , 0                , @eventStartDate1           , @eventEndDate1, 0                  , 0        , 1        , null                   , 4                  , 0                   , null                       , null             , null              , null            , null               , 1            , null                  , 0               , 0                    , 0              , null                   , null                   , null                   , null           , null       , 1        , null             , null                     , 0                     , null             , null                     , null             , null            , null                   , null        , null                         , 1            , null         , 2                     , null            , null );   

INSERT INTO Event_Groups
(Event_ID, Group_ID, Room_ID, Domain_ID, [__Secure_Check-in], Closed) VALUES
(@eventId, @groupId, null   , 1        , null               , null  )

SET @eventId = (SELECT MAX(Event_ID) FROM Events) + 1 ;
SET @eventStartDate2 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('15:00' AS TIME)) ;
SET @eventEndDate2 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('16:00' AS TIME)) ;

INSERT INTO Events
(Event_ID, Event_Title                  , Event_Type_ID, Congregation_ID, Location_ID, Meeting_Instructions, Description, Program_ID, Primary_Contact, Participants_Expected, Minutes_for_Setup, Event_Start_Date           , Event_End_Date, Minutes_for_Cleanup, Cancelled, _Approved, Public_Website_Settings, Visibility_Level_ID, Featured_On_Calendar, Online_Registration_Product, Registration_Form, Registration_Start, Registration_End, Registration_Active, _Web_Approved, [Check-in_Information], [Allow_Check-in], Ignore_Program_Groups, Prohibit_Guests, [Early_Check-in_Period], [Late_Check-in_Period], Other_Event_Information, Parent_Event_ID, Priority_ID, Domain_ID, On_Connection_Card, External_Registration_URL, On_Donation_Batch_Tool, __ExternalEventID, __ExternalOrganizerUserID, __ExternalGroupID, __ExternalRoomID, __ExternalContactUserID, Project_Code, Participant_Reminder_Settings, Send_Reminder, Reminder_Sent, Reminder_Days_Prior_ID, __ExternalTripID, __ExternalTripLegID ) VALUES
(@eventId, '(t) SU2S Mason Weekly 3:00' , @eventTypeId2, 6              , 5          , null                , null       , 109       , 2562428        , null                 , 0                , @eventStartDate2           , @eventEndDate2, 0                  , 0        , 1        , null                   , 4                  , 0                   , null                       , null             , null              , null            , null               , 1            , null                  , 0                 , 0                    , 0              , null                 , null                  , null                   , null           , null       , 1        , null              , null                     , 0                     , null             , null                     , null             , null            , null                   , null        , null                         , 1            , null         , 2                     , null            , null );   

INSERT INTO Event_Groups
(Event_ID, Group_ID, Room_ID, Domain_ID, [__Secure_Check-in], Closed) VALUES
(@eventId, @groupId, null   , 1        , null               , null  );

SET @eventDate = DATEADD(DAY, 7, @eventDate);
SET @eventId = (SELECT MAX(Event_ID) FROM Events) + 1 ;
SET @eventStartDate1 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('10:00' AS TIME)) ;
SET @eventEndDate1 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('11:00' AS TIME));

INSERT INTO Events
(Event_ID, Event_Title                  , Event_Type_ID, Congregation_ID, Location_ID, Meeting_Instructions, Description, Program_ID, Primary_Contact, Participants_Expected, Minutes_for_Setup, Event_Start_Date           , Event_End_Date, Minutes_for_Cleanup, Cancelled, _Approved, Public_Website_Settings, Visibility_Level_ID, Featured_On_Calendar, Online_Registration_Product, Registration_Form, Registration_Start, Registration_End, Registration_Active, _Web_Approved, [Check-in_Information], [Allow_Check-in], Ignore_Program_Groups, Prohibit_Guests, [Early_Check-in_Period], [Late_Check-in_Period], Other_Event_Information, Parent_Event_ID, Priority_ID, Domain_ID, On_Connection_Card, External_Registration_URL, On_Donation_Batch_Tool, __ExternalEventID, __ExternalOrganizerUserID, __ExternalGroupID, __ExternalRoomID, __ExternalContactUserID, Project_Code, Participant_Reminder_Settings, Send_Reminder, Reminder_Sent, Reminder_Days_Prior_ID, __ExternalTripID, __ExternalTripLegID ) VALUES
(@eventId, '(t) SU2S Mason Weekly 10:00', @eventTypeId1, 6              , 5          , null                , null       , 109       , 2562428        , null                 , 0                , @eventStartDate1           , @eventEndDate1, 0                  , 0        , 1        , null                   , 4                  , 0                   , null                       , null             , null              , null            , null               , 1            , null                  , 0               , 0                    , 0              , null                   , null                  , null                   , null           , null       , 1        , null              , null                     , 0                     , null             , null                     , null             , null            , null                   , null        , null                         , 1            , null         , 2                     , null            , null );   

INSERT INTO Event_Groups
(Event_ID, Group_ID, Room_ID, Domain_ID, [__Secure_Check-in], Closed) VALUES
(@eventId, @groupId, null   , 1        , null               , null  );

SET @eventId = (SELECT MAX(Event_ID) FROM Events) + 1 ;
SET @eventStartDate2 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('15:00' AS TIME)) ;
SET @eventEndDate2 = (CAST(CAST(@eventDate AS DATE) AS DATETIME) + CAST('16:00' AS TIME)) ;

INSERT INTO Events
(Event_ID, Event_Title                  , Event_Type_ID, Congregation_ID, Location_ID, Meeting_Instructions, Description, Program_ID, Primary_Contact, Participants_Expected, Minutes_for_Setup, Event_Start_Date           , Event_End_Date, Minutes_for_Cleanup, Cancelled, _Approved, Public_Website_Settings, Visibility_Level_ID, Featured_On_Calendar, Online_Registration_Product, Registration_Form, Registration_Start, Registration_End, Registration_Active, _Web_Approved, [Check-in_Information], [Allow_Check-in], Ignore_Program_Groups, Prohibit_Guests, [Early_Check-in_Period], [Late_Check-in_Period], Other_Event_Information, Parent_Event_ID, Priority_ID, Domain_ID, On_Connection_Card, External_Registration_URL, On_Donation_Batch_Tool, __ExternalEventID, __ExternalOrganizerUserID, __ExternalGroupID, __ExternalRoomID, __ExternalContactUserID, Project_Code, Participant_Reminder_Settings, Send_Reminder, Reminder_Sent, Reminder_Days_Prior_ID, __ExternalTripID, __ExternalTripLegID ) VALUES
(@eventId, '(t) SU2S Mason Weekly 3:00' , @eventTypeId2, 6              , 5          , null                , null       , 109       , 2562428        , null                 , 0                , @eventStartDate2           , @eventEndDate2, 0                  , 0        , 1        , null                   , 4                  , 0                   , null                       , null             , null              , null            , null               , 1            , null                  , 0               , 0                    , 0              , null                   , null                  , null                   , null           , null       , 1        , null              , null                     , 0                     , null             , null                     , null             , null            , null                   , null        , null                         , 1            , null         , 2                     , null            , null );   

INSERT INTO Event_Groups
(Event_ID, Group_ID, Room_ID, Domain_ID, [__Secure_Check-in], Closed) VALUES
(@eventId, @groupId, null   , 1        , null               , null  );

SET IDENTITY_INSERT [dbo].[Events] OFF;