USE [eCheckIn]
GO

/****** Object:  StoredProcedure [dbo].[crds_Echeck_ETL_Load]    Script Date: 12/17/2015 3:43:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_Echeck_ETL_Load]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[crds_Echeck_ETL_Load] AS' 
END
GO

-- =============================================
-- Author:      Charlie Retzler
-- Create date: 12/17/2015
-- Description: Runs the ETL load process from Ministry Platform to eCheckIn
-- =============================================
ALTER PROCEDURE [dbo].[crds_Echeck_ETL_Load]
AS
BEGIN
	SET NOCOUNT ON;

	TRUNCATE TABLE dbo.tblPerson
	TRUNCATE TABLE dbo.tblRelationship
	TRUNCATE TABLE dbo.tblAdditionalGuardians

	INSERT INTO dbo.tblPerson
		(Person_ID, Person_LName, Person_FName, Person_Nick_Name, Person_Birth_Date, Person_ClassOf, Person_House_ID)	
		SELECT Contact_Id, Last_Name, First_Name, Nickname, Date_of_Birth, HS_Graduation_Year, Person_House_ID 
			FROM OPENQUERY([MinistryPlatformServer], 'EXECUTE MinistryPlatform.dbo.crds_Echeck_ETL_tblPerson')

	INSERT INTO dbo.tblRelationship 
		(Party1ID, Party2ID)
		SELECT Parent_Contact_ID, Child_Contact_ID 
			FROM OPENQUERY([MinistryPlatformServer], 'EXECUTE MinistryPlatform.dbo.crds_Echeck_ETL_tblRelationship')

	INSERT INTO dbo.tblAdditionalGuardians 
		(Child_ID, Guardian_ID)
		SELECT Party2ID, Party1ID 
			FROM echeckIn_Integration.dbo.tblRelationship 		
END

GO