Documentation

1. Download SQL Server Management Studio and install SQL Server.
2. Download VendingMachine from github, unzip it and open it in visual studio. 
3. Delete ALL the files from folder VendingMachine.API-master\VeendingMachine.API.DataAccess\Migrations
3. Open Package Manager Console, change default project to VeendingMachine.API.DataAccess, run the following commands:
	add-migration initialMigration
	update-database
4. Check the newly created VendingMachineDB database in SQL Server management studio. Tables will be empty. Data will be seeded when you run the application. 
5. Run application.
6. You will be directed to Swagger website to use Post and Put API endpoints.

How to use swagger:

1. The POST endpoint emulates a product selection from the vending machine. There are 11 products in total. You can enter any product id from 1 to 11.
2. The PUT endpoint represents you paying for the product you selected in the POST endpoint. You have to enter the AMOUNT of coins of each type you have inserted
 into the vending machine. E.g.:

{
  "oneEuroCoinAmount": 0, => 0 represents coin amount of 1 euro denomination. Change 0 to amount you want to pay
  "twoEuroCoinAmount": 0,
  "centDeposit": {
    "fiftyCentCoinAmount": 10, => means you have inserted 10 fifty cent coins into the vending machine, i.e. 50 cents x 10 = 5 euros.  
    "tenCentCoinAmount": 0,
    "fiveCentCoinAmount": 0,
    "twoCentCoinAmount": 0,
    "oneCentCoinAmount": 0
  }
}

3. Response body to your PUT endpoint represents the change that you got.

{
  "5": 2, => 5 represents 5 cent denomination, 2 represents the amount of coins received as change, you have received 5cents x 2 = 10 cents
  "20": 1,
  "50": 1,
  "100": 3 => 100 represents 1 euro denomination, 3 represents the amount of coins you received as change, you have received 100 cents x 3 = 3 euro coins 
}
