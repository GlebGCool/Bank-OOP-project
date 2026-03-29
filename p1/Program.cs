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

            #region BankAccountTest
            Console.WriteLine("\t\t~~~BankAccount Tests~~~");
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
            try { frozenAccount.Withdraw(1488); }
            catch(Exception e) { Console.WriteLine($"Ошибка: {e.Message}"); }
            Console.WriteLine("\n\n\n");
            #endregion

            #region PremiumAccountTest
            Console.WriteLine("\t\t~~~PremiumAccount Tests~~~");
            var premium = new PremiumAccount("Anna", Status.Active, Currency.USD, -500, 10);
            premium.Deposit(100);
            premium.Withdraw(300); // Уйдет в минус (овердрафт) с учетом комиссии
            Console.WriteLine(premium.GetAccountInfo());
            Console.WriteLine("\n\n\n");
            #endregion

            #region SavingAccountTest
            Console.WriteLine("\t\t~~~SavingAccount Tests~~~");
            var saving = new SavingAccount("Oleg", Status.Active, Currency.RUB, 1000, 5);
            saving.Deposit(2000);
            Console.WriteLine(saving); // Проверка ToString
            try { saving.Withdraw(1500); } catch (Exception e) { Console.WriteLine(e.Message); } // Ошибка MinBalance
            Console.WriteLine($"Monthly Income: {saving.calculateMonthlyIncome()}");
            Console.WriteLine("\n\n\n");
            #endregion

            #region InvestmentAccountTest
            Console.WriteLine("\t\t~~~InvestAccount Tests~~~");
            // Инвестиционный счет (баланс 5000)
            var investAcc = new InvestmentAccount("Ivan Ivanov", Status.Active, Currency.USD);
            investAcc.Deposit(5000);

            // Покупаем активы на 3200$
            investAcc.BuyAsset(new VirtualAsset("Apple", "Stock", 1200, 15)); // Ожидаем 15% годовых
            investAcc.BuyAsset(new VirtualAsset("Bitcoin", "Crypto", 2000, 50)); // Ожидаем 50% годовых

            // Проверка вычислений
            Console.WriteLine(investAcc.GetAccountInfo());
            Console.WriteLine($"Ожидаемая прибыль за год: {investAcc.projectYearlyGrowth()} {investAcc.Currency}");

            // Проверка вывода через ToString
            Console.WriteLine(investAcc);

            // Попытка снять больше, чем осталось свободного баланса (после покупок осталось 1800)
            try { investAcc.Withdraw(2000); }
            catch (Exception e) { Console.WriteLine($"Ошибка: {e.Message}"); }
            Console.WriteLine("\n\n\n");
            #endregion

        }
    }
}
