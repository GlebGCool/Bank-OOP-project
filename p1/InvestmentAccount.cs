using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p1.AccountExceptions;

namespace p1
{
    // Вспомогательный класс для описания виртуального актива
    public class VirtualAsset
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal InvestedAmount { get; set; }
        public decimal YearlyYieldRate { get; set; } // Ожидаемая годовая доходность в %

        public VirtualAsset(string name, string type, decimal investedAmount, decimal yearlyYieldRate)
        {
            Name = name;
            Type = type;
            InvestedAmount = investedAmount;
            YearlyYieldRate = yearlyYieldRate;
        }
    }

    internal class InvestmentAccount : BankAccount
    {
        // Инвестиционный портфель
        private List<VirtualAsset> _portfolio;

        public InvestmentAccount(string owner, Status status, Currency currency, Guid? id = null)
            : base(owner, status, currency, id)
        {
            _portfolio = new List<VirtualAsset>();
        }

        // Метод для покупки актива
        public void BuyAsset(VirtualAsset asset)
        {
            if (_balance < asset.InvestedAmount)
                throw new InvalidAccountOperationException("Недостаточно свободных средств (баланса) для покупки актива.");

            _balance -= asset.InvestedAmount;
            _portfolio.Add(asset);
        }
        public override void Withdraw(decimal amount)
        {
            base.Withdraw(amount);
        }

        // Вычисление ежегодной доходности от активов
        public decimal projectYearlyGrowth()
        {
            decimal totalExpectedProfit = 0;
            foreach (var asset in _portfolio)
            {
                totalExpectedProfit += asset.InvestedAmount * (asset.YearlyYieldRate / 100m);
            }
            return totalExpectedProfit;
        }

        public override string GetAccountInfo()
        {
            // Общая стоимость вложенных активов
            decimal totalAssetsValue = _portfolio.Sum(a => a.InvestedAmount);

            return base.GetAccountInfo() + $" | Portfolio Value: {totalAssetsValue} {Currency}";
        }

        // Вывод класса при печати
        public override string ToString()
        {
            return $"[InvestmentAccount] Owner: {Owner}, Portfolio Size: {_portfolio.Count} assets, Projected Profit: {projectYearlyGrowth()} {Currency}";
        }
    }
}
