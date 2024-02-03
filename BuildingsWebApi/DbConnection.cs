using System.Data.SqlClient;

namespace BuildingsWebApi
{
    public static class DbConnection
    {
        private static string _connectionString;
        public static string ConectionString => _connectionString;
        public static void InitDbConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(_connectionString)) { _connectionString = connectionString; }
        }

        public static async Task ExecuteQueryAsync(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public static async Task ExecuteQueryReadAsync(string query, Action<SqlDataReader> func)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(query, connection);
                    var reader = await command.ExecuteReaderAsync();
                    func(reader);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }
}
