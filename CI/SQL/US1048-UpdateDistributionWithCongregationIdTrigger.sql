USE MinistryPlatform
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.crds_tr_Update_With_Congregation_Id', 'TR') IS NULL
    EXEC('CREATE TRIGGER dbo.crds_tr_Update_With_Congregation_Id ON [dbo].[Donation_Distributions] AFTER INSERT AS SELECT 1')
GO
-- =============================================
-- Author:		Matt Brewer
-- Create date: 6/10/2015
-- Description:	Update with Congregation ID on every INSERT
-- =============================================
ALTER TRIGGER [dbo].[crds_tr_Update_With_Congregation_Id] 
   ON  [dbo].[Donation_Distributions] 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Distribution_Id INT;
	DECLARE @Household_Id INT;
	DECLARE @Congregation_Id INT;

	SELECT @Distribution_Id = dd.Donation_Distribution_ID, @Household_Id = c.household_id, @Congregation_Id = h.congregation_id
		FROM [dbo].[Donation_Distributions] dd
		JOIN [dbo].[Donations] d ON d.donation_id = dd.donation_id
		JOIN [dbo].[Donors] do ON do.donor_id = d.donor_id
		JOIN [dbo].[Contacts] c ON c.contact_id = do.contact_id
		LEFT JOIN [dbo].[Households] h ON h.household_id = c.household_id
		JOIN INSERTED ON INSERTED.Donation_Distribution_ID = dd.Donation_Distribution_ID;

	-- Set to 'No Site Specified' if Household has no Congregation		
	IF (@Household_Id IS NULL) OR (@Congregation_Id IS NULL)
		SET @Congregation_Id = 5;

	UPDATE [dbo].[Donation_Distributions]
		SET Congregation_ID = @Congregation_Id
		WHERE Donation_Distribution_ID = @Distribution_Id;

END
GO
