USE [MinistryPlatform];
GO

/****** Object:  View [dbo].[vw_crds_Contact_Attributes]    Script Date: 10/22/2015 6:44:31 PM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE VIEW [dbo].[vw_crds_Contact_Attributes]
AS
     SELECT ca.Contact_ID,
            at.Attribute_Type_ID,
            at.Attribute_Type,
            a.Attribute_ID,
            a.Attribute_Name,
            ca.Notes,
            ca.Start_Date,
            ca.End_Date
     FROM dbo.Contact_Attributes AS ca
          INNER JOIN dbo.Attributes AS a ON ca.Attribute_ID = a.Attribute_ID
          INNER JOIN dbo.Attribute_Types AS at ON a.Attribute_Type_ID = at.Attribute_Type_ID
     WHERE( ca.End_Date IS NULL
          );
GO