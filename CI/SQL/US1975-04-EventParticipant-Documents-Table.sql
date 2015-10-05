USE [MinistryPlatform]
GO


/****** Object:  Table [dbo].[cr_EventParticipant_Documents]    Script Date: 9/29/2015 4:15:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_NAME = 'cr_EventParticipant_Documents'))
BEGIN
	CREATE TABLE [dbo].[cr_EventParticipant_Documents](
		[EventParticipant_Document_ID] [int] IDENTITY(1,1) NOT NULL,
		[Event_Participant_ID] [int] NOT NULL,
		[Document_ID] [int] NOT NULL,
		[Received] [bit] NULL,
		[Notes] [nvarchar](255) NULL,
		[Domain_ID] [int] NOT NULL,
	 CONSTRAINT [PK_EventParticipation_Documents] PRIMARY KEY CLUSTERED
	(
		[EventParticipant_Document_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * 
           FROM sys.foreign_keys 
           WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventParticipant_Documents_Documents]') 
             AND parent_object_id = OBJECT_ID(N'[dbo].[cr_EventParticipant_Documents]'))
BEGIN
	ALTER TABLE [dbo].[cr_EventParticipant_Documents]  WITH CHECK ADD  CONSTRAINT [FK_EventParticipant_Documents_Documents] FOREIGN KEY([Document_ID])
	REFERENCES [dbo].[cr_Documents] ([Document_ID])
END

ALTER TABLE [dbo].[cr_EventParticipant_Documents] CHECK CONSTRAINT [FK_EventParticipant_Documents_Documents]
GO

IF NOT EXISTS (SELECT * 
           FROM sys.foreign_keys 
           WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventParticipant_Documents_dp_Domains]') 
             AND parent_object_id = OBJECT_ID(N'[dbo].[cr_EventParticipant_Documents]'))
BEGIN
	ALTER TABLE [dbo].[cr_EventParticipant_Documents]  WITH CHECK ADD  CONSTRAINT [FK_EventParticipant_Documents_dp_Domains] FOREIGN KEY([Domain_ID])
	REFERENCES [dbo].[dp_Domains] ([Domain_ID])
END

ALTER TABLE [dbo].[cr_EventParticipant_Documents] CHECK CONSTRAINT [FK_EventParticipant_Documents_dp_Domains]
GO

IF NOT EXISTS (SELECT * 
           FROM sys.foreign_keys 
           WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventParticipant_Documents_Event_Participants]') 
             AND parent_object_id = OBJECT_ID(N'[dbo].[cr_EventParticipant_Documents]'))
BEGIN
	ALTER TABLE [dbo].[cr_EventParticipant_Documents]  WITH CHECK ADD  CONSTRAINT [FK_EventParticipant_Documents_Event_Participants] FOREIGN KEY([Event_Participant_ID])
	REFERENCES [dbo].[Event_Participants] ([Event_Participant_ID])
END

ALTER TABLE [dbo].[cr_EventParticipant_Documents] CHECK CONSTRAINT [FK_EventParticipant_Documents_Event_Participants]
GO
