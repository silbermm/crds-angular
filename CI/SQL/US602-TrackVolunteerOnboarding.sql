USE [MinistryPlatform]
GO

ALTER TABLE [dbo].[Group_Attributes] DROP CONSTRAINT [FK_Group_Attributes_Groups]
GO

ALTER TABLE [dbo].[Group_Attributes] DROP CONSTRAINT [FK_Group_Attributes_dp_Domains]
GO

ALTER TABLE [dbo].[Group_Attributes] DROP CONSTRAINT [FK_Group_Attributes_Attributes]
GO

/****** Object:  Table [dbo].[Group_Attributes]    Script Date: 6/5/2015 4:04:39 PM ******/
DROP TABLE [dbo].[Group_Attributes]
GO

/****** Object:  Table [dbo].[Group_Attributes]    Script Date: 6/5/2015 4:04:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Group_Attributes](
  [Group_Attribute_ID] [int] IDENTITY(1,1) NOT NULL,
  [Attribute_ID] [int] NOT NULL,
  [Group_ID] [int] NOT NULL,
  [Domain_ID] [int] NOT NULL,
  [Start_Date] [datetime] NOT NULL,
  [End_Date] [datetime] NULL,
  [Notes] [nvarchar](255) NULL,
  [Order] [int] NULL
 CONSTRAINT [PK_Group_Attributes] PRIMARY KEY CLUSTERED 
(
  [Group_Attribute_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Group_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Group_Attributes_Attributes] FOREIGN KEY([Attribute_ID])
REFERENCES [dbo].[Attributes] ([Attribute_ID])
GO

ALTER TABLE [dbo].[Group_Attributes] CHECK CONSTRAINT [FK_Group_Attributes_Attributes]
GO

ALTER TABLE [dbo].[Group_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Group_Attributes_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[Group_Attributes] CHECK CONSTRAINT [FK_Group_Attributes_dp_Domains]
GO

ALTER TABLE [dbo].[Group_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Group_Attributes_Groups] FOREIGN KEY([Group_ID])
REFERENCES [dbo].[Groups] ([Group_ID])
GO

ALTER TABLE [dbo].[Group_Attributes] CHECK CONSTRAINT [FK_Group_Attributes_Groups]
GO

UPDATE [dbo].[dp_Sub_Pages]
SET [Default_Field_List] = 'Attribute_ID_Table_Attribute_Type_ID_Table.Attribute_Type,Attribute_ID_Table.Attribute_Name,Group_Attributes.Start_Date,Group_Attributes.End_Date, Group_Attributes.[Order]'
WHERE [Sub_Page_ID] = 303
GO

/****** Object:  Table [dbo].[cr_Onboarding_Statuses]    Script Date: 6/9/2015 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Onboarding_Statuses]') AND type in (N'U'))
CREATE TABLE [dbo].[cr_Onboarding_Statuses](
	[Onboarding_Status_ID] [int] IDENTITY(1,1) NOT NULL,
	[Onboarding_Status] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_cr_Onboarding_Status] PRIMARY KEY CLUSTERED 
(
	[Onboarding_Status_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Response_Attributes]    Script Date: 6/9/2015 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Response_Attributes]') AND type in (N'U'))
CREATE TABLE [dbo].[Response_Attributes](
	[Response_Attribute_ID] [int] IDENTITY(1,1) NOT NULL,
	[Attribute_ID] [int] NOT NULL,
	[Response_ID] [int] NOT NULL,
	[Domain_ID] [int] NOT NULL,
	[Start_Date] [datetime] NOT NULL,
	[End_Date] [datetime] NULL,
	[Notes] [nvarchar](255) NULL,
	[Order] [int] NULL,
	[Onboarding_Status_ID] [int] NOT NULL,
 CONSTRAINT [PK_Response_Attributes] PRIMARY KEY CLUSTERED 
(
	[Response_Attribute_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Response_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Response_Attributes_Attributes] FOREIGN KEY([Attribute_ID])
REFERENCES [dbo].[Attributes] ([Attribute_ID])
GO

ALTER TABLE [dbo].[Response_Attributes] CHECK CONSTRAINT [FK_Response_Attributes_Attributes]
GO

ALTER TABLE [dbo].[Response_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Response_Attributes_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[Response_Attributes] CHECK CONSTRAINT [FK_Response_Attributes_dp_Domains]
GO

ALTER TABLE [dbo].[Response_Attributes]  WITH CHECK ADD  CONSTRAINT [FK_Response_Attributes_Responses] FOREIGN KEY([Response_ID])
REFERENCES [dbo].[Responses] ([Response_ID])
GO

ALTER TABLE [dbo].[Response_Attributes] CHECK CONSTRAINT [FK_Response_Attributes_Responses]
GO

ALTER TABLE [dbo].[Response_Attributes] WITH CHECK ADD CONSTRAINT [FK_Response_Attributes_cr_Onboarding_Statuses] FOREIGN KEY([Onboarding_Status_ID])
REFERENCES [dbo].[cr_Onboarding_Statuses] ([Onboarding_Status_ID])

ALTER TABLE [dbo].[Response_Attributes] CHECK CONSTRAINT [FK_Response_Attributes_cr_Onboarding_Statuses]
GO

/****** Add Lookup Table to MP Page ******/

DECLARE @pageIds table (id int);
DECLARE @PageId int;

IF EXISTS (
	SELECT 1 
	FROM [dbo].[dp_Pages]
	WHERE [dbo].[dp_Pages].[Display_Name] = 'Onboarding Statuses')
	BEGIN
		UPDATE [dbo].[dp_Pages]
		SET [Display_Name] = 'Onboarding Statuses',
			[Singular_Name] = 'Onboarding Status',
			[Description] = 'Status of an Onboarding Step',
			[View_Order] = 305,
			[Table_Name] = 'cr_Onboarding_Statuses',
			[Primary_Key] = 'Onboarding_Status_ID',
			[Default_Field_List] = 'Onboarding_Status',
			[Selected_Record_Expression] = 'Onboarding_Status',
			[Display_Copy] = 0
		OUTPUT INSERTED.Page_ID INTO @pageIds
		WHERE [dbo].[dp_Pages].[Display_Name] = 'Onboarding Statuses' 
		SELECT TOP 1 @PageId = id from @pageIds
	END
ELSE
	BEGIN
		INSERT INTO [dbo].[dp_Pages]
				   ([Display_Name]
				   ,[Singular_Name]
				   ,[Description]
				   ,[View_Order]
				   ,[Table_Name]
				   ,[Primary_Key]
				   ,[Default_Field_List]
				   ,[Selected_Record_Expression]
				   ,[Display_Copy])
			 VALUES
				   ('Onboarding Statuses'
				   ,'Onboarding Status'
				   ,'Status of an Onboarding Step'
				   ,305
				   ,'cr_Onboarding_Statuses'
				   ,'Onboarding_Status_ID'
				   ,'Onboarding_Status'
				   ,'Onboarding_Status'
				   ,0)
		SET @PageId = SCOPE_IDENTITY()
	END

IF NOT EXISTS (
  SELECT 1 
  FROM [dbo].[dp_Page_Section_Pages]
  WHERE [dbo].[dp_Page_Section_Pages].[Page_ID] = @PageId 
  AND [dbo].[dp_Page_Section_Pages].[Page_ID] = 11
)
BEGIN
	INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
    VALUES
           (@PageId
           ,11)
END

IF EXISTS (
  SELECT 1 
  FROM [dbo].[dp_Sub_Pages]
  WHERE [dbo].[dp_Sub_Pages].[Display_Name] = 'Onboarding'
  )
	BEGIN
		UPDATE [dbo].[dp_Sub_Pages]
		SET [Display_Name] = 'Onboarding',
			[Singular_Name] = 'Onboarding',
			[Page_ID] = 382,
			[View_Order] = 6,
			[Link_To_Page_ID] = 277,
			[Link_From_Field_Name] = 'Attribute_ID',
			[Select_To_Page_ID] = 277,
			[Select_From_Field_Name] = 'Response_Attributes.Attribute_ID',
			[Primary_Table] = 'Response_Attributes',
			[Primary_Key] = 'Response_Attribute_ID',
			[Default_Field_List] = 'Attribute_ID_Table.[Attribute_Name] AS Step, Attribute_ID_Table.[Description] AS [Description] , Response_Attributes.[Start_Date] AS [Start Date] , Response_Attributes.[End_Date] AS [End Date], Onboarding_Status_ID_Table.[Onboarding_Status] AS [Onboarding Status] , Response_Attributes.[Notes] AS [Notes] , [dp_Updated].[Date_Time] AS [Date Updated] , [dp_Updated].[User_Name] AS [Updated By]',
			[Selected_Record_Expression] = 'Attribute_ID_Table.Attribute_Name',
			[Filter_Key] = 'Response_ID',
			[Relation_Type_ID] = 2
		WHERE [Display_Name] = 'Onboarding'
	END
ELSE 
	BEGIN
		INSERT INTO [dbo].[dp_Sub_Pages]
           ([Display_Name]
           ,[Singular_Name]
           ,[Page_ID]
           ,[View_Order]
           ,[Link_To_Page_ID]
           ,[Link_From_Field_Name]
           ,[Select_To_Page_ID]
           ,[Select_From_Field_Name]
           ,[Primary_Table]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Filter_Key]
           ,[Relation_Type_ID])
		VALUES
           ('Onboarding'
           ,'Onboarding'
           ,382
           ,6
           ,277
           ,'Attribute_ID'
           ,277
           ,'Response_Attributes.Attribute_ID'
           ,'Response_Attributes'
           ,'Response_Attribute_ID'
           ,'Attribute_ID_Table.[Attribute_Name] AS Step, Attribute_ID_Table.[Description] AS [Description] , Response_Attributes.[Start_Date] AS [Start Date] , Response_Attributes.[End_Date] AS [End Date], Onboarding_Status_ID_Table.[Onboarding_Status] AS [Onboarding Status] , Response_Attributes.[Notes] AS [Notes] , [dp_Updated].[Date_Time] AS [Date Updated] , [dp_Updated].[User_Name] AS [Updated By]'
           ,'Attribute_ID_Table.Attribute_Name'
           ,'Response_ID'
           ,2)
	END

/****** Object:  StoredProcedure [dbo].[cr_CopyOnboarding]    Script Date: 6/10/2015 ******/
-- =============================================
-- Author:		Canterbury, Andy
-- Create date: 06/10/2015
-- Description:	Copy Onboarding Steps to a Response
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_CopyOnboardingSteps]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[cr_CopyOnboardingSteps] AS' 
END
GO

ALTER PROCEDURE [dbo].[cr_CopyOnboardingSteps] 
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