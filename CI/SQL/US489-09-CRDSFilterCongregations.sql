USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_filter_congregations]    Script Date: 9/1/2015 8:52:41 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/**This is a copy of report_filter_congregations. It functions the same except it does not return a value for "All Congregation Sites"***/
CREATE PROCEDURE [dbo].[report_CRDS_filter_congregations]

	@DomainID varchar(40)
	,@UserID varchar(40)
	,@PageID Int
	,@AccountingCompanyID INT = NULL

AS
BEGIN

	SELECT C.[Congregation_Name], C.Congregation_ID
FROM DBO.Congregations C
 INNER JOIN dp_Domains ON dp_Domains.Domain_ID = C.Domain_ID

WHERE dp_Domains.Domain_GUID = @DomainID
 AND C.Accounting_Company_ID = ISNULL(@AccountingCompanyID,C.Accounting_Company_ID)

ORDER BY C.Congregation_Name

END



GO
