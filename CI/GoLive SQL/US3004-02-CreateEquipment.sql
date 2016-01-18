USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =========================================================================
-- Author:		Sara Seissiger
-- Create date: 01/15/2016
-- Description:	This script is a one-time run for go-live to create
-- records for Non-Oakley Equipment records to live.
--
-- Pre-Condition - Equipment Rooms Created - Run US3004-01_CreateRooms.sql
--
-- DO NOT RUN THIS SCRIPT AFTER GO-LIVE NIGHT.
-- =========================================================================

	DECLARE @Oakley_Room_Id AS INT
	DECLARE @Mason_Room_Id AS INT
	DECLARE @Florence_Room_Id AS INT
	DECLARE @West_Side_Room_Id AS INT
	DECLARE @Uptown_Room_Id AS INT

	SELECT @Oakley_Room_Id = Room_ID FROM dbo.Rooms WHERE Room_Name = 'Oakley Equipment'
	SELECT @Mason_Room_Id = Room_ID FROM dbo.Rooms WHERE Room_Name = 'Mason Equipment'
	SELECT @Florence_Room_Id = Room_ID FROM dbo.Rooms WHERE Room_Name = 'Florence Equipment'
	SELECT @West_Side_Room_Id = Room_ID FROM dbo.Rooms WHERE Room_Name =  'West Side Equipment'
	SELECT @Uptown_Room_Id = Room_ID FROM dbo.Rooms WHERE Room_Name =  'Uptown Equipment'

	DECLARE @Oakley_Coordinator AS INT
	DECLARE @Mason_Coordinator AS INT
	DECLARE @Florence_Coordinator AS INT
	DECLARE @West_Side_Coordinator AS INT

	--Albanese, David; dalbanese@crossroads.net
	SELECT @Oakley_Coordinator = User_ID  FROM dbo.dp_Users WHERE User_Email = 'dalbanese@crossroads.net'

	--Gerke, Karen; kgerke@crossroads.net
	SELECT @Mason_Coordinator = User_ID  FROM dbo.dp_Users WHERE User_Email = 'kgerke@crossroads.net'
	 
	-- Levey, Susan; slevey@crossroads.net
	SELECT @Florence_Coordinator = User_ID  FROM dbo.dp_Users WHERE User_Email = 'slevey@crossroads.net'
	
	--Rueve, Michele; mrueve@crossroads.net
	SELECT @West_Side_Coordinator = User_ID  FROM dbo.dp_Users WHERE User_Email = 'mrueve@crossroads.net'
	

	--Florence Equipment
	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('4X4 PLATFORMS' , GetDate(), 7, @Florence_Room_Id, 1, 1, @Florence_Coordinator, 0, 2, 'N' )

	INSERT INTO dbo.Equipment 
	(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('4X8 Platform (portable)' , GetDate(), 7, @Florence_Room_Id, 1, 1, @Florence_Coordinator, 0, 3, 'N' ) 

	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		 VALUES 
		 ('CHAIRS - Metal Cafe' , GetDate(), 7, @Florence_Room_Id, 1, 1, @Florence_Coordinator, 0, 2, 'N' )

	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('CHAIRS-Black Folding' , GetDate(), 7, @Florence_Room_Id, 1, 1, @Florence_Coordinator, 0, 288, 'N' )

	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('COAT RACKS' , GetDate(), 7, @Florence_Room_Id, 1, 1, @Florence_Coordinator, 0, 2, 'N' )
	
	INSERT INTO dbo.Equipment  
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('SIGN(S) - Bubble Signs' , GetDate(), 7, @Florence_Room_Id, 1, 1, @Florence_Coordinator, 0, 20, 'N' )
	
	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('STANCHIONS' , GetDate(), 7, @Florence_Room_Id, 1, 1, @Florence_Coordinator, 0, 10, 'N' )
	
	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('TABLE - 6 ft x 30 in Rectagular' , GetDate(), 7, @Florence_Room_Id, 1, 1, @Florence_Coordinator, 0, 8, 'N' )
	
	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('TABLE - 6 ft. Round' , GetDate(), 7, @Florence_Room_Id, 1, 1, @Florence_Coordinator, 0, 32, 'N' )
	
	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('TABLE - CAFE HI TOP' , GetDate(), 7, @Florence_Room_Id, 1, 1, @Florence_Coordinator, 0, 5, 'N' )
	
	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('TRASH CAN' , GetDate(), 7, @Florence_Room_Id, 1, 1, @Florence_Coordinator, 0, 19, 'N' )
	
	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES
		 ('WHITEBOARD - large' , GetDate(), 7, @Florence_Room_Id, 1, 1, @Florence_Coordinator, 0, 3, 'N' )


	--Mason Equipment
	--INSERT INTO dbo.Equipment 
		--(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		--VALUES 


	--West Side Equipment
	--INSERT INTO dbo.Equipment 
		--(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		--VALUES 		

