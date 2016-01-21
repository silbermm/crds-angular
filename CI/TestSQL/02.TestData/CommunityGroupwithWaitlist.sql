USE [MinistryPlatform]
GO

DECLARE @groupId AS INT;
DECLARE @parentGroupId AS INT;

SET IDENTITY_INSERT [dbo].[Groups] ON;

SET @groupId = (SELECT MAX(Group_ID) FROM Groups) + 1 ;

INSERT INTO Groups
(Group_ID, Group_Name             , Group_Type_ID, Ministry_ID, Congregation_ID, Primary_Contact, Description, Start_Date      , End_Date, Target_Size, Parent_Group, Priority_ID, Enable_Waiting_List, Small_Group_Information, Offsite_Meeting_Address, Group_Is_Full, Available_Online, Life_Stage_ID, Group_Focus_ID, Meeting_Time, Meeting_Day_ID, Descended_From, Reason_Ended, Domain_ID, Check_in_Information, [Secure_Check-in], Suppress_Care_Note, On_Classroom_Manager, Promotion_Information, Promote_to_Group, Age_in_Months_to_Promote , Promote_Weekly , __ExternalGroupID , __ExternalParentGroupID , __IsPublic , __ISBlogEnabled , __ISWebEnabled , Group_Notes , Sign_Up_To_Serve , Deadline_Passed_Message_ID , Notifications , Send_Attendance_Notification , Send_Service_Notification , Child_Care_Available , Meeting_Frequency_ID , Meeting_Duration_ID , Required_Book ) VALUES
(@groupId, '(t) Fathers Oakley CG', 8            , 8          ,  1             , 2562428        , null       , {d '2015-11-01'}, null    , 4          , null        , null       , 1                  , null                   , null                   , 0            , 1               , null         , null          , null        , null          , null          , null        , 1        , null                , null             , null              , null                , null                 , null            , null                     , null           , null              , null                    , null       , null            , null           , null        , null             , 58                         , null          , 0                            , 0                         , 0                    , null                 , null                , null) ;

SET @groupId = (SELECT MAX(Group_ID) FROM Groups) + 1 ;
SET @parentGroupId = (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) Fathers Oakley CG');

INSERT INTO Groups
(Group_ID, Group_Name                         , Group_Type_ID, Ministry_ID, Congregation_ID, Primary_Contact, Description, Start_Date      , End_Date, Target_Size, Parent_Group   , Priority_ID, Enable_Waiting_List, Small_Group_Information, Offsite_Meeting_Address, Group_Is_Full, Available_Online, Life_Stage_ID, Group_Focus_ID, Meeting_Time, Meeting_Day_ID, Descended_From, Reason_Ended, Domain_ID, Check_in_Information, [Secure_Check-in], Suppress_Care_Note, On_Classroom_Manager, Promotion_Information, Promote_to_Group, Age_in_Months_to_Promote , Promote_Weekly , __ExternalGroupID , __ExternalParentGroupID , __IsPublic , __ISBlogEnabled , __ISWebEnabled , Group_Notes , Sign_Up_To_Serve , Deadline_Passed_Message_ID , Notifications , Send_Attendance_Notification , Send_Service_Notification , Child_Care_Available , Meeting_Frequency_ID , Meeting_Duration_ID , Required_Book ) VALUES
(@groupId, '(t) Fathers Oakley CG - Waitlist ', 20           , 8          ,  1             , 2562428        , null       , {d '2015-11-01'}, null    , 10         , @parentGroupId , null       , null               , null                   , null                   , 0            , 1               , null         , null          , null        , null          , null          , null        , 1        , null                , null             , null              , null                , null                 , null            , null                     , null           , null              , null                    , null       , null            , null           , null        , null             , 58                         , null          , 0                            , 0                         , 0                    , null                 , null                , null) ;

SET IDENTITY_INSERT [dbo].[Groups] OFF;
