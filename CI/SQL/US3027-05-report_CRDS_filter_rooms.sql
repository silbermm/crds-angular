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
              WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_filter_rooms]')
                    AND type IN
                                (N'P', N'PC'
                                )
             )
    BEGIN
        EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_filter_rooms] AS';
    END;
GO

-- =============================================

ALTER PROCEDURE [dbo].[report_CRDS_filter_rooms]
-- Add the parameters for the stored procedure here
      @DomainID VARCHAR(40), @UserID VARCHAR(40), @PageID INT, @BuildingID INT= NULL, @LocationID INT= NULL  --mth20160109 added parm is order to filter room selection list based on location

AS
     BEGIN
         SELECT R.Room_Name AS Room_Name  --mth20160109 remove room number from label
         , R.Room_ID
         FROM DBO.Rooms R
              INNER JOIN dp_Domains ON dp_Domains.Domain_ID = R.Domain_ID
              JOIN buildings b ON b.building_id = r.building_id
         WHERE dp_Domains.Domain_GUID = @DomainID
               AND (ISNULL(@BuildingID, 0) = 0
                    OR @BuildingID = r.building_id
                   )  --mth20160109 allow for BuildingID = 0, i.e. All Buildiings
               AND (ISNULL(@LocationID, 0) = 0
                    OR @LocationID = b.location_id
                   )  --mth20160109 allow for LocationID = 0, i.e. All Locations

         UNION
         SELECT '*All Rooms' AS Room_Name, 0 AS Room_ID  --mth20160109 changed All value to be 0 rather than null

         ORDER BY Room_Name;
     END;
GO