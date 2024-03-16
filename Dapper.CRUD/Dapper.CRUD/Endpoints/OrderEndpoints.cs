using Dapper.CRUD.Helpers;
using Dapper.CRUD.Models;

namespace Dapper.CRUD.Endpoints
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this IEndpointRouteBuilder builder)
        {
            var groupBuilder = builder.MapGroup("orders");

            groupBuilder.MapGet("{id}", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                var sql = "SELECT Id, CustomerId, Amount, NumberOfProducts FROM Order WHERE Id = @Id";
                var result = await connection.QueryFirstOrDefaultAsync<Order>(sql, new { Id = id });

                return result != null ? Results.Ok(result) : Results.NotFound();
            });

            groupBuilder.MapPost("", async (Order order, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                var sql = "INSERT INTO [Order] (CustomerId, Amount, NumberOfProducts) VALUES (@CustomerId, @Amount, @NumberOfProducts)";
                var result = await connection.ExecuteAsync(sql, order);

                return Results.Ok();
            });
        }
    }
}
