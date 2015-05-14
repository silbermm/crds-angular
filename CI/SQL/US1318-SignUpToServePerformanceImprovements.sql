USE [MinistryPlatform]
GO

/****** Object:  View [dbo].[vw_crds_Serving_Participants]    Script Date: 5/13/2015 11:43:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_crds_Serving_Participants]
AS
SELECT        g.Group_ID, gp.Participant_ID, c.Email_Address, c.Display_Name, c.Nickname, c.Last_Name, gp.Domain_ID, g.Group_Name, g.Group_Type_ID, 
                         groupContact.Email_Address AS Primary_Contact_Email, o.Opportunity_ID, o.Opportunity_Title, o.Minimum_Needed, o.Maximum_Needed, o.Room, o.Shift_Start, o.Shift_End, 
                         o.Sign_Up_Deadline_ID, gr.Role_Title, e.Event_ID, e.Event_Title, e.Event_Start_Date,
                             (SELECT        TOP (1) Response_Result_ID
                               FROM            dbo.Responses AS r
                               WHERE        (Event_ID = e.Event_ID) AND (Opportunity_ID = o.Opportunity_ID) AND (Participant_ID = gp.Participant_ID)) AS RSVP, c.Contact_ID, gr.Group_Role_ID, 
                         et.Event_Type_ID, et.Event_Type
FROM            dbo.Group_Participants AS gp INNER JOIN
                         dbo.Contacts AS c ON gp.Participant_ID = c.Participant_Record INNER JOIN
                         dbo.Groups AS g ON gp.Group_ID = g.Group_ID AND g.Group_Type_ID = 9 INNER JOIN
                         dbo.Contacts AS groupContact ON g.Primary_Contact = groupContact.Contact_ID INNER JOIN
                         dbo.Opportunities AS o ON o.Add_to_Group = g.Group_ID AND o.Group_Role_ID = gp.Group_Role_ID INNER JOIN
                         dbo.Event_Types AS et ON et.Event_Type_ID = o.Event_Type_ID INNER JOIN
                         dbo.Events AS e ON e.Event_Type_ID = et.Event_Type_ID INNER JOIN
                         dbo.Group_Roles AS gr ON gr.Group_Role_ID = o.Group_Role_ID

GO
