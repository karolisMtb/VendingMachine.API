namespace VeendingMachine.API.DataAccess.Model
{
    public class Deposit
    {
        public int OneEuroCoinAmount { get; set; }
        public int TwoEuroCoinAmount { get; set; }
        public CentDeposit? CentDeposit { get; set; }
    }
}
