using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p1.AccountExceptions;

namespace p1
{
    enum Currency
    {
        RUB,
        USD,
        EUR,
        KZT,
        CNY
    }
    internal class BankAccount : AbstractAccount
    {
        public Currency Currency {  get; private set; }
        public BankAccount(string Owner, Status Status, Currency currency, Guid? id = null) : base(Owner, Status)
        {
            Id = id ?? Guid.NewGuid();
            Currency = currency;
        }

        // Ввод
        public override void Deposit(decimal amount)
        {
            ValidateOperation(amount);
            _balance += amount;
        }

        // Вывод
        public override void Withdraw(decimal amount)
        {
            ValidateOperation(amount);

            if (_balance < amount)
                throw new InsufficientFundsException(_balance, amount);

            _balance -= amount;
        }
        // Проверка операции на валидность
        private void ValidateOperation(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidAccountOperationException("Сумма должна быть положительной.");

            if (Status != Status.Active)
                throw new InvalidAccountOperationException($"Операция запрещена. Статус счета: {Status}");
        }

        // Возвращает строку со статусом аккаунта
        public string GetAccountStatus()
        {
            return (this.Status.ToString());
        }

        // Возвращает строку с основной информацией аккаунта
        public override string GetAccountInfo()
        {
            return $"Owner: {Owner} | ID: ***{Id.ToString().Substring(Id.ToString().Length - 4)} | Balance: {_balance} {Currency} | Status: {Status}";
        }

    }
}
