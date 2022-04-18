using System;

namespace OnlineBank
{
    public class Program
    {
        static void Main(string[] args)
        {
            string option = Menu.GetUserOption();

            while (option.ToUpper() != "X")
            {
                switch (option)
                {
                    case "1":
                        UserDataConnection.listClients();
                        break;
                    case "2":
                        UserDataConnection.SQLInsertClient();
                        break;
                    case "3":
                        UserDataConnection.updateAndDeleteClient();
                        break;
                    case "4":
                        Console.WriteLine("Insert the mount you want to deposit: ");
                        UserAccountConnection.Deposit(long.Parse(Console.ReadLine()));
                        break;
                    case "5":
                        Console.WriteLine("Insert the amount you want to withdraw from the account:");
                        UserAccountConnection.Withdraw(long.Parse(Console.ReadLine()));
                        break;
                    case "6":
                        Console.WriteLine("Insert the amount you want to transfer:");
                        TransferConnection.Transfer(long.Parse(Console.ReadLine()));
                        break;
                    case "X":
                        Console.WriteLine("Thank you for using our services.");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to go back to main menu: ");
                        Console.ReadLine();
                        break;
                }
                option = Menu.GetUserOption();
            }
            Console.WriteLine("Thank you for using our services. Press any key to close this session.");
            Console.ReadLine();
            Console.WriteLine("Session closed");
        }
    }
}