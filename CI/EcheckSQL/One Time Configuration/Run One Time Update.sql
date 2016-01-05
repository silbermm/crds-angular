USE eCheckIN
GO

CREATE TABLE #ContactMapping
(
    Contact_ID			INT,
    ExternalPersonID	INT
)
    
INSERT INTO #ContactMapping
	(
		Contact_ID,
		ExternalPersonID
	)   
	SELECT Contact_ID, __ExternalPersonID 
		FROM OPENQUERY([MinistryPlatformServer], 'EXECUTE MinistryPlatform.dbo.crds_Echeck_ETL_One_Time_Contact_Mapping')
		    
CREATE CLUSTERED INDEX Contact_ID_PK ON #ContactMapping(Contact_ID)

-- Update Archived_Registrations
UPDATE kc
	SET 
		-- TODO: Confirm with Markku Person_ID if this is ID
		Child_ID = mp.Contact_ID
	FROM
		dbo.tblKidsClub_Archived_Registrations kc
			INNER JOIN #ContactMapping mp 
			-- TODO: Confirm with Markku Person_ID if this is ID
				ON mp.ExternalPersonID = kc.Child_ID


-- Update TurnedAway_Registrations
UPDATE kc
	SET 
		-- TODO: Confirm with Markku Person_ID if this is ID
		Child_ID = mp.Contact_ID
	FROM
		dbo.tblKidsClub_TurnedAway_Registrations kc
			INNER JOIN #ContactMapping mp 
			-- TODO: Confirm with Markku Person_ID if this is ID
				ON mp.ExternalPersonID = kc.Child_ID
