USE [MinistryPlatform];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
IF NOT EXISTS
             (
              SELECT *
              FROM sys.objects
              WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_filter_locations]')
                    AND type IN
                                (N'P', N'PC'
                                )
             )
    BEGIN
        EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_filter_locations] AS';
    END;
GO

-- =============================================

ALTER PROCEDURE [dbo].[report_CRDS_filter_locations]
-- Add the parameters for the stored procedure here
      @DomainID VARCHAR(40), @UserID VARCHAR(40), @PageID INT, @CongregationID INT= NULL  --mth20160109 add parm to limit dropdown when specified

AS
     BEGIN
         SELECT l.Location_Name, l.Location_ID
         FROM DBO.Locations l
              INNER JOIN dp_Domains ON dp_Domains.Domain_ID = l.Domain_ID
         WHERE dp_Domains.Domain_GUID = @DomainID
               AND (ISNULL(@CongregationID, 0) = 0
                    OR EXISTS
                             (
                              SELECT 1
                              FROM dbo.congregations c
                              WHERE c.location_id = l.location_id
                                    AND c.congregation_id = @CongregationID
                             )
                   )  --mth20160109 include Location only if it is a location for the congregation

         UNION
         SELECT '*All Locations' AS Location_Name, 0 AS Location_ID  --mth20160109 changed All value to be 0 rather than null

         ORDER BY Location_Name;
     END;
GO