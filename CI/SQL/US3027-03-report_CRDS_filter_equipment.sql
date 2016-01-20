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
              WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_filter_equipment]')
                    AND type IN
                                (N'P', N'PC'
                                )
             )
    BEGIN
        EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_filter_equipment] AS';
    END;
GO

-- =============================================

ALTER PROCEDURE [dbo].[report_CRDS_filter_equipment]
-- Add the parameters for the stored procedure here
      @DomainID VARCHAR(40), @UserID VARCHAR(40), @PageID INT
AS
     BEGIN
         SELECT e.equipment_name, e.equipment_id
         FROM DBO.equipment e
              JOIN dp_Domains ON dp_Domains.Domain_ID = e.Domain_ID
         WHERE dp_Domains.Domain_GUID = @DomainID
         UNION
         SELECT '*All Equipment' AS equipment_name, 0 AS equipment_id
         ORDER BY equipment_name;
     END;
GO