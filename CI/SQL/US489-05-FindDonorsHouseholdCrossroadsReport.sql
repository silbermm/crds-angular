USE [MinistryPlatform]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Donor_Search_Households]') AND type in (N'P'))
DROP PROCEDURE [dbo].[report_CRDS_Donor_Search_Households]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Donor_Search_Households]    Script Date: 8/31/2015 10:14:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[report_CRDS_Donor_Search_Households]

	@DomainID varchar(40)
	,@UserID varchar(40)
	,@PageID Int
	,@FromDate DateTime
	,@ToDate DateTime
	,@CongregationID INT = NULL
	,@StmtHeaderID NVarchar(MAX) = '0'
	,@ProgramID NVarchar(MAX) = '0'
	,@MinGiv Money
	,@MaxGiv Money
	,@FirstTimeGiver INT = 0--0 all givers, 1 1st individual gift, 2 first family gift, 3 first ever gift
	,@CreateSelection BIT = 0
	,@AccountingCompanyID INT = NULL
	,@MinGifts INT = NULL
	,@LastHHDonationBefore DateTime = NULL
	,@HeadsOnly bit = 0

AS
	BEGIN

SET NOCOUNT ON
SET FMTONLY OFF

CREATE TABLE #D (First_Donation DateTime, Last_Donation DateTime, Period_Amount Money, Period_Distributions INT, Period_Donations INT, Household_ID INT)
INSERT INTO #D (First_Donation, Last_Donation, Period_Amount, Period_Distributions, Period_Donations, Household_ID)
SELECT First_Donation = Min(D.Donation_Date)
, Last_Donation = Max(D.Donation_Date)
, Period_Amount = SUM(DD.Amount)
, Period_Distributions = Count(*)
, Period_Donations = Count(DISTINCT D.Donation_ID)
, C.Household_ID
FROM Donations D
 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = D.Domain_ID
 INNER JOIN Donors Do ON Do.Donor_ID = D.Donor_ID
 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID AND C.Household_ID IS NOT NULL
 INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
 INNER JOIN Programs Prog ON Prog.Program_ID = DD.Program_ID
 INNER JOIN Households H ON H.Household_ID = C.Household_ID
 INNER JOIN Congregations Cong ON Cong.Congregation_ID = H.Congregation_ID
 LEFT OUTER JOIN Statement_Headers SH ON SH.Statement_Header_ID = Prog.Statement_Header_ID
WHERE (ISNULL(@StmtHeaderID,'0') = '0' OR Prog.Statement_Header_ID IN (SELECT * FROM dp_Split(@StmtHeaderID,',')))
 AND (ISNULL(@ProgramID,'0') = '0' OR Prog.Program_ID IN (SELECT * FROM dp_Split(@ProgramID,',')))
 AND Cong.Accounting_Company_ID = ISNULL(@AccountingCompanyID, Cong.Accounting_Company_ID)
 AND Cong.Congregation_ID = ISNULL(@CongregationID, Cong.Congregation_ID)
 AND @DomainID = Dom.Domain_GUID
 AND D.Batch_ID IS NOT NULL
 AND D.Donation_Date >= @FromDate
 AND D.Donation_Date < @ToDate+1
 --AND ((@HeadsOnly = 1 and C.Household_Position_ID = 1) OR (@HeadsOnly = 0))
GROUP BY C.Household_ID

CREATE INDEX IX_D_Household_ID ON #D(Household_ID)

CREATE TABLE #FirstEverThese (Household_ID INT)
INSERT INTO #FirstEverThese (Household_ID)
SELECT C.Household_ID
FROM Donations D
 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = D.Domain_ID
 INNER JOIN Donors Do ON Do.Donor_ID = D.Donor_ID
 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID AND C.Household_ID IS NOT NULL
 INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
 INNER JOIN Programs Prog ON Prog.Program_ID = DD.Program_ID
 INNER JOIN Households H ON H.Household_ID = C.Household_ID
 INNER JOIN Congregations Cong ON Cong.Congregation_ID = H.Congregation_ID
 LEFT OUTER JOIN Statement_Headers SH ON SH.Statement_Header_ID = Prog.Statement_Header_ID
WHERE (ISNULL(@StmtHeaderID,'0') = '0' OR Prog.Statement_Header_ID IN (SELECT * FROM dp_Split(@StmtHeaderID,',')))
 AND (ISNULL(@ProgramID,'0') = '0' OR Prog.Program_ID IN (SELECT * FROM dp_Split(@ProgramID,',')))
 AND Cong.Accounting_Company_ID = ISNULL(@AccountingCompanyID, Cong.Accounting_Company_ID)
 AND H.Congregation_ID = ISNULL(@CongregationID, H.Congregation_ID)
 AND @DomainID = Dom.Domain_GUID
 AND D.Batch_ID IS NOT NULL
 --AND ((@HeadsOnly = 1 and C.Household_Position_ID = 1) OR (@HeadsOnly = 0))
GROUP BY C.Household_ID
HAVING MIN(D.Donation_Date) >= @FromDate
 AND MIN(D.Donation_Date) < @ToDate+1

CREATE INDEX IX_FirstEverThese_Household_ID ON #FirstEverThese(Household_ID)

CREATE TABLE #FirstEverAny (Household_ID INT)
INSERT INTO #FirstEverAny (Household_ID)
SELECT C.Household_ID
FROM Donations D
 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = D.Domain_ID
 INNER JOIN Donors Do ON Do.Donor_ID = D.Donor_ID
 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID AND C.Household_ID IS NOT NULL
 INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
 INNER JOIN Programs Prog ON Prog.Program_ID = DD.Program_ID
 INNER JOIN Households H ON H.Household_ID = C.Household_ID
 INNER JOIN Congregations Cong ON Cong.Congregation_ID = H.Congregation_ID
 LEFT OUTER JOIN Statement_Headers SH ON SH.Statement_Header_ID = Prog.Statement_Header_ID
WHERE @DomainID = Dom.Domain_GUID
 AND D.Batch_ID IS NOT NULL
 --AND ((@HeadsOnly = 1 and C.Household_Position_ID = 1) OR (@HeadsOnly = 0))
GROUP BY C.Household_ID
HAVING MIN(D.Donation_Date) >= @FromDate
 AND MIN(D.Donation_Date) < @ToDate+1

CREATE INDEX IX_FirstEverAny_Household_ID ON #FirstEverAny(Household_ID)

IF @LastHHDonationBefore IS NOT NULL
BEGIN
DELETE FROM #D WHERE EXISTS (SELECT 1 FROM Donations D INNER JOIN Donors Do ON D.Donor_ID = Do.Donor_ID INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID WHERE #D.Household_ID = C.Household_ID AND D.Donation_Date >= @LastHHDonationBefore)
END

SELECT Household_ID, Household_Position_ID, Display_Name, Company, COALESCE(Nickname, First_Name, Display_Name) AS Nickname, Gender_ID, Position = Row_Number() OVER (Partition by Household_ID ORDER BY Gender_ID)
INTO #HH
FROM Contacts C
WHERE EXISTS (SELECT 1 FROM #D WHERE #D.Household_ID = C.Household_ID)
 AND C.Household_Position_ID IN (1)

CREATE INDEX IX_TempHH_Household_ID ON #HH(Household_ID)

SELECT #D.Household_ID, Head_Names = ISNULL(H1.Nickname,'') + ISNULL(' & ' + H2.Nickname,'')
INTO #HHNames
FROM #D
 INNER JOIN #HH H1 ON H1.Household_ID = #D.Household_ID AND H1.Position = 1
 LEFT OUTER JOIN #HH H2 ON H2.Household_ID = #D.Household_ID AND H2.Position = 2 AND H2.Household_Position_ID = 1


CREATE INDEX IX_TempHHNames_HouseholdID ON #HHNames(Household_ID)

SELECT Household_Name = H.Household_Name + ISNULL(', ' + #HHNames.Head_Names, '')
,Household_Congregation = ISNULL(Cong.Congregation_Name, 'No Household Cong found')
,Period_Amount
,Period_Donations
,First_Donation
,Last_Donation
,H.Household_ID
,H.Home_Phone
,A.Address_Line_1
,A.Address_Line_2
,A.City
,A.[State/Region] AS [State]
,A.Postal_Code
FROM #D
 INNER JOIN Households H ON H.Household_ID = #D.Household_ID
 LEFT OUTER JOIN #HHNames ON #HHNames.Household_ID = #D.Household_ID
 LEFT OUTER JOIN Addresses A ON A.Address_ID = H.Address_ID
 LEFT OUTER JOIN Congregations Cong ON Cong.Congregation_ID = H.Congregation_ID
WHERE #D.Period_Amount >= ISNULL(@MinGiv,#D.Period_Amount)
 AND #D.Period_Amount <= ISNULL(@MaxGiv, #D.Period_Amount)
 AND #D.Period_Donations >= ISNULL(@MinGifts,#D.Period_Donations)
 AND (@FirstTimeGiver <= 1
		OR (@FirstTimeGiver = 2 AND #D.Household_ID IN (SELECT Household_ID FROM #FirstEverThese))
		OR (@FirstTimeGiver = 3 AND #D.Household_ID IN (SELECT Household_ID FROM #FirstEverAny))
	)



	IF @CreateSelection > 0
	BEGIN
		CREATE TABLE #D1 (Contact_ID INT, Donor_ID INT, Display_Name Varchar(75))
		INSERT INTO #D1 (Contact_ID, Donor_ID, Display_Name)
		SELECT Contact_ID, Donor_Record, Display_Name
		FROM #D
		 INNER JOIN Contacts C ON #D.Household_ID = C.Household_ID
		WHERE C.Donor_Record IS NOT NULL
		 AND #D.Period_Amount >= ISNULL(@MinGiv,#D.Period_Amount)
		 AND #D.Period_Amount <= ISNULL(@MaxGiv, #D.Period_Amount)
		 AND #D.Period_Donations >= ISNULL(@MinGifts,#D.Period_Donations)
		 AND (@HeadsOnly = 0 OR (@HeadsOnly = 1 AND C.Household_Position_ID IN (1,6)))
		 AND (@FirstTimeGiver <= 1
				OR (@FirstTimeGiver = 2 AND #D.Household_ID IN (SELECT Household_ID FROM #FirstEverThese))
				OR (@FirstTimeGiver = 3 AND #D.Household_ID IN (SELECT Household_ID FROM #FirstEverAny))
			)

		DECLARE @SelectionID2 int, @ParticipantsPageID INT
		SET @ParticipantsPageID = (SELECT Top 1 Page_ID FROM dp_Pages WHERE Table_Name LIKE 'Donors')
		SET @SelectionID2 = 0
		DECLARE @UserID2 INT
		SELECT @UserID2 = [User_ID] FROM dp_Users WHERE User_GUID = @UserID
		SET @SelectionID2 = ISNULL((SELECT Top 1 Selection_ID FROM dp_Selections
			WHERE Selection_Name = 'Donor HH Search Results'
			AND [User_ID] = @UserID2
			AND Page_ID = @ParticipantsPageID),0)

		DELETE FROM dp_Selected_Records WHERE Selection_ID = @SelectionID2
		DELETE FROM dp_Selected_Contacts WHERE Selection_ID = @SelectionID2

			IF @SelectionID2 = 0

			BEGIN

					  INSERT INTO dp_Selections
					(Selection_Name,Page_ID,User_ID)

					VALUES
					 ('Donor HH Search Results', @ParticipantsPageID, @UserID2)

					  SET @SelectionID2 = SCOPE_IDENTITY()

			END

		INSERT INTO dp_Selected_Records (Record_ID, Record_Description, Selection_ID, Delete_Record_ID)
		SELECT DISTINCT Donor_ID, Display_Name, @SelectionID2, Donor_ID
		FROM #D1

	END


END

GO
