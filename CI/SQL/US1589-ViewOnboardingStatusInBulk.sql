USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

UPDATE dp_Sub_Pages
SET Default_Field_List = 'Responses.Response_Date
,Participant_ID_Table_Contact_ID_Table.Display_Name
,(SELECT TOP 1 Follow_Up_Date FROM Response_Follow_Ups WHERE Response_Follow_Ups.Response_ID = Responses.Response_ID ORDER BY Follow_Up_Date DESC) AS Last_Follow_Up
,Responses.Closed
,Event_ID_Table.[Event_Start_Date] AS [Event Start Date], (SELECT TOP 1 a.Attribute_Name FROM Response_Attributes as ra JOIN [MinistryPlatform].[dbo].cr_Onboarding_Statuses as s on ra.Onboarding_Status_ID = s.Onboarding_Status_ID
  JOIN [MinistryPlatform].[dbo].[Attributes] as a on ra.Attribute_ID = a.Attribute_ID
  WHERE ra.Response_ID = Responses.Response_ID AND s.[Final_Status] = 0
  ORDER BY ra.[Order]) AS Current_Onboarding_Step'
WHERE Sub_Page_ID = 324
GO