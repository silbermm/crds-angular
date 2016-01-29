Use [MinistryPlatform]
GO

--This script will remove Event_Type_ID from opportunities that don't have future dates in Legacy. This needs to be run in DEMO before copying to prod. 
update [dbo].opportunities set Event_Type_ID = null
where __ExternalRecurrenceSubsetCode in
(
   select
   distinct cest.recurrence_subset_code
   FROM LegacyStaging.dbo.calendar_event_serving_time cest
   join LegacyStaging.dbo.calendar_event ce ON ce.calendar_event_id = cest.calendar_event_id
   where ce.calendar_event_date < '2016-01-26'
   and cest.recurrence_subset_code not in
   (
      select
      distinct serve.recurrence_subset_code
      from LegacyStaging.dbo.calendar_event_serving_time serve,
      LegacyStaging.dbo.calendar_event calevnt
      where serve.calendar_event_id = calevnt.calendar_event_id
      and calendar_event_date > '2016-01-26'
   )
);