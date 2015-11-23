USE [MinistryPlatform]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.crds_tr_Update_Donation_To_Reconcile_Change_Needed', 'TR') IS NULL
  EXEC('CREATE TRIGGER dbo.crds_tr_Update_Donation_To_Reconcile_Change_Needed ON [dbo].[Donations] AFTER UPDATE AS SELECT 1')
GO

-- =============================================
-- Author:      Dustin Kocher
-- Create date: 8/13/2015
-- Update date: 11/18/2015
-- Description: Calls crds_Update_Donation_To_Reconcile_Change_Needed when Donation Amount Updated
-- =============================================
ALTER TRIGGER [dbo].[crds_tr_Update_Donation_To_Reconcile_Change_Needed]
  ON  [dbo].[Donations]
  AFTER UPDATE
AS
BEGIN

  SET NOCOUNT ON;

  -- If Donation Amount column or Donation Date column is being updated
  IF UPDATE(Donation_Amount) OR UPDATE(Donation_Date)
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

    -- if we have a donation id and Amount is being updated
    IF @donation_id IS NOT NULL AND UPDATE(Donation_Amount)
    BEGIN
      EXEC [dbo].[crds_Update_Donation_To_Reconcile_Change_Needed]
        @Donation_Id = @donation_id,
        @ReconcileChangeNeededOn = 'Donation Amount'
    END
    ELSE IF @donation_id IS NOT NULL AND UPDATE(Donation_Date)
    BEGIN
      EXEC [dbo].[crds_Update_Donation_To_Reconcile_Change_Needed]
        @Donation_Id = @donation_id,
        @ReconcileChangeNeededOn = 'Donation Date'
    END
  END

END
GO
