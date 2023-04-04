using VeendingMachine.API.DataAccess.Model;

namespace VendingMachine.API.BusinessLogic
{
    public static class Utilities
    {
        public static decimal ConvertDepositIntoDecimal(Deposit deposit)
        {
            decimal sumOutOfEuroCoins = (deposit.OneEuroCoinAmount * 100 + deposit.TwoEuroCoinAmount * 200)/100;

            decimal sumOutOfCentCoins = 0.0M;

            if (deposit.CentDeposit != null)
            {
                sumOutOfCentCoins = (deposit.CentDeposit.oneCentCoinAmount * 1
                                    + deposit.CentDeposit.twoCentCoinAmount * 2
                                    + deposit.CentDeposit.fiveCentCoinAmount * 5
                                    + deposit.CentDeposit.tenCentCoinAmount * 10
                                    + deposit.CentDeposit.twentyCentCoinAmount * 20
                                    + deposit.CentDeposit.fiftyCentCoinAmount * 50) / 100.0M;
            }

            return sumOutOfEuroCoins + sumOutOfCentCoins;
        }

    }
}
