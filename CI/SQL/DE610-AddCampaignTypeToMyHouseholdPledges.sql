USE [MinistryPlatform]
GO

IF (NOT EXISTS (SELECT 1
                FROM [dbo].[dp_Pages]
                WHERE [Page_ID] = 525
                AND [Default_Field_List] LIKE '%Pledge_Campaign_Type_ID%'))
BEGIN
  UPDATE [dbo].[dp_Pages]
  SET [Default_Field_List] = CONCAT([Default_Field_List],
    ', Pledge_Campaign_ID_Table_Pledge_Campaign_Type_ID_Table.[Pledge_Campaign_Type_ID]
     , Pledge_Campaign_ID_Table_Pledge_Campaign_Type_ID_Table.[Campaign_Type]')
  WHERE Page_ID = 525;
END;