namespace Dapper.CRUD.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public double Amount { get; set; }
        public int NumberOfProducts { get; set; }
    }
}
