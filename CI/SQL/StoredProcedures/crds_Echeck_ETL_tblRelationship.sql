USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[crds_Echeck_ETL_tblRelationship]    Script Date: 12/17/2015 3:43:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_Echeck_ETL_tblRelationship]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[crds_Echeck_ETL_tblRelationship] AS' 
END
GO

-- =============================================
-- Author:      Charlie Retzler
-- Create date: 12/17/2015
-- Description: Runs the ETL (extract/transform/load) process for the echeck tblRelationship table
-- =============================================
ALTER PROCEDURE [dbo].[crds_Echeck_ETL_tblRelationship]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT 
			Related_Contact_ID AS Parent_Contact_ID, 
			Contact_ID AS Child_Contact_ID 
		FROM dbo.vw_crds_Active_Echeck_Contact_Relationships 
		WHERE Relationship_ID = 45
END

GO