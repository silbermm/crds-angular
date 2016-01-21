 /*********************************************************

 Setup:
	1. Place Mandrill rejects csv file on db server at C:\Test\Mandrill Rejects List.csv
	2. Place Constant Contact unsubscribed csv on db server at C:\Test\CC_Unsubscribed_Contacts.csv
	3. Place Constant Contact bounced csv on db server at C:\Test\CC_Bounced_Contacts.csv
	4. User running script needs 'bulkadmin' server role

 Description: Script to read csv file containing mandrill reject list
	Entries marked as 'spam' are set as unsubscribed = 1  in dp_Contact_Publications table

 Output: log results of entries in dp_Contact_Publications that are contained in the
         csv file and marked as 'hard-bounce' to table dbo.cr_tmp_hardbounce
	 
		 

 *********************************************************/
 
 
 use MinistryPlatform

 IF OBJECT_ID('tempdb..#MPEMAILREJECTS') IS NOT NULL
    DROP TABLE #MPEMAILREJECTS

IF OBJECT_ID('tempdb..#CCEMAILUNSUBSCRIBE') IS NOT NULL
    DROP TABLE #CCEMAILUNSUBSCRIBE

IF OBJECT_ID('tempdb..#CCEMAILBOUNCE') IS NOT NULL
    DROP TABLE #CCEMAILBOUNCE

 --Load the mandril csv file 
 create table #MPEMAILREJECTS(
   email varchar(1000),
   reason varchar(1000),
   detail varchar(1000),
   created_at varchar(1000),
   expires_at varchar(1000),
   last_event_at varchar(1000),
   expires_at2 varchar(1000),
   subaccount varchar(1000)
 )

 BULK INSERT #MPEMAILREJECTS
   FROM 'C:\Test\Mandrill Rejects List.csv'
   WITH 
      (
         FIELDTERMINATOR =',',
         ROWTERMINATOR = '\n',
         FIRSTROW = 2
      )

	  
--Load the constant contact unsubscribe csv file 
 create table #CCEMAILUNSUBSCRIBE(
   firstname varchar(1000),
   lastname varchar(1000),

   emailaddr_Home varchar(1000),
   emailstatus_Home varchar(1000),
   emailpermstatus_Home varchar(1000),

   emailaddr_Other varchar(1000),
   emailstatus_Other varchar(1000),
   emailpermstatus_Other varchar(1000),

   emailaddr_Other2 varchar(1000),
   emailstatus_Other2 varchar(1000),
   emailpermstatus_Other2 varchar(1000),

   emailaddr_Work varchar(1000),
   emailstatus_Work varchar(1000),
   emailpermstatus_Work varchar(1000),

   phonehome varchar(1000),
   phonework varchar(1000),

   addr_home varchar(1000),
   city_home varchar(1000),
   state_home varchar(1000),
   zip_home varchar(1000),
   country_home varchar(1000),

   addr_work varchar(1000),
   city_work varchar(1000),
   state_work varchar(1000),
   zip_work varchar(1000),
   country_work varchar(1000),

   emaillist varchar(1000),
   notes varchar(1000),
   sourcename varchar(1000),
   createdat varchar(1000),
   updatedat varchar(1000)
 )

 BULK INSERT #CCEMAILUNSUBSCRIBE
   FROM 'C:\Test\CC_Unsubscribed_Contacts.csv'
   WITH 
      (
         FIELDTERMINATOR =',',
         ROWTERMINATOR = '\n',
         FIRSTROW = 2
      )
 
 --Load the constant contact bounce csv file 
 create table #CCEMAILBOUNCE(
   A varchar(MAX),
   B varchar(MAX),
   C varchar(MAX),
   D varchar(MAX),

   emailaddr_Home varchar(MAX),
   emailstatus_Home varchar(MAX),
   emailpermstatus_Home varchar(MAX),
   emailupdatesource_Home varchar(MAX),

   emailaddr_Other varchar(MAX),
   emailstatus_Other varchar(MAX),
   emailpermstatus_Other varchar(MAX),
   emailupdatesource_Other varchar(MAX),

   emailaddr_Other2 varchar(MAX),
   emailstatus_Other2 varchar(MAX),
   emailpermstatus_Other2 varchar(MAX),
   emailupdatesource_Other2 varchar(MAX),

   Q varchar(MAX),
   R varchar(MAX),
   S varchar(MAX),
   T varchar(MAX),
   U varchar(MAX),
   V varchar(MAX),
   W varchar(MAX),
   X varchar(MAX),
   Y varchar(MAX),
   Z varchar(MAX),
   AA varchar(MAX),
   AB varchar(MAX),
   AC varchar(MAX),
   AD varchar(MAX),
   AE varchar(MAX),
   AF varchar(MAX),
   AG varchar(MAX),
   AH varchar(MAX),
   AI varchar(MAX),
   AJ varchar(MAX),
   AK varchar(MAX),
   AL varchar(MAX),
   AM varchar(MAX),
   AN varchar(MAX),
   AO varchar(MAX),
   AP varchar(MAX),
   AQ varchar(MAX),
   AR varchar(MAX),
   ColAS varchar(MAX),
   AT varchar(MAX),
   AU varchar(MAX),
   AV varchar(MAX),
   AW varchar(MAX),
   AX varchar(MAX),
   AY varchar(MAX)
 )

  BULK INSERT #CCEMAILBOUNCE
   FROM 'C:\Test\CC_Bounced_Contacts.csv'
   WITH 
      (
         FIELDTERMINATOR =',',
         ROWTERMINATOR = '\n',
         FIRSTROW = 2
      )


 -- spam - unsubscribe user
 update dp_Contact_Publications set unsubscribed = 1 where Contact_ID in(select contact_id from contacts where Email_address in (select email from #MPEMAILREJECTS where reason = 'spam') )
 -- constant contact unsubscribe
 update dp_Contact_Publications set unsubscribed = 1 
	where Contact_ID in(select contact_id from contacts where Email_address in(select emailaddr_Home from #CCEMAILUNSUBSCRIBE where emailstatus_Home = 'Unsubscribed'
	                                 UNION (select emailaddr_Other from #CCEMAILUNSUBSCRIBE where emailstatus_Other = 'Unsubscribed')
									 UNION (select emailaddr_Other2 from #CCEMAILUNSUBSCRIBE where emailstatus_Other2 = 'Unsubscribed')
									 UNION (select emailaddr_Work from #CCEMAILUNSUBSCRIBE where emailstatus_Work = 'Unsubscribed') 
									))

 IF OBJECT_ID('dbo.cr_tmp_hardbounce') IS NULL
	create table cr_tmp_hardbounce(email varchar(1000), createdon datetime, listsource varchar(1000) )

 --hard-bounce - list email addresses that show in dp_Contact_Publications
 insert into cr_tmp_hardbounce
	select c.email_address,getdate(),'mandrill' 
	from contacts c 
	where email_address in (select email from #MPEMAILREJECTS where reason = 'hard-bounce')
 
 --bounced constant contact addresses
 insert into cr_tmp_hardbounce
	select c.email_address,getdate(),'constant contact' 
	from contacts c  
	where email_address in(select emailaddr_Home from #CCEMAILBOUNCE where emailaddr_Home is not null 
													UNION (select emailaddr_Other from #CCEMAILBOUNCE where emailaddr_Other is not null)
													UNION (select emailaddr_Other2 from #CCEMAILBOUNCE where emailaddr_Other2 is not null)
													)
  
 DROP TABLE #MPEMAILREJECTS
 DROP TABLE #CCEMAILUNSUBSCRIBE
 DROP TABLE #CCEMAILBOUNCE
