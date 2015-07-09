USE MinistryPlatform

GO

UPDATE [dbo].[Form_Fields]
SET [Field_Label] = 'Parent/Guardian Last Name'
WHERE [CrossroadsId] = 366

UPDATE [dbo].[Form_Fields]
SET [Field_Label] = 'Parent/Guardian First Name'
WHERE [CrossroadsId] = 367

UPDATE [dbo].[Form_Fields]
SET [Field_Label] = 'Parent/Guardian Home Phone'
WHERE [CrossroadsId] = 368

UPDATE [dbo].[Form_Fields]
SET [Field_Label] = 'Parent/Guardian Cell Phone'
WHERE [CrossroadsId] = 369

UPDATE [dbo].[Form_Fields] 
SET [Field_Label] = 'Parent/Guardian Email'
WHERE [CrossroadsId] = 370

UPDATE [dbo].[Form_Fields]
SET [Field_Label] = 'Parent/Guardian Consent'
WHERE [CrossroadsId] = 371

UPDATE [dbo].[Form_Fields]
SET [Field_Label] = 'Parent/Guardian Date'
WHERE [CrossroadsId] = 372