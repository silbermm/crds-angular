DECLARE @ContactAttributesToUpdate TABLE (Contact_ID INT, Attribute_Type_ID INT, TotalRows INT);
INSERT INTO @ContactAttributesToUpdate
       SELECT ca.Contact_ID, a.Attribute_Type_ID, COUNT(*) AS Count
       FROM [MinistryPlatform].[dbo].[Contact_Attributes] ca
            INNER JOIN attributes a ON ca.attribute_ID = a.attribute_id
            INNER JOIN Attribute_Types at ON a.Attribute_Type_ID = at.Attribute_Type_ID
                                             AND at.Prevent_Multiple_Selection = 1
       WHERE ca.End_Date IS NULL --and contact_id = 2562428 --and a.Attribute_Type_ID = 23
       GROUP BY ca.Contact_ID, a.Attribute_Type_ID
       HAVING COUNT(*) > 1;

DECLARE @AllAttributesForContact TABLE (Contact_Attribute_ID INT, Contact_ID INT, Attribute_Type_ID INT);
INSERT INTO @AllAttributesForContact
       SELECT ca.Contact_Attribute_ID, ca.Contact_ID, at.Attribute_Type_ID
       FROM Contact_Attributes ca
            INNER JOIN Attributes a ON ca.Attribute_ID = a.Attribute_ID
            INNER JOIN Attribute_Types at ON a.Attribute_Type_ID = at.Attribute_Type_ID
                                             AND at.Prevent_Multiple_Selection = 1
            INNER JOIN @ContactAttributesToUpdate x ON ca.Contact_ID = x.Contact_ID
                                                       AND at.Attribute_Type_ID = x.Attribute_Type_ID;
--where ca.contact_id = 1670863 --and a.Attribute_Type_ID = 23;

DECLARE @ContactAttributesToNotUpdate TABLE (Contact_Attribute_ID INT, Contact_ID INT, Attribute_Type_ID INT);
INSERT INTO @ContactAttributesToNotUpdate
       SELECT MAX(ca.contact_attribute_id) Contact_Attribute_ID, ca.Contact_ID, a.Attribute_Type_ID
       FROM Contact_Attributes ca
            INNER JOIN attributes a ON ca.attribute_ID = a.attribute_id
            INNER JOIN @ContactAttributesToUpdate tmp ON ca.contact_id = tmp.contact_id
                                                         AND a.Attribute_Type_ID = tmp.Attribute_Type_ID
       GROUP BY ca.contact_id, a.attribute_type_id;

DECLARE @FinalCountDown TABLE (Contact_Attribute_ID INT, Contact_ID INT, Attribute_Type_ID INT);
INSERT INTO @FinalCountDown
       SELECT *
       FROM @AllAttributesForContact
       EXCEPT
       SELECT *
       FROM @ContactAttributesToNotUpdate;

--make this update to update END_DATE to TODAY
SELECT *
FROM @FinalCountDown
ORDER BY Contact_ID, Attribute_Type_ID;


-- uncomment this when you are ready to update everything
/*

UPDATE [dbo].[Contact_Attributes]
   SET [End_Date] = GETDATE()
 WHERE Contact_Attribute_ID IN (SELECT Contact_Attribute_ID FROM @FinalCountDown)

 */