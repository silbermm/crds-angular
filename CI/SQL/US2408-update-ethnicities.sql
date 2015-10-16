USE [MinistryPlatform]

-- Remove old values that are no longer wanted
DELETE FROM Contact_Attributes WHERE Attribute_ID IN (3783, 3784)
DELETE FROM Attributes WHERE Attribute_ID IN (3783, 3784)

-- Remap to new names
UPDATE Attributes SET Attribute_Name = 'White' WHERE Attribute_ID = 3776
UPDATE Attributes SET Attribute_Name = 'Latino or Hispanic' WHERE Attribute_ID = 3777
UPDATE Attributes SET Attribute_Name = 'Black or African American' WHERE Attribute_ID = 3778
UPDATE Attributes SET Attribute_Name = 'American Indian or Alaska Native' WHERE Attribute_ID = 3779
UPDATE Attributes SET Attribute_Name = 'Asian' WHERE Attribute_ID = 3780
UPDATE Attributes SET Attribute_Name = 'Some other race' WHERE Attribute_ID = 3781

-- Remove data nd re-use old 'Multi-Racial' for 'Pacific Islander' an existing entry
DELETE FROM Contact_Attributes WHERE Attribute_ID IN (3782)
UPDATE Attributes SET Attribute_Name = 'Pacific Islander' WHERE Attribute_ID = 3782 

GO