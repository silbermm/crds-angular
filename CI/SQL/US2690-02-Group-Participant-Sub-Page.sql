USE [MinistryPlatform]
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
