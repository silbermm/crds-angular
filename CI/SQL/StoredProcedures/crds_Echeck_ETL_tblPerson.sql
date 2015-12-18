USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[crds_Echeck_ETL_tblPerson]    Script Date: 12/17/2015 3:43:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_Echeck_ETL_tblPerson]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[crds_Echeck_ETL_tblPerson] AS' 
END
GO

-- =============================================
-- Author:      Charlie Retzler
-- Create date: 12/17/2015
-- Description: Runs the ETL (extract/transform/load) process for the echeck tblPerson table
-- =============================================
ALTER PROCEDURE [dbo].[crds_Echeck_ETL_tblPerson]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT
			c.Contact_ID,
			c.Last_Name,
			c.First_Name, 
			c.Nickname,
			c.Date_of_Birth,
			c.HS_Graduation_Year
		FROM dbo.Contacts c
			INNER JOIN dbo.vw_crds_Active_Echeck_Contact_Relationships cr 
				ON c.Contact_ID = cr.Contact_ID
			    
END

GO