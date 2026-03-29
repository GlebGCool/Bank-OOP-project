using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p1.AccountExceptions;

namespace p1
{
    internal class PremiumAccount : BankAccount
    {
        private decimal _overdraftLimit {  get; set; }
        private decimal _fixedFee {  get; set; }
        public PremiumAccount(string Owner, Status Status, Currency currency, decimal overdraftLimit, decimal fixedFee, Guid? id = null) : base(Owner, Status, currency, id)
        {
            _overdraftLimit = overdraftLimit > 0 ? -overdraftLimit : overdraftLimit;
            _fixedFee = fixedFee < 0 ? 0 : fixedFee;
        }

        public override void Withdraw(decimal amount)
        {
            // Итоговая сумма с учётом комиссии
            decimal totalAmount = amount + _fixedFee;
        
            if (_balance - totalAmount < _overdraftLimit)
                throw new InvalidAccountOperationException("Превышен лимит овердрафта.");

        base.ValidateOperation(amount);
        _balance -= totalAmount;
        }

        public override string GetAccountInfo() =>
        base.GetAccountInfo() + $" | Overdraft: {_overdraftLimit} | Fee: {_fixedFee}";

        public override string ToString() => $"[Premium] {Owner} | Balance: {_balance} {Currency}";
    }
}
