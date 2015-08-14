using System;
using System.Collections.Generic;
using System.Data;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services.Interfaces;

namespace crds_angular.Services
{
    public class EzScanCheckScannerService : ICheckScannerService
    {
        private readonly IDbConnection _dbConnection;

        public EzScanCheckScannerService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<CheckScannerBatch> GetOpenBatches()
        {
            List<CheckScannerBatch> batches;
            IDataReader reader = null;
            try
            {
                batches = WithDbCommand(dbCommand =>
                {
                    dbCommand.CommandType = CommandType.Text;
                    dbCommand.CommandText = "SELECT ID, IDBatch, DateProcess, BatchStatus FROM batches WHERE COALESCE(BatchStatus, 0) <> 1 ORDER BY DateProcess DESC";

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
                            BatchStatus = reader.IsDBNull(i) || reader.GetInt32(i) == 0 ? BatchStatus.NotExported : BatchStatus.Exported
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
                            Amount = reader[i++] as decimal? ?? 0M,
                            CheckNumber = reader[i++] as string,
                            ScanDate = reader[i++] as DateTime? ?? DateTime.Now,
                            RoutingNumber = reader[i++] as string,
                            Name1 = reader[i++] as string,
                            CheckDate = reader[i++] as DateTime? ?? DateTime.Now,
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

        public CheckScannerBatch UpdateBatchStatus(string batchName, int newStatus)
        {
            throw new NotImplementedException();
        }
    }
}