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

            builder.MapGet("customers/{id}", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
  
                var sql = "SELECT Id, FirstName, LastName, Email, DateOfBirth FROM Customer WHERE Id = @Id";
                var result = await connection.QueryFirstOrDefaultAsync<Customer>(sql, new { Id = id });

                return result != null ? Results.Ok(result): Results.NotFound();
            });

            builder.MapPost("customers", async (Customer customer, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                var sql = "INSERT INTO Customer (Id, FirstName, LastName, Email, DateOfBirth) VALUES (@Id, @FirstName, @LastName, @Email, @DateOfBirth)";
                var result = await connection.ExecuteAsync(sql, customer);

                return Results.Ok();
            });

            builder.MapPut("customers", async (Customer customer, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                var sql = "UPDATE Customer SET FirstName = @FirstName, LastName = @LastName, Email = @Email, DateOfBirth = @DateOfBirth WHERE Id = @Id";
                var result = await connection.ExecuteAsync(sql, customer);

                return Results.NoContent();
            });

            builder.MapDelete("customers/{id}", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                var sql = "DELETE FROM Customer WHERE Id = @Id";
                var result = await connection.ExecuteAsync(sql, new { Id = id });

                return Results.NoContent();
            });
        }
    }
}
