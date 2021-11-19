using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetBoilerplate.Infrastructure.Interfaces
{
    public interface IDbConnectionWrapper
    {
        Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default);

        Task<int> ExecuteScalarAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default);

        Task<T> LoadAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default);

        void Open();

        void Close();

        IDbTransaction BeginTransaction();

        ConnectionState GetConnectionState();
    }
}
