using System.Data.SqlClient;
using Dapper;


namespace PetCareAPI.Service
{
    public class DatabaseService
    {
        private readonly IConfiguration _config;
        public DatabaseService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("PetCareConnection"));
            await connection.OpenAsync();
            return await connection.QueryAsync<T>(sql, parameters);
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object parameters = null)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("PetCareConnection"));
            await connection.OpenAsync();
            return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("PetCareConnection"));
            await connection.OpenAsync();
            return await connection.ExecuteAsync(sql, parameters);
        }
    }
}
