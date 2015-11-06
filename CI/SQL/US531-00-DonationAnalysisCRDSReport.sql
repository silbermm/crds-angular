USE [MinistryPlatform]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.report_CRDS_Donation_Analysis', 'p') IS NULL
  EXEC('CREATE PROCEDURE dbo.report_CRDS_Donation_Analysis AS SELECT 1')
GO

ALTER PROCEDURE [dbo].[report_CRDS_Donation_Analysis]

  @DomainID varchar(40)
  ,@UserID varchar(40)
  ,@PageID Int
  ,@FromDate DateTime
  ,@ToDate DateTime
  ,@StmtHeaderID NVarchar(MAX)
  ,@ProgramID NVarchar(MAX)
  ,@MinGiv Money
  ,@ColumnGroup Int --(1=Donation Cong,2=Stmt Header,3=Program,4=Month,5=Quarter,6=Account_Number,7=Participant Type,8=Payment TypeNULL=none)
  ,@TaxDedOnly Bit
  ,@AccountingCompanyID INT
  ,@CongregationID INT = NULL
AS

BEGIN

SET NOCOUNT ON
SET FMTONLY OFF

SELECT D.Donor_ID
,DD.Amount
,D.Donation_Date
,Cong.Congregation_Name
,Prog.Program_Name
,Prog.Account_Number
,SH.Statement_Header
,PT.Payment_Type
INTO #DPrep
FROM Donations D
 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = D.Domain_ID
 INNER JOIN Payment_Types PT ON PT.Payment_Type_ID = D.Payment_Type_ID
 INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
 INNER JOIN Programs Prog ON Prog.Program_ID = DD.Program_ID
 INNER JOIN Congregations Cong ON Cong.Congregation_ID = DD.Congregation_ID
 INNER JOIN Statement_Headers SH ON SH.Statement_Header_ID = Prog.Statement_Header_ID
WHERE Cong.Accounting_Company_ID = ISNULL(@AccountingCompanyID,Cong.Accounting_Company_ID)
 AND (ISNULL(@StmtHeaderID,'0') = '0' OR Prog.Statement_Header_ID IN (SELECT * FROM dp_Split(@StmtHeaderID,',')))
 AND (ISNULL(@ProgramID,'0') = '0' OR Prog.Program_ID IN (SELECT * FROM dp_Split(@ProgramID,',')))
 AND DD.Congregation_ID = ISNULL(@CongregationID,DD.Congregation_ID)
 AND Prog.Program_Type_ID <> 3
 AND @DomainID = Dom.Domain_GUID
 AND D.Batch_ID IS NOT NULL
 AND D.Donation_Date >= @FromDate 
 AND D.Donation_Date < @ToDate+1 
 AND Prog.Tax_Deductible_Donations = ISNULL(@TaxDedOnly,Prog.Tax_Deductible_Donations)

CREATE INDEX IX_DPrep_DonorID ON #DPrep(Donor_ID)

SELECT Do.Donor_ID
, MAX(PT.Participant_Type) AS Participant_Type
INTO #DPersonal
FROM Donors Do
 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
 LEFT OUTER JOIN Participants P ON P.Contact_ID = C.Contact_ID
 LEFT OUTER JOIN Participant_Types PT ON PT.Participant_Type_ID = P.Participant_Type_ID 
WHERE EXISTS (SELECT 1 FROM #DPrep WHERE #DPrep.Donor_ID = Do.Donor_ID)
GROUP BY Do.Donor_ID

CREATE INDEX IX_DPersonal_DonorID ON #DPersonal(Donor_ID)
	
CREATE TABLE #D (Donor_ID INT, First_Donation DateTime, Last_Donation DateTime, Period_Amount Money, Period_Donations INT, Column_Group Varchar(75))
INSERT INTO #D (Donor_ID, First_Donation, Last_Donation, Period_Amount, Period_Donations, Column_Group)
SELECT D.Donor_ID
, First_Donation = Min(D.Donation_Date)
, Last_Donation = Max(D.Donation_Date)
, Period_Amount = SUM(D.Amount)
, Period_Donations = Count(*)
, Column_Group = CASE @ColumnGroup WHEN 1 THEN Congregation_Name WHEN 2 THEN ISNULL(Statement_Header,'Stmt Header Not Set') WHEN 3 THEN [Program_Name] WHEN 4 THEN Convert(Varchar(4), Year(Donation_Date)) + '-' + RIGHT(Convert(Varchar(3), 100+Month(Donation_Date)),2) WHEN 5 THEN Convert(Varchar(4), Year(Donation_Date)) + '-' + RIGHT(Convert(Varchar(3), 100+DatePart(q,Donation_Date)),2) WHEN 6 THEN ISNULL(Account_Number,'*Acct # Missing') WHEN 7 THEN ISNULL(Participant_Type,'*Not a Participant') WHEN 8 THEN Payment_Type ELSE 'Column Group Not Set' END
FROM #DPrep D
 LEFT OUTER JOIN #DPersonal DP ON DP.Donor_ID = D.Donor_ID
GROUP BY D.Donor_ID
, CASE @ColumnGroup WHEN 1 THEN Congregation_Name WHEN 2 THEN ISNULL(Statement_Header,'Stmt Header Not Set') WHEN 3 THEN [Program_Name] WHEN 4 THEN Convert(Varchar(4), Year(Donation_Date)) + '-' + RIGHT(Convert(Varchar(3), 100+Month(Donation_Date)),2) WHEN 5 THEN Convert(Varchar(4), Year(Donation_Date)) + '-' + RIGHT(Convert(Varchar(3), 100+DatePart(q,Donation_Date)),2) WHEN 6 THEN ISNULL(Account_Number,'*Acct # Missing') WHEN 7 THEN ISNULL(Participant_Type,'*Not a Participant') WHEN 8 THEN Payment_Type ELSE 'Column Group Not Set' END

CREATE INDEX IX_D_DonorID ON #D(Donor_ID)

SELECT
Donor_Name = C.Display_Name
,Contact_Status = CS.Contact_Status
,Household_Name = H.Household_Name
,Donation_Distribution_Congregation = ISNULL(Cong.Congregation_Name,'No Donation Distribution Congr.')
,Period_Amount
,Period_Donations
,First_Donation
,Last_Donation
,C.Contact_ID
,Do.Donor_ID
,H.Household_ID
,Column_Group
FROM Donors Do
INNER JOIN #D ON #D.Donor_ID = Do.Donor_ID
INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
INNER JOIN Contact_Statuses CS ON CS.Contact_Status_ID = C.Contact_Status_ID
INNER JOIN Donations D ON D.Donor_ID = Do.Donor_ID
INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
INNER JOIN Congregations Cong ON Cong.Congregation_ID = DD.Congregation_ID
LEFT OUTER JOIN Households H ON H.Household_ID = C.Household_ID
WHERE #D.Donor_ID IN (SELECT Donor_ID FROM #D GROUP BY Donor_ID HAVING SUM(#D.Period_Amount) >= ISNULL(@MinGiv,SUM(#D.Period_Amount)))


END
