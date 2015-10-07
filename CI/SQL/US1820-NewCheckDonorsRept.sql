USE [MinistryPlatform]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_New_Check_Donors]') AND type in (N'P'))
DROP PROCEDURE [dbo].[report_CRDS_New_Check_Donors]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_New_Check_Donors]    Script Date: 9/2/2015 10:45:43 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Description:	New report needed to find new donors that were added as a result of processing a check
-- =============================================
CREATE PROCEDURE [dbo].[report_CRDS_New_Check_Donors]

	 @UserID varchar(40)
	,@PageID Int
	,@StartDate DateTime
	,@EndDate DateTime
	,@CongregationID INT = NULL
	,@StmtHeaderID NVarchar(MAX) = '0'
	,@ProgramID NVarchar(MAX) = '0'
	,@AccountingCompanyID INT = NULL

AS
BEGIN

Select  Donation_Date, D.Payment_Type_ID, C.Display_Name, A.Address_Line_1, A.Address_Line_2, A.City, A.[State/Region] AS [State], A.Postal_Code, Donation_Amount
	FROM Donations D
		LEFT JOIN Donors Do ON D.Donor_ID = Do.Donor_ID
		LEFT JOIN Donation_Distributions Dd ON Dd.Donation_ID = D.Donation_ID
		LEFT JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
		LEFT JOIN Households H ON H.Household_ID = C.Household_ID
		LEFT JOIN Addresses A ON A.Address_ID = H.Address_ID
		LEFT JOIN dp_Audit_Log Al ON Al.Record_ID = Do.Donor_ID
		LEFT JOIN Programs P ON P.Program_ID = DD.Program_ID
		LEFT JOIN Congregations Cong ON Cong.Congregation_ID = DD.Congregation_ID
		LEFT OUTER JOIN Statement_Headers SH ON SH.Statement_Header_ID = P.Statement_Header_ID
	 WHERE
		(ISNULL(@StmtHeaderID,'0') = '0' OR P.Statement_Header_ID IN (SELECT * FROM dp_Split(@StmtHeaderID,','))) AND
		(ISNULL(@ProgramID,'0') = '0' OR P.Program_ID IN (SELECT * FROM dp_Split(@ProgramID,','))) AND
		Cong.Accounting_Company_ID = ISNULL(@AccountingCompanyID, Cong.Accounting_Company_ID) AND
		DD.Congregation_ID = ISNULL(@CongregationID, DD.Congregation_ID) AND
		Al.Date_Time >= @StartDate AND
		Al.Date_Time <= @EndDate AND
		D.Payment_Type_ID = '1' AND
		Al.Table_Name = 'Donors' AND
		Al.Audit_Description = 'Created'

		ORDER BY D.Donation_Date Desc, D.Donation_Amount Desc
END

GO


SET IDENTITY_INSERT [dbo].[dp_Reports] ON

INSERT INTO [dbo].[dp_Reports]
           ([Report_ID]
           ,[Report_Name]
           ,[Description]
           ,[Report_Path]
           ,[Pass_Selected_Records]
           ,[Pass_LinkTo_Records]
           ,[On_Reports_Tab])
     VALUES
           (254
           ,'CRDS Find New Check Donors'
           ,'Crossroads specific report that will locate new donors created form the check scanning/importing process.'
           ,'/MPReports/Find New Check Donors CRDS'
           ,0
           ,0
           ,1)
GO

SET IDENTITY_INSERT [dbo].[dp_Reports] OFF
GO


SET IDENTITY_INSERT [dbo].[dp_Report_Pages] ON
GO

INSERT INTO [dbo].[dp_Report_Pages]
           ([Report_Page_ID]
		       ,[Report_ID]
           ,[Page_ID])
     VALUES
           (1603
		       ,254
           ,299)
GO

SET IDENTITY_INSERT [dbo].[dp_Report_Pages] OFF
GO



INSERT INTO [dbo].[dp_Role_Reports]
           ([Role_ID]
           ,[Report_ID]
           ,[Domain_ID])
     VALUES
           (2
           ,254
           ,1)
GO
