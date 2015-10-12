USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (2187
		   ,'DI - Fix Too Many Heads'
           ,327
           ,'Data Integrity View - Households should have no more than 2 heads. Please review and correct'
           ,NULL
           ,'EXISTS (SELECT 1 FROM Contacts C WHERE C.Household_ID = Households.Household_ID AND Household_Position_ID = 1 AND C.Contact_Status_ID = 1 GROUP BY Household_ID, C.Contact_Status_ID HAVING Count(*)>2)')

GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO





SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- Author:    Sandi Ritter
-- Create date: 10/12/2015
-- Description: This SP will change the donor Statement Type to Individual 
--               when the Contact is not a Head of Household
-- =======================================================================
CREATE PROCEDURE  [dbo].[crds_Update_Donor_Statement_Type]  
  
AS
BEGIN
  
  SET NOCOUNT ON;
  DECLARE @Donors_Cursor CURSOR;
  
  SET @Donors_Cursor = CURSOR FOR
    SELECT D.Donor_ID 
  FROM Donors D 
    INNER JOIN Contacts C ON C.Contact_ID = D.Contact_ID
    INNER JOIN Households H ON H.Household_ID = C.Household_ID
    INNER JOIN Statement_Types S ON S.Statement_Type_ID =  D.Statement_Type_ID
  WHERE D.Statement_Type_ID = 2  and C.Household_Position_ID > 1
  
  DECLARE @Update_Donor_ID INT;
  OPEN @Donors_Cursor
    FETCH NEXT FROM @Donors_Cursor INTO @Update_Donor_ID
    WHILE @@FETCH_STATUS = 0
    
    BEGIN
      UPDATE Donors
      SET Statement_Type_ID = 1
      WHERE Donor_ID = @Update_Donor_ID
      
    FETCH NEXT FROM @Donors_Cursor INTO @Update_Donor_ID
    END

  CLOSE @Donors_Cursor
  DEALLOCATE @Donors_Cursor 
END

GO

