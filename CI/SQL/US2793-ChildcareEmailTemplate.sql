USE [MinistryPlatform]
GO

IF EXISTS (SELECT * FROM [dbo].[dp_Communications] WHERE [Communication_ID] = 1459)
UPDATE [dbo].[dp_Communications]
   SET [Body] = N'<span style="color: rgb(51, 51, 51); font-family: arial, sans, sans-serif; font-size: 13px; line-height: 18.5714282989502px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">[Nickname],</span><div><span style="color: rgb(51, 51, 51); font-family: arial, sans, sans-serif; font-size: 13px; line-height: 18.5714282989502px; white-space: pre-wrap; background-color: rgb(255, 255, 255);">Just a reminder you are signed up to serve on [Event_Start_Date] at [Event_Start_Time] for [Event_Title]. If you are unable to make it, please contact us as soon as possible.<br /> <br />[cmsChildcareEventReminder]<br />[Childcare_Children] <br />[Childcare_Contact] <br /> <br /> <br />We look forward to serving with you!</span><br /></div>'
 WHERE [Communication_ID] = 1459
GO


