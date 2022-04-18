using System;

namespace OnlineBank
{
    public class Menu
    {
        public static string GetUserOption()
        {
            Console.WriteLine(@"
            #### Home Menu ####
            
            Welcome to OnlineBank!
            
            Choose one of the following options:

            1 - List accounts
            2 - Insert new client account
            3 - Update client account information
            4 - Deposit
            5 - Withdraw
            6 - Transfer
            X - Exit");
            Console.WriteLine();
            string userOption = Console.ReadLine().ToUpper();
            Console.WriteLine();
            return userOption;
        }
    }
}