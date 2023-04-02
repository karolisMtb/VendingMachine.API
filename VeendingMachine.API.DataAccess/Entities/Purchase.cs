namespace VeendingMachine.API.DataAccess.Entities
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public Product Product { get; set; }
        public bool Paid { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
