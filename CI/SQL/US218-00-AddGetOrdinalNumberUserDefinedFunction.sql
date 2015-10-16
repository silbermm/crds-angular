USE [MinistryPlatform]
GO
/****** Object:  UserDefinedFunction [dbo].[crds_udfGetOrdinalNumber]    Script Date: 10/13/2015 16:15:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Create a placeholder function, so the below ALTER will always work.
-- This allows the same SQL file to be run in any environment, without errors.
-- This is preferable to dropping the function, as permissions will be maintained
-- on an existing function.
IF OBJECT_ID('dbo.crds_udfGetOrdinalNumber', 'FN') IS NULL
    EXEC('CREATE FUNCTION dbo.crds_udfGetOrdinalNumber(@InputNumber INT) RETURNS VARCHAR AS BEGIN; RETURN ''1''; END')
GO
-- =============================================
-- Author:      Kriz, Jim
-- Create date: 10/13/2015
-- Description:  Returns an ordinal number string for the given input number.
--               For example:
--                1  = 1st
--                2  = 2nd
--                3  = 3rd
--                4  = 4th
--                11 = 11th
--                12 = 12th
--                13 = 13th
-- =============================================
ALTER FUNCTION [dbo].[crds_udfGetOrdinalNumber]
(
  @InputNumber INT
)
RETURNS VARCHAR(20)
AS
BEGIN
  DECLARE @OrdinalNumber VARCHAR(20);

  SELECT @OrdinalNumber = 
        CAST(@InputNumber AS VARCHAR) +
        CASE
          WHEN @InputNumber % 100 IN (11, 12, 13) THEN 'th'
          WHEN @InputNumber % 10 = 1 THEN 'st'
          WHEN @InputNumber % 10 = 2 THEN 'nd'
          WHEN @InputNumber % 10 = 3 THEN 'rd'
          ELSE 'th'
        END;
  RETURN @OrdinalNumber
END
GO