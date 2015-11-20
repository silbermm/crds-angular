USE [MinistryPlatform]
GO

TRUNCATE TABLE dp_Contact_Publications
GO

TRUNCATE TABLE dp_User_Publications
GO

TRUNCATE TABLE dp_Communication_Publications

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dp_Contact_Publications_dp_Publications]') AND parent_object_id = OBJECT_ID(N'[dbo].[dp_Contact_Publications]'))
ALTER TABLE [dbo].[dp_Contact_Publications] DROP CONSTRAINT [FK_dp_Contact_Publications_dp_Publications]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dp_User_Publications_dp_Publications]') AND parent_object_id = OBJECT_ID(N'[dbo].[dp_User_Publications]'))
ALTER TABLE [dbo].[dp_User_Publications] DROP CONSTRAINT [FK_dp_User_Publications_dp_Publications]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dp_Communication_Publications_dp_Publications]') AND parent_object_id = OBJECT_ID(N'[dbo].[dp_Communication_Publications]'))
ALTER TABLE [dbo].[dp_Communication_Publications] DROP CONSTRAINT [FK_dp_Communication_Publications_dp_Publications]
GO

TRUNCATE TABLE dp_Publications
GO

ALTER TABLE [dbo].[dp_Contact_Publications]  WITH CHECK ADD  CONSTRAINT [FK_dp_Contact_Publications_dp_Publications] FOREIGN KEY([Publication_ID])
REFERENCES [dbo].[dp_Publications] ([Publication_ID])
GO

ALTER TABLE [dbo].[dp_Contact_Publications] CHECK CONSTRAINT [FK_dp_Contact_Publications_dp_Publications]
GO

ALTER TABLE [dbo].[dp_User_Publications]  WITH CHECK ADD  CONSTRAINT [FK_dp_User_Publications_dp_Publications] FOREIGN KEY([Publication_ID])
REFERENCES [dbo].[dp_Publications] ([Publication_ID])
GO

ALTER TABLE [dbo].[dp_User_Publications] CHECK CONSTRAINT [FK_dp_User_Publications_dp_Publications]
GO

ALTER TABLE [dbo].[dp_Communication_Publications]  WITH CHECK ADD  CONSTRAINT [FK_dp_Communication_Publications_dp_Publications] FOREIGN KEY([Publication_ID])
REFERENCES [dbo].[dp_Publications] ([Publication_ID])
GO

ALTER TABLE [dbo].[dp_Communication_Publications] CHECK CONSTRAINT [FK_dp_Communication_Publications_dp_Publications]
GO

INSERT INTO [dbo].[dp_Publications]
           ([Title],
            [Description],
			[Domain_ID],
            [Available_Online],
            [Online_Sort_Order])
     VALUES
           ('The Daily',
		    'Daily emails to help you connect with God',
			1,
			1,
			1)

INSERT INTO [dbo].[dp_Publications]
           ([Title],
            [Description],
			[Domain_ID],
            [Available_Online],
            [Online_Sort_Order])
     VALUES
           ('General Crossroads Communication',
		    'Roughly once a month',
			1,
			1,
			1)

INSERT INTO [dbo].[dp_Publications]
           ([Title],
            [Description],
			[Domain_ID],
            [Available_Online],
            [Online_Sort_Order])
     VALUES
           ('Kids'' Club & Student Ministry',
		    'Once a week. Parents only.',
			1,
			1,
			1)

GO
