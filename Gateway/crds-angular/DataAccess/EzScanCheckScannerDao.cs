using System;
using System.Collections.Generic;
using System.Data;
using crds_angular.DataAccess.Interfaces;
using crds_angular.Models.Crossroads.Stewardship;
using Crossroads.Utilities.Interfaces;

namespace crds_angular.DataAccess
{
    public class EzScanCheckScannerDao : ICheckScannerDao
    {
        private readonly IDbConnection _dbConnection;
        private readonly ICryptoProvider _crypto;

        /// <summary>
        /// This must match the length of the check_scanner.itemsStatus.ErrorMessage column.
        /// </summary>
        private const int ErrorMessageMaxLength = 2048;

        public EzScanCheckScannerDao(IDbConnection dbConnection, ICryptoProvider crypto)
        {
            _dbConnection = dbConnection;
            _crypto = crypto;
        }

        public List<CheckScannerBatch> GetBatches(bool onlyOpenBatches = true)
        {
            List<CheckScannerBatch> batches;
            IDataReader reader = null;
            try
            {
                var whereClause = onlyOpenBatches ? "WHERE COALESCE(BatchStatus, 0) <> 1" : string.Empty;

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

        public List<CheckScannerCheck> GetChecksForBatch(string batchName)
        {
            List<CheckScannerCheck> checks;
            IDataReader reader = null;
            try
            {
                checks = WithDbCommand(dbCommand =>
                {
                    dbCommand.CommandType = CommandType.Text;
                    dbCommand.CommandText = "SELECT ID, COALESCE(Exported, 0), ErrorMessage, Account, Amount, CheckNo, DateScan, Route, Payor, DateCheck, Payor2, Address, Address2, City, State, Zip FROM items LEFT JOIN itemsStatus ON items.ID = itemsStatus.ItemID WHERE IDBatch = @IDBatch";
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
                            Exported = (reader[i++] as long? ?? 0) > 0,
                            Error = reader[i++] as string,
                            AccountNumber = _crypto.EncryptValueToString(reader[i++] as string),
                            Amount = (decimal)(reader[i++] as double? ?? 0.0),
                            CheckNumber = reader[i++] as string,
                            ScanDate = reader[i++] as DateTime?,
                            RoutingNumber = _crypto.EncryptValueToString(reader[i++] as string),
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

        public CheckScannerBatch UpdateBatchStatus(string batchName, BatchStatus newStatus)
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

            return (new CheckScannerBatch
            {
                Name = batchName,
                Status = newStatus
            });
        }

        public void UpdateCheckStatus(int checkId, bool exported, string errorMessage = null)
        {
            WithDbCommand(dbCommand =>
            {
                dbCommand.CommandType = CommandType.Text;
                dbCommand.CommandText = "SELECT COUNT(*) FROM itemsStatus WHERE ItemID = @ItemID";

                var itemIdParam = dbCommand.CreateParameter();
                itemIdParam.ParameterName = "ItemID";
                itemIdParam.DbType = DbType.Int16;
                itemIdParam.Value = checkId;
                dbCommand.Parameters.Add(itemIdParam);

                dbCommand.Prepare();

                var x = dbCommand.ExecuteScalar() as long?;
                if (x.HasValue && x > 0)
                {
                    dbCommand.CommandText = "UPDATE itemsStatus SET Exported = @Exported, ErrorMessage = @ErrorMessage WHERE ItemID = @ItemID";
                }
                else
                {
                    dbCommand.CommandText = "INSERT INTO itemsStatus (ItemID, Exported, ErrorMessage) VALUES (@ItemID, @Exported, @ErrorMessage)";
                }

                dbCommand.CommandType = CommandType.Text;
                dbCommand.Parameters.Clear();

                itemIdParam = dbCommand.CreateParameter();
                itemIdParam.ParameterName = "ItemID";
                itemIdParam.DbType = DbType.Int16;
                itemIdParam.Value = checkId;
                dbCommand.Parameters.Add(itemIdParam);

                var exportedParam = dbCommand.CreateParameter();
                exportedParam.ParameterName = "Exported";
                exportedParam.DbType = DbType.Int16;
                exportedParam.Value = exported ? 1 : 0;
                dbCommand.Parameters.Add(exportedParam);

                var errorMessageParam = dbCommand.CreateParameter();
                errorMessageParam.ParameterName = "ErrorMessage";
                errorMessageParam.DbType = DbType.String;
                errorMessageParam.Value = errorMessage != null && errorMessage.Length > ErrorMessageMaxLength ? errorMessage.Substring(ErrorMessageMaxLength) : errorMessage;
                dbCommand.Parameters.Add(errorMessageParam);

                dbCommand.Prepare();
                dbCommand.ExecuteNonQuery();

                return (true);
            });
        }

        private T WithDbCommand<T>(Func<IDbCommand, T> func)
        {
            IDbCommand dbCommand = null;
            try
            {
                _dbConnection.Open();
                dbCommand = _dbConnection.CreateCommand();
                return (func(dbCommand));
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


    }
}