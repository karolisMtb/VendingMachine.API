namespace VeendingMachine.API.DataAccess.Entities
{
    public class DepositStack
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public MoneyUnit MoneyUnit { get; set; }
        public int Denomination { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
