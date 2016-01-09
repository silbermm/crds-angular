use MinistryPlatform;

IF NOT EXISTS (SELECT * FROM   INFORMATION_SCHEMA.COLUMNS
			   WHERE  TABLE_NAME = 'Opportunities' AND COLUMN_NAME = 'Send_Reminder')
BEGIN
	ALTER TABLE [dbo].[Opportunities] ADD Send_Reminder bit not null default 0;
END

IF NOT EXISTS (SELECT * FROM   INFORMATION_SCHEMA.COLUMNS
			   WHERE  TABLE_NAME = 'Opportunities' AND COLUMN_NAME = 'Reminder_Template')
BEGIN
	ALTER TABLE [dbo].[Opportunities] ADD Reminder_Template int null;
END

IF NOT EXISTS (SELECT * FROM   INFORMATION_SCHEMA.COLUMNS
			   WHERE  TABLE_NAME = 'Opportunities' AND COLUMN_NAME = 'Reminder_Days_Prior')
BEGIN
	ALTER TABLE [dbo].[Opportunities] ADD Reminder_Days_Prior int null;
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys 
			   WHERE object_id = OBJECT_ID(N'[dbo].[FK_Opportunities_Messages]') 
			   AND parent_object_id = OBJECT_ID(N'[dbo].[Opportunities]'))
BEGIN
	ALTER TABLE [dbo].[Opportunities] ADD CONSTRAINT FK_Opportunties_Messages FOREIGN KEY ( Reminder_Template ) references dp_Communications(Communication_ID);
END