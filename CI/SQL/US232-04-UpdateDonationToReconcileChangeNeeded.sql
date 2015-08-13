USE [MinistryPlatform]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.crds_Update_Donation_To_Reconcile_Change_Needed', 'p') IS NULL
  EXEC('CREATE PROCEDURE dbo.crds_Update_Donation_To_Reconcile_Change_Needed AS SELECT 1')
GO

-- =============================================
-- Author:  Dustin Kocher
-- Create date: 8/13/2015
-- Description: Update the dbo.Donations table by setting
--                Reconcile_Change_Complete to false and
--                Reconcile_Change_Needed field to the proper
--                field that was changed in the dbo.Donations or
--                dbo.Donation_Distributions table.
-- =============================================
ALTER PROCEDURE [dbo].[crds_Update_Donation_To_Reconcile_Change_Needed]
  @Donation_Id INT = NULL,
  @ReconcileChangeNeededOn nvarchar(50) = NULL
AS
BEGIN

  SET NOCOUNT ON;

  UPDATE [dbo].[Donations]
   SET Reconcile_Change_Needed = @ReconcileChangeNeededOn, Reconcile_Change_Complete = 0
  WHERE Donation_ID = @Donation_Id;

END
GO

IF OBJECT_ID('dbo.crds_tr_Update_Donation_To_Reconcile_Change_Needed', 'TR') IS NULL
  EXEC('CREATE TRIGGER dbo.crds_tr_Update_Donation_To_Reconcile_Change_Needed ON [dbo].[Donations] AFTER UPDATE AS SELECT 1')
GO

-- =============================================
-- Author:      Dustin Kocher
-- Create date: 8/13/2015
-- Description: Calls crds_Update_Donation_To_Reconcile_Change_Needed when Donation Amount Updated
-- =============================================
ALTER TRIGGER [dbo].[crds_tr_Update_Donation_To_Reconcile_Change_Needed]
  ON  [dbo].[Donations]
  AFTER UPDATE
AS
BEGIN

  SET NOCOUNT ON;

  -- If Donation Amount column is being updated
  if UPDATE(Donation_Amount)
  begin

    DECLARE @donation_id INT;

    -- Get the Donation ID if it belongs to a finalized
    -- and deposited batch
    SELECT @donation_id = I.Donation_ID
    FROM INSERTED I
    INNER JOIN DELETED D ON I.Donation_ID = D.Donation_ID
    INNER JOIN [dbo].[Donations] dtns on dtns.Donation_ID = I.Donation_ID
    INNER JOIN [dbo].[Batches] b on b.Batch_ID = d.Batch_ID
    WHERE dtns.Donation_Status_ID = 2 and b.Finalize_Date is not NULL and b.Deposit_ID is not NULL

    -- if we have a donation id
    if @donation_id is not NULL
    begin
      EXEC [dbo].[crds_Update_Donation_To_Reconcile_Change_Needed]
        @Donation_Id = @donation_id,
        @ReconcileChangeNeededOn = 'Donation Amount'
    end
  end

END
GO

IF OBJECT_ID('dbo.crds_tr_Update_Donation_To_Reconcile_Change_Needed', 'TR') IS NULL
  EXEC('CREATE TRIGGER dbo.crds_tr_Update_Donation_To_Reconcile_Change_Needed ON [dbo].[Donation_Distributions] AFTER UPDATE AS SELECT 1')
GO

-- =============================================
-- Author:      Dustin Kocher
-- Create date: 8/13/2015
-- Description: Calls crds_Update_Donation_To_Reconcile_Change_Needed when Distribution Amount or Program is Updated
-- =============================================
ALTER TRIGGER [dbo].[crds_tr_Update_Donation_To_Reconcile_Change_Needed]
  ON  [dbo].[Donations]
  AFTER UPDATE
AS
BEGIN

  SET NOCOUNT ON;

  -- If Donation Amount column or Program is being updated
  if UPDATE(Amount) or UPDATE(Program_ID)
  begin

    DECLARE @donation_id INT;
    DECLARE @amount MONEY;
    DECLARE @program_name nvarchar(130);

    -- Get the Donation ID if it belongs to a finalized
    -- and deposited batch
    SELECT @donation_id = I.Donation_ID, @amount = dd.Amount, @program_name = p.Program_Name
    FROM INSERTED I
    INNER JOIN DELETED D ON I.Donation_Distribution_ID = D.Donation_Distribution_ID
    INNER JOIN [dbo].[Donation_Distributions] dd on dd.Donation_Distribution_ID = I.Donation_Distribution_ID
    INNER JOIN [dbo].[Donations] dtns on dtns.Donation_ID = dd.Donation_ID
    INNER JOIN [dbo].[Batches] b on b.Batch_ID = d.Batch_ID
    INNER JOIN [dbo].[Programs] p on p.Program_ID = dtns.Program_ID
    WHERE dtns.Donation_Status_ID = 2 and b.Finalize_Date is not NULL and b.Deposit_ID is not NULL

    -- if we have a donation id and Amount is being updated
    if @donation_id is not NULL and UPDATE(Amount)
    begin
      EXEC [dbo].[crds_Update_Donation_To_Reconcile_Change_Needed]
        @Donation_Id = @donation_id,
        @ReconcileChangeNeededOn = 'Distribution Amount: ' + @amount
    end
    else if @donation_id is not NULL and UPDATE(Program_ID)
    begin
      EXEC [dbo].[crds_Update_Donation_To_Reconcile_Change_Needed]
        @Donation_Id = @donation_id,
        @ReconcileChangeNeededOn = 'Distribution Program: ' + @program_name
    end

  end

END
