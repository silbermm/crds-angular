USE [MinistryPlatform]
GO
/****** Object:  Trigger [dbo].[tr_Use_Proxy_Email]    Script Date: 2/17/2015 4:03:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Kevin McCord
-- Create date: 17 February 2015
-- Description: Replaces the From value with a proxy email address
-- =============================================
ALTER TRIGGER [dbo].[tr_Use_Proxy_Email] 
   ON  [dbo].[dp_Communication_Messages] 
   AFTER INSERT
AS 
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

DECLARE @DomainID INT = (SELECT Top 1 Domain_ID FROM INSERTED)
DECLARE @ProxyEmailFrom Nvarchar(254) = (SELECT Top 1 Value FROM dp_Configuration_Settings CS WHERE CS.Application_Code = 'COMMON' AND CS.Key_Name = 'EmailProxyFrom' AND CS.Domain_ID = @DomainID) 
DECLARE @EmailDomain Nvarchar(100) = (SELECT Top 1 Value FROM dp_Configuration_Settings CS WHERE CS.Application_Code = 'COMMON' AND CS.Key_Name = 'EmailDomain' AND CS.Domain_ID = @DomainID) 

IF @DomainID IS NOT NULL AND @EmailDomain IS NOT NULL AND @ProxyEmailFrom LIKE '%@%.%'
	BEGIN

	UPDATE dp_Communication_Messages 
	SET [From] = ISNULL(REPLACE(CM.[From],C.Email_Address,@ProxyEmailFrom),CM.[FROM])
	FROM dp_Communication_Messages CM 
	 INNER JOIN dp_Communications Com ON Com.Communication_ID = CM.Communication_ID 
	 INNER JOIN Contacts C ON C.Contact_ID = Com.From_Contact 
	WHERE CM.[From] NOT LIKE '%' + @EmailDomain + '%'
	 AND CM.Action_Status_ID IN (1,2)
	 AND EXISTS (SELECT 1 FROM INSERTED I WHERE I.Communication_Message_ID = CM.Communication_Message_ID)

	END

END
