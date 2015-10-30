USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[report_cr_di_SingleContactAttribute]    Script Date: 10/22/2015 2:57:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.report_cr_di_SingleContactAttribute', 'p') IS NULL
    EXEC('CREATE PROCEDURE dbo.report_cr_di_SingleContactAttribute AS SELECT 1')
GO

ALTER PROCEDURE [dbo].[report_cr_di_SingleContactAttribute]
AS
BEGIN

DECLARE @server_url VARCHAR(MAX) 
SELECT TOP 1 @server_url = External_Server_Name from dp_Domains

-- get the list of attribute types that need to be unique
DECLARE @single_attr_tab TABLE(attr_type_id INT, attr_id INT, unique_attribute_name VARCHAR(MAX))

INSERT INTO @single_attr_tab 
SELECT attr_t.Attribute_Type_ID, Attribute_ID, attr_t.Attribute_Type 
FROM Attribute_Types attr_t 
INNER JOIN Attributes attr ON attr_t.Attribute_Type_ID = attr.Attribute_Type_ID 
WHERE attr_t.Prevent_Multiple_Selection=1

-- look for multiple contact attributes that are supposed to be single attributes
SELECT unique_attribute_name, attr_type_id, contact_id, COUNT(contact_id) attr_count, @server_url server_url 
FROM Contact_Attributes ca 
INNER JOIN @single_attr_tab sa ON ca.Attribute_ID = sa.attr_id 
WHERE ca.End_Date IS NULL 
GROUP BY unique_attribute_name, attr_type_id, contact_id 
HAVING COUNT(contact_id) > 1 
ORDER BY attr_type_id

END