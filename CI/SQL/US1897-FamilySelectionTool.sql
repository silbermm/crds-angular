USE [MinistryPlatform]
GO

DECLARE @PAGE_VIEW_ID int = 92219;;
DECLARE @FORM_RESPONSES_PAGE_ID int = 424;

IF EXISTS (Select 1 FROM [dbo].[dp_Page_Views] WHERE [dbo].[dp_Page_Views].[Page_View_ID] = @PAGE_VIEW_ID)
BEGIN
		UPDATE [dbo].[dp_Page_Views] SET
			 View_Title = 'GO Trips Responses'
			,Page_ID = @FORM_RESPONSES_PAGE_ID
			,Field_List = 'Form_ID_Table.[Form_ID]
				, Contact_ID_Table.[Contact_ID]
				, Form_Responses.[Response_Date]
				, Pledge_Campaign_ID_Table.[Pledge_Campaign_ID]'
			,View_Clause = 'Pledge_Campaign_ID_Table_Pledge_Campaign_Type_ID_Table.[Pledge_Campaign_Type_ID] = 2'
		WHERE dp_Page_Views.Page_View_ID = @PAGE_VIEW_ID
	END
ELSE
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
	INSERT INTO [dbo].[dp_Page_Views]
				([Page_View_ID]
				,[View_Title]
				,[Page_ID]
				,[Field_List]
				,[View_Clause])
			VALUES
				(@PAGE_VIEW_ID
				,'GO Trips Responses'
				,@FORM_RESPONSES_PAGE_ID
				,'Form_ID_Table.[Form_ID]
				, Contact_ID_Table.[Contact_ID]
				, Form_Responses.[Response_Date]
				, Pledge_Campaign_ID_Table.[Pledge_Campaign_ID]'
				,'Pledge_Campaign_ID_Table_Pledge_Campaign_Type_ID_Table.[Pledge_Campaign_Type_ID] = 2')
	SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END