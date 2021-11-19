using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DotnetBoilerplate.Infrastructure.Interfaces;

namespace DotnetBoilerplate.Infrastructure.Database
{
	public class DbConnectionWrapper : IDbConnectionWrapper
	{
		private readonly IDbConnection _connection;

		public DbConnectionWrapper(IDbConnection connection)
		{
			_connection = connection;
		}

		public Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
		{
			return _connection.ExecuteAsync(new CommandDefinition(sql, param, transaction, commandTimeout, commandType, cancellationToken: cancellationToken));
		}

		public async Task<int> ExecuteScalarAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
		{
			var result = await _connection.ExecuteScalarAsync(new CommandDefinition(sql, param, transaction, commandTimeout, commandType, cancellationToken: cancellationToken));
			return (int)result;
		}

		public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
		{
			return _connection.QueryAsync<T>(new CommandDefinition(sql, param, transaction, commandTimeout, commandType, cancellationToken: cancellationToken));
		}

		public Task<T> LoadAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
		{
			return _connection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, param, transaction, commandTimeout, commandType, cancellationToken: cancellationToken));
		}

		public void Open()
		{
			_connection.Open();
		}

		public void Close()
		{
			_connection.Close();
		}

		public IDbTransaction BeginTransaction()
		{
			return _connection.BeginTransaction();
		}

		public ConnectionState GetConnectionState() => _connection.State;
	}
}
