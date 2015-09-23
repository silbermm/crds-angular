USE [MinistryPlatform]
GO

UPDATE [dp_Pages]
SET [Default_Field_List] = CONCAT([Default_Field_List], ',Donations.Check_Scanner_Batch')
WHERE [Page_ID] = 297;

UPDATE [dp_Page_Views]
SET [Field_List] = CONCAT([Field_List], ',Donations.Check_Scanner_Batch')
WHERE [Page_View_ID] IN (357, 92199, 354);

GO
