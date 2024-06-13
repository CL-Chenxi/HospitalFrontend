//// sample solution to CA1 - author GC


//using System;


//namespace Bank
//{
//    // account transactions - either deposit or withdrawal
//    public enum TransactionType
//    {
//        Deposit, Withdrawal
//    }

//    // delegate for handling situation when account becomes overdrawn i.e. negative balance
//    public delegate void AccountLowFunds(BankAccount account);
//    //This keyword is used to declare a delegate type.A delegate is a type that represents references to methods
//    //with a specific parameter list and return type.Delegates are similar to function pointers in C/C++ but are type-safe and secure.

//    // an accont transaction
//    public class AccountTransaction
//    {
//        private TransactionType type;       // deposit or withdrawal
//        private double amount;              // amount concerned

//        // constructor
//        public AccountTransaction(TransactionType type, double amount)
//        {
//            this.type = type;
//            this.amount = amount;
//        }

//        // return human readable String
//        public override String ToString()
//        {
//            return "type: " + type + "     amount: " + amount;
//        }
//    }

//    // a generic bank account, abstract class
//    public abstract class BankAccount
//    {
//        private String accountNumber;           // the account number e.g 903508-1111111
//        private double balance;                 // the current balance on the account

//        // constructor
//        public BankAccount(String accountNumber)
//        {
//            this.accountNumber = accountNumber;
//            Balance = 0;
//        }

//        // read-write property for balance
//        public double Balance
//        {
//            get
//            {
//                return balance;
//            }
//            set
//            {
//                balance = value;
//            }
//        }

//        // read only property for accountNumber
//        public String AccountNumber
//        {
//            get
//            {
//                return accountNumber;
//            }
//        }

//        // abstract methods
//        public abstract void MakeDeposit(double amount);
//        public abstract void MakeWithdrawal(double amount);
//    }


//    // a Current Account
//    public class CurrentAccount : BankAccount       // special type of BankAccount
//    {
//        private double overDraftLimit;

//        // history of transactions on this account
//        private AccountTransaction[] transactionHistory;
//        private int nextTransactionNo;              // next transaction in history array

//        // notification that the balance is negative i.e. low funds
//        public event AccountLowFunds LowFunds;

//        // constructor
//        public CurrentAccount(String accountNumber, double overDraftLimit) : base(accountNumber)
//        {
//            this.overDraftLimit = overDraftLimit;

//            transactionHistory = new AccountTransaction[100];
//            nextTransactionNo = 0;
//        }

//        // read-only property
//        public double OverDraftLimit
//        {
//            get
//            {
//                return overDraftLimit;
//            }
//        }

//        // make a deposit
//        public override void MakeDeposit(double amount)     // assume amount positive
//        {
//            // update balance
//            Balance += amount;

//            // update Transaction history
//            transactionHistory[nextTransactionNo++] = new AccountTransaction(TransactionType.Deposit, amount);
//        }

//        // make a withdrawal
//        public override void MakeWithdrawal(double amount)  // assume amount positive
//        {
//            if (amount < (Balance + overDraftLimit))
//            {
//                Balance -= amount;              // update balance

//                // update Transaction history
//                transactionHistory[nextTransactionNo++] = new AccountTransaction(TransactionType.Withdrawal, amount);

//                if (Balance <= 0)
//                {
//                    LowFunds(this);         // account is in the red, notify event
//                }
//            }
//            else
//            {
//                throw new ApplicationException("Insufficient Funds for Withdrawal");
//            }
//        }

//        // return human readable String - for CurrentAccount including transaction history
//        public override String ToString()
//        {
//            String output;

//            output = "CurrentAcount:\t" + "number: " + AccountNumber + " balance: " + Balance;

//            output += "\nTransaction history:\n";
//            for (int i = 0; i < nextTransactionNo; i++)
//            {
//                output += transactionHistory[i].ToString() + "\n";
//            }

//            return output;
//        }
//    }
//}
