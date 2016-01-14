USE [MinistryPlatform]
GO

--Tifa Lockhart Donations
--Note Donations need to be added  in order by Donation_Date
--Add a donation from last year for Tifa
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date             ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date      ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId  ,250.0000       ,{ts '2014-09-03 00:00:00'},2              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,4                 ,{ts '2015-09-01 00:00:00'},null                 ,null                       ,null            ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. This has a lot of sub-selects to get the right data. Sorry :(
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                                                                         ,Amount  ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = (select donor_record from contacts where email_address = 'mpcrds+TifaLockhart@gmail.com')),250.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a soft credit donation from last year for tifa lockhart
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @fidelityDonor as int
set @fidelityDonor = (select donor_record from Contacts where company_name like 'Fidelity%');

INSERT INTO [dbo].Donations 
(Donor_ID       ,Donation_Amount ,Donation_Date             ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date      ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@fidelityDonor ,1500.0000       ,{ts '2014-09-03 01:00:00'},5              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,4                 ,{ts '2015-09-01 00:00:00'},null                 ,null                       ,null            ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution for the Soft Credit Donation
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                                                                                                   ,Amount   ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = (select donor_record from contacts where company_name like 'Fidelity%') and donation_date = '2014-09-03 01:00:00.0'),1500.0000,3         ,null     ,null        ,@tifaDonorId     ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Donation to the pledge by Tifa - not anonymous.
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date             ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date      ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId  ,150.0000       ,{ts '2015-09-03 00:00:00'},2              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,4                 ,{ts '2015-09-01 00:00:00'},null                 ,null                       ,null            ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. This has a lot of sub-selects to get the right data. Sorry :(
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                                                                         ,Amount  ,Program_ID                                                                 ,Pledge_ID                                                                                                                                  ,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = (select donor_record from contacts where email_address = 'mpcrds+TifaLockhart@gmail.com')),150.0000,(select program_id from programs where program_name like '(t) GO Midgar%') ,(select pledge_id from pledges where donor_id = (select donor_record from contacts where Email_Address = 'mpcrds+CloudStrife@gmail.com')) ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Donation to the pledge by Tifa - Anonymous.
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date             ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date      ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId  ,100.0000       ,{ts '2015-09-05 00:00:00'},2              ,null                  ,null       ,null    ,null ,null            ,1          ,null               ,null                       ,4                 ,{ts '2015-09-01 00:00:00'},null                 ,null                       ,null            ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. This has a lot of sub-selects to get the right data. Sorry :(
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                                                                                                     ,Amount  ,Program_ID                                                                 ,Pledge_ID                                                                                                                                 ,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = (select donor_record from contacts where email_address = 'mpcrds+TifaLockhart@gmail.com') order by Donation_date desc),100.0000,(select program_id from programs where program_name like '(t) GO Midgar%') ,(select pledge_id from pledges where donor_id = (select donor_record from contacts where Email_Address = 'mpcrds+CloudStrife@gmail.com')) ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Donation by Tifa, with credit card
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date             ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date      ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code              ,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId  ,150.0000       ,{ts '2015-09-06 00:00:00'},4              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,4                 ,{ts '2015-09-01 00:00:00'},null                 ,null                       ,'ch_16odS5Eldv5NE53si5NDZzoN' ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. This has a lot of sub-selects to get the right data. Sorry :(
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                                                                                                     ,Amount  ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = (select donor_record from contacts where email_address = 'mpcrds+TifaLockhart@gmail.com') order by Donation_date desc),100.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Donation by Tifa, with bank
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date             ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date      ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId  ,200.0000       ,{ts '2015-09-06 01:00:00'},1              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,4                 ,{ts '2015-09-01 00:00:00'},null                 ,null                       ,null            ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. This has a lot of sub-selects to get the right data. Sorry :(
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                                                                                                     ,Amount  ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = (select donor_record from contacts where email_address = 'mpcrds+TifaLockhart@gmail.com') order by Donation_date desc),200.0000,54        ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a soft credit donation from this year for tifa lockhart
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @fidelityDonor as int
set @fidelityDonor = (select donor_record from Contacts where company_name like 'Fidelity%');

INSERT INTO [dbo].Donations 
(Donor_ID       ,Donation_Amount ,Donation_Date             ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date      ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@fidelityDonor ,1500.0000       ,{ts '2015-09-07 00:00:00'},5              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,4                 ,{ts '2015-09-01 00:00:00'},null                 ,null                       ,null            ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution for the Soft Credit Donation
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                                                                                                   ,Amount   ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = (select donor_record from contacts where company_name like 'Fidelity%') and donation_date = '2015-09-07 00:00:00.0'),1500.0000,3         ,null     ,null        ,@tifaDonorId     ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Donation by Tifa, with credit card that we will do a partial refund on
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date             ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date      ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code             ,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId  ,555.0000       ,{ts '2015-09-07 00:00:00'},4              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,4                 ,{ts '2015-09-01 00:00:00'},null                 ,null                       ,'ch_16ngVCEldv5NE53s9ZK1bq1Z',null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. This has a lot of sub-selects to get the right data. Sorry :(
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                                                                                                     ,Amount  ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = (select donor_record from contacts where email_address = 'mpcrds+TifaLockhart@gmail.com') order by Donation_date desc),555.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Donation refund by Tifa, with credit card
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date             ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes                     ,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date      ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code             ,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId  ,-55.0000       ,{ts '2015-09-07 01:00:00'},4              ,null                  ,null       ,null    ,'Refund on last donation' ,null            ,0          ,null               ,null                       ,4                 ,{ts '2015-09-01 00:00:00'},null                 ,null                       ,'re_16ngWoEldv5NE53sFQdhoGr8',null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. This has a lot of sub-selects to get the right data. Sorry :(
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                                                                                                     ,Amount  ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = (select donor_record from contacts where email_address = 'mpcrds+TifaLockhart@gmail.com') order by Donation_date desc),-55.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a declined donation for Tifa Lockhart
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

INSERT INTO [dbo].donations 
(Donor_ID     ,Donation_Amount,Donation_Date             ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date      ,Donation_Status_Notes                                                          ,Online_Donation_Information,Transaction_Code             ,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId ,300.0000       ,{ts '2015-09-09 11:27:27'},5              ,null                  ,null       ,null    ,null ,null            ,null       ,null               ,null                       ,3                 ,{ts '2015-09-24 15:27:27'},'insufficient_funds: Your account has insufficient funds to cover the payment.',null                       ,'py_16obd5Eldv5NE53s36krdjBt',null             ,null            ,null     ,1        ,null    ,0        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,1               ,null        ,0.2500              ,null                   ,null                     );

INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                                                                                                      ,Amount  ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = (select donor_record from contacts where email_address = 'mpcrds+TifaLockhart@gmail.com') order by Donation_date desc) ,300.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a pending donation for Tifa Lockhart
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

INSERT INTO [dbo].donations 
(Donor_ID     ,Donation_Amount,Donation_Date             ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date      ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code             ,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId ,180.0000       ,{ts '2015-09-09 12:27:27'},5              ,null                  ,null       ,null    ,null ,null            ,null       ,null               ,null                       ,1                 ,{ts '2015-09-24 15:27:27'},null                 ,null                       ,'py_16obd5Eldv5NE53s36krdjBt',null             ,null            ,null     ,1        ,null    ,0        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,1               ,null        ,0.2500              ,null                   ,null                     );

INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                                                                                                      ,Amount  ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = (select donor_record from contacts where email_address = 'mpcrds+TifaLockhart@gmail.com') order by Donation_date desc) ,180.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO