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
-- Author:      Charlie Retzler / Dan Rye / Darryl Woods
-- Create date: 12/17/2015
-- Description: Runs the ETL (extract/transform/load) process for the echeck tblRelationship table
-- =============================================
ALTER PROCEDURE [dbo].[crds_Echeck_ETL_tblRelationship]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Contact_ID,
	       Person_House_TX,
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
		) c		    
END

GO