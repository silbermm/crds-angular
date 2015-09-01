USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Donor_Search]    Script Date: 9/1/2015 9:21:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[report_CRDS_Donor_Search]

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
	,@FirstTimeGiver INT --0 all givers, 1 1st individual gift, 2 first family gift, 3 first ever gift
	,@CreateSelection BIT = 0
	,@AccountingCompanyID INT = NULL
	,@MinGifts INT = NULL
	,@LastHHDonationBefore DateTime = NULL

AS
	BEGIN

SET NOCOUNT ON
SET FMTONLY OFF


CREATE TABLE #D1 (Donor_ID INT)

	IF @FirstTimeGiver <= 1
	BEGIN
	INSERT INTO #D1 (Donor_ID)
	SELECT D.Donor_ID
	FROM Donations D
	 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = D.Domain_ID
	 INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
	 INNER JOIN Programs Prog ON Prog.Program_ID = DD.Program_ID
	 INNER JOIN Congregations Cong ON Cong.Congregation_ID = DD.Congregation_ID
	 LEFT OUTER JOIN Statement_Headers SH ON SH.Statement_Header_ID = Prog.Statement_Header_ID
	WHERE @DomainID = Dom.Domain_GUID
	 AND (ISNULL(@StmtHeaderID,'0') = '0' OR Prog.Statement_Header_ID IN (SELECT * FROM dp_Split(@StmtHeaderID,',')))
	 AND (ISNULL(@ProgramID,'0') = '0' OR Prog.Program_ID IN (SELECT * FROM dp_Split(@ProgramID,',')))
	 AND Cong.Accounting_Company_ID = ISNULL(@AccountingCompanyID, Cong.Accounting_Company_ID)
	 AND DD.Congregation_ID = ISNULL(@CongregationID, DD.Congregation_ID)
	 AND D.Batch_ID IS NOT NULL
	GROUP BY D.Donor_ID
	HAVING   MIN(D.Donation_Date) >= @FromDate
	 AND MIN(D.Donation_Date) < @ToDate+1
	END

	IF @FirstTimeGiver = 2
	BEGIN
	INSERT INTO #D1 (Donor_ID)
	SELECT Donor_Record
	FROM Contacts
	WHERE Donor_Record IS NOT NULL AND Household_ID IN
										(SELECT ISNULL(Household_ID,0)
										FROM Donations D
										 INNER JOIN Donors Do ON Do.Donor_ID = D.Donor_ID
										 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
										 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = D.Domain_ID
										 INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
										 INNER JOIN Programs Prog ON Prog.Program_ID = DD.Program_ID
										 INNER JOIN Congregations Cong ON Cong.Congregation_ID = DD.Congregation_ID
										 LEFT OUTER JOIN Statement_Headers SH ON SH.Statement_Header_ID = Prog.Statement_Header_ID
										WHERE @DomainID = Dom.Domain_GUID
										 AND (ISNULL(@StmtHeaderID,'0') = '0' OR Prog.Statement_Header_ID IN (SELECT * FROM dp_Split(@StmtHeaderID,',')))
										 AND (ISNULL(@ProgramID,'0') = '0' OR Prog.Program_ID IN (SELECT * FROM dp_Split(@ProgramID,',')))
										 AND Cong.Accounting_Company_ID = ISNULL(@AccountingCompanyID, Cong.Accounting_Company_ID)
										 AND DD.Congregation_ID = ISNULL(@CongregationID, DD.Congregation_ID)
										 AND D.Batch_ID IS NOT NULL
										GROUP BY ISNULL(Household_ID,0)
										HAVING   MIN(D.Donation_Date) >= @FromDate
										 AND MIN(D.Donation_Date) < @ToDate+1
										)
	END


	IF @FirstTimeGiver = 3
	BEGIN
	INSERT INTO #D1 (Donor_ID)
	SELECT Donor_Record
	FROM Contacts
	WHERE Donor_Record IS NOT NULL AND Household_ID IN
										(SELECT ISNULL(Household_ID,0)
										FROM Donations D
										 INNER JOIN Donors Do ON Do.Donor_ID = D.Donor_ID
										 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
										 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = D.Domain_ID
										 INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
										 INNER JOIN Programs Prog ON Prog.Program_ID = DD.Program_ID
										 INNER JOIN Congregations Cong ON Cong.Congregation_ID = DD.Congregation_ID
										 LEFT OUTER JOIN Statement_Headers SH ON SH.Statement_Header_ID = Prog.Statement_Header_ID
										WHERE  @DomainID = Dom.Domain_GUID
										 AND D.Batch_ID IS NOT NULL
										GROUP BY ISNULL(Household_ID,0)
										HAVING   MIN(D.Donation_Date) >= @FromDate
										 AND MIN(D.Donation_Date) < @ToDate+1
										)
	END

CREATE INDEX IX_D1_Donor_ID ON #D1(Donor_ID)

CREATE TABLE #D (Donor_ID INT, Contact_ID INT, Display_Name Varchar(75), First_Donation DateTime, Last_Donation DateTime, Period_Amount Money, Period_Distributions INT, Period_Donations INT, Household_ID INT)
INSERT INTO #D (Donor_ID, Contact_ID, Display_Name, First_Donation, Last_Donation, Period_Amount, Period_Distributions, Period_Donations, Household_ID)
SELECT D.Donor_ID
, Do.Contact_ID
, C.Display_Name
, First_Donation = Min(D.Donation_Date)
, Last_Donation = Max(D.Donation_Date)
, Period_Amount = SUM(DD.Amount)
, Period_Distributions = Count(*)
, Period_Donations = Count(DISTINCT D.Donation_ID)
, C.Household_ID
FROM Donations D
 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = D.Domain_ID
 INNER JOIN Donors Do ON Do.Donor_ID = D.Donor_ID
 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
 INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
 INNER JOIN Programs Prog ON Prog.Program_ID = DD.Program_ID
 INNER JOIN Congregations Cong ON Cong.Congregation_ID = DD.Congregation_ID
 LEFT OUTER JOIN Statement_Headers SH ON SH.Statement_Header_ID = Prog.Statement_Header_ID
WHERE (ISNULL(@StmtHeaderID,'0') = '0' OR Prog.Statement_Header_ID IN (SELECT * FROM dp_Split(@StmtHeaderID,',')))
 AND (ISNULL(@ProgramID,'0') = '0' OR Prog.Program_ID IN (SELECT * FROM dp_Split(@ProgramID,',')))
 AND Cong.Accounting_Company_ID = ISNULL(@AccountingCompanyID, Cong.Accounting_Company_ID)
 AND DD.Congregation_ID = ISNULL(@CongregationID, DD.Congregation_ID)
 AND @DomainID = Dom.Domain_GUID
 AND D.Batch_ID IS NOT NULL
 AND D.Donation_Date >= @FromDate
 AND D.Donation_Date < @ToDate+1

GROUP BY D.Donor_ID, Do.Contact_ID, C.Display_Name, C.Household_ID

CREATE INDEX IX_D_Donor_ID ON #D(Donor_ID)
CREATE INDEX IX_D_Household_ID ON #D(Household_ID)

IF @LastHHDonationBefore IS NOT NULL
BEGIN
DELETE FROM #D WHERE EXISTS (SELECT 1 FROM Donations D WHERE D.Donor_ID = #D.Donor_ID AND D.Donation_Date >= @LastHHDonationBefore)
DELETE FROM #D WHERE EXISTS (SELECT 1 FROM Donations D INNER JOIN Donors Do ON D.Donor_ID = Do.Donor_ID INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID WHERE #D.Household_ID = C.Household_ID AND D.Donation_Date >= @LastHHDonationBefore)
END

SELECT
Donor_Name = C.Display_Name
,Contact_Status = CS.Contact_Status
,Household_Name = H.Household_Name
,Household_Congregation = ISNULL(Cong.Congregation_Name,'No Household Congr.')
,Period_Amount
,Period_Donations
,First_Donation
,Last_Donation
,C.Contact_ID
,Do.Donor_ID
,H.Household_ID
, C.First_Name
, C.Nickname
, C.Last_Name
, C.Email_Address
, C.Mobile_Phone
, H.Home_Phone
, A.Address_Line_1
, A.Address_Line_2
, A.City
, A.[State/Region] AS [State]
, A.Postal_Code
, Spouse.Spouse_First
, Spouse.Spouse_Nickname
, Spouse.Spouse_Last
, C.__Age AS Age
, Spouse.Spouse_Age
, Participant_Type
,Do.Envelope_No

FROM Donors Do
 INNER JOIN #D ON #D.Donor_ID = Do.Donor_ID
 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
 INNER JOIN Contact_Statuses CS ON CS.Contact_Status_ID = C.Contact_Status_ID
 LEFT OUTER JOIN Participants Part ON Part.Contact_Id = C.Contact_ID
 LEFT OUTER JOIN Participant_Types PT ON PT.Participant_Type_ID = Part.Participant_Type_ID
 LEFT OUTER JOIN Households H ON H.Household_ID = C.Household_ID
 LEFT OUTER JOIN Addresses A ON A.Address_ID = H.Address_ID
 LEFT OUTER JOIN Congregations Cong ON Cong.Congregation_ID = ISNULL(@CongregationID, H.Congregation_ID)
 OUTER APPLY (SELECT Top 1 First_Name AS Spouse_First,Nickname AS Spouse_Nickname, Last_Name AS Spouse_Last, S.__Age AS Spouse_Age FROM Contacts S WHERE S.Household_Position_ID = 1 AND C.Household_Position_ID = 1 AND S.Household_ID = C.Household_ID AND S.Contact_ID <> C.Contact_ID) Spouse
WHERE #D.Period_Amount >= ISNULL(@MinGiv,#D.Period_Amount)
 AND #D.Period_Amount <= ISNULL(@MaxGiv, #D.Period_Amount)
 AND #D.Period_Donations >= ISNULL(@MinGifts,#D.Period_Donations)
 AND (@FirstTimeGiver = 0 OR (@FirstTimeGiver >= 1 AND Do.Donor_ID IN (SELECT Donor_ID FROM #D1)))

	IF @CreateSelection > 0
	BEGIN
		DECLARE @SelectionID2 int, @ParticipantsPageID INT
		SET @ParticipantsPageID = (SELECT Top 1 Page_ID FROM dp_Pages WHERE Table_Name LIKE 'Donors')
		SET @SelectionID2 = 0
		DECLARE @UserID2 INT
		SELECT @UserID2 = [User_ID] FROM dp_Users WHERE User_GUID = @UserID
		SET @SelectionID2 = ISNULL((SELECT Top 1 Selection_ID FROM dp_Selections
			WHERE Selection_Name = 'Donor Search Results'
			AND [User_ID] = @UserID2
			AND Page_ID = @ParticipantsPageID),0)

		DELETE FROM dp_Selected_Records WHERE Selection_ID = @SelectionID2
		DELETE FROM dp_Selected_Contacts WHERE Selection_ID = @SelectionID2

			IF @SelectionID2 = 0

			BEGIN

					  INSERT INTO dp_Selections
					(Selection_Name,Page_ID,User_ID)

					VALUES
					 ('Donor Search Results', @ParticipantsPageID, @UserID2)

					  SET @SelectionID2 = SCOPE_IDENTITY()

			END

		INSERT INTO dp_Selected_Records (Record_ID, Record_Description, Selection_ID, Delete_Record_ID)
		SELECT DISTINCT Donor_ID, Display_Name, @SelectionID2, Donor_ID
		FROM #D
		WHERE #D.Period_Amount >= ISNULL(@MinGiv,#D.Period_Amount)
		 AND #D.Period_Amount <= ISNULL(@MaxGiv, #D.Period_Amount)
		 AND #D.Period_Donations >= ISNULL(@MinGifts,#D.Period_Donations)
		 AND (@FirstTimeGiver = 0 OR (@FirstTimeGiver >= 1 AND #D.Donor_ID IN (SELECT Donor_ID FROM #D1)))


	END

END



GO
