using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotnetBoilerplate.Infrastructure.Interfaces;

namespace DotnetBoilerplate.Infrastructure.Repository
{
    public class BaseRepository
    {
		private readonly IDbConnectionWrapper _connection;

		public BaseRepository(IDbConnectionWrapper connection)
		{
			_connection = connection;
		}

		public async Task<IEnumerable<T>> GetCollectionAsync<T>(string query, object input = null)
		{
			try
			{
				if (_connection.GetConnectionState() == ConnectionState.Open)
				{
					return await _connection.QueryAsync<T>(query, input);

				}
				else
				{
					_connection.Open();

					var result = await _connection.QueryAsync<T>(query, input);

					_connection.Close();

					return result;
				}
			}
			catch
			{
				throw;
			}
		}

		public async Task<bool> ExecuteAsync(string query, object input, CancellationToken cancellationToken)
		{
			try
			{
				if (input != null)
				{
					_connection.Open();
					using (var transaction = _connection.BeginTransaction())
					{
						var result = await _connection.ExecuteAsync(query, input, transaction, cancellationToken: cancellationToken);

						transaction.Commit();
						_connection.Close();

						return result > 0;
					}
				}
				else
				{
					return false;
				}
			}
			catch
			{
				throw;
			}
		}

		public async Task<int> ExecuteScalarAsync(string query, object input)
		{
			try
			{
				if (input != null)
				{
					_connection.Open();
					using (var transaction = _connection.BeginTransaction())
					{
						var result = await _connection.ExecuteScalarAsync(query, input, transaction);

						transaction.Commit();
						_connection.Close();

						return Convert.ToInt32(result);
					}
				}
				else
				{
					return 0;
				}
			}
			catch
			{
				throw;
			}
		}

		public async Task<T> GetAsync<T>(string query, object input)
		{
			try
			{
				_connection.Open();
				var result = await _connection.QueryAsync<T>(query, input);
				_connection.Close();
				return result.FirstOrDefault();
			}
			catch
			{
				throw;
			}
		}

		public async Task<T> LoadAsync<T>(string query, object input, CancellationToken cancellationToken)
		{
			try
			{
				_connection.Open();
				var result = await _connection.LoadAsync<T>(query, input, cancellationToken: cancellationToken);
				_connection.Close();
				return result;
			}
			catch
			{
				throw;
			}
		}

		public virtual async Task ExecuteInTransactionAsync<T>(Func<T, IDbTransaction, Task> func, T @object)
		{
			try
			{
				_connection.Open();

				using (var transaction = _connection.BeginTransaction())
				{
					await func.Invoke(@object, transaction);

					transaction.Commit();
				}

				_connection.Close();
			}
			catch
			{
				throw;
			}
		}

		public virtual async Task ExecuteInTransactionAsync<T>(Func<T, T, IDbTransaction, Task> func, T @object, T @object2)
		{
			try
			{
				_connection.Open();

				using (var transaction = _connection.BeginTransaction())
				{
					await func.Invoke(@object, object2, transaction);

					transaction.Commit();
				}

				_connection.Close();
			}
			catch
			{
				throw;
			}
		}
	}
}
