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
-- Author:      Charlie Retzler / Dan Rye / Darryl Woods
-- Create date: 12/17/2015
-- Description: Runs the ETL (extract/transform/load) process for the echeck tblPerson table
-- =============================================
--
-- **************************
-- Change History
-- **************************
-- Story    Date	    Author           Description	
-- ------   --------    -------          -----------
-- US1984   01/04/2016  Charlie Retzler  Merged logic for Person_House_ID into main procedure as a CTE
-- DE979    01/20/2016  Chareli Retzler  Fixed issue where null HS_Graduation_Year was not blanked out properly

ALTER PROCEDURE [dbo].[crds_Echeck_ETL_tblPerson]
AS
BEGIN
	SET NOCOUNT ON;

	;
	WITH Children_Household (Contact_ID, Person_House_ID)
	AS 
		(SELECT Contact_ID,
				DENSE_RANK() OVER (ORDER BY Person_House_TX) as Person_House_ID
			FROM 
			(SELECT t1.Contact_ID,
					(SELECT cast(t2.Related_Contact_ID as varchar) + '-'				   
						FROM dbo.vw_crds_Active_Echeck_Contact_Relationships t2
						WHERE 
							t1.Contact_ID = t2.Contact_ID AND
							t2.Relationship_ID = 45
						ORDER BY t2.Related_Contact_ID
						FOR XML PATH('') 
					) AS Person_House_TX 
				FROM dbo.vw_crds_Active_Echeck_Contact_Relationships t1
				WHERE t1.Relationship_ID = 45
				GROUP BY t1.Contact_ID
			) h
		)
	SELECT DISTINCT
			c.Contact_ID,
			c.Last_Name,
			c.First_Name, 
			c.Nickname,
			c.Date_of_Birth,
			COALESCE(CAST(c.HS_Graduation_Year AS VARCHAR), '') AS HS_Graduation_Year,
			COALESCE(ch.Person_House_ID, 0) AS Person_House_ID
		FROM dbo.Contacts c
			INNER JOIN dbo.vw_crds_Active_Echeck_Contact_Relationships cr 
				ON c.Contact_ID = cr.Contact_ID
			LEFT JOIN 
				Children_Household ch
				ON ch.Contact_ID = c.Contact_ID
			    
END

GO