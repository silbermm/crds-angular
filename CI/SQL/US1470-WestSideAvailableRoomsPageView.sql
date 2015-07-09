USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([View_Title]
           ,[Page_ID]
           ,[Description]        
           ,[View_Clause])
     VALUES
           ('West Side Available Rooms'
           ,386
           ,'This view shows bookable rooms on the rooms grid for West Side, but it shows only bookable rooms already in use when used from a sub page associated with an event. This ensures the operator is not picking rooms already reserved by another event.'
           ,'Rooms.Bookable=1 AND Building_ID_Table.Location_ID = ISNULL((SELECT Top 1 Location_ID FROM Events WHERE Event_ID = dp_ParentID),Building_ID_Table.Location_ID) AND Rooms.Building_ID=6 AND NOT EXISTS (SELECT 1 FROM Event_Rooms ER INNER JOIN Events E1 ON E1.Event_ID = ER.Event_ID INNER JOIN Events E2 ON E2.Event_ID = dp_ParentID AND E2.__Reservation_Start <= E1.__Reservation_End AND E2.__Reservation_End >= E1.__Reservation_Start WHERE ER.Room_ID = Rooms.Room_ID )')
           
GO
