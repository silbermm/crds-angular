
SELECT 'INSERT INTO [dbo].[Form_Fields] ([Field_Order], [Field_Label], [Field_Type_ID], [Field_Values], [Required], [Form_ID], [Domain_ID], [Placement_Required], [CrossroadsId]) VALUES ('
+ CONVERT(varchar, Form_Field_ID) + ','
+ CONVERT(varchar, [Field_Order]) + ','
+ '''' + REPLACE([Field_Label],'''','''''') + '''' + ','
+  CONVERT(varchar, [Field_Type_ID]) + ','
+ '''' + ISNULL(REPLACE([Field_Values],'''',''''''),'NULL') + '''' + ','
+  CONVERT(varchar, [Required]) + ','
+  CONVERT(varchar, [Form_ID]) + ','
+  CONVERT(varchar, [Domain_ID]) + ','
+  CONVERT(varchar, [Placement_Required]) + ','
+  ISNULL(CONVERT(varchar, [CrossroadsId]),'NULL')
+ ')' AS InsertStatement
  from dbo.Form_Fields where (Form_ID = 16 or Form_ID = 17)