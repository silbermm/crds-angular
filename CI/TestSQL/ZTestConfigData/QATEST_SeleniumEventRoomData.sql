USE [MinistryPlatform]
GO
--Cancelled Room
INSERT INTO EVENT_ROOMS
(
   Event_ID,
   Room_ID,
   Room_Layout_ID,
   Chairs,
   [Tables],
   Notes,
   Domain_ID,
   _Approved,
   __Start_Time_Offset,
   __End_Time_Offset,
   Cancelled,
   __ExternalStartTime,
   __ExternalFinishTime,
   __ExternalSetupTime,
   __ExternalTeardownTime,
   __ExternalAudioVisual,
   Hidden
)
select
   event_id, 243, null, null, null, '***CANCELLED***', 1, null, null, null, 1, null, null, null, null, null, 0
   from Events
   where Event_Title = 'Selenium Test Event XX'
   and event_id not in ( select event_id from event_rooms where room_id = 243 and Notes ='***CANCELLED***');
GO

--Approved Room
INSERT INTO EVENT_ROOMS
(
   Event_ID,
   Room_ID,
   Room_Layout_ID,
   Chairs,
   [Tables],
   Notes,
   Domain_ID,
   _Approved,
   __Start_Time_Offset,
   __End_Time_Offset,
   Cancelled,
   __ExternalStartTime,
   __ExternalFinishTime,
   __ExternalSetupTime,
   __ExternalTeardownTime,
   __ExternalAudioVisual,
   Hidden
)
select
   event_id, 244, null, null, null, null, 1, 1, null, null, 0, null, null, null, null, null, 0
   from Events
   where Event_Title = 'Selenium Test Event XX'
   and event_id not in ( select event_id from event_rooms where room_id =244 and notes is null);
GO

--Requested but not approved room
INSERT INTO EVENT_ROOMS
(
   Event_ID,
   Room_ID,
   Room_Layout_ID,
   Chairs,
   [Tables],
   Notes,
   Domain_ID,
   _Approved,
   __Start_Time_Offset,
   __End_Time_Offset,
   Cancelled,
   __ExternalStartTime,
   __ExternalFinishTime,
   __ExternalSetupTime,
   __ExternalTeardownTime,
   __ExternalAudioVisual,
   Hidden
)
select
   event_id, 243, null, null, null, null, 1, null, null, null, 0, null, null, null, null, null, 0
   from Events
   where Event_Title = 'Selenium Test Event XX'
   and event_id not in ( select event_id from event_rooms where room_id = 243 and Notes is null);
GO