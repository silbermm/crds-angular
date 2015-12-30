USE [MinistryPlatform]
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
						   @C_Contact = c.Contact_ID
				    FROM [dbo].[Events] e
					JOIN [dbo].[Congregations] con on con.Congregation_ID = e.Congregation_ID
					JOIN [dbo].[Contacts] c on c.Contact_ID = con.Childcare_Contact
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
