USE [MinistryPlatform]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.report_CRDS_filter_statement_headers', 'p') IS NULL
  EXEC('CREATE PROCEDURE dbo.report_CRDS_filter_statement_headers AS SELECT 1')
GO

ALTER PROCEDURE [dbo].[report_CRDS_filter_statement_headers]
  @DomainID varchar(40)
  ,@UserID varchar(40)
  ,@PageID Int
  ,@IsMulti BIT = 0	
AS
BEGIN

--Temporary tables below will enforce a rule that there always exist statement headers with header sort from 1 to 6.
DECLARE @Domain_ID INT = (SELECT Domain_ID FROM dp_Domains WHERE Domain_GUID = @DomainID)

CREATE TABLE #SH (Statement_Header Varchar(50), Statement_Header_ID INT, Header_Sort INT, Domain_ID INT)
INSERT INTO #SH (Statement_Header, Statement_Header_ID, Header_Sort, Domain_ID)
SELECT 'Add Header 1',0,1,@Domain_ID
UNION
SELECT 'Add Header 3',0,3,@Domain_ID
UNION
SELECT 'Add Header 4',0,4,@Domain_ID
UNION
SELECT 'Add Header 5',0,5,@Domain_ID
UNION
SELECT 'Add Header 6',0,6,@Domain_ID
UNION
SELECT Statement_Header, Statement_Header_ID, Header_Sort, Domain_ID
FROM Statement_Headers
WHERE ISNULL(Header_Sort,99) > 6 AND Domain_ID = @Domain_ID

SELECT COALESCE(SH.Statement_Header,#SH.Statement_Header) AS Statement_Header
,COALESCE(SH.Statement_Header_ID,#SH.Statement_Header_ID) AS Statement_Header_ID
,COALESCE(SH.Header_Sort, #SH.Header_Sort) AS Header_Sort
INTO #SH2
FROM #SH
 FULL OUTER JOIN Statement_Headers SH ON #SH.Header_Sort = SH.Header_Sort AND #SH.Domain_ID = SH.Domain_ID 
WHERE #SH.Domain_ID = @Domain_ID

  IF @IsMulti = 1
  BEGIN
  SELECT SH.Statement_Header, SH.Statement_Header_ID, ISNULL(SH.Header_Sort,99) AS Header_Sort
  FROM #SH2 AS SH
  UNION 
  SELECT '*Any Statement Header' AS Statement_Headers, 0 AS Statement_Header_ID, -1 AS Header_Sort
  ORDER BY Header_Sort, Statement_Header
  END
  ELSE
  BEGIN
  SELECT SH.Statement_Header, SH.Statement_Header_ID, ISNULL(SH.Header_Sort,99) AS Header_Sort
  FROM #SH2 AS SH
  UNION 
  SELECT '*All Statement Headers' AS Statement_Headers, NULL AS Statement_Header_ID, -1 AS Header_Sort
  ORDER BY Header_Sort, Statement_Header
  END

END
