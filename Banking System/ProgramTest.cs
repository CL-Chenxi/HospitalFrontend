// Sample solution to CA1 - author GC

using System;
using static System.Formats.Asn1.AsnWriter;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using System.Transactions;
using System.Net.NetworkInformation;
using System.Data.Common;
using System.Collections.Generic;

namespace Bank
{
    // Account transactions - either deposit or withdrawal
    public enum TransactionType
    {
        Deposit, Withdrawal
    }

    // Delegate for handling situation when account becomes overdrawn i.e. negative balance
    public delegate void AccountLowFunds(BankAccount account);

    // An account transaction
    public class AccountTransaction
    {
        private TransactionType type;   // Deposit or withdrawal
        private double amount;          // Amount concerned

        // Constructor
        public AccountTransaction(TransactionType type, double amount)
        {
            this.type = type;
            this.amount = amount;
        }

        //Return human readable string
        public override string ToString()
        {
            return $"Type: {type} Amount: {amount}";
        }
    }

    // A generic bank account, abstract class
    public abstract class BankAccount
    {
        private string accountNumber;   // The account number e.g 903508-1111111
        private double balance;         // The current balance on the account

        // Constructor
        public BankAccount(string accountNumber)
        {
            this.accountNumber = accountNumber;
            Balance = 0;
        }

        // Read-write property for balance
        public double Balance
        {
            get => balance;
            set => balance = value;
        }

        // Read-only property for accountNumber
        public string AccountNumber => accountNumber;

        // Abstract methods
        public abstract void MakeDeposit(double amount);
        public abstract void MakeWithdrawal(double amount);
    }

    // A Current Account
    public class CurrentAccount : BankAccount  // Special type of BankAccount
    {
        private double overDraftLimit;

        // History of transactions on this account
        private AccountTransaction[] transactionHistory;
        private int nextTransactionNo;  // Next transaction in history array

        // Notification that the balance is negative i.e. low funds
        public event AccountLowFunds LowFunds;

        // Constructor
        public CurrentAccount(string accountNumber, double overDraftLimit) : base(accountNumber)
        {
            this.overDraftLimit = overDraftLimit;

            transactionHistory = new AccountTransaction[100];  //Initializes the transactionHistory array with a length of 100.
                                                               //This array will be used to store transaction history for this account.
            nextTransactionNo = 0;            //Initializes nextTransactionNo to 0.
                                              //This variable keeps track of the index for the next transaction in the transactionHistory array.
        }

        // Read-only property
        public double OverDraftLimit => overDraftLimit;

        // Make a deposit
        public override void MakeDeposit(double amount)  // Assume amount positive
        {
            // Update balance
            Balance += amount;

            // Update Transaction history
            transactionHistory[nextTransactionNo++] = new AccountTransaction(TransactionType.Deposit, amount);
        }

        // Make a withdrawal
        public override void MakeWithdrawal(double amount)  // Assume amount positive
        {
            if (amount < (Balance + overDraftLimit))
            {
                Balance -= amount;  // Update balance

                // Update Transaction history
                transactionHistory[nextTransactionNo++] = new AccountTransaction(TransactionType.Withdrawal, amount);

                if (Balance <= 0)
                {
                    LowFunds?.Invoke(this);  // Account is in the red, notify event
                                             //In this example, if LowFundsEvent is null:

                    //LowFunds?.Invoke(this) will safely skip the invocation and will not throw an exception.
                    //LowFunds(this) will throw a NullReferenceException because it directly attempts to invoke
                    //the event without checking for null.
                    //Using the null-conditional operator (?.) is safer when dealing with events, as it prevents
                    //null reference exceptions and ensures robustness in your code.
                }
            }
            else
            {
                throw new ApplicationException("Insufficient Funds for Withdrawal");
            }
        }

        // Return human readable string - for CurrentAccount including transaction history
        public override string ToString()
        {
            string output = $"CurrentAccount:\tNumber: {AccountNumber} Balance: {Balance}";
            //Initializes a string variable output with the account number (AccountNumber) and
            //balance (Balance) formatted as a string using string interpolation ($"" syntax).

            output += "\nTransaction history:\n";
            for (int i = 0; i < nextTransactionNo; i++)
            {
                output += transactionHistory[i].ToString() + "\n";
            }
            //Appends a header for transaction history to the output string.
            //then, it iterates through the transactionHistory array up to the nextTransactionNo(which tracks the number of transactions).
            //For each transaction, it appends the result of calling ToString() on the AccountTransaction object and adds a newline character.
            return output;
            //    Returns the constructed string output as the string representation of the CurrentAccount object.
            //    This ToString() method provides a human-readable representation of the CurrentAccount object, including its account number,
            //    balance, and a list of transactions.It's useful for logging, debugging, or displaying information to users.
        }
    }

    class Test
    {
        static void Main(string[] args)
        {
            CurrentAccount account = new CurrentAccount("CL", 500); //分别代表账户号码和透支限额。

            account.LowFunds += Account_LowFunds;

            //This line of code subscribes the Account_LowFunds method to the LowFunds event of the account object.

            //account: Refers to an instance of the CurrentAccount class.
            //LowFunds: Represents an event in the CurrentAccount class.
            //+=: Is the subscription operator used to add an event handler to an event.
            //Account_LowFunds: Is the method that will handle the LowFunds event.
            //So, this line essentially says that whenever the LowFunds event is raised by the account object,
            //            the Account_LowFunds method will be called to handle it.



            account.MakeDeposit(2000);
            Console.WriteLine(account.ToString());

            account.MakeWithdrawal(1200);
            Console.WriteLine(account.ToString());

            account.MakeWithdrawal(300);
            Console.WriteLine(account.ToString());
        }

        private static void Account_LowFunds(BankAccount account)
        {
            Console.WriteLine($"Warning: Account {account.AccountNumber} has low funds. Current balance: {account.Balance}");
        }
    }
}
