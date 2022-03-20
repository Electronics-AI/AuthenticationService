using System;
using System.Data;
using System.Threading.Tasks;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;


namespace AuthenticationService.Infrastructure.UnitsOfWork.Dapper
{
    public abstract class DapperBaseUnitOfWork : BaseUnitOfWork, IUnitOfWork
    {   
        protected IDbConnection _dbConnection;
        protected IDbTransaction _dbTransaction;

        public DapperBaseUnitOfWork(IDbConnection dbConnection)
        {
            this._dbConnection = dbConnection;
            _dbConnection.Open();

            _dbTransaction = _dbConnection.BeginTransaction();
        }
    

        public override Task CompleteAsync()
        {
            try {
                _dbTransaction.Commit();
            }
            catch {
                _dbTransaction.Rollback();
                throw;
            }
            finally {
                _dbTransaction.Dispose();
                _dbTransaction = _dbConnection.BeginTransaction();
                resetRepositories();
            }

            return Task.CompletedTask;
        }

        protected override Task disposeAsync(bool disposing)
        {

            if (disposing) {
                _dbTransaction?.Dispose();
                _dbTransaction = null;

                _dbConnection?.Dispose();
                _dbConnection = null;
            }

            return Task.CompletedTask;
        }
    }
}