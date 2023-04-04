Instructions how to start using VendingMachine.API V1

1. https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16
	=> Download SSMS link
2. Download VendingMaschine as zip file and unzip
3. In Visual studio => Package Manager Console type "update-database"
4. All database tables will be created
5. Run application
6. You will be directed to Swagger website to use Post and Put API endpoints.

What to follow:

1. Post endpoint awaits product id input.
2. Put endpoint awaits payment for the product received from post endpoint.
3. Following is to edit:


{
  "oneEuroCoinAmount": 0, => 0 represents coin amount of 1 euro denomination. Change 0 to amount you want to pay
  "twoEuroCoinAmount": 0,
  "centDeposit": {
    "fiftyCentCoinAmount": 0, => 0 represents coin amount of 50 cent denomination. Change 0 to amount you want to pay
    "twentyCentCoinAmount": 0,
    "tenCentCoinAmount": 0,
    "fiveCentCoinAmount": 0,
    "twoCentCoinAmount": 0,
    "oneCentCoinAmount": 0
  }
}

4. Response body as JSON object is to follow:
Dictionary<TKey, TValue> changeResult

{
  "5": 1, => "5" represents 5 cent denomination. => :1 represents amount of coins received as change
  "20": 1,
  "50": 1,
  "100": 1 => 100 represents 100 cents times 1 (coin amount)
}

