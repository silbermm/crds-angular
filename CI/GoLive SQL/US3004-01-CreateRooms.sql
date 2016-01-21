USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =========================================================================
-- Author:		Sara Seissiger
-- Create date: 01/18/2016
-- Description:	This script is a one-time run for go-live to create Room 
-- Records for Equipment for go live.
--
-- DO NOT RUN THIS SCRIPT AFTER GO-LIVE NIGHT.
-- =========================================================================

	DECLARE @Oakley_Building AS INT
	DECLARE @Mason_Building AS INT
	DECLARE @Florence_Building AS INT
	DECLARE @West_Side_Building AS INT
	DECLARE @Uptown_Building AS INT

	SELECT @Oakley_Building = Building_ID FROM dbo.Buildings WHERE Building_Name = 'Oakley'
	SELECT @Mason_Building = Building_ID FROM dbo.Buildings WHERE Building_Name = 'Mason'
	SELECT @Florence_Building = Building_ID FROM dbo.Buildings WHERE Building_Name = 'Florence'
	SELECT @West_Side_Building = Building_ID FROM dbo.Buildings WHERE Building_Name = 'West Side'
	SELECT @Uptown_Building = Building_ID FROM dbo.Buildings WHERE Building_Name = 'Uptown'


	INSERT INTO dbo.Rooms
			(Room_Name, Building_ID, Domain_ID, Room_Usage_Type_ID, Bookable, Auto_Approve, Setup_Time, Teardown_Time)
			VALUES
			('Oakley Equipment', @Oakley_Building , 1, 1, 0, 0, 0, 0);

	INSERT INTO dbo.Rooms
			(Room_Name, Building_ID, Domain_ID, Room_Usage_Type_ID, Bookable, Auto_Approve, Setup_Time, Teardown_Time)
			VALUES
			('Mason Equipment', @Mason_Building, 1, 1, 0, 0, 0, 0)

	INSERT INTO dbo.Rooms
			(Room_Name, Building_ID, Domain_ID, Room_Usage_Type_ID, Bookable, Auto_Approve, Setup_Time, Teardown_Time)
			VALUES
			('Florence Equipment', @Florence_Building, 1, 1, 0, 0, 0, 0);

	INSERT INTO dbo.Rooms
			(Room_Name, Building_ID, Domain_ID, Room_Usage_Type_ID, Bookable, Auto_Approve, Setup_Time, Teardown_Time)
			VALUES
			('Uptown Equipment', @Uptown_Building, 1, 1, 0, 0, 0, 0);

	INSERT INTO dbo.Rooms
			(Room_Name, Building_ID, Domain_ID, Room_Usage_Type_ID, Bookable, Auto_Approve, Setup_Time, Teardown_Time)
			VALUES
			('West Side Equipment', @West_Side_Building, 1, 1, 0, 0, 0, 0)

	INSERT INTO dbo.Rooms
			(Room_Name, Building_ID, Domain_ID, Room_Usage_Type_ID, Bookable, Auto_Approve, Setup_Time, Teardown_Time)
			VALUES
			('Uptown Equipment', @Uptown_Building, 1, 1, 0, 0, 0, 0)
