USE [MinistryPlatform]
GO

--Mason Fathers' Community Group - A full group without a wait list
--Remove Wait List from (d) Fathers' Group Mason Community Group and set the size of the group to 4
update [dbo].groups set Parent_Group = null where Group_name = '(d) Fathers'' Group Mason-Waiting List';
update [dbo].groups set Target_Size = 4 where group_name = '(d) Fathers'' Group Mason';
update [dbo].groups set Enable_Waiting_list = 0 where group_name = '(d) Fathers'' Group Mason';
GO

--Reset and Add 4 people to Fathers' Group Mason
DELETE from [dbo].Group_Participants where group_id = (select group_id from [dbo].Groups where group_name = '(d) Fathers'' Group Mason');
INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                           ,Participant_ID                                                                                                     ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id from [dbo].groups where group_name = '(d) Fathers'' Group Mason') ,(select Participant_Record from [dbo].contacts where Email_Address = 'mpcrds+20@gmail.com' and Nickname = 'Neece') ,16           ,1        ,{ts '2015-07-29 12:06:50'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                             ,Participant_ID                                                                                       ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id from [dbo].groups where group_name = '(d) Fathers'' Group Mason')   ,(select Participant_Record from [dbo].contacts where Email_Address = 'mpcrds+peterparker@gmail.com') ,16           ,1        ,{ts '2015-07-29 12:06:50'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                             ,Participant_ID                                                                                     ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id from [dbo].groups where group_name = '(d) Fathers'' Group Mason')   ,(select Participant_Record from [dbo].contacts where Email_Address = 'mpcrds+pamparker@gmail.com') ,16           ,1        ,{ts '2015-07-29 12:06:50'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                             ,Participant_ID                                                                                     ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id from [dbo].groups where group_name = '(d) Fathers'' Group Mason')   ,(select Participant_Record from [dbo].contacts where Email_Address = 'kidsclubmom@gmail.com') ,16           ,1        ,{ts '2015-07-29 12:06:50'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );
GO

--Oakley Fathers Community Group - A full group with a wait list
--Set the size of the group to 4
update [dbo].groups set Target_Size = 4 where group_name = '(d) Fathers'' Group Oakley';

--Add 4 people to the Fathers' at Oakley Group
DELETE from [dbo].Group_Participants where group_id = (select group_id from [dbo].Groups where group_name = '(d) Fathers'' Group Oakley');
INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                            ,Participant_ID                                                                                ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id from [dbo].groups where group_name = '(d) Fathers'' Group Oakley') ,(select Participant_Record from [dbo].contacts where Email_Address = 'silbermm+23@gmail.com') ,16           ,1        ,{ts '2015-07-29 12:06:50'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                             ,Participant_ID                                                                                       ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id from [dbo].groups where group_name = '(d) Fathers'' Group Oakley')  ,(select Participant_Record from [dbo].contacts where Email_Address = 'mpcrds+peterparker@gmail.com') ,16           ,1        ,{ts '2015-07-29 12:06:50'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                             ,Participant_ID                                                                                     ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id from [dbo].groups where group_name = '(d) Fathers'' Group Oakley')  ,(select Participant_Record from [dbo].contacts where Email_Address = 'mpcrds+pamparker@gmail.com') ,16           ,1        ,{ts '2015-07-29 12:06:50'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                             ,Participant_ID                                                                                     ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id from [dbo].groups where group_name = '(d) Fathers'' Group Oakley')  ,(select Participant_Record from [dbo].contacts where Email_Address = 'kidsclubmom@gmail.com') ,16           ,1        ,{ts '2015-07-29 12:06:50'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );
GO

--Florence Fathers Community Group - A group with 1 open spot. A family shouldn't be able to both sign up for this.
--Set the size of the group to 4
update [dbo].groups set Target_Size = 4 where group_name = '(d) Fathers'' Group Florence';

--Add 3 people to the Fathers' at Florence Group
DELETE from [dbo].Group_Participants where group_id = (select group_id from [dbo].Groups where group_name = '(d) Fathers'' Group Florence');
INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                            ,Participant_ID                                                                                  ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id from [dbo].groups where group_name = '(d) Fathers'' Group Florence') ,(select Participant_Record from [dbo].contacts where Email_Address = 'silbermm+23@gmail.com') ,16           ,1        ,{ts '2015-07-29 12:06:50'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                             ,Participant_ID                                                                                       ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id from [dbo].groups where group_name = '(d) Fathers'' Group Florence')  ,(select Participant_Record from [dbo].contacts where Email_Address = 'mpcrds+peterparker@gmail.com') ,16           ,1        ,{ts '2015-07-29 12:06:50'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

INSERT INTO [dbo].Group_Participants 
(Group_ID                                                                             ,Participant_ID                                                                                     ,Group_Role_ID,Domain_ID,Start_Date                ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id from [dbo].groups where group_name = '(d) Fathers'' Group Florence')  ,(select Participant_Record from [dbo].contacts where Email_Address = 'mpcrds+pamparker@gmail.com') ,16           ,1        ,{ts '2015-07-29 12:06:50'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );
GO