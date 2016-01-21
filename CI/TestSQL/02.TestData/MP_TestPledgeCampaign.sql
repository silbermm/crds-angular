USE [MinistryPlatform]
GO

DECLARE @curr_pledgeCampaign_id AS INT
DECLARE @pledgeCampaignId AS INT
DECLARE @programId AS INT
DECLARE @donorId AS INT


--Pledge Set up for Head of the household1
--Program for (t) Test Pledge Program1

INSERT INTO [dbo].programs 
(Program_Name              ,Congregation_ID,Ministry_ID,[Start_Date]               ,End_Date                       ,Program_Type_ID,Leadership_Team,Primary_Contact,Priority_ID,On_Connection_Card,Stewardship_Information,Tax_Deductible_Donations,Statement_Title           ,Statement_Header_ID,Allow_Online_Giving,Online_Sort_Order,Pledge_Campaign_ID,Account_Number,Default_Target_Event,On_Donation_Batch_Tool,Domain_ID,Available_Online,__ExternalFundID,Communication_ID) VALUES
('(t) Test Pledge Program1',12             ,8          ,DATEADD(YEAR, -1,GETDATE()),DATEADD(YEAR, 1,GETDATE())     ,1              ,null           ,2562428        ,null       ,null              ,null                   ,1                       ,'(t) Test Pledge Program1',1                  ,1                  ,2                ,null              ,null          ,null                ,1                     ,1        ,1               ,null            ,null            );

--Pledge Campaign for (t) Test Pledge Campaign1
SET IDENTITY_INSERT Pledge_Campaigns ON;

SET @curr_pledgeCampaign_id = IDENT_CURRENT('Pledge_Campaigns');
SET @pledgeCampaignId = 10000010;
SET @programId = (SELECT Program_ID FROM Programs WHERE Program_Name = '(t) Test Pledge Program1');

INSERT INTO Pledge_Campaigns 
(Pledge_Campaign_ID,Campaign_Name                 ,Nickname                   ,Pledge_Campaign_Type_ID,[Description]  ,Campaign_Goal,[Start_Date]              ,End_Date ,Domain_ID,Event_ID,Program_ID,Registration_Details,Registration_Start ,Registration_End ,Maximum_Registrants,Youngest_Age_Allowed,Registration_Deposit,Fundraising_Goal,Registration_Form,Online_Pledge_Details,Allow_Online_Pledge,Online_Thank_You_Message,Pledge_Beyond_End_Date,Show_On_My_Pledges,__ExternalTripID,__ExternalFundID) VALUES
(@pledgeCampaignId ,'(t) Test Pledge Campaign1'   ,'(t) Test Pledge Campaign1',1                      ,null           ,1000.00      ,DATEADD(DAY,-30,GETDATE()),null     ,1        ,null    ,@programId,null                ,null               ,null             ,null               , null               ,null                ,null            ,null             ,null                 ,1                  ,null                    ,1                     ,1                 ,40              ,99              );

SET IDENTITY_INSERT [dbo].[Pledge_Campaigns] OFF;

DBCC CHECKIDENT (Pledge_Campaigns, reseed, @curr_pledgeCampaign_id);

--Pledge for (t) Test Pledge

SET @donorId = (SELECT Donor_ID FROM Donors WHERE Contact_ID IN (SELECT Contact_ID FROM Contacts WHERE Email_Address = 'mpcrds+tremplay.richard@gmail.com'));

INSERT INTO Pledges
(Donor_ID, Pledge_Campaign_ID, Pledge_Status_ID, Total_Pledge, Installments_Planned, Installments_Per_Year, First_Installment_Date      , Notes, Domain_ID, Beneficiary, Trip_Leader, Currency, __ExternalPersonID1, __ExternalPersonID2, __ExternalCommitmentID, __ExternalApplicationID, Trip_General_Fund) VALUES
(@donorId, @pledgeCampaignId , 1               , 1000.00     , 10                  , 1000.00              ,  DATEADD(DAY, -3, GETDATE()), null , 1        , null       , null       , null    , null               , null               , null                  , null                   , null );               

---------------------------------------------------------------------------------------------------------------------------
--Pledge Set up for Head of the household2
--Program for (t) Test Pledge Program2

INSERT INTO [dbo].programs 
(Program_Name              ,Congregation_ID,Ministry_ID,[Start_Date]               ,End_Date                       ,Program_Type_ID,Leadership_Team,Primary_Contact,Priority_ID,On_Connection_Card,Stewardship_Information,Tax_Deductible_Donations,Statement_Title           ,Statement_Header_ID,Allow_Online_Giving,Online_Sort_Order,Pledge_Campaign_ID,Account_Number,Default_Target_Event,On_Donation_Batch_Tool,Domain_ID,Available_Online,__ExternalFundID,Communication_ID) VALUES
('(t) Test Pledge Program2',12             ,8          ,DATEADD(YEAR, -1,GETDATE()),DATEADD(YEAR, 1,GETDATE())     ,1              ,null           ,2562428        ,null       ,null              ,null                   ,1                       ,'(t) Test Pledge Program2',1                  ,1                  ,2                ,null              ,null          ,null                ,1                     ,1        ,1               ,null            ,null            );

--Pledge Campaign for (t) Test Pledge Campaign
SET IDENTITY_INSERT Pledge_Campaigns ON;

SET @curr_pledgeCampaign_id = IDENT_CURRENT('Pledge_Campaigns');
SET @pledgeCampaignId = 10000011;
SET @programId = (SELECT Program_ID FROM Programs WHERE Program_Name = '(t) Test Pledge Program2');

INSERT INTO Pledge_Campaigns 
(Pledge_Campaign_ID,Campaign_Name                 ,Nickname                   ,Pledge_Campaign_Type_ID,[Description]  ,Campaign_Goal,[Start_Date]              ,End_Date ,Domain_ID,Event_ID,Program_ID,Registration_Details,Registration_Start ,Registration_End ,Maximum_Registrants,Youngest_Age_Allowed,Registration_Deposit,Fundraising_Goal,Registration_Form,Online_Pledge_Details,Allow_Online_Pledge,Online_Thank_You_Message,Pledge_Beyond_End_Date,Show_On_My_Pledges,__ExternalTripID,__ExternalFundID) VALUES
(@pledgeCampaignId ,'(t) Test Pledge Campaign2'   ,'(t) Test Pledge Campaign2',1                      ,null           ,1000.00      ,DATEADD(DAY,-30,GETDATE()),null     ,1        ,null    ,@programId,null                ,null               ,null             ,null               , null               ,null                ,null            ,null             ,null                 ,1                  ,null                    ,1                     ,1                 ,40              ,99              );

SET IDENTITY_INSERT [dbo].[Pledge_Campaigns] OFF;

DBCC CHECKIDENT (Pledge_Campaigns, reseed, @curr_pledgeCampaign_id);

--Pledge for (t) Test Pledge

SET @donorId = (SELECT Donor_ID FROM Donors WHERE Contact_ID = (SELECT Contact_ID FROM Contacts WHERE Email_Address = 'mpcrds+tremplay.mary@gmail.com'));

INSERT INTO Pledges
(Donor_ID, Pledge_Campaign_ID, Pledge_Status_ID, Total_Pledge, Installments_Planned, Installments_Per_Year, First_Installment_Date      , Notes, Domain_ID, Beneficiary, Trip_Leader, Currency, __ExternalPersonID1, __ExternalPersonID2, __ExternalCommitmentID, __ExternalApplicationID, Trip_General_Fund) VALUES
(@donorId, @pledgeCampaignId , 1               , 1000.00     , 10                  , 1000.00              ,  DATEADD(DAY, -3, GETDATE()), null , 1        , null       , null       , null    , null               , null               , null                  , null                   , null );               



