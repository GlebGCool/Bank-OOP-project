using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p1.AccountExceptions;

namespace p1
{
    internal class SavingAccount : BankAccount
    {
        private decimal _minBalance { get; set; }
        public decimal monthlyRate { get; protected set; }

        public SavingAccount(string Owner, Status Status, Currency currency, decimal minBalance, decimal MonthlyRate, Guid? id = null) : base(Owner, Status, currency, id)
        {
            _minBalance = minBalance < 0 ? 0 : minBalance;
            monthlyRate = MonthlyRate < 0 ? 0 : MonthlyRate;
        }

        public override void Withdraw(decimal amount)
        {
            // Новая проверка для SavingAccount
            if (_balance - amount < _minBalance)
            {
                throw new InvalidAccountOperationException("Нельзя снять сумму: баланс станет ниже минимального лимита.");
            }

            // Вызов базовой логики (там пройдут ValidateOperation и проверка на баланс)
            base.Withdraw(amount);
        }

        public decimal calculateMonthlyIncome()
        {
            return _balance + (_balance * (monthlyRate / 100));
        }
        public override string GetAccountInfo() =>
        base.GetAccountInfo() + $" | MinBalance: {_minBalance} | Rate: {monthlyRate}%";

        public override string ToString() => $"[Saving] {Owner} | Balance: {_balance} {Currency}";
    }
}
