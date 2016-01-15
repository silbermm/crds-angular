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
              WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_filter_buildings]')
                    AND type IN
                                (N'P', N'PC'
                                )
             )
    BEGIN
        EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_filter_buildings] AS';
    END;
GO

-- =============================================

ALTER PROCEDURE [dbo].[report_CRDS_filter_buildings]
-- Add the parameters for the stored procedure here
      @DomainID VARCHAR(40), @UserID VARCHAR(40), @PageID INT, @LocationID INT= NULL
AS
     BEGIN
         SELECT B.Building_Name, B.Building_ID
         FROM DBO.Buildings B
              INNER JOIN dp_Domains ON dp_Domains.Domain_ID = B.Domain_ID
         WHERE dp_Domains.Domain_GUID = @DomainID
               AND (ISNULL(@LocationID, 0) = 0
                    OR @LocationID = B.Location_ID
                   )  --mth20160109 allow for LocationID = 0

         UNION
         SELECT '*All Buildings' AS Building_Name, 0 AS Building_ID  --mth20160109 changed All value to be 0 rather than null
         ORDER BY Building_Name;
     END;
GO