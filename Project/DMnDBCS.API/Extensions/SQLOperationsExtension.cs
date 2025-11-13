using Npgsql;

namespace DMnDBCS.API.Extensions
{
    internal static class SQLOperationsExtension
    {
        public static async Task<bool> CreateDBEntity(this NpgsqlConnection connection, string procedureName, params object?[] arguments)
        {
            var formattedArgs = arguments.Select(arg =>
                NeedsToBeString(arg) ? $"'{arg?.ToString().Replace("'", "''")}'" : arg?.ToString() ?? "NULL"
            );

            string args = string.Join(", ", formattedArgs);

            await connection.OpenAsync();

            try
            {
                var sql = $"CALL {procedureName}({args});";
                using var command = new NpgsqlCommand(sql, connection);
                await command.ExecuteNonQueryAsync();
                
                return true;
            }
            catch (NpgsqlException ex)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public static async Task<IEnumerable<T>> QueryDBEntities<T>(this NpgsqlConnection connection, string procedureName, Func<NpgsqlDataReader, T> mapper, params object?[] arguments)
        {
            var formattedArgs = arguments.Select(arg =>
                NeedsToBeString(arg) ? $"'{arg?.ToString().Replace("'", "''")}'" : arg?.ToString() ?? "NULL"
            );

            string args = string.Join(", ", formattedArgs);
            var results = new List<T>();

            await connection.OpenAsync();

            try
            {
                var sql = $"CALL {procedureName}({args}{(args == string.Empty ? "" : ", ")}'cursor'); FETCH ALL FROM cursor;";
                using var command = new NpgsqlCommand(sql, connection);
                using var reader = await command.ExecuteReaderAsync();

                await reader.NextResultAsync();
                while (await reader.ReadAsync())
                {
                    results.Add(mapper(reader));
                }

                return results;
            }
            catch (NpgsqlException ex)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public static async Task<T> QueryDBEntity<T>(this NpgsqlConnection connection, string procedureName, Func<NpgsqlDataReader, T> mapper, params object?[] arguments)
        {
            var formattedArgs = arguments.Select(arg =>
                NeedsToBeString(arg) ? $"'{arg?.ToString().Replace("'", "''")}'" : arg?.ToString() ?? "NULL"
            );

            string args = string.Join(", ", formattedArgs);

            await connection.OpenAsync();

            try
            {
                var sql = $"CALL {procedureName}({args}{(args == string.Empty ? "" : ", ")}'cursor'); FETCH ALL FROM cursor;";
                using var command = new NpgsqlCommand(sql, connection);
                using var reader = await command.ExecuteReaderAsync();

                await reader.NextResultAsync();
                if (await reader.ReadAsync())
                {
                    var entity = mapper(reader);
                    return entity;
                }

                throw new ArgumentException($"No entity of type {typeof(T)} was found");
            }
            catch (NpgsqlException ex)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public static async Task<bool> UpdateDBEntity(this NpgsqlConnection connection, string procedureName, params object?[] arguments)
        {
            var formattedArgs = arguments.Select(arg =>
                NeedsToBeString(arg) ? $"'{arg?.ToString().Replace("'", "''")}'" : arg?.ToString() ?? "NULL"
            );

            string args = string.Join(", ", formattedArgs);

            await connection.OpenAsync();

            try
            {
                var sql = $"CALL {procedureName}({args});";
                using var command = new NpgsqlCommand(sql, connection);
                return await command.ExecuteNonQueryAsync() != 0;
            }
            catch (NpgsqlException ex)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public static async Task<bool> DeleteDBEntity(this NpgsqlConnection connection, string procedureName, params object?[] arguments)
        {
            var formattedArgs = arguments.Select(arg =>
                NeedsToBeString(arg) ? $"'{arg?.ToString().Replace("'", "''")}'" : arg?.ToString() ?? "NULL"
            );

            string args = string.Join(", ", formattedArgs);

            await connection.OpenAsync();

            try
            {
                var sql = $"CALL {procedureName}({args});";
                using var command = new NpgsqlCommand(sql, connection);
                return await command.ExecuteNonQueryAsync() != 0;
            }
            catch (NpgsqlException ex)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        private static bool NeedsToBeString(object? arg)
        {
            return arg is string || arg is DateOnly || arg is DateTime;
        }
    }
}
