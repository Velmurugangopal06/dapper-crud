namespace Dapper.CRUD.Models.Response
{
    public class OrderMinimizedResponse
    {
        public int OrderId { get; set; }
        public double Amount { get; set; }
        public int NumberOfProducts { get; set; }
    }
}
