USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =========================================================================
-- Author:		Sara Seissiger
-- Create date: 01/15/2016
-- Description:	This script is a one-time run for go-live to create Room 
-- Records for Equipment and to create Non-Oakley Equipment records to live.
--
-- DO NOT RUN THIS SCRIPT AFTER GO-LIVE NIGHT.
-- =========================================================================
 

 --Create equipment rooms
	DECLARE @Oakley_Building AS INT
	DECLARE @Mason_Building AS INT
	DECLARE @Florence_Building AS INT
	DECLARE @West_Side_Building AS INT
	DECLARE @Uptown_Building AS INT

	DECLARE @Oakley_Room_Id AS INT
	DECLARE @Mason_Room_Id AS INT
	DECLARE @Florence_Room_Id AS INT
	DECLARE @West_Side_Room_Id AS INT
	DECLARE @Uptown_Room_Id AS INT

	SELECT @Oakley_Building = Building_ID FROM dbo.Buildings WHERE Building_Name = 'Oakley'
	SELECT @Mason_Building = Building_ID FROM dbo.Buildings WHERE Building_Name = 'Mason'
	SELECT @Florence_Building = Building_ID FROM dbo.Buildings WHERE Building_Name = 'Florence'
	SELECT @West_Side_Building = Building_ID FROM dbo.Buildings WHERE Building_Name = 'West Side'
	SELECT @Uptown_Building = Building_ID FROM dbo.Buildings WHERE Building_Name = 'Uptown'


	INSERT INTO dbo.Rooms
			(Room_Name, Building_ID, Domain_ID, Room_Usage_Type_ID, Bookable, Auto_Approve, Setup_Time, Teardown_Time)
			VALUES
			('Oakley Equipment', @Oakley_Building , 1, 1, 1, 0, 0, 0);

		SELECT @Oakley_Room_Id =  SCOPE_IDENTITY()

	INSERT INTO dbo.Rooms
			(Room_Name, Building_ID, Domain_ID, Room_Usage_Type_ID, Bookable, Auto_Approve, Setup_Time, Teardown_Time)
			VALUES
			('Mason Equipment', @Mason_Building, 1, 1, 1, 0, 0, 0)

		SELECT @Mason_Room_Id =  SCOPE_IDENTITY()

	INSERT INTO dbo.Rooms
			(Room_Name, Building_ID, Domain_ID, Room_Usage_Type_ID, Bookable, Auto_Approve, Setup_Time, Teardown_Time)
			VALUES
			('Florence Equipment', @Florence_Building, 1, 1, 1, 0, 0, 0);

		SELECT @Florence_Room_Id =  SCOPE_IDENTITY()

	INSERT INTO dbo.Rooms
			(Room_Name, Building_ID, Domain_ID, Room_Usage_Type_ID, Bookable, Auto_Approve, Setup_Time, Teardown_Time)
			VALUES
			('Uptown Equipment', @Uptown_Building, 1, 1, 1, 0, 0, 0);

		SELECT @Uptown_Room_Id =  SCOPE_IDENTITY()

	INSERT INTO dbo.Rooms
			(Room_Name, Building_ID, Domain_ID, Room_Usage_Type_ID, Bookable, Auto_Approve, Setup_Time, Teardown_Time)
			VALUES
			('West Side Equipment', @West_Side_Building, 1, 1, 1, 0, 0, 0)

		SELECT @West_Side_Room_Id =  SCOPE_IDENTITY()

	INSERT INTO dbo.Rooms
			(Room_Name, Building_ID, Domain_ID, Room_Usage_Type_ID, Bookable, Auto_Approve, Setup_Time, Teardown_Time)
			VALUES
			('Uptown Equipment', @Uptown_Building, 1, 1, 1, 0, 0, 0)

		SELECT @Uptown_Room_Id =  SCOPE_IDENTITY()

--SELECT *  FROM [MinistryPlatform].[dbo].[Buildings]
--SELECT *  FROM [MinistryPlatform].[dbo].[Rooms]
--SELECT *  FROM [MinistryPlatform].[dbo].[Equipment]


--******************************************************************
-- Make sure to add in Equipment_Coordinator data when get from Keri
--******************************************************************

	--Florence Equipment
	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('4X4 PLATFORMS' , GetDate(), 7, @Florence_Room_Id, 1, 1, null, 0, 2, 'N' )

	INSERT INTO dbo.Equipment 
	(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('4X8 Platform (portable)' , GetDate(), 7, @Florence_Room_Id, 1, 1, null, 0, 3, 'N' ) 

	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		 VALUES 
		 ('CHAIRS - Metal Cafe' , GetDate(), 7, @Florence_Room_Id, 1, 1, null, 0, 2, 'N' )

	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('CHAIRS-Black Folding' , GetDate(), 7, @Florence_Room_Id, 1, 1, null, 0, 288, 'N' )

	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('COAT RACKS' , GetDate(), 7, @Florence_Room_Id, 1, 1, null, 0, 2, 'N' )
	
	INSERT INTO dbo.Equipment  
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('SIGN(S) - Bubble Signs' , GetDate(), 7, @Florence_Room_Id, 1, 1, null, 0, 20, 'N' )
	
	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('STANCHIONS' , GetDate(), 7, @Florence_Room_Id, 1, 1, null, 0, 10, 'N' )
	
	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('TABLE - 6 ft x 30 in Rectagular' , GetDate(), 7, @Florence_Room_Id, 1, 1, null, 0, 8, 'N' )
	
	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('TABLE - 6 ft. Round' , GetDate(), 7, @Florence_Room_Id, 1, 1, null, 0, 32, 'N' )
	
	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('TABLE - CAFE HI TOP' , GetDate(), 7, @Florence_Room_Id, 1, 1, null, 0, 5, 'N' )
	
	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES 
		('TRASH CAN' , GetDate(), 7, @Florence_Room_Id, 1, 1, null, 0, 19, 'N' )
	
	INSERT INTO dbo.Equipment 
		(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		VALUES
		 ('WHITEBOARD - large' , GetDate(), 7, @Florence_Room_Id, 1, 1, null, 0, 3, 'N' )


	--Mason Equipment
	--INSERT INTO dbo.Equipment 
		--(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		--VALUES 


	--West Side Equipment
	--INSERT INTO dbo.Equipment 
		--(Equipment_Name, Date_Acquired, Equipment_Type_ID, Room_ID, Bookable, Domain_ID, Equipment_Coordinator, Auto_Approve, Quantity_On_Hand, Audio_Visual ) 
		--VALUES 		

GO
