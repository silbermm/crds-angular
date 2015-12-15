USE [MinistryPlatform]
GO

IF (NOT EXISTS (SELECT 1
                FROM [dbo].[dp_Page_Section_Pages]
                WHERE [Page_ID] = 509
                AND [Page_Section_ID] = 4
                ))
BEGIN
    INSERT INTO [dbo].[dp_Page_Section_Pages] (
         [Page_ID]
        ,[Page_Section_ID]
    ) VALUES (
         509
        ,4
    );
END;