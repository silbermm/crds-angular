USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[Attribute_Types] ADD Prevent_Multiple_Selection BIT NOT NULL 
CONSTRAINT Attribute_Types_Prevent_Multiple_Selection_DF DEFAULT 0

EXEC sp_addextendedproperty 
@name = N'Description', @value = 'When set to true only 1 attribute of this type can be selected, when set to false it will allow mutiple attributes of this type to be selected',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Attribute_Types',
@level2type = N'Column', @level2name = 'Prevent_Multiple_Selection';

GO

-- Update attributes for trips to be single selection
UPDATE dbo.Attribute_Types SET Prevent_Multiple_Selection = 1 WHERE Attribute_Type_ID IN (21, 22, 23, 62, 66, 67, 68, 69)
GO