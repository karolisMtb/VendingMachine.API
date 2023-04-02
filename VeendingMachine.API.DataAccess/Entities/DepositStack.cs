using System.ComponentModel.DataAnnotations;

namespace VeendingMachine.API.DataAccess.Entities
{
    public class DepositStack
    {
        public Guid Id { get; set; }
        public int Amount { get; set; } // 100, 100 vientu of denomination(10, 20, 10, 2, 1, 5)
        public MoneyUnit MoneyUnit { get; set; } // euro cent
        public int Denomination { get; set; } // 1,2,5,10,20,50,1 eur, 2 eur
        public DateTime Timestamp { get; set; }
    }
}
