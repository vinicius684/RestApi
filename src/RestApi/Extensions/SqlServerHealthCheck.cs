using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DevIO.Api.Extensions
{
    public class SqlServerHealthCheck : IHealthCheck
    {
        readonly string _connection;

        public SqlServerHealthCheck(string connection)
        {
            _connection = connection;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                using (var connection = new SqlConnection(_connection))//abrir um sql connection com a conexão passada
                {
                    await connection.OpenAsync(cancellationToken);//abrir a coneção com o banco

                    var command = connection.CreateCommand();//criar um command
                    command.CommandText = "select count(id) from produtos";// desse command fazer um Select Count nos produtos

                    //converter o resultado para ToInt32, para que o ExecuteScalar retorne um numero do count,s e for maior que 0 Healthy, senão Unhealth
                    return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken)) < 0 ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
                }
            }
            catch (Exception)
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}