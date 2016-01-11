USE [MinistryPlatform]
GO

CREATE TRIGGER [dbo].[crds_Create_Echeck_Relationship]
ON [dbo].[Contacts]
AFTER INSERT, UPDATE
AS
BEGIN
	DECLARE @changed_rows AS TABLE (Contact_ID INTEGER, Household_ID INTEGER, Household_Position_ID INTEGER)
	INSERT INTO @changed_rows SELECT Contact_ID, Household_ID, Household_Position_ID FROM INSERTED

	DECLARE @Contact_ID AS INTEGER
	DECLARE @Household_ID AS INTEGER
	DECLARE @Contact_Household_Pos AS INTEGER

	DECLARE changed_rows_cursor CURSOR FOR SELECT Contact_ID, Household_ID, Household_Position_ID FROM @changed_rows
	OPEN changed_rows_cursor
	FETCH NEXT FROM changed_rows_cursor INTO @Contact_ID, @Household_ID, @Contact_Household_Pos

	WHILE @@FETCH_STATUS = 0
		BEGIN
			
			DECLARE @household_contacts AS TABLE(Contact_ID INT)

			-- get matching adults or matching children, opposite of whatever the contact's position is
			IF (@Contact_Household_Pos IN (1, 7))
			BEGIN

				-- this will get all contacts in a household besides the contact we're looking at, that are children in that household with no relationship record to the contact
				INSERT INTO @household_contacts SELECT DISTINCT c.Contact_ID FROM Contact_Relationships cr RIGHT OUTER JOIN 
					(SELECT Contact_ID FROM Contacts WHERE Household_ID = @Household_ID AND Contact_ID != @Contact_ID AND Household_Position_ID IN (2, 5)) AS c 
					ON cr.Contact_ID = c.Contact_ID
			END
			ELSE IF (@Contact_Household_Pos IN (2, 5))
			BEGIN
				-- this will get all contacts in a household besides the contact we're looking at, that are adults in that household with no relationship record to the contact
				INSERT INTO @household_contacts SELECT DISTINCT c.Contact_ID FROM Contact_Relationships cr RIGHT OUTER JOIN 
					(SELECT Contact_ID FROM Contacts WHERE Household_ID = @Household_ID AND Contact_ID != @Contact_ID AND Household_Position_ID IN (1, 7)) AS c 
					ON cr.Contact_ID = c.Contact_ID
			END

			DECLARE @household_contact_id AS INT
			DECLARE contacts_curs CURSOR FOR SELECT Contact_ID FROM @household_contacts
			OPEN contacts_curs
			FETCH NEXT FROM contacts_curs INTO @household_contact_id

			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF (@Contact_Household_Pos IN (1, 7))
				BEGIN
					-- create the adult to child mapping here - 45 is e-check child
					INSERT INTO Contact_Relationships(Contact_ID, Relationship_ID, Related_Contact_ID, Start_Date, Domain_ID)
					VALUES (@Contact_ID, 45, @household_contact_id, GETDATE(), 1)

					-- create the child to adult mapping here - 40 is e-check adult is handled by the other trigger
					--INSERT INTO Contact_Relationships(Contact_ID, Relationship_ID, Related_Contact_ID, Start_Date, Domain_ID)
					--VALUES (@household_contact_id, 40, @Contact_ID, GETDATE(), 1)
				END
				ELSE IF (@Contact_Household_Pos IN (2, 5))
				BEGIN 
					-- create the adult to child mapping here - 45 is e-check child
					INSERT INTO Contact_Relationships(Contact_ID, Relationship_ID, Related_Contact_ID, Start_Date, Domain_ID)
					VALUES (@Contact_ID, 40, @household_contact_id, GETDATE(), 1)

					-- create the child to adult mapping here - 40 is e-check adult is handled by the other trigger
					-- INSERT INTO Contact_Relationships(Contact_ID, Relationship_ID, Related_Contact_ID, Start_Date, Domain_ID)
					-- VALUES (@household_contact_id, 45, @Contact_ID, GETDATE(), 1)
				END
			FETCH NEXT FROM contacts_curs INTO @household_contact_id
			END

	FETCH NEXT FROM changed_rows_cursor INTO @Contact_ID, @Household_ID, @Contact_Household_Pos
	END

	CLOSE changed_rows_cursor
	DEALLOCATE changed_rows_cursor

END