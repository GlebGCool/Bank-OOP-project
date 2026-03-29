using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p1
{
    class AccountExceptions
    {
        // Базовый класс для всех исключений аккаунта
        public abstract class AccountException : Exception
        {
            protected AccountException(string message) : base(message) { }
        }

        // Счёт заморожен
        public class AccountFrozenException : AccountException
        {
            public AccountFrozenException(string message = "Счет заморожен. Операции ограничены.")
                : base(message) { }
        }

        // Счёт закрыт
        public class AccountClosedException : AccountException
        {
            public AccountClosedException(string message = "Счет закрыт.")
                : base(message) { }
        }

        // Неверная операция (например, отрицательная сумма)
        public class InvalidAccountOperationException : AccountException
        {
            public InvalidAccountOperationException(string message) : base(message) { }
        }

        // Недостаточно средств
        public class InsufficientFundsException : AccountException
        {
            public decimal CurrentBalance { get; }
            public decimal RequestedAmount { get; }

            public InsufficientFundsException(decimal balance, decimal requested)
                : base($"Недостаточно средств. Баланс: {balance}, запрошено: {requested}")
            {
                CurrentBalance = balance;
                RequestedAmount = requested;
            }
        }
    }
}
