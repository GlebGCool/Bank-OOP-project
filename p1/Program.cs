using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            BankAccount account = new BankAccount("Pupa", Status.Active, Currency.RUB);
            BankAccount frozenAccount = new BankAccount("Lupa", Status.Frozen, Currency.EUR);

            
            // Вывод информации об активном аккаунте
            Console.WriteLine(account.GetAccountInfo());

            // Пополнение баланса аккаунта
            account.Deposit(1488);
            Console.WriteLine(account.GetAccountInfo());
            // Снятие средств с аккаунта
            account.Withdraw(1337);
            Console.WriteLine(account.GetAccountInfo());

            // Вывод информации о замороженном аккаунте
            Console.WriteLine(frozenAccount.GetAccountInfo());

            // Попытка снаятия средств с замороженного аккаунта (прокидывается InvalidAccountOperationException)
            frozenAccount.Withdraw(1488);

        }
    }
}
