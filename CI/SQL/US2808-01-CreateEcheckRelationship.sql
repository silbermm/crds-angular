USE [MinistryPlatform]
GO

/****** Object:  Trigger [dbo].[crds_Create_Echeck_Relationship]    Script Date: 1/11/2016 7:18:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[crds_Create_Echeck_Relationship]'))
DROP TRIGGER [dbo].[crds_Create_Echeck_Relationship]
GO

CREATE TRIGGER [dbo].[crds_Create_Echeck_Relationship]
ON [dbo].[Contacts]
AFTER INSERT, UPDATE
AS
-- =============================================
-- Author:      John Cleaver
-- Create date: 01/10/2016
-- Description: US2808 - Trigger to insert eCheck relationships for household for new / update contacts
-- =============================================
--
-- **************************
-- Change History
-- **************************
-- Story    Date	    Author            Description	
-- ------   --------    ----------------  -----------
-- DE906    01/11/2016  Charlie Retzler   Fixed bugs with duplicate relationships when adding child to multiple parents or vice versa.
-- DE906    01/11/2016  Charlie Retzler   Fixed trigger to handle multiple rows inserted into contacts at same time

BEGIN
	DECLARE @changed_rows AS TABLE (Contact_ID INTEGER, Household_ID INTEGER, Household_Position_ID INTEGER)
	INSERT INTO @changed_rows SELECT Contact_ID, Household_ID, Household_Position_ID FROM INSERTED

	DECLARE @Contact_ID AS INTEGER
	DECLARE @Household_ID AS INTEGER
	DECLARE @Contact_Household_Pos AS INTEGER

	DECLARE changed_rows_cursor CURSOR FOR SELECT Contact_ID, Household_ID, Household_Position_ID FROM @changed_rows
	OPEN changed_rows_cursor
	FETCH NEXT FROM changed_rows_cursor INTO @Contact_ID, @Household_ID, @Contact_Household_Pos

	DECLARE @household_contacts AS TABLE(Contact_ID INT)

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (@Contact_Household_Pos IN (1, 7))
		BEGIN
			-- Parent being added to household, so add child relationships
			INSERT INTO @household_contacts 
				SELECT DISTINCT c.Contact_ID 
					FROM dbo.Contacts c
					LEFT OUTER JOIN dbo.Contact_Relationships cr
						ON
							c.Contact_ID = cr.Contact_ID
							AND cr.Relationship_ID IN (45)
							AND cr.Related_Contact_ID = @Contact_ID
				WHERE
					c.Household_ID = @Household_ID
					AND Household_Position_ID IN (2, 5)
					AND cr.Contact_Relationship_ID IS NULL
		END
		ELSE IF (@Contact_Household_Pos IN (2, 5))
		BEGIN
			-- Child being added to household, so add parent relationships
			INSERT INTO @household_contacts 
				SELECT DISTINCT c.Contact_ID 
					FROM dbo.Contacts c
					LEFT OUTER JOIN dbo.Contact_Relationships cr
						ON
							c.Contact_ID = cr.Contact_ID
							AND cr.Relationship_ID IN (40)
							AND cr.Related_Contact_ID = @Contact_ID
				WHERE
					c.Household_ID = @Household_ID
					AND Household_Position_ID IN (1, 7)
					AND cr.Contact_Relationship_ID IS NULL
		END

		DECLARE @household_contact_id AS INT
		DECLARE contacts_curs CURSOR FOR SELECT Contact_ID FROM @household_contacts
		OPEN contacts_curs
		FETCH NEXT FROM contacts_curs INTO @household_contact_id

		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF (@Contact_Household_Pos IN (1, 7))
			BEGIN
				-- create the adult to child mapping here - 40 is X can checkin Y
				INSERT INTO dbo.Contact_Relationships(Contact_ID, Relationship_ID, Related_Contact_ID, Start_Date, Domain_ID)
				VALUES (@Contact_ID, 40, @household_contact_id, GETDATE(), 1)
			END
			ELSE IF (@Contact_Household_Pos IN (2, 5))
			BEGIN 
				-- create the child to adult mapping here - 45 is X can be checked in by Y
				INSERT INTO dbo.Contact_Relationships(Contact_ID, Relationship_ID, Related_Contact_ID, Start_Date, Domain_ID)
				VALUES (@Contact_ID, 45, @household_contact_id, GETDATE(), 1)
			END

			FETCH NEXT FROM contacts_curs INTO @household_contact_id
		END

		CLOSE contacts_curs
		DEALLOCATE contacts_curs
		
		DELETE FROM @household_contacts

		FETCH NEXT FROM changed_rows_cursor INTO @Contact_ID, @Household_ID, @Contact_Household_Pos
	END

	CLOSE changed_rows_cursor
	DEALLOCATE changed_rows_cursor

END
GO