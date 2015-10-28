USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[report_cr_di_NullPreferredName]    Script Date: 10/22/2015 2:57:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.report_cr_di_NullPreferredName', 'p') IS NULL
    EXEC('CREATE PROCEDURE dbo.report_cr_di_NullPreferredName AS SELECT 1')
GO

ALTER PROCEDURE [dbo].[report_cr_di_NullPreferredName]
AS
BEGIN

SELECT * 
FROM Contacts 
WHERE First_Name IS NOT NULL AND Nickname IS NULL

END