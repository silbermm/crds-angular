USE [MinistryPlatform]
GO

--Delete all donations for Test Pledge Campaigns's program.
DECLARE @donationsTable table
(
	donation_id int
)

insert into @donationsTable (donation_id) (select donation_id from donation_distributions where program_id in (select program_id from programs where program_name like '(t) Test Pledge Program%'));

delete from donation_distributions where donation_id in (select donation_id from @donationsTable);

delete from donations where donation_id in (select donation_id from @donationsTable);

--Delete all donations associated to any pledges for this pledge campaign.
delete from @donationsTable;

insert into @donationsTable (donation_id) (select donation_id from [dbo].donation_distributions where pledge_id in (select pledge_id from [dbo].pledges where pledge_campaign_id in (select pledge_campaign_id from pledge_campaigns where Campaign_Name like '(t) Test Pledge Campaign%')));

delete from donation_distributions where donation_id in (select donation_id from @donationsTable);

delete from donations where donation_id in (select donation_id from @donationsTable);

--Delete any pledges associated to the pledge campaign
delete from [dbo].pledges where pledge_campaign_id in (select pledge_campaign_id from pledge_campaigns where Campaign_Name like '(t) Test Pledge Campaign%');

update [dbo].Pledge_campaigns set program_id = null where pledge_campaign_id in (select pledge_campaign_id from pledge_campaigns where campaign_name like '(t) Test Pledge Campaign%');

--Set any events/opportunities to general fund that have mistakenly chosen these test pledges. 
update [dbo].events set program_id = 3 where program_id in (select program_id from programs where program_name like '(t) Test Pledge Program%');
update [dbo].opportunities set program_id = 3 where program_id in (select program_id from programs where program_name like '(t) Test Pledge Program%');

delete from [dbo].programs where program_id in (select program_id from programs where program_name like '(t) Test Pledge Program%');

delete from [dbo].pledge_campaigns where pledge_campaign_id in (select pledge_campaign_id from pledge_campaigns where campaign_name like '(t) Test Pledge Campaign%');
GO