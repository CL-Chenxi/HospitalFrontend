//// sample solution to CA1 - author GC

//using System;
//using Bank;

//// simple test class
//public class Test
//{
//    public static void Main()
//    {
//        try
//        {
//            // create an account, make some deposits and withdrawals
//            CurrentAccount myAccount = new CurrentAccount("GC", 1000);

//            // register handler for when account becomes overdrawn
//            myAccount.LowFunds += new AccountLowFunds(LowFunds);

//            myAccount.MakeDeposit(650);
//            myAccount.MakeDeposit(100);
//            myAccount.MakeWithdrawal(50);
//            myAccount.MakeWithdrawal(500);

//            // Console.WriteLine(myAccount)
//        }
//        catch (Exception e)
//        {
//            Console.WriteLine(e.Message);
//        }
//    }

//    // low funds, 
//    private static void LowFunds(BankAccount b)
//    {
//        Console.WriteLine("Account is overdrawn!\n" + b); //The \n is an escape character that represents a newline.
//                                                          //It moves the cursor to the next line after printing the text.
//                                                          //This is a string concatenation operation.
//                                                          //The + operator concatenates the string "Account is overdrawn!\n"
//                                                          //with the string representation of the variable b.
//                                                          //The variable b must be an object whose string representation is desired.
//                                                          //In C#, when you concatenate an object with a string,
//                                                          //the ToString method of that object is called implicitly to get its string representation.
//    }
//}
