USE [MinistryPlatform]
GO

/****** Object:  Index [IX_crds_ContactRelationships_Triggered_By]    Script Date: 1/13/2016 2:02:52 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Contact_Relationships]') AND name = N'IX_crds_ContactRelationships_Triggered_By')
DROP INDEX [IX_crds_ContactRelationships_Triggered_By] ON [dbo].[Contact_Relationships]
GO

/****** Object:  Index [IX_crds_ContactRelationships_Triggered_By]    Script Date: 1/13/2016 2:02:52 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Contact_Relationships]') AND name = N'IX_crds_ContactRelationships_Triggered_By')
CREATE NONCLUSTERED INDEX [IX_crds_ContactRelationships_Triggered_By] ON [dbo].[Contact_Relationships]
(
	[_Triggered_By] ASC,
	[Relationship_ID] ASC
)
INCLUDE ( 	[End_Date]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


