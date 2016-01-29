USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[report_CRDS_DI_Too_Many_Heads]    Script Date: 11/2/2015 10:17:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[report_CRDS_DI_Too_Many_Heads]  
AS
BEGIN

SELECT H.Household_ID, H.Household_Name, A.Address_Line_1, A.Address_Line_2, A.[State/Region] AS State_Code, A.City, A.Postal_Code
FROM Households H 
	LEFT JOIN Contacts C ON H.Household_ID = C.Household_ID 
	LEFT JOIN Addresses A on A.Address_ID = H.Address_ID
WHERE H.Household_ID = C.Household_ID AND Household_Position_ID = 1 AND C.Contact_Status_ID = 1 
GROUP BY A.Address_Line_1,A.Address_Line_2, A.City, A.[State/Region] ,A.Postal_Code, H.Household_Name, H.Household_ID HAVING Count(*)>2
ORDER BY H.Household_Name

END