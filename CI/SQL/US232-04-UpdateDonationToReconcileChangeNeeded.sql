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
  @ReconcileChangeNeededOn NVARCHAR(50) = NULL
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
  IF UPDATE(Donation_Amount)
  BEGIN

    DECLARE @donation_id INT;

    -- Get the Donation ID if it belongs to a finalized
    -- and deposited batch
    SELECT @donation_id = I.Donation_ID
    FROM INSERTED I
    INNER JOIN DELETED D ON I.Donation_ID = D.Donation_ID
    INNER JOIN [dbo].[Donations] dtns ON dtns.Donation_ID = I.Donation_ID
    INNER JOIN [dbo].[Batches] b ON b.Batch_ID = dtns.Batch_ID
    INNER JOIN [dbo].[Deposits] dp ON dp.Deposit_ID = b.Deposit_ID
    WHERE dtns.Donation_Status_ID = 2 AND b.Finalize_Date IS NOT NULL AND
      b.Deposit_ID IS NOT NULL AND dp.Exported = 1

    -- if we have a donation id
    IF @donation_id IS NOT NULL
    BEGIN
      EXEC [dbo].[crds_Update_Donation_To_Reconcile_Change_Needed]
        @Donation_Id = @donation_id,
        @ReconcileChangeNeededOn = 'Donation Amount'
    END
  END

END
GO

IF OBJECT_ID('dbo.crds_tr_Update_Donation_To_Reconcile_Change_Needed_Due_To_Distribution', 'TR') IS NULL
  EXEC('CREATE TRIGGER dbo.crds_tr_Update_Donation_To_Reconcile_Change_Needed_Due_To_Distribution ON [dbo].[Donation_Distributions] AFTER UPDATE AS SELECT 1')
GO

-- =============================================
-- Author:      Dustin Kocher
-- Create date: 8/13/2015
-- Description: Calls crds_Update_Donation_To_Reconcile_Change_Needed when Distribution Amount or Program is Updated
-- =============================================
ALTER TRIGGER [dbo].[crds_tr_Update_Donation_To_Reconcile_Change_Needed_Due_To_Distribution]
  ON  [dbo].[Donation_Distributions]
  AFTER UPDATE
AS
BEGIN

  SET NOCOUNT ON;

  -- If Donation Amount column or Program is being updated
  IF UPDATE(Amount) OR UPDATE(Program_ID)
  BEGIN

    DECLARE @donation_id INT;
    DECLARE @amount NVARCHAR(100);
    DECLARE @program_name NVARCHAR(130);

    -- Get the Donation ID if it belongs to a finalized
    -- and deposited batch
    SELECT @donation_id = I.Donation_ID, @amount = 'Distribution Amount ' + CAST(dd.Amount AS VARCHAR(100)),
      @program_name = 'Distribution Program ' + CAST(p.Program_Name AS VARCHAR(100))
    FROM INSERTED I
    INNER JOIN DELETED D ON I.Donation_Distribution_ID = D.Donation_Distribution_ID
    INNER JOIN [dbo].[Donation_Distributions] dd ON dd.Donation_Distribution_ID = I.Donation_Distribution_ID
    INNER JOIN [dbo].[Donations] dtns ON dtns.Donation_ID = dd.Donation_ID
    INNER JOIN [dbo].[Batches] b ON b.Batch_ID = dtns.Batch_ID
    INNER JOIN [dbo].[Programs] p ON p.Program_ID = dd.Program_ID
    INNER JOIN [dbo].[Deposits] dp ON dp.Deposit_ID = b.Deposit_ID
    WHERE dtns.Donation_Status_ID = 2 AND b.Finalize_Date IS NOT NULL AND
      b.Deposit_ID IS NOT NULL AND dp.Exported = 1

    -- if we have a donation id and Amount is being updated
    IF @donation_id IS NOT NULL AND UPDATE(Amount)
    BEGIN
      EXEC [dbo].[crds_Update_Donation_To_Reconcile_Change_Needed]
        @Donation_Id = @donation_id,
        @ReconcileChangeNeededOn = @amount
    END
    ELSE IF @donation_id IS NOT NULL AND UPDATE(Program_ID)
    BEGIN
      EXEC [dbo].[crds_Update_Donation_To_Reconcile_Change_Needed]
        @Donation_Id = @donation_id,
        @ReconcileChangeNeededOn = @program_name
    END

  END

END
