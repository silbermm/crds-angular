USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[crds_Enable_Wait_List]    Script Date: 2/17/2015 3:36:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dan Rye
-- Create date: 02/17/2015
-- Description: Identify groups needing wait list and create needed sub groups
-- =============================================
ALTER PROCEDURE [dbo].[crds_Enable_Wait_List] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
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
	FROM [dbo].[Groups]
	WHERE Enable_Waiting_List = 1 AND NOT EXISTS (SELECT 1 FROM Groups WHERE Parent_Group = [dbo].[Groups].Group_ID AND Group_Type_ID=20)
END
