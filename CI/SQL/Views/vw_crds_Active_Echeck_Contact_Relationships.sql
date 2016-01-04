USE [MinistryPlatform]
GO
/****** Object:  View [dbo].[vw_crds_Active_Echeck_Contact_Relationships]    Script Date: 12/18/2015 1:09:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  View [dbo].[vw_crds_Active_Echeck_Contact_Relationships]    Script Date: 12/18/2015 1:28:16 PM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_crds_Active_Echeck_Contact_Relationships]'))
DROP VIEW [dbo].[vw_crds_Active_Echeck_Contact_Relationships]
GO

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_crds_Active_Echeck_Contact_Relationships]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_crds_Active_Echeck_Contact_Relationships]
AS
-- =============================================
-- Author:      Charlie Retzler
-- Create date: 12/18/2015
-- Description: View to limit the echeck relationships to active contacts that have end dates in the future 
--              Addressed issue with relationships where end_date is only applied to one of the 2 relationship records
-- =============================================
--
-- **************************
-- Change History
-- **************************
-- Story    Date	    Author            Description	
-- ------   --------    ----------------  -----------
-- US1984   01/04/2016  Charlie Retzler   Fixed bug with c2 joins checking status on c rather than c2

	SELECT
			cr.*
		FROM Contact_Relationships cr			
			INNER JOIN Contact_Relationships cr2
				ON
					cr2.Relationship_ID IN (40, 45) AND
					cr._Triggered_By = cr2.Contact_Relationship_ID 
			-- Only Active Contacts
			INNER JOIN Contacts c ON c.Contact_ID = cr.Contact_ID AND c.Contact_Status_ID = 1
			INNER JOIN Contacts c2 ON c2.Contact_ID = cr.Related_Contact_ID AND c2.Contact_Status_ID = 1
		WHERE
			cr.Relationship_ID IN (40, 45) 
			AND						
			-- The minimum End_Date to use from the linked relationship			
			CASE 
				WHEN CR.End_date IS NULL OR CR2.End_Date IS NULL 
					THEN COALESCE(CR.End_date, CR2.End_Date, GETDATE() + 1)
				WHEN CR.End_Date < CR2.End_Date 
					THEN CR.End_Date 
				ELSE 
					CR2.End_Date 
			END > GETDATE()
	UNION
	SELECT
			cr.*
		FROM Contact_Relationships cr			
			INNER JOIN Contact_Relationships cr2
				ON
					cr2.Relationship_ID IN (40, 45) AND
					cr.Contact_Relationship_Id = cr2._Triggered_By
			-- Only Active Contacts
			INNER JOIN Contacts c ON c.Contact_ID = cr.Contact_ID AND c.Contact_Status_ID = 1
			INNER JOIN Contacts c2 ON c2.Contact_ID = cr.Related_Contact_ID AND c2.Contact_Status_ID = 1
		WHERE
			cr.Relationship_ID IN (40, 45) 
			AND						
			-- The minimum End_Date to use from the linked relationship			
			CASE 
				WHEN CR.End_date IS NULL OR CR2.End_Date IS NULL 
					THEN COALESCE(CR.End_date, CR2.End_Date, GETDATE() + 1)
				WHEN CR.End_Date < CR2.End_Date 
					THEN CR.End_Date 
				ELSE 
					CR2.End_Date 
			END > GETDATE()
' 
GO
