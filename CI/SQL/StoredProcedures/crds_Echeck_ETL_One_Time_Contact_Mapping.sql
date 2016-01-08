USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[crds_Echeck_ETL_One_Time_Contact_Mapping]    Script Date: 12/17/2015 3:43:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_Echeck_ETL_One_Time_Contact_Mapping]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[crds_Echeck_ETL_One_Time_Contact_Mapping] AS' 
END
GO

-- =============================================
-- Author:      Charlie Retzler
-- Create date: 01/05/2016
-- Description: Extract data for one-time go live update to echeck DB
-- =============================================
--
-- **************************
-- Change History
-- **************************
-- Story    Date	    Author   Description	
-- ------   --------    -------  -----------

ALTER PROCEDURE [dbo].[crds_Echeck_ETL_One_Time_Contact_Mapping]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT Contact_ID, __ExternalPersonID  FROM dbo.Contacts WHERE __ExternalPersonID IS NOT NULL
			    
END

GO