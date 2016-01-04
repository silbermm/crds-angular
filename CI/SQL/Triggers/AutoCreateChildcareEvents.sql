USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[crds_tr_create_childcare_events]'))
BEGIN
	exec('CREATE Trigger [crds_tr_create_childcare_events] ON [Event_Groups] AFTER INSERT AS BEGIN SET NOCOUNT ON; END')
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Matt Silbernagel
-- Create date: 12/30/2015
-- Description:	When an event is inserted also create childcare events if needed
-- =============================================
ALTER TRIGGER [dbo].[crds_tr_create_childcare_events] 
   ON  [dbo].[Event_Groups]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @INSERTED_EVENT int;

    -- Get ID of inserted event for future use
	SET @INSERTED_EVENT = (SELECT Event_ID from inserted);
	
	-- Call stored procedure
	Exec [dbo].[crds_Create_Childcare_Event] @INSERTED_EVENT;

END
GO
