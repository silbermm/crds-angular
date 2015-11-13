USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO
DELETE FROM [dbo].[dp_Page_Views]
WHERE Page_View_ID = 2139
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
      			,[View_Title]
      			,[Page_ID]
      			,[Field_List]
      			,[View_Clause])
     VALUES
           (2139
    		   ,'crdsFormFieldsWithCrossroadsId'
    		   ,422
    		   ,'Form_Fields.[Form_Field_ID], Form_Fields.[CrossroadsId], Form_Fields.[Field_Order], Field_Type_ID_Table.[Field_Type], Form_ID_Table.[Form_ID], Form_ID_Table.[Form_Title], Form_Fields.[Required], Form_Fields.[Field_Label]'
    		   ,'Form_Fields.[Form_Field_ID] IS NOT NULL')
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO
