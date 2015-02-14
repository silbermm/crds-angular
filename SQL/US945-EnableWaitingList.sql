USE [MinistryPlatform]
GO
/****** Object:  Trigger [dbo].[tr_Enable_Group_Waiting_List]    Script Date: 2/13/2015 11:26:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dan Rye
-- Create date: 02/13/2015
-- Description:	Create sub group for waiting list when enabled
-- =============================================
ALTER TRIGGER [dbo].[tr_Enable_Group_Waiting_List] 
   ON  [dbo].[Groups] 
   AFTER INSERT,UPDATE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO Groups
	(
		Group_Name,
		Group_Type_ID,
		Ministry_ID,
		Congregation_ID,
		Primary_Contact,
		Description,
		Start_Date,
		End_Date,
		Target_Size,
		Parent_Group,
		Domain_ID
	)
	SELECT Group_Name+'-Waiting List',
		20,
		Ministry_ID,
		Congregation_ID,
		Primary_Contact,
		'Waiting list for: '+Description,
		Start_Date,
		End_Date,
		0,
		Group_ID,
		Domain_ID
	FROM INSERTED
	WHERE Has_Waiting_List = 1 AND
		NOT EXISTS (SELECT 1 FROM Groups WHERE Parent_Group = INSERTED.Group_ID AND Group_Type_ID=20)
END
