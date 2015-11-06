-- ====================================================================================
-- Author:		Sandi Ritter
-- Create date: 11/5/2015
-- Description:	This SP will sum donations for staff members for a specific timeframe
--              Staff members are determeined based on a role of "CR Staff"
-- ===================================================================================
CREATE PROCEDURE [dbo].[report_CRDS_Staff_Tithing]
	 @DomainID varchar(40)
	,@UserID varchar(40)
	,@PageID Int
	,@FromDate DateTime
	,@ToDate DateTime

AS
BEGIN

	SET NOCOUNT ON;

    CREATE TABLE #D1 (Donor INT, Name nvarchar(75), ID INT)

	BEGIN
	INSERT INTO #D1 (Donor, Name, ID)

	SELECT  DISTINCT Donor_Record, Display_Name, Household_ID
	FROM Contacts
	WHERE Donor_Record IS NOT NULL AND  Household_ID IN
									(SELECT ISNULL(Household_ID,0)
										FROM Donations D
										 INNER JOIN Donors Do ON Do.Donor_ID = D.Donor_ID
										 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
										 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = D.Domain_ID
										 INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
										 INNER JOIN dp_Users U ON U.Contact_ID = C.Contact_ID
										 INNER JOIN dp_User_Roles R ON R.User_ID = U.User_ID
										WHERE
										 R.Role_ID = 73	)


	CREATE INDEX IX_D1_ID ON #D1(ID)

	SELECT Name,  Period_Donations = SUM(DN.Donation_Amount)
	FROM Donations DN
	 INNER JOIN Donors Do ON Do.Donor_ID = DN.Donor_ID
	 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
	 INNER JOIN Donation_Distributions DD ON DD.Donation_ID = DN.Donation_ID
	 INNER JOIN #D1 D ON D.ID = C.Household_ID
	 INNER JOIN Households H ON (H.Household_ID = C.Household_ID)
	WHERE
	 @DomainID = Dom.Domain_GUID
	 AND D.Batch_ID IS NULL
	 AND DN.Donation_Date >= @FromDate
	 AND DN.Donation_Date < @ToDate+1

	GROUP BY H.Household_ID, Name

	END
END
GO


