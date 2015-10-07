USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Event_Selected_Campaign_Donations]    Script Date: 10/1/2015 4:18:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Event_Selected_Campaign_Donations]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Event_Selected_Campaign_Donations] AS' 
END
GO




ALTER PROCEDURE [dbo].[report_CRDS_Event_Selected_Campaign_Donations]

	@DomainID varchar(40)
	,@UserID varchar(40)
	,@PageID Int
	,@SelectionID Int
	,@EventID INT
	,@PledgeStatusID Int
AS
BEGIN

DECLARE @C Table (Pledge_Campaign_ID INT)
INSERT INTO @C (Pledge_Campaign_ID)
SELECT Pledge_Campaign_ID
FROM Pledge_Campaigns 
WHERE Event_ID = @EventID

--List 1: Donations to the Target Event

SELECT Donation_Distribution_ID
, Amount
, D.Donation_Date
, DD.Pledge_ID
, COALESCE(DD.Notes,CASE WHEN D.Notes LIKE 'First%Name%' THEN '' ELSE D.Notes END) AS Notes
, E.Event_ID
, ISNULL(PC.Campaign_Name, 'Pledge Not Assigned') AS Campaign_Name
, ISNULL(E.Event_Title, 'Project/Target Event Not Set') AS Event_Title
, ISNULL(PLC.Display_Name + ISNULL(' for ' + PL.Beneficiary,''), '*Pledge Not Assigned')  AS Pledge_Owner
, ISNULL(PLDo.Donor_ID, 0) AS Pledge_Owner_ID
, CASE WHEN ISNULL(D.[Anonymous],0)=1 THEN 'Anonymous Donor' ELSE C.Display_Name END AS Donor_Credited
, Do.Donor_ID AS Donor_Credited_ID
, CASE WHEN DD.Target_Event IS NULL THEN 'Project/Target Event Not Set' ELSE 'Project/Target Event Set' END AS Event_Status
, CASE WHEN DD.Pledge_ID IS NULL THEN 'No Pledge Credited' WHEN PL.Donor_ID = D.Donor_ID THEN 'Donation By Pledge Owner' ELSE '3rd Party Donation' END AS Pledge_From
, 1 AS Distributions
, 'Project/Target Event Assigned' AS List
, PL.Total_Pledge
, PS.Pledge_Status
, A.Address_Line_1
, A.Address_Line_2  
, A.City
, A.[State/Region] 
, A.Postal_Code 
, CASE WHEN A.Foreign_Country IN ('United States','USA','US') THEN NULL ELSE ISNULL(UPPER(A.Foreign_Country),'') END AS Foreign_Country


FROM Donation_Distributions DD
 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = DD.Domain_ID
 INNER JOIN Donations D ON D.Donation_ID = DD.Donation_ID
 INNER JOIN Donors Do ON Do.Donor_ID = D.Donor_ID
 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
 LEFT OUTER JOIN Households H ON H.Household_ID = C.Household_ID
 LEFT OUTER JOIN Addresses A ON A.Address_ID = H.Address_ID AND ISNULL(D.[Anonymous],0) = 0
 INNER JOIN dbo.[Events] E ON E.Event_ID = DD.Target_Event 
 LEFT OUTER JOIN Pledges PL ON PL.Pledge_ID = DD.Pledge_ID AND PL.Pledge_Status_ID = @PledgeStatusID AND (PL.Trip_General_Fund = 0 OR PL.Trip_General_Fund is null)
 LEFT OUTER JOIN Pledge_Statuses PS ON PS.Pledge_Status_ID = PL.Pledge_Status_ID
 LEFT OUTER JOIN Pledge_Campaigns PC ON PC.Pledge_Campaign_ID = PL.Pledge_Campaign_ID
 LEFT OUTER JOIN Donors PLDo ON PLDo.Donor_ID = PL.Donor_ID
 LEFT OUTER JOIN Contacts PLC ON PLC.Contact_ID = PLDo.Contact_ID

WHERE DD.Target_Event = @EventID
 AND Dom.Domain_GUID = @DomainID

UNION ALL

--List 2: Pledges that do not yet have a gift

SELECT 0 AS Donation_Distribution_ID
, 0 AS Amount
, NULL AS Donation_Date
, PL.Pledge_ID
, '' AS Notes
, PC.Event_ID
, ISNULL(PC.Campaign_Name, 'Pledge Not Assigned') AS Campaign_Name
, ISNULL(E.Event_Title, 'Project/Target Event Not Set') AS Event_Title
, PLC.Display_Name + ISNULL(' for ' + PL.Beneficiary,'') AS Pledge_Owner
, ISNULL(PLDo.Donor_ID, 0) AS Pledge_Owner_ID
, NULL AS Donor_Credited
, 0 AS Donor_Credited_ID
, 'No Donations Yet' AS Event_Status
, 'No Donations Yet' AS Pledge_From
, 0 AS Distributions
, 'No Donations Yet' AS List
, PL.Total_Pledge
, PS.Pledge_Status
, NULL AS Address_Line_1
, NULL AS Address_Line_2  
, NULL AS City
, NULL AS [State/Region] 
, NULL AS Postal_Code 
, NULL AS Foreign_Country

FROM Pledge_Campaigns PC
 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = PC.Domain_ID
 INNER JOIN Pledges PL ON PC.Pledge_Campaign_ID = PL.Pledge_Campaign_ID AND PL.Pledge_Status_ID = @PledgeStatusID AND (PL.Trip_General_Fund = 0 OR PL.Trip_General_Fund is null)
 INNER JOIN Pledge_Statuses PS ON PS.Pledge_Status_ID = PL.Pledge_Status_ID
 INNER JOIN dbo.[Events] E ON E.Event_ID = PC.Event_ID
 INNER JOIN Donors PLDo ON PLDo.Donor_ID = PL.Donor_ID
 INNER JOIN Contacts PLC ON PLC.Contact_ID = PLDo.Contact_ID
 
WHERE PC.Pledge_Campaign_ID IN (SELECT Pledge_Campaign_ID FROM @C C) 
 AND PL.Pledge_ID NOT IN (SELECT Pledge_ID FROM Donation_Distributions WHERE Pledge_ID IS NOT NULL)
 AND Dom.Domain_GUID = @DomainID
 
--List 3: Donations without a target event
UNION ALL

SELECT Donation_Distribution_ID
, Amount
, D.Donation_Date
, DD.Pledge_ID
, COALESCE(DD.Notes,CASE WHEN D.Notes LIKE 'First%Name%' THEN '' ELSE D.Notes END) AS Notes
, E.Event_ID
, ISNULL(PC.Campaign_Name, 'Pledge Not Assigned') AS Campaign_Name
, ISNULL(E.Event_Title, 'Project/Target Event Not Set') AS Event_Title
, PLC.Display_Name + ISNULL(' for ' + PL.Beneficiary,'') AS Pledge_Owner
, ISNULL(PLDo.Donor_ID, 0) AS Pledge_Owner_ID
, CASE WHEN ISNULL(D.[Anonymous],0)=1 THEN 'Anonymous Donor' ELSE C.Display_Name END AS Donor_Credited
, Do.Donor_ID AS Donor_Credited_ID
, CASE WHEN DD.Target_Event IS NULL THEN 'Project/Target Event Not Set' ELSE 'Project/Target Event Set' END AS Event_Status
, CASE WHEN DD.Pledge_ID IS NULL THEN 'No Pledge Credited' WHEN PL.Donor_ID = D.Donor_ID THEN 'Donation By Pledge Owner' ELSE '3rd Party Donation' END AS Pledge_From
, 1 AS Distributions
, 'Project/Target Event Not Assigned' AS List
, PL.Total_Pledge
, PS.Pledge_Status
, A.Address_Line_1
, A.Address_Line_2  
, A.City
, A.[State/Region] 
, A.Postal_Code 
, CASE WHEN A.Foreign_Country IN ('United States','USA','US') THEN NULL ELSE ISNULL(UPPER(A.Foreign_Country),'') END AS Foreign_Country

FROM Pledge_Campaigns PC
 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = PC.Domain_ID
 INNER JOIN Pledges PL ON PC.Pledge_Campaign_ID = PL.Pledge_Campaign_ID AND PL.Pledge_Status_ID = @PledgeStatusID AND (PL.Trip_General_Fund = 0 OR PL.Trip_General_Fund is null)
 INNER JOIN Pledge_Statuses PS ON PS.Pledge_Status_ID = PL.Pledge_Status_ID
 INNER JOIN dbo.[Events] E ON E.Event_ID = PC.Event_ID
 INNER JOIN Donors PLDo ON PLDo.Donor_ID = PL.Donor_ID
 INNER JOIN Contacts PLC ON PLC.Contact_ID = PLDo.Contact_ID
 INNER JOIN Donation_Distributions DD ON DD.Pledge_ID = PL.Pledge_ID
 INNER JOIN Donations D ON D.Donation_ID = DD.Donation_ID
 INNER JOIN Donors Do ON Do.Donor_ID = D.Donor_ID
 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
 LEFT OUTER JOIN Households H ON H.Household_ID = C.Household_ID
 LEFT OUTER JOIN Addresses A ON A.Address_ID = H.Address_ID  AND ISNULL(D.[Anonymous],0) = 0
 
WHERE PC.Pledge_Campaign_ID IN (SELECT Pledge_Campaign_ID FROM @C C) 
 AND ISNULL(DD.Target_Event,0) <> @EventID 
 AND Dom.Domain_GUID = @DomainID

END





GO


