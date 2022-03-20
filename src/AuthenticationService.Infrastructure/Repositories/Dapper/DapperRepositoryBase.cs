using System.Data;

namespace AuthenticationService.Infrastructure.Repositories.Dapper
{
    public abstract class DapperRepositoryBase
    {
        protected IDbConnection DbConnection { get { return this.DbTransaction.Connection; } }
        protected IDbTransaction DbTransaction { get; private set; }
        
        protected DapperRepositoryBase(IDbTransaction dbTransaction)
        {
            this.DbTransaction = dbTransaction;
        }
    }
}   