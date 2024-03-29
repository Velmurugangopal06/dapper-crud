﻿using Dapper.CRUD.Helpers;
using Dapper.CRUD.Models;
using Dapper.CRUD.Models.Response;

namespace Dapper.CRUD.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder builder)
        {
            var groupBuilder = builder.MapGroup("customers");

            groupBuilder.MapGet("", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                var sql = "SELECT Id, FirstName, LastName, Email, DateOfBirth FROM Customer";
                var result = await connection.QueryAsync<Customer>(sql);

                return Results.Ok(result);
            });

            groupBuilder.MapGet("{id}", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();
  
                var sql = "SELECT Id, FirstName, LastName, Email, DateOfBirth FROM Customer WHERE Id = @Id";
                var result = await connection.QueryFirstOrDefaultAsync<Customer>(sql, new { Id = id });

                return result != null ? Results.Ok(result): Results.NotFound();
            });

            groupBuilder.MapPost("", async (Customer customer, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                var sql = "INSERT INTO Customer (Id, FirstName, LastName, Email, DateOfBirth) VALUES (@Id, @FirstName, @LastName, @Email, @DateOfBirth)";
                var result = await connection.ExecuteAsync(sql, customer);

                return Results.Ok();
            });

            groupBuilder.MapPut("", async (Customer customer, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                var sql = "UPDATE Customer SET FirstName = @FirstName, LastName = @LastName, Email = @Email, DateOfBirth = @DateOfBirth WHERE Id = @Id";
                var result = await connection.ExecuteAsync(sql, customer);

                return Results.NoContent();
            });

            groupBuilder.MapDelete("", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                var sql = "DELETE FROM Customer WHERE Id = @Id";
                var result = await connection.ExecuteAsync(sql, new { Id = id });

                return Results.NoContent();
            });

            groupBuilder.MapGet("orders", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
            {
                var result = new CustomerMinimizedResponse();
                using var connection = sqlConnectionFactory.Create();

                var sql = """
                    SELECT C.Id, C.Email, O.Id AS OrderId, O.Amount, O.NumberOfProducts
                    FROM Customer C 
                    INNER JOIN [Order] O ON O.CustomerId = C.Id
                    WHERE C.Id = @Id
                """;
                await connection.QueryAsync<CustomerMinimizedResponse, OrderMinimizedResponse, CustomerMinimizedResponse>(
                    sql,
                    (customer, order) =>
                    {
                        result = result.Email?.Length > 0 ? result: customer;
                        if(result.Orders == null) 
                            result.Orders = new List<OrderMinimizedResponse>();
                        result.Orders.Add(order);
                        return result;
                    },
                    new
                    {
                        id
                    },
                    splitOn: "OrderId");

                return Results.Ok(result);
            });
        }
    }
}
