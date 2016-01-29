USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_filter_rooms]    Script Date: 1/26/2016 3:01:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================

ALTER PROCEDURE [dbo].[report_CRDS_filter_rooms]
-- Add the parameters for the stored procedure here
      @DomainID VARCHAR(40), @UserID VARCHAR(40), @PageID INT, @BuildingID INT= NULL, @CongregationID INT= NULL  --mth20160109 added parm is order to filter room selection list based on location

AS
     BEGIN

		-- pull the location ID based on the congregation id
		DECLARE @Location_ID AS INT

		SELECT @Location_ID = loc.location_id 
		FROM Congregations con INNER JOIN Locations loc on con.Location_ID = loc.Location_ID
		WHERE con.Congregation_ID = @CongregationID

		SELECT @BuildingID = building_id
		FROM Buildings 
		WHERE Location_ID = @Location_ID

         SELECT R.Room_Name AS Room_Name  --mth20160109 remove room number from label
         , R.Room_ID
         FROM DBO.Rooms R
              INNER JOIN dp_Domains ON dp_Domains.Domain_ID = R.Domain_ID
              JOIN buildings b ON b.building_id = r.building_id
         WHERE dp_Domains.Domain_GUID = @DomainID
               AND (ISNULL(@BuildingID, 0) = 0
                    OR @BuildingID = r.building_id
                   )  --mth20160109 allow for BuildingID = 0, i.e. All Buildiings
               AND (ISNULL(@Location_ID, 0) = 0
                    OR @Location_ID = b.location_id
                   )  --mth20160109 allow for LocationID = 0, i.e. All Locations

         UNION
         SELECT '*All Rooms' AS Room_Name, 0 AS Room_ID  --mth20160109 changed All value to be 0 rather than null

         ORDER BY Room_Name;
     END;


GO


