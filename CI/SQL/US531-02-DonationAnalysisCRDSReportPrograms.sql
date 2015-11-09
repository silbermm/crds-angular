USE [MinistryPlatform]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.report_CRDS_filter_programs_donations_multi_stmt_header', 'p') IS NULL
  EXEC('CREATE PROCEDURE dbo.report_CRDS_filter_programs_donations_multi_stmt_header AS SELECT 1')
GO

ALTER PROCEDURE [dbo].[report_CRDS_filter_programs_donations_multi_stmt_header]
  @DomainID varchar(40)
  ,@UserID varchar(40)
  ,@PageID Int
  ,@CongregationID Int = NULL
  ,@StmtHeaderID NVarchar(MAX) = '0'
  ,@IsMulti BIT = 0
  ,@AccountingCompanyID INT = NULL
  ,@SortOrder INT = 1 --1 = Program Name, 2 = Statement Title

AS
BEGIN

DECLARE @CurrentProgramFilters BIT
SET @CurrentProgramFilters = ISNULL((SELECT Top 1 1 FROM dp_Configuration_Settings CS INNER JOIN dp_Domains Dom on Dom.Domain_ID = CS.Domain_ID AND Dom.Domain_GUID = @DomainID WHERE CS.Application_Code = 'SSRS' AND Key_Name = 'CurrentProgramFilters' AND Value = 'Yes'),0)

  IF @IsMulti = 1
  BEGIN
    SELECT P.[Program_Name] + ISNULL(' (' + P.Account_Number + ')','') AS [Program_Name], P.Statement_Title, P.Program_ID
  ,CASE WHEN @SortOrder = 1 THEN Program_Name WHEN @SortOrder = 2 THEN Statement_Title ELSE Program_Name END AS Sort_Order
  FROM DBO.Programs P
   INNER JOIN dp_Domains ON dp_Domains.Domain_ID = P.Domain_ID
   INNER JOIN Congregations Cong ON Cong.Congregation_ID = P.Congregation_ID
  WHERE dp_Domains.Domain_GUID = @DomainID
   AND (@CurrentProgramFilters = 0 OR Getdate() BETWEEN P.Start_Date AND ISNULL(P.End_Date, GetDate()))
   AND EXISTS (SELECT 1 FROM Donation_Distributions DD WHERE DD.Program_ID = P.Program_ID)
   AND ISNULL(@CongregationID, P.Congregation_ID) = P.Congregation_ID
   AND (ISNULL(@StmtHeaderID,'0') = '0' OR P.Statement_Header_ID IN (SELECT * FROM dp_Split(@StmtHeaderID,',')))
   AND ISNULL(Cong.Accounting_Company_ID,1) = ISNULL(@AccountingCompanyID,ISNULL(Cong.Accounting_Company_ID,1))
   AND P.Program_Type_ID <> 3
  UNION
  SELECT '* Any Program' AS Program_Name,'* Any Programs' AS Statement_Title, 0 AS Program_ID, '' AS Sort_Order

  ORDER BY Sort_Order

  END

  ELSE

  BEGIN
  SELECT P.[Program_Name] + ISNULL(' (' + P.Account_Number + ')','') AS [Program_Name], P.Statement_Title, P.Program_ID
  ,CASE WHEN @SortOrder = 1 THEN Program_Name WHEN @SortOrder = 2 THEN Statement_Title ELSE Program_Name END AS Sort_Order
  FROM DBO.Programs P
   INNER JOIN dp_Domains ON dp_Domains.Domain_ID = P.Domain_ID
   INNER JOIN Congregations Cong ON Cong.Congregation_ID = P.Congregation_ID
  WHERE dp_Domains.Domain_GUID = @DomainID
   AND (@CurrentProgramFilters = 0 OR Getdate() BETWEEN P.Start_Date AND ISNULL(P.End_Date, GetDate()))
   AND EXISTS (SELECT 1 FROM Donation_Distributions DD WHERE DD.Program_ID = P.Program_ID)
   AND ISNULL(@CongregationID, P.Congregation_ID) = P.Congregation_ID
   AND (ISNULL(@StmtHeaderID,'0') = '0' OR P.Statement_Header_ID IN (SELECT * FROM dp_Split(@StmtHeaderID,',')))
   AND ISNULL(Cong.Accounting_Company_ID,1) = ISNULL(@AccountingCompanyID,ISNULL(Cong.Accounting_Company_ID,1))
   AND P.Program_Type_ID <> 3
  UNION
  SELECT '* All Programs' AS Program_Name,'* All Programs' AS Statement_Title, NULL AS Program_ID, '' AS Sort_Order
  ORDER BY Sort_Order

  END

END
