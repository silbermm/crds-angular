USE [MinistryPlatform];
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Childcare_By_Day]    Script Date: 11/19/2015 3:16:17 PM ******/


IF OBJECT_ID('dbo.report_CRDS_Childcare_By_Day', 'p') IS NOT NULL
BEGIN
DROP PROCEDURE [dbo].[report_CRDS_Childcare_By_Day];
END
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Childcare_By_Day]    Script Date: 11/19/2015 3:16:17 PM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[report_CRDS_Childcare_By_Day]
       @ReportDate DATETIME, @CongregationId INT
AS
    BEGIN
        SET NOCOUNT ON;
        SELECT g.Group_Name,
               parentEvent.Event_Title AS ParentEventTitle,
               child.Event_Title AS ChildEventTitle,
               parentEvent.Event_Start_Date,
               parentEvent.Event_End_Date,
               c.Last_Name AS ChildLastName,
               c.First_Name AS ChildFirstName,
               c.__Age AS ChildAge,
               c.HS_Graduation_Year,
                 CASE
                     WHEN c.HS_Graduation_Year IS NULL
                     THEN CONCAT('Age: ', c.__Age)
                     ELSE CONCAT('Age: ', c.__Age, ' (', c.HS_Graduation_Year, ')')
                 END AS AgeHighSchoolGrad,
               parent.DisplayName AS ParentName,
               parent.Email AS ParentEmail
        FROM MinistryPlatform.dbo.Events child
             INNER JOIN MinistryPlatform.dbo.Event_Participants ep ON child.Event_ID = ep.Event_ID and ep.Participation_Status_ID = 2
             INNER JOIN MinistryPlatform.dbo.Events parentEvent ON child.Parent_Event_ID = parentEvent.Event_ID
             INNER JOIN MinistryPlatform.dbo.Event_Groups eg ON parentEvent.Event_ID = eg.Event_ID and parentEvent.Congregation_ID = @CongregationId
                                                            AND DATEDIFF(dd, parentEvent.Event_Start_Date, @ReportDate) = 0
             INNER JOIN MinistryPlatform.dbo.Groups g ON eg.Group_ID = g.Group_ID
                                                     AND g.Child_Care_Available = 1
             INNER JOIN MinistryPlatform.dbo.Participants p ON ep.Participant_ID = p.Participant_ID
             INNER JOIN MinistryPlatform.dbo.Contacts c ON p.Contact_ID = c.Contact_ID
             CROSS APPLY MinistryPlatform.dbo.crds_FindParentAttendingRelatedEvent( c.Contact_ID, ep.Event_ID ) parent;
    END;
GO