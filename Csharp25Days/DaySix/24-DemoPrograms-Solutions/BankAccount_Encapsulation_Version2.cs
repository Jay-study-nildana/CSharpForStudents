using System;

class BankAccount_Encapsulation
{
    // BankAccount: private balance field, controlled Deposit/Withdraw methods.
    public class BankAccount
    {
        private decimal _balance;
        public string Owner { get; }
        public decimal Balance => _balance; // read-only property

        public BankAccount(string owner, decimal initialDeposit = 0m)
        {
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            if (initialDeposit < 0) throw new ArgumentOutOfRangeException(nameof(initialDeposit));
            _balance = initialDeposit;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
            _balance += amount;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
            if (amount > _balance) return false;
            _balance -= amount;
            return true;
        }
    }

    static void Main()
    {
        var acct = new BankAccount("Bob", 100m);
        acct.Deposit(50m);
        var ok = acct.Withdraw(30m);
        Console.WriteLine($"{acct.Owner} balance: {acct.Balance} (withdraw success: {ok})");
        // Encapsulation ensures balance can only be changed through Deposit/Withdraw.
    }
}