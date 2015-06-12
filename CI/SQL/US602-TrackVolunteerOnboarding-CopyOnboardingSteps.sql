USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[cr_CopyOnboarding]    Script Date: 6/10/2015 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Canterbury, Andy
-- Create date: 06/10/2015
-- Description:	Copy Onboarding Steps to a Response
-- =============================================
CREATE PROCEDURE [dbo].[cr_CopyOnboardingSteps] 
	-- Add the parameters for the stored procedure here
	@ResponseID int 	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;	

	INSERT INTO [dbo].[Response_Attributes] 
		([Attribute_ID], [Response_ID], [Domain_ID], [Start_Date], [Order], [Onboarding_Status_ID])
    SELECT GA.[Attribute_ID], @ResponseID as Response_ID, 1 as Domain_ID, GETDATE() as Start_Date, GA.[Order],  1 as Onboarding_Status_ID FROM [dbo].[Group_Attributes] GA
	JOIN [MinistryPlatform].[dbo].[Attributes] AT ON GA.Attribute_ID = AT.Attribute_ID
	WHERE GA.Group_ID = (SELECT O.Add_to_Group FROM [dbo].[Responses] R JOIN [dbo].[Opportunities] O on O.Opportunity_ID = R.Opportunity_ID WHERE Response_ID = @ResponseID) and AT.Attribute_Type_ID = 58
	ORDER BY GA.[Order]

END

GO