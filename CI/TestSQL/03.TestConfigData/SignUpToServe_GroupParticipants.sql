Use [MinistryPlatform]
GO

INSERT INTO [dbo].Group_Participants
(Group_ID                                                                                 ,Participant_ID                                                                                              ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups with (NOLOCK) where group_name = '(t) SU2S KC Mason Group') ,(select Participant_record from contacts with (NOLOCK) where email_address = 'mpcrds+SU2SFather@gmail.com') ,22           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants
(Group_ID                                                                                 ,Participant_ID                                                                                              ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups with (NOLOCK) where group_name = '(t) SU2S KC Mason Group') ,(select Participant_record from contacts with (NOLOCK) where email_address = 'mpcrds+SU2SMother@gmail.com') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants
(Group_ID                                                                                 ,Participant_ID                                                                                              ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups with (NOLOCK) where group_name = '(t) SU2S KC Mason Group') ,(select Participant_record from contacts with (NOLOCK) where email_address = 'mpcrds+SU2SChild18@gmail.com'),16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants
(Group_ID                                                                                 ,Participant_ID                                                                                              ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups with (NOLOCK) where group_name = '(t) SU2S KC Mason Group') ,(select Participant_record from contacts with (NOLOCK) where email_address = 'mpcrds+SU2SChild4@gmail.com') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants
(Group_ID                                                                                 ,Participant_ID                                                                                                   ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups with (NOLOCK) where group_name = '(t) SU2S KC Mason Group') ,(select Participant_record from contacts with (NOLOCK) where email_address = 'mpcrds+SU2SFosterChild@gmail.com') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants
(Group_ID                                                                                 ,Participant_ID                                                                                                 ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id FROM groups with (NOLOCK) where group_name = '(t) SU2S KC Mason Group') ,(select Participant_record from contacts with (NOLOCK) where email_address = 'mpcrds+SU2SLegalWard@gmail.com') ,16           ,1        ,{ts '2015-05-01 00:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );