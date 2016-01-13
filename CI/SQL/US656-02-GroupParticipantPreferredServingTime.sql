USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'Preferred_Serving_Time_ID' AND Object_ID = Object_ID(N'dbo.Group_Participants'))
BEGIN
	ALTER TABLE [dbo].[Group_Participants]
	ADD Preferred_Serving_Time_ID INT
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Group_Participant_cr_Preferred_Serve_Time]') AND parent_object_id = OBJECT_ID(N'[dbo].[Group_Participants]'))
ALTER TABLE [dbo].[Group_Participants]  WITH CHECK ADD  CONSTRAINT [FK_Group_Participant_cr_Preferred_Serve_Time] FOREIGN KEY([Preferred_Serving_Time_ID])
REFERENCES [dbo].[cr_Preferred_Serve_Time] ([Preferred_Serving_Time_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Group_Participant_cr_Preferred_Serve_Time]') AND parent_object_id = OBJECT_ID(N'[dbo].[Group_Participants]'))
ALTER TABLE [dbo].[Group_Participants] CHECK CONSTRAINT FK_Group_Participant_cr_Preferred_Serve_Time
GO