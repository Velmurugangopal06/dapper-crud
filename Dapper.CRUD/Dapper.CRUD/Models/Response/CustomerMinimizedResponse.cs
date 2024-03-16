namespace Dapper.CRUD.Models.Response
{
    public class CustomerMinimizedResponse
    {
        public int Id { get; set; } 
        public string Email { get; set; }
        public List<OrderMinimizedResponse> Orders { get; set; }
    }
}
