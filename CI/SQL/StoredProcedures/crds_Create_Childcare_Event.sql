USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT *
         FROM   INFORMATION_SCHEMA.COLUMNS
         WHERE  TABLE_NAME = 'Congregations'
                AND COLUMN_NAME = 'Childcare_Contact')
BEGIN
	ALTER Table [dbo].[Congregations] ADD Childcare_Contact int null
END

IF NOT EXISTS (SELECT * 
          FROM sys.foreign_keys 
          WHERE object_id = OBJECT_ID(N'[dbo].[FK_CONGREGATIONS_CHILDCARE_CONTACT]') 
            AND parent_object_id = OBJECT_ID(N'[dbo].[Congregations]'))
BEGIN
	Alter Table [dbo].[Congregations] WITH CHECK ADD CONSTRAINT [FK_CONGREGATIONS_CHILDCARE_CONTACT] 
		FOREIGN KEY(Childcare_Contact) references [dbo].[Contacts](Contact_ID)
END

UPDATE [dbo].[dp_Pages] 
	SET DEFAULT_FIELD_LIST = N' Congregations.Congregation_Name
							   ,Location_ID_Table.Location_Name
							   ,Congregations.Start_Date
								,Contact_ID_Table.Display_Name AS Contact_Person
								,Pastor_Table.Display_Name AS Pastor
								,Congregations.End_Date
								,Childcare_Contact_Table.[Display_Name] AS [Childcare_Contact]'
	WHERE PAGE_ID = 288
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('dbo.crds_Create_Childcare_Event'))
  exec('CREATE PROCEDURE [crds_Create_Childcare_Event] AS BEGIN SET NOCOUNT ON; END')
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Matt Silbernagel
-- Create date: 12/30/2016
-- Description:	Given an event id, create the corresponing childcare
--				event if neccessary. 
--				Returns -1 if there was no event found for the ID given
--						 0 if there was no need to create a childcare event
--						 1 if a childcare event now exists for the event
-- =============================================
ALTER PROCEDURE crds_Create_Childcare_Event 
	-- Add the parameters for the stored procedure here
	@PARENT_EVENT_ID int
AS
BEGIN
	
	DECLARE @CHILDCARE_EVENT_TYPE int = 243;
	DECLARE @VISIBILITY_LEVEL int = 1;

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- Does the inserted event have an associated group with childcare enabled?
	IF EXISTS (
		SELECT * FROM [dbo].[Event_Groups] e
		JOIN [dbo].[Groups] g ON e.Group_ID = g.Group_ID
		WHERE e.EVENT_ID = @PARENT_EVENT_ID and g.Child_Care_Available = 1
	)
		BEGIN
			-- Does a childcare event for this event already exist?
			IF NOT EXISTS (
				SELECT * FROM [dbo].[Events] 
				WHERE Parent_Event_ID = @PARENT_EVENT_ID and Event_Type_ID = @CHILDCARE_EVENT_TYPE
			)
				BEGIN
					DECLARE @P_EVENT_TITLE varchar(255);
					DECLARE @P_CONGREGATION int;
					DECLARE @P_PROGRAM int;
					DECLARE @P_START DateTime;
					DECLARE @P_END DateTime;
					DECLARE @C_CONTACT int = 5;

					SELECT @P_EVENT_TITLE = e.Event_Title, 
						   @P_CONGREGATION = e.Congregation_ID,
						   @P_Program = e.Program_ID,
						   @P_Start = e.Event_Start_Date,
						   @P_End = e.Event_End_Date,
						   @C_Contact = con.Childcare_Contact
				    FROM [dbo].[Events] e
					JOIN [dbo].[Congregations] con on con.Congregation_ID = e.Congregation_ID
					WHERE e.Event_ID = @PARENT_EVENT_ID

					-- Insert new childcare event
					INSERT INTO [dbo].[Events](
						[Domain_ID]
					   ,[Event_Title]
					   ,[Event_Type_ID]
					   ,[Congregation_ID]
					   ,[Program_ID]
					   ,[Primary_Contact]
					   ,[Minutes_for_Setup]
					   ,[Event_Start_Date]
					   ,[Event_End_Date]
					   ,[Minutes_for_Cleanup]
					   ,[Visibility_Level_ID]
					   ,[Parent_Event_ID]
					   ) 
					VALUES(
						 1
						,@P_EVENT_TITLE + N' - Childcare'
						,@CHILDCARE_EVENT_TYPE
						,@P_CONGREGATION
						,@P_PROGRAM
						,@C_Contact
						,0
						,@P_START
						,@P_END
						,0
						,@VISIBILITY_LEVEL
						,@PARENT_EVENT_ID
						)
					RETURN 1;											
				END
			ELSE
				BEGIN					
					RETURN 0;
				END
		END 
	ELSE 
		RETURN -1
END
GO
