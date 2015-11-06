USE [MinistryPlatform]
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Group_Participants ADD
	Child_Care_Requested bit NULL
GO
ALTER TABLE dbo.Group_Participants ADD CONSTRAINT
	DF_Group_Participants_Child_Care_Requested DEFAULT 0 FOR Child_Care_Requested
GO
ALTER TABLE dbo.Group_Participants SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

UPDATE [dbo].[Group_Participants]
   SET [Child_Care_Requested] = 0
GO

UPDATE [dbo].[dp_Sub_Pages]
   SET [Default_Field_List] = N'
    Participant_ID_Table_Contact_ID_Table.Display_Name
    ,Group_Role_ID_Table.Role_Title
    ,(SELECT MAX(E.Event_Start_Date) FROM Event_Participants EP
        INNER JOIN Events E ON E.Event_ID = EP.Event_ID
        WHERE EP.Group_Participant_ID = Group_Participants.Group_Participant_ID
        AND EP.Participation_Status_ID IN (3,4)) AS Last_Attended
    ,Group_Participants.Start_Date
    ,Group_Participants.End_Date
    ,Group_Participants.Employee_Role
    ,Group_Participants.Hours_Per_Week
    ,Group_Participants.Participant_ID
    ,Group_Participants.[Child_Care_Requested]
   '
 WHERE dp_Sub_Pages.Sub_Page_ID = 298
GO
