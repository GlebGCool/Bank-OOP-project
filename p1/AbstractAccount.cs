using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p1
{
     enum Status{
         Active,
         Frozen
     }
    internal abstract class AbstractAccount
    {
        public Guid Id { get; protected set; }
        public string Owner { get; protected set; }
        protected decimal _balance;
        public Status Status { get; set; }

        public AbstractAccount (string owner, Status Status)
        {
            if (string.IsNullOrWhiteSpace(owner))
                throw new ArgumentException("Владелец должен быть указан.");
            this.Owner = owner;
            this.Status = Status;
        }

        public abstract void Deposit(decimal amount);
        public abstract void Withdraw(decimal amount);
        public abstract string GetAccountInfo();



    }
}
