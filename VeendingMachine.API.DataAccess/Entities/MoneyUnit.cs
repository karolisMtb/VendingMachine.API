using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;

namespace VeendingMachine.API.DataAccess.Entities
{
    public class MoneyUnit
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<DepositStack> DepositStack { get; set; }
    }
}
