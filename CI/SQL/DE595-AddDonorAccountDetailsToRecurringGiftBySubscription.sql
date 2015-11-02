USE [MinistryPlatform]
GO

IF (NOT EXISTS (SELECT 1
                FROM [dbo].[dp_Page_Views]
                WHERE [Page_View_ID] = 2182
                AND [Field_List] LIKE '%Processor_Account_ID%'))
BEGIN
  UPDATE [dbo].[dp_Page_Views]
  SET [Field_List] = CONCAT([Field_List], ', Donor_Account_ID_Table.[Processor_ID]
                                           , Donor_Account_ID_Table.[Processor_Account_ID]')
  WHERE Page_View_ID = 2182;
END;