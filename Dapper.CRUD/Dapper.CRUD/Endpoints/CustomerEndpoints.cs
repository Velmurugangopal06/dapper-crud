using Dapper.CRUD.Helpers;
using Dapper.CRUD.Models;

namespace Dapper.CRUD.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("customers", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                var sql = "SELECT Id, FirstName, LastName, Email, DateOfBirth FROM Customer";
                var result = await connection.QueryAsync<Customer>(sql);

                return Results.Ok(result);
            });
        }
    }
}
