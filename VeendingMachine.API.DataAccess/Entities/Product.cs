namespace VeendingMachine.API.DataAccess.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public int ProductNumber { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; }
        public int Count { get; set; }
        public ICollection<Purchase>? Purchase { get; set; }
    }
}
