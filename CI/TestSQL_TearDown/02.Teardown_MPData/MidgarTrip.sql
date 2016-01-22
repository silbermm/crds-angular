USE [MinistryPlatform]
GO

--Trip Name
DECLARE @tripName AS VARCHAR(18)
set @tripName = '(t) GO Midgar '+CONVERT(VARCHAR(4), datepart(year, getdate()));

Declare @pledgeCampaignId as int
set @pledgeCampaignId = (select top 1 pledge_campaign_id from [dbo].Pledge_Campaigns where Campaign_name = @tripName);

delete from [dbo].event_participants where event_id in (select event_id from [dbo].events where event_title = @tripName);

delete from [dbo].event_groups where event_id in (select event_id from [dbo].events where event_title = @tripName);

delete from [dbo].event_groups where group_id in (select group_id from [dbo].groups where group_name = @tripName);

update [dbo].Pledge_Campaigns set EVENT_ID = null, Program_id = null where Campaign_Name = @tripName;

delete FROM [dbo].events where event_title = @tripName;

--Delete all donations for Midgar's program.
DECLARE @donationsTable table
(
	donation_id int
)

insert into @donationsTable (donation_id) (select donation_id from donation_distributions where program_id in (select program_id from programs where program_name like '(t) GO Midgar%'));

delete from donation_distributions where donation_id in (select donation_id from @donationsTable);

delete from donations where donation_id in (select donation_id from @donationsTable);

--Delete all donations associated to any pledges for this pledge campaign.
delete from @donationsTable;

insert into @donationsTable (donation_id) (select donation_id from [dbo].donation_distributions where pledge_id in (select pledge_id from [dbo].pledges where pledge_campaign_id = @pledgeCampaignId));

delete from donation_distributions where donation_id in (select donation_id from @donationsTable);

delete from donations where donation_id in (select donation_id from @donationsTable);

--Delete any pledges associated to the pledge campaign
delete from [dbo].pledges where pledge_campaign_id = @pledgeCampaignId;

delete from [dbo].Group_Participants where group_id in (select group_id from [dbo].groups where group_name = @tripName);

delete from [dbo].Program_groups where program_id in (Select program_id from programs where program_name = @tripName);

delete from [dbo].Groups where Group_id in (select group_id from groups where group_name = @tripName);

delete from GL_Account_Mapping where program_id in (select program_id from programs where program_name = @tripName);

delete from [dbo].programs where program_name = @tripName;

delete from [dbo].pledge_campaigns where pledge_campaign_id = @pledgeCampaignId;
