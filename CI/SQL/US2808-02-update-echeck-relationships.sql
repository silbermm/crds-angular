USE [MinistryPlatform]
GO

UPDATE [dbo].Relationships
   SET 
		[Relationship_Name] = 'Can Check-In',
		[Description] = 'E-CheckIn - Adult that can check-in kids'
	WHERE Relationship_ID = 40
GO

UPDATE [dbo].Relationships
   SET 
		[Relationship_Name] = 'Can be Checked-In by',
		[Description] = 'E-CheckIn - Child that can checked-in by adult'
	WHERE Relationship_ID = 45
GO