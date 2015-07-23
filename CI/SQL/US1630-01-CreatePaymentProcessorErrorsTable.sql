USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Payment_Processor_Event_Errors]    Script Date: 7/23/2015 15:30:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_Payment_Processor_Event_Errors](
	[Event_Error_ID] [int] IDENTITY(1,1) NOT NULL,
	[Event_Date_Time] [datetime] NOT NULL,
	[Event_ID] [nvarchar](100) NULL,
	[Event_Type] [nvarchar](50) NULL,
	[Event_Message] [nvarchar](max) NOT NULL,
	[Response_Message] [nvarchar](max) NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_cr_Payment_Processor_Event_Errors] PRIMARY KEY CLUSTERED 
(
	[Event_Error_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[cr_Payment_Processor_Event_Errors]  WITH CHECK ADD  CONSTRAINT [FK_cr_Payment_Processor_Event_Errors_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO