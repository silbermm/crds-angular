/*
	Associate responses with new events after migration

	Assumptions:
		1) New events have been created with Sandi's script.
		2) There is only 1 event per every event_type_ID/Event_Start_Date combination (if not the script will error)
*/
USE MinistryPlatform

IF OBJECT_ID('tempdb..#Temp') IS NOT NULL
    DROP TABLE #Temp


CREATE TABLE #Temp
(
    respid INT null, 
    origeventid INT null, 
    EventStartDate DATETIME null, 
    oppid INT null ,
    eventtypeid INT null,
	neweventid INT null,
	fromdate datetime null,
	todate datetime null,
	eventsinrange int null
)

-- Query the Responses and get the associated opportunity, and new event id to use. Store in temp table
INSERT INTO #TEMP(respid,origeventid,eventstartdate,oppid,eventtypeid,fromdate,todate)
SELECT r.response_id, r.Event_Id as OriginalEventID,  e.Event_Start_Date, r.Opportunity_ID, o.event_type_id
--,(SELECT count(event_ID) FROM events WHERE Event_Type_ID = o.event_type_id and Event_Start_Date = e.Event_Start_Date) as NewEventId
, (SELECT CAST(CONVERT(VARCHAR,e.Event_Start_Date,102) AS DATETIME)) as fromdate
, (SELECT DATEADD(mi, 1439, CAST(CONVERT(VARCHAR,e.Event_Start_Date,102) AS DATETIME)))
FROM responses r
JOIN Events e ON r.Event_ID = e.Event_ID
JOIN Opportunities o ON o.Opportunity_ID = r.Opportunity_ID
WHERE e.Event_Start_Date > getdate()
and e.Event_Start_Date < '2017/1/1'

UPDATE #TEMP  
set eventsinrange = (select count(*) from events e  where e.Event_Type_ID =eventtypeid and e.event_start_date between fromdate and todate)

UPDATE #TEMP
SET NewEventID = (select event_id from events e  where e.Event_Type_ID =eventtypeid and e.event_start_date between fromdate and todate)
where eventsinrange = 1

--use data in the temp table to update the responses
UPDATE  r
SET r.Event_ID = t.neweventid
FROM Responses r
join #TEMP t on r.Response_ID = t.respid
WHERE t.neweventid is not NULL



