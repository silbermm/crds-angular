--(t) Go Midgar participant records
USE [MinistryPlatform]
GO

DECLARE @cloudPartId as int
set @cloudPartId = (select participant_record from Contacts where Email_address = 'mpcrds+CloudStrife@gmail.com');

DECLARE @cloudDonorId as int
set @cloudDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+CloudStrife@gmail.com');

--Add Cloud Strife to the GO Midgar GROUP
INSERT INTO [dbo].Group_Participants 
(Group_ID                                                            ,Participant_ID,Group_Role_ID,Domain_ID,[Start_Date]              ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
((select group_id from groups where group_name like '(t) GO Midgar%'),@cloudPartId  ,16           ,1        ,{ts '2015-09-09 15:00:00'},null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

--Add Cloud Strife to Event_Participant list
INSERT INTO [dbo].Event_Participants 
(Event_ID                                                             ,Participant_ID,Participation_Status_ID,Time_In                   ,Time_Confirmed            ,Time_Out,Notes,Domain_ID,Group_Participant_ID,[Check-in_Station],_Setup_Date               ,Group_ID,Room_ID,Call_Parents,Group_Role_ID,Response_ID,__ExternalCalendarServingtimePersonID,Opportunity_ID) VALUES
((select event_id from events where Event_Title like '(t) GO Midgar%'),@cloudPartId  ,4                      ,{ts '2015-09-01 00:00:00'},{ts '2015-09-01 00:00:00'},null    ,null ,1        ,null                ,null              ,{ts '2015-09-09 19:03:37'},null    ,null   ,null        ,null         ,null       ,null                                 ,null          );

--Add a Pledge for Cloud Strife. Pledge campaign ID is hard coded since it will not change.
INSERT INTO [dbo].Pledges 
(Donor_ID      ,Pledge_Campaign_ID,Pledge_Status_ID,Total_Pledge,Installments_Planned,Installments_Per_Year,First_Installment_Date    ,Notes,Domain_ID,Beneficiary,Trip_Leader,Currency,__ExternalPersonID1,__ExternalPersonID2,__ExternalCommitmentID,__ExternalApplicationID) VALUES
(@cloudDonorId ,10000000          ,1               ,1000.0000   ,0                   ,0                    ,{ts '2015-07-01 12:00:00'},null ,1        ,null       ,0          ,null    ,null               ,null               ,null                  ,null                   );

--Add a deposit Donation to the pledge by Cloud.
INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date             ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date      ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@cloudDonorId ,100.0000       ,{ts '2015-09-01 00:00:00'},2              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,4                 ,{ts '2015-09-01 00:00:00'},null                 ,null                       ,null            ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. This has a lot of sub-selects to get the right data. Sorry :(
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                                                                   ,Amount  ,Program_ID                                                                 ,Pledge_ID                                                                                                                                 ,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select donation_id from donations where donor_id = (select donor_record from contacts where email_address = 'mpcrds+CloudStrife@gmail.com')) ,100.0000,(select program_id from programs where program_name like '(t) GO Midgar%') ,(select pledge_id from pledges where donor_id = (select donor_record from contacts where Email_Address = 'mpcrds+CloudStrife@gmail.com')) ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

