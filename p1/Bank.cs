using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static p1.Client;

namespace p1
{
    internal class Bank
    {
        private List<Client> clients;
        public Bank() {  clients = new List<Client>(); }

        public void add_client(Client client) { clients.Add(client); }

        public void authenticateClient(Client client, string password)
        {
            while(client.CheckPassword(password) == false && client.LoginAttempts <= 3) { }
        }

        // Открыть счёт для клиента
        public void open_account(Client client, BankAccount account)
        {
            if (client.isAuthenticated == false)
            {
                Console.WriteLine("Ошибка. Перед началом действий с клиентом аутентифицируйтесь.");
                return;
            }
            if (client == null) throw new Exception("Клиент не указан");

            // Проверяем, есть ли такой клиент вообще в банке
            if (!clients.Contains(client))
                throw new Exception("Этот клиент не зарегистрирован в нашем банке!");

            client.accounts.Add(account);
            Console.WriteLine($"Успех! Для {client.FIO} открыт счет {account.GetType().Name}");
        }

        // Поиск всех счетов клиента
        public void searchAccounts(Client client)
        {
            for (int i = 0; i < client.accounts.Count; i++)
            {
                var acc = client.accounts[i];
                // Показываем тип счета, ID и баланс
                Console.WriteLine($"ID:[{i}] {acc.GetAccountInfo()}");
            }
        }

        // Закрыть счёт клиента
        public void close_account(Client client)
        {
            if (client.isAuthenticated == false)
            {
                Console.WriteLine("Ошибка. Перед началом действий с клиентом аутентифицируйтесь.");
                return;
            }
            Console.WriteLine($"\n--- Закрытие счета для клиента: {client.FIO} ---");

            // Проверяем, есть ли что закрывать
            if (client.accounts.Count == 0)
            {
                Console.WriteLine("У клиента нет открытых счетов.");
                return;
            }

            // Выводим список счетов этого клиента
            searchAccounts(client);

            // Запрос выбора
            Console.Write("\nВыберите номер счета для ЗАКРЫТИЯ: ");
            if (int.TryParse(Console.ReadLine(), out int index))
            {
                if (index >= 0 && index < client.accounts.Count)
                {
                    Guid removedId = client.accounts[index].Id;

                    // Удаляем счет из списка клиента
                    client.accounts.RemoveAt(index);

                    Console.WriteLine($"Счет {removedId} удален.");
                }
                else
                {
                    Console.WriteLine("Неверный номер счета.");
                }
            }
            else
            {
                Console.WriteLine("Введите число.");
            }
        }

        // Заморозка счёта клиента
        public void freeze_account(Client client)
        {
            Console.WriteLine($"\n--- Заморозка счета для клиента: {client.FIO} ---");
            if (client.accounts.Count == 0)
            {
                Console.WriteLine("У клиента нет открытых счетов.");
                return;
            }
            searchAccounts(client);
            Console.Write("\nВыберите номер счета для заморозки: ");
            if (int.TryParse(Console.ReadLine(), out int index))
            {
                if (index >= 0 && index < client.accounts.Count)
                {
                    Guid removedId = client.accounts[index].Id;

                    // Замораживаем счет из списка клиента
                    client.accounts[index].Status = Status.Frozen;

                    Console.WriteLine($"Счет {removedId} заморожен.");
                }
                else
                {
                    Console.WriteLine("Неверный номер счета.");
                }
            }
            else
            {
                Console.WriteLine("Введите число.");
            }
        }

        // Разморозка счёта клиента
        public void unfreeze_account(Client client)
        {
            Console.WriteLine($"\n--- Разморозка счета для клиента: {client.FIO} ---");
            if (client.accounts.Count == 0)
            {
                Console.WriteLine("У клиента нет открытых счетов.");
                return;
            }
            searchAccounts(client);
            Console.Write("\nВыберите номер счета для разморозки: ");
            if (int.TryParse(Console.ReadLine(), out int index))
            {
                if (index >= 0 && index < client.accounts.Count)
                {
                    Guid removedId = client.accounts[index].Id;

                    // Размораживаем счет из списка клиента
                    client.accounts[index].Status = Status.Active;

                    Console.WriteLine($"Счет {removedId} разморожен.");
                }
                else
                {
                    Console.WriteLine("Неверный номер счета.");
                }
            }
            else
            {
                Console.WriteLine("Введите число.");
            }
        }

        public void BankDeposit(Client client)
        {
            // Проверка на блокировку
            if (client.Status == ClientStatus.Blocked)
            {
                Console.WriteLine("Действие невозможно. Клиент заблокирован службой безопасности.");
                return;
            }

            // Проверка на вход
            if (client.isAuthenticated == false)
            {
                Console.WriteLine("Ошибка. Перед началом действий с клиентом аутентифицируйтесь.");
                return;
            }

            if (client.accounts.Count == 0)
            {
                Console.WriteLine("У клиента нет открытых счетов.");
                return;
            }

            for (int i = 0; i < client.accounts.Count; i++)
            {
                Console.WriteLine($"[{i}] {client.accounts[i].GetAccountInfo()}");
            }

            Console.Write("Выберите номер счета для пополнения: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < client.accounts.Count)
            {
                try
                {
                    Console.Write("Введите сумму пополнения: ");
                    decimal amount = decimal.Parse(Console.ReadLine());
                    client.accounts[index].Deposit(amount);
                    Console.WriteLine("Пополнение успешно.");
                }
                catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            }
        }

        public void BankWithdraw(Client client)
        {
            // Проверка на блокировку
            if (client.Status == ClientStatus.Blocked)
            {
                Console.WriteLine("Снятие средств невозможно. Клиент полностью заблокирован.");
                return;
            }

            // Проверка аутентификации
            if (client.isAuthenticated == false)
            {
                Console.WriteLine("Ошибка. Перед началом действий с клиентом аутентифицируйтесь.");
                return;
            }

            // Проверка Ночного времени
            int hour = DateTime.Now.Hour;
            if (hour >= 0 && hour < 5)
            {
                Console.WriteLine("Технический перерыв до 05:00.");
                return;
            }

            if (client.accounts.Count == 0)
            {
                Console.WriteLine("У клиента нет открытых счетов.");
                return;
            }

            for (int i = 0; i < client.accounts.Count; i++)
            {
                Console.WriteLine($"[{i}] {client.accounts[i].GetAccountInfo()}");
            }

            Console.Write("Выберите номер счета для снятия: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < client.accounts.Count)
            {
                var selectedAccount = client.accounts[index];

                Console.Write("Введите сумму снятия: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                if (amount > 1000)
                {
                    Console.WriteLine("--- ПОДОЗРИТЕЛЬНАЯ ОПЕРАЦИЯ ---");
                    Console.Write("Сумма превышает 1000. Введите ваш пароль еще раз: ");
                    string confirmPass = Console.ReadLine();

                    if (client.ComparePassword(confirmPass) == false)
                    {
                        Console.WriteLine("Ошибка безопасности! Неверный пароль. Операция отменена.");
                        return;
                    }
                }

                try
                {
                    selectedAccount.Withdraw(amount);
                    Console.WriteLine("Снятие прошло успешно!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }
    }
}
