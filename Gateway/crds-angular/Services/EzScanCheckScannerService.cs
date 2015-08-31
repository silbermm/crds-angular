using System;
using System.Collections.Generic;
using System.Data;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MPServices=MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class EzScanCheckScannerService : ICheckScannerService
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDonorService _donorService;
        private readonly IPaymentService _paymentService;
        private readonly MPServices.IDonorService _mpDonorService;

        public EzScanCheckScannerService(IDbConnection dbConnection, IDonorService donorService, IPaymentService paymentService, MPServices.IDonorService mpDonorService)
        {
            _dbConnection = dbConnection;
            _donorService = donorService;
            _paymentService = paymentService;
            _mpDonorService = mpDonorService;
        }

        public List<CheckScannerBatch> GetAllBatches()
        {
            return (GetBatches(false));
        }

        public List<CheckScannerBatch> GetOpenBatches()
        {
            return (GetBatches(true));
        }

        private List<CheckScannerBatch> GetBatches(bool onlyOpen)
        {
            List<CheckScannerBatch> batches;
            IDataReader reader = null;
            try
            {
                var whereClause = onlyOpen ? "WHERE COALESCE(BatchStatus, 0) <> 1" : string.Empty;

                batches = WithDbCommand(dbCommand =>
                {
                    dbCommand.CommandType = CommandType.Text;
                    dbCommand.CommandText = string.Format("SELECT ID, IDBatch, DateProcess, BatchStatus FROM batches {0} ORDER BY DateProcess DESC", whereClause);

                    var b = new List<CheckScannerBatch>();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = 0;
                        b.Add(new CheckScannerBatch
                        {
                            Id = reader[i++] as int? ?? 0,
                            Name = reader[i++] as string,
                            ScanDate = reader[i++] as DateTime? ?? DateTime.Now,
                            Status = reader.IsDBNull(i) || reader.GetInt32(i) == 0 ? BatchStatus.NotExported : BatchStatus.Exported
                        });
                    }
                    return (b);
                });
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return (batches);
        }

        public virtual List<CheckScannerCheck> GetChecksForBatch(string batchName)
        {
            List<CheckScannerCheck> checks;
            IDataReader reader = null;
            try
            {
                checks = WithDbCommand(dbCommand =>
                {
                    dbCommand.CommandType = CommandType.Text;
                    dbCommand.CommandText = "SELECT ID, Account, Amount, CheckNo, DateScan, Route, Payor, DateCheck, Payor2, Address, Address2, City, State, Zip FROM items WHERE IDBatch = @IDBatch";
                    var idBatchParam = dbCommand.CreateParameter();
                    idBatchParam.ParameterName = "IDBatch";
                    idBatchParam.DbType = DbType.String;
                    idBatchParam.Value = batchName;
                    dbCommand.Parameters.Add(idBatchParam);
                    dbCommand.Prepare();

                    var c = new List<CheckScannerCheck>();
                    reader = dbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = 0;
                        c.Add(new CheckScannerCheck
                        {
                            Id = reader[i++] as int? ?? 0,
                            AccountNumber = reader[i++] as string,
                            Amount = (decimal) (reader[i++] as double? ?? 0.0),
                            CheckNumber = reader[i++] as string,
                            ScanDate = reader[i++] as DateTime?,
                            RoutingNumber = reader[i++] as string,
                            Name1 = reader[i++] as string,
                            CheckDate = reader[i++] as DateTime?,
                            Name2 = reader[i++] as string,
                            Address = new Address
                            {
                                Line1 = reader[i++] as string,
                                Line2 = reader[i++] as string,
                                City = reader[i++] as string,
                                State = reader[i++] as string,
                                PostalCode = reader[i] as string
                            }
                        });
                    }

                    return (c);
                });
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return (checks);
        }

        private T WithDbCommand<T>(Func<IDbCommand,T> func)
        {
            IDbCommand dbCommand = null;
            try
            {
                _dbConnection.Open();
                dbCommand = _dbConnection.CreateCommand();
                return(func(dbCommand));
            }
            finally
            {
                if (dbCommand != null)
                {
                    dbCommand.Dispose();
                }

                _dbConnection.Close();
            }
        }

        public virtual CheckScannerBatch UpdateBatchStatus(string batchName, BatchStatus newStatus)
        {
            WithDbCommand(dbCommand =>
            {
                dbCommand.CommandType = CommandType.Text;
                dbCommand.CommandText = "UPDATE batches SET BatchStatus = @BatchStatus WHERE IDBatch = @IDBatch";

                var batchStatusParam = dbCommand.CreateParameter();
                batchStatusParam.ParameterName = "BatchStatus";
                batchStatusParam.DbType = DbType.Int16;
                batchStatusParam.Value = newStatus == BatchStatus.Exported ? 1 : 0;
                dbCommand.Parameters.Add(batchStatusParam);

                var idBatchParam = dbCommand.CreateParameter();
                idBatchParam.ParameterName = "IDBatch";
                idBatchParam.DbType = DbType.String;
                idBatchParam.Value = batchName;
                dbCommand.Parameters.Add(idBatchParam);

                dbCommand.Prepare();
                dbCommand.ExecuteNonQuery();

                return (true);
            });

            return(new CheckScannerBatch
            {
                Name = batchName,
                Status = newStatus
            });
        }

        public virtual CheckScannerBatch CreateDonationsForBatch(CheckScannerBatch batchDetails)
        {
            var checks = GetChecksForBatch(batchDetails.Name);
            foreach (var check in checks)
            {
                try
                {
                    var contactDonor = _donorService.GetContactDonorForDonorAccount(check.AccountNumber, check.RoutingNumber) ?? new ContactDonor();
                    if (!contactDonor.HasPaymentProcessorRecord)
                    {
                        var token = _paymentService.CreateToken(check.AccountNumber, check.RoutingNumber);
                        contactDonor.Details = new ContactDetails
                        {
                            DisplayName = check.Name1,
                            Address = new PostalAddress
                            {
                                Line1 = check.Address.Line1,
                                Line2 = check.Address.Line2,
                                City = check.Address.City,
                                State = check.Address.State,
                                PostalCode = check.Address.PostalCode
                            }
                        };
                        contactDonor.Account = new DonorAccount
                        {
                            AccountNumber = check.AccountNumber,
                            RoutingNumber = check.RoutingNumber,
                            Type = AccountType.Checking
                        };

                        contactDonor = _donorService.CreateOrUpdateContactDonor(contactDonor, string.Empty, token, DateTime.Now);
                    }

                    var charge = _paymentService.ChargeCustomer(contactDonor.ProcessorId, (int) (check.Amount), contactDonor.DonorId);
                    var fee = charge.BalanceTransaction != null ? charge.BalanceTransaction.Fee : null;

                    var programId = batchDetails.ProgramId == null ? null : batchDetails.ProgramId + "";

                    var donationId = _mpDonorService.CreateDonationAndDistributionRecord((int) (check.Amount),
                                                                                         fee,
                                                                                         contactDonor.DonorId,
                                                                                         programId,
                                                                                         charge.Id,
                                                                                         "check",
                                                                                         contactDonor.ProcessorId,
                                                                                         check.CheckDate ?? (check.ScanDate ?? DateTime.Now),
                                                                                         contactDonor.RegisteredUser);
                    check.DonationId = donationId;

                    batchDetails.Checks.Add(check);
                }
                catch (Exception e)
                {
                    check.Error = e.ToString();
                    batchDetails.ErrorChecks.Add(check);
                }
            }

            batchDetails.Status = BatchStatus.Exported;
            UpdateBatchStatus(batchDetails.Name, batchDetails.Status);

            return (batchDetails);
        }
    }
}