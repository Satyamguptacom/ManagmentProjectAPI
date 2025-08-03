using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace ProjectManagementAPI.Data
{
    //public class SqlHelper
    //{
    //    private readonly string _connectionString;

    //    public SqlHelper(IConfiguration configuration)  
    //    {
    //        _connectionString = configuration.GetConnectionString("DefaultConnection");
    //    }

    //    public async Task<DataTable> ExecuteQueryAsync(string query, params SqlParameter[] parameters)
    //    {
    //        using var conn = new SqlConnection(_connectionString);
    //        using var cmd = new SqlCommand(query, conn);
    //        cmd.Parameters.AddRange(parameters);
    //        using var adapter = new SqlDataAdapter(cmd);
    //        var dt = new DataTable();
    //        await conn.OpenAsync();
    //        adapter.Fill(dt);
    //        return dt;
    //    }

    //    public async Task<int> ExecuteNonQueryAsync(string query, params SqlParameter[] parameters)
    //    {
    //        using var conn = new SqlConnection(_connectionString);
    //        using var cmd = new SqlCommand(query, conn);
    //        cmd.Parameters.AddRange(parameters);
    //        await conn.OpenAsync();
    //        return await cmd.ExecuteNonQueryAsync();
    //    }

    //    public async Task<object> ExecuteScalarAsync(string query, params SqlParameter[] parameters)
    //    {
    //        using var conn = new SqlConnection(_connectionString);
    //        using var cmd = new SqlCommand(query, conn);
    //        cmd.Parameters.AddRange(parameters);
    //        await conn.OpenAsync();
    //        return await cmd.ExecuteScalarAsync();
    //    }
    //}
    public static class SqlHelper
    {

        /// <summary>
        /// Executes a non-query SQL command (e.g., INSERT, UPDATE, DELETE).
        /// This method is used for executing SQL commands that do not return any data, such as modifying the database.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="cmdText">The SQL command text (e.g., INSERT INTO TableName...)</param>
        /// <param name="cmdType">The command type (e.g., Text for raw SQL, StoredProcedure for stored procedures).</param>
        /// <param name="commandParameters">Optional SQL parameters to be used in the command.</param>
        /// <returns>The number of rows affected by the command (e.g., number of rows inserted or updated).</returns>
        /// <exception cref="SqlException">Thrown when there is an error executing the SQL command.</exception>
        /// <exception cref="Exception">Thrown for any unexpected errors.</exception>
        public static int ExecuteNonQuery(string connectionString, string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            try
            {
                // Create a new SQL connection using the provided connection string.
                using (var connection = new SqlConnection(connectionString))
                {
                    // Prepare the SQL command with the given SQL text and connection.
                    using (var command = new SqlCommand(cmdText, connection))
                    {
                        // Set the command type (e.g., Text for raw SQL or StoredProcedure).
                        command.CommandType = cmdType;

                        // Add parameters to the SQL command to prevent SQL injection.
                        command.Parameters.AddRange(commandParameters);

                        // Open the connection to the database.
                        connection.Open();

                        // Execute the non-query command (INSERT, UPDATE, DELETE) and return the number of rows affected.
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log any SQL-related errors for debugging purposes.
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw; // Rethrow the exception for further handling.
            }
            catch (Exception ex)
            {
                // Log any unexpected errors.
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                throw; // Rethrow the exception for further handling.
            }
        }

        /// <summary>
        /// Executes a scalar SQL command (e.g., COUNT, SUM).
        /// This method is used to execute SQL commands that return a single value (e.g., aggregates or queries returning a single result).
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="cmdText">The SQL command text (e.g., SELECT COUNT(*) FROM TableName)</param>
        /// <param name="cmdType">The command type (e.g., Text for raw SQL, StoredProcedure for stored procedures).</param>
        /// <param name="commandParameters">Optional SQL parameters to be used in the command.</param>
        /// <returns>The result of the scalar query (e.g., a single value like COUNT or SUM).</returns>
        /// <exception cref="SqlException">Thrown when there is an error executing the SQL command.</exception>
        /// <exception cref="Exception">Thrown for any unexpected errors.</exception>
        public static object ExecuteScalar(string connectionString, string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            try
            {
                // Create a new SQL connection using the provided connection string.
                using (var connection = new SqlConnection(connectionString))
                {
                    // Prepare the SQL command with the given SQL text and connection.
                    using (var command = new SqlCommand(cmdText, connection))
                    {
                        // Set the command type (e.g., Text for raw SQL or StoredProcedure).
                        command.CommandType = cmdType;

                        // Add parameters to the SQL command to prevent SQL injection.
                        command.Parameters.AddRange(commandParameters);

                        // Open the connection to the database.
                        connection.Open();

                        // Execute the scalar command and return the result (e.g., a single value like COUNT or SUM).
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log any SQL-related errors for debugging purposes.
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw; // Rethrow the exception for further handling.
            }
            catch (Exception ex)
            {
                // Log any unexpected errors.
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                throw; // Rethrow the exception for further handling.
            }
        }


        /// <summary>
        /// Executes a SQL query that returns a data reader.
        /// This method is used to execute queries that return multiple rows of data, such as SELECT statements.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="cmdText">The SQL query to execute (e.g., SELECT * FROM TableName).</param>
        /// <param name="cmdType">The command type (e.g., Text for raw SQL, StoredProcedure for stored procedures).</param>
        /// <param name="commandParameters">An array of SqlParameter objects to be used with the command, if any.</param>
        /// <returns>A SqlDataReader that can be used to read the results of the SQL query.</returns>
        /// <exception cref="SqlException">Thrown when there is an error executing the SQL query.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public static SqlDataReader ExecuteReader(string connectionString, string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            try
            {
                // Create a new SQL connection using the provided connection string.
                var connection = new SqlConnection(connectionString);

                // Prepare the SQL command with the given SQL text and connection.
                var command = new SqlCommand(cmdText, connection)
                {
                    CommandType = cmdType // Set the command type (e.g., Text for raw SQL, StoredProcedure for stored procedures).
                };

                // Add any provided parameters to the SQL command.
                command.Parameters.AddRange(commandParameters);

                // Open the connection to the database.
                connection.Open();

                // Execute the query and return a SqlDataReader to read the results.
                // CommandBehavior.CloseConnection ensures that the connection is closed when the reader is closed.
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                // Log the SQL error message for debugging purposes.
                Console.WriteLine($"SQL Error: {ex.Message}");

                // Rethrow the exception to allow further handling in the calling code.
                throw;
            }
            catch (Exception ex)
            {
                // Log any unexpected error message for debugging purposes.
                Console.WriteLine($"Unexpected Error: {ex.Message}");

                // Rethrow the exception to allow further handling in the calling code.
                throw;
            }
        }


        /// <summary>
        /// Executes a SQL query and returns the results as a DataTable.
        /// This method is used to execute queries that return results, such as SELECT statements.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="cmdText">The SQL query to execute (e.g., SELECT * FROM TableName).</param>
        /// <param name="cmdType">The command type (e.g., Text for raw SQL, StoredProcedure for stored procedures).</param>
        /// <param name="commandParameters">An array of SqlParameter objects to be used with the command, if any.</param>
        /// <returns>A DataTable containing the results of the SQL query.</returns>
        /// <exception cref="SqlException">Thrown when there is an error executing the SQL query.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public static DataTable ExecuteQuery(string connectionString, string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            try
            {
                // Create a new SQL connection using the provided connection string.
                using (var connection = new SqlConnection(connectionString))
                {
                    // Prepare the SQL command with the given SQL text and connection.
                    using (var command = new SqlCommand(cmdText, connection))
                    {
                        // Set the command type (e.g., Text for raw SQL, StoredProcedure for stored procedures).
                        command.CommandType = cmdType;

                        // Add any provided parameters to the SQL command.
                        command.Parameters.AddRange(commandParameters);

                        // Open the connection to the database.
                        connection.Open();

                        // Execute the query and get the data reader to retrieve results.
                        using (var reader = command.ExecuteReader())
                        {
                            // Create a DataTable to hold the results of the query.
                            var dataTable = new DataTable();

                            // Load the data from the reader into the DataTable.
                            dataTable.Load(reader);

                            // Return the populated DataTable containing the query results.
                            return dataTable;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the SQL error message for debugging purposes.
                Console.WriteLine($"SQL Error: {ex.Message}");

                // Rethrow the exception to allow further handling in the calling code.
                throw;
            }
            catch (Exception ex)
            {
                // Log any unexpected error message for debugging purposes.
                Console.WriteLine($"Unexpected Error: {ex.Message}");

                // Rethrow the exception to allow further handling in the calling code.
                throw;
            }
        }


        /// <summary>
        /// Asynchronously executes a non-query SQL command (e.g., INSERT, UPDATE, DELETE) and returns the number of rows affected.
        /// This method is useful for executing commands that modify data in the database and do not return a result set.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="cmdText">The SQL command text to execute (e.g., INSERT INTO TableName ...).</param>
        /// <param name="cmdType">The command type (e.g., Text for raw SQL, StoredProcedure for stored procedures).</param>
        /// <param name="commandParameters">An array of SqlParameter objects to be used with the command, if any.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of the number of rows affected by the SQL command.</returns>
        /// <exception cref="SqlException">Thrown when there is an error executing the SQL command.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public static async Task<int> ExecuteNonQueryAsync(string connectionString, string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            try
            {
                // Create a new SQL connection using the provided connection string.
                using (var connection = new SqlConnection(connectionString))
                {
                    // Prepare the SQL command to execute the non-query.
                    using (var command = new SqlCommand(cmdText, connection))
                    {
                        // Set the command type (e.g., Text for raw SQL, StoredProcedure for stored procedures).
                        command.CommandType = cmdType;

                        // Add any provided parameters to the SQL command.
                        command.Parameters.AddRange(commandParameters);

                        // Open the connection asynchronously.
                        await connection.OpenAsync();

                        // Execute the non-query command asynchronously and return the number of rows affected.
                        return await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the SQL error message for debugging.
                Console.WriteLine($"SQL Error: {ex.Message}");

                // Rethrow the exception to allow further handling in the calling code.
                throw;
            }
            catch (Exception ex)
            {
                // Log any unexpected error message for debugging.
                Console.WriteLine($"Unexpected Error: {ex.Message}");

                // Rethrow the exception to allow further handling in the calling code.
                throw;
            }
        }

        /// <summary>
        /// Asynchronously executes a scalar SQL command (e.g., COUNT, SUM) and returns the result.
        /// This method is useful for executing queries that return a single value (e.g., aggregate functions).
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="cmdText">The SQL command text to execute (e.g., SELECT COUNT(*) or SUM(Column1))</param>
        /// <param name="cmdType">The command type (e.g., Text or StoredProcedure).</param>
        /// <param name="commandParameters">An array of SqlParameter objects to be used with the command, if any.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of the scalar value returned by the SQL command.</returns>
        /// <exception cref="SqlException">Thrown when there is an error executing the SQL command.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public static async Task<object> ExecuteScalarAsync(string connectionString, string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            try
            {
                // Create a new SQL connection using the provided connection string.
                using (var connection = new SqlConnection(connectionString))
                {
                    // Prepare the SQL command to execute the query.
                    using (var command = new SqlCommand(cmdText, connection))
                    {
                        // Set the command type (e.g., Text for raw SQL, StoredProcedure for stored procedures).
                        command.CommandType = cmdType;

                        // Add any provided parameters to the SQL command.
                        command.Parameters.AddRange(commandParameters);

                        // Open the connection asynchronously.
                        await connection.OpenAsync();

                        // Execute the scalar query asynchronously and return the result.
                        // The result will be the first column of the first row in the result set.
                        return await command.ExecuteScalarAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the SQL error message for debugging.
                Console.WriteLine($"SQL Error: {ex.Message}");

                // Rethrow the exception to allow further handling in the calling code.
                throw;
            }
            catch (Exception ex)
            {
                // Log any unexpected error message for debugging.
                Console.WriteLine($"Unexpected Error: {ex.Message}");

                // Rethrow the exception to allow further handling in the calling code.
                throw;
            }
        }


        /// <summary>
        /// Asynchronously executes a SQL query that returns a data reader.
        /// This method is useful for executing queries that return a result set and require streaming access to the data.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="cmdText">The SQL query or command text to execute (e.g., SELECT statement).</param>
        /// <param name="cmdType">The command type (e.g., Text, StoredProcedure).</param>
        /// <param name="commandParameters">An array of SqlParameter objects to be used with the command, if any.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of a SqlDataReader that can be used to read the data.</returns>
        /// <exception cref="SqlException">Thrown when there is an error executing the SQL command.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public static async Task<SqlDataReader> ExecuteReaderAsync(IConfiguration _configuration, string connectionString, string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            try
            {
                // Establish a new SQL connection using the provided connection string.
                var connection = new SqlConnection(connectionString);

                // Prepare the SQL command to execute the query.
                var command = new SqlCommand(cmdText, connection)
                {
                    CommandType = cmdType // Set the command type (e.g., Text or StoredProcedure)
                };

                // Add the provided parameters (if any) to the SQL command.
                command.Parameters.AddRange(commandParameters);

                // Open the connection asynchronously.
                await connection.OpenAsync();

                // Execute the query asynchronously and return a SqlDataReader that can be used to read the data.
                // The CommandBehavior.CloseConnection ensures that the connection is closed when the reader is closed.
                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                // Log the SQL error message to the console for debugging.
                Console.WriteLine($"SQL Error: {ex.Message}");

                // Rethrow the exception to allow further handling in the calling code.
                throw;
            }
            catch (Exception ex)
            {
                // Log any unexpected error messages to the console for debugging.
                Console.WriteLine($"Unexpected Error: {ex.Message}");

                // Rethrow the exception to allow further handling in the calling code.
                throw;
            }
        }


        /// <summary>
        /// Asynchronously executes a SQL query and returns the results as a DataTable.
        /// This method is useful for executing SELECT queries that return a result set.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="cmdText">The SQL query or command text to execute (e.g., SELECT statement).</param>
        /// <param name="cmdType">The command type (e.g., Text, StoredProcedure).</param>
        /// <param name="commandParameters">An array of SqlParameter objects to be used with the command, if any.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of a DataTable containing the result set.</returns>
        /// <exception cref="SqlException">Thrown when there is an error executing the SQL command.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public static async Task<DataTable> ExecuteQueryAsync(IConfiguration _configuration, string connectionString, string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            try
            {
                // Establish a new SQL connection using the provided connection string.
                using (var connection = new SqlConnection(connectionString))
                {
                    // Prepare the SQL command to execute the query.
                    using (var command = new SqlCommand(cmdText, connection))
                    {
                        // Set the command type (e.g., Text or StoredProcedure).
                        command.CommandType = cmdType;

                        // Add the provided parameters (if any) to the SQL command.
                        command.Parameters.AddRange(commandParameters);

                        // Open the connection asynchronously.
                        await connection.OpenAsync();

                        // Execute the query asynchronously and retrieve the result set using a SqlDataReader.
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            // Create a new DataTable to hold the results.
                            var dataTable = new DataTable();

                            // Load the data from the SqlDataReader into the DataTable.
                            dataTable.Load(reader);

                            // Return the populated DataTable.
                            return dataTable;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the SQL error message to the console for debugging.
                Console.WriteLine($"SQL Error: {ex.Message}");

                // Rethrow the exception to allow further handling in the calling code.
                throw;
            }
            catch (Exception ex)
            {
                // Log any unexpected error messages to the console for debugging.
                Console.WriteLine($"Unexpected Error: {ex.Message}");

                // Rethrow the exception to allow further handling in the calling code.
                throw;
            }
        }


        /// <summary>
        /// Executes a stored procedure (non-query) against the provided connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="spName">The name of the stored procedure to execute.</param>
        /// <param name="commandParameters">An array of SqlParameter objects used in the stored procedure.</param>
        /// <returns>The number of rows affected by the stored procedure.</returns>
        public static int ExecuteStoredProcedure(string connectionString, string spName, params SqlParameter[] commandParameters)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(commandParameters);
                        connection.Open();
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the SQL exception details for debugging and analysis.
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw; // Rethrow the exception to be handled by the calling code.
            }
            catch (Exception ex)
            {
                // Log any other unexpected exceptions.
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                throw;
            }
        }

        
    }
}
