using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p1.AccountExceptions;

namespace p1
{
    internal class Client
    {
        public enum ClientStatus
        {
            Active,
            Frozen,
            Blocked
        }
        public string FIO {  get; protected set; }
        public Guid Id { get; protected set; }
        public ClientStatus Status { get; protected set; }
        public List<BankAccount> accounts { get; protected set; }
        public int age { get; protected set; }
        public string mail { get; protected set; }
        private string _password;
        public int LoginAttempts { get; set; } = 0; // Счетчик попыток
        public bool isAuthenticated { get; protected set; }

        public Client(string FIO,string password, ClientStatus status, int  age, string mail, Guid? id = null)
        {
            this.FIO = FIO.Trim();
            this._password = password;
            Status = status;
            accounts = new List<BankAccount>();
            this.age = age >= 18 ? age : throw new AgeRestrictionException(age);
            this.mail = mail.Trim();
            Id = id ?? Guid.NewGuid();

        }
        public bool CheckPassword(string input)
        {
            if (isAuthenticated)
                return true;
            if (LoginAttempts > 3)
            {
                Status = ClientStatus.Blocked;
                return false;
            }
            if (input == _password)
            {
                isAuthenticated = true;
                return true;
            }
            LoginAttempts+=1;
            return false;
        }

        public bool ComparePassword(string input)
        {
            if(input ==  _password) return true;
            return false;
        }
    }
}
