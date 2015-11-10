USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Reports] WHERE Report_ID = 259)
BEGIN
  SET IDENTITY_INSERT [dbo].[dp_Reports] ON

  INSERT INTO [dbo].[dp_Reports] ([Report_ID], [Report_Name], [Description], [Report_Path], [Pass_Selected_Records], [Pass_LinkTo_Records], [On_Reports_Tab])
  VALUES (259, 'CRDS Donation Analysis', NULL, '/MPReports/CRDS Donation Analysis', 0, 0, 1)

  SET IDENTITY_INSERT [dbo].[dp_Reports] OFF
END

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Report_Pages] WHERE Report_ID = 259)
BEGIN

  INSERT INTO [dbo].[dp_Report_Pages] ([Report_ID], [Page_ID])
  VALUES (259, 297)

  INSERT INTO [dbo].[dp_Report_Pages] ([Report_ID], [Page_ID])
  VALUES (259, 296)

END

GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Role_Reports] WHERE [Report_ID] = 259)
BEGIN

  INSERT INTO [dbo].[dp_Role_Reports] ([Role_ID], [Report_ID], [Domain_ID])
  VALUES(7, 259, 1)

END
GO
