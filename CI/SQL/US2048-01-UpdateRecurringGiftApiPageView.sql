USE [MinistryPlatform]
GO

IF (NOT EXISTS (SELECT 1
                FROM [dbo].[dp_Page_Views]
                WHERE [Page_View_ID] = 2184
                AND [Field_List] LIKE '%Program_Name%'))
BEGIN
  UPDATE [dbo].[dp_Page_Views]
  SET [Field_List] = CONCAT([Field_List], '
  , Program_ID_Table.[Program_Name]
  , CASE(Frequency_ID_Table.Frequency_ID)
    WHEN 1 THEN
      CONCAT(''Every '', RTRIM(Day_Of_Week_ID_Table.Day_Of_Week))
    ELSE
      CONCAT(
        [dbo].[crds_udfGetOrdinalNumber](Day_Of_Month),
        '' of the month''
      )
    END AS Recurrence')
  WHERE Page_View_ID = 2184;
END;

