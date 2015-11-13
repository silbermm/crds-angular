USE [MinistryPlatform]
GO
-- ====================================================================================
-- Author:		Sandi Ritter
-- Create date: 11/5/2015
-- Description:	This SP will sum donations for staff members for a specific timeframe
--              Staff members are determined based on a role of "CR Staff"
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

    CREATE TABLE #D1 (ID INT, Period_Donations money)

	BEGIN
	INSERT INTO #D1 (Period_Donations, ID)
	SELECT Period_Amount = SUM(DD.Amount)
		 , C.Household_ID
	FROM Donations D
	  INNER JOIN Donors Do ON Do.Donor_ID = D.Donor_ID
	  INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID AND C.Household_ID IS NOT NULL
	  INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
	  INNER JOIN Households H ON H.Household_ID = C.Household_ID
	WHERE
	  D.Batch_ID IS NOT NULL
	  AND D.Donation_Date >= @FromDate
	  AND D.Donation_Date <  @ToDate+1

	GROUP BY C.Household_ID

	CREATE INDEX IX_D1_ID ON #D1(ID)

	CREATE TABLE #D2 (ID INT, Name nvarchar(40))
	INSERT INTO #D2 (ID, Name)
	SELECT Household_ID, C.Display_Name
	FROM Contacts C
	  INNER JOIN dp_Users U ON U.Contact_ID = C.Contact_ID
      INNER JOIN dp_User_Roles R ON R.User_ID = U.User_ID
	WHERE EXISTS (SELECT 1 FROM #D1 WHERE #D1.ID = C.Household_ID)
	  AND R.Role_ID = 73


	SELECT #D1.ID, Name, Period_Donations
	FROM #D1
	  INNER JOIN #D2 ON #D2.ID = #D1.ID
	WHERE #D2.ID = #D1.ID

	END
END
GO

SET IDENTITY_INSERT [dbo].[dp_Reports] ON
GO

INSERT INTO [dbo].[dp_Reports]
           ([Report_ID]
		   ,[Report_Name]
           ,[Description]
           ,[Report_Path]
           ,[Pass_Selected_Records]
           ,[Pass_LinkTo_Records]
           ,[On_Reports_Tab])
     VALUES
           (260
		   			,'CRDS Staff Tithing report'
           ,'Crossroads specific report that details staff household donations'
           ,'/MPReports/CRDS Staff Tithing Report'
           ,0
           ,0
           ,1)
GO

SET IDENTITY_INSERT [dbo].[dp_Reports] OFF
GO

INSERT INTO [dbo].[dp_Report_Pages]
           ([Report_ID]
           ,[Page_ID])
     VALUES
           (260
		  	,297)
GO


INSERT INTO [dbo].[dp_Role_Reports]
           ([Role_ID]
           ,[Report_ID]
           ,[Domain_ID])
     VALUES
           (2
           ,260
           ,1)
GO
