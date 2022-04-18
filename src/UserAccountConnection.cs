using System;
using System.Data.SqlClient;

namespace OnlineBank
{
    public class UserAccountConnection
    {
        public static void Withdraw(long WithdrawValue)
        {
            SqlConnection sqlConnection;
            // Insert the connectionString
            string connectionString = "";
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                Console.WriteLine("Insert the agency number: ");
                string Agency = Console.ReadLine();

                Console.WriteLine("Insert the account number: ");
                string AccountNumber = Console.ReadLine();

                string numbersQuery = $"SELECT Name, Agency, AccountNumber, Balance, Credit, Email, Adress, Phone, ID, MaxCredit FROM clients WHERE Agency = '{Agency}' AND AccountNumber = '{AccountNumber}';";
                SqlCommand listCommand = new SqlCommand(numbersQuery, sqlConnection);

                SqlDataReader dataReader = listCommand.ExecuteReader();
                if (!dataReader.Read())
                {
                    Console.WriteLine("Client does not exist. Returning to main menu.");
                    sqlConnection.Close();
                    return;
                }

                long actualBalance = long.Parse(dataReader.GetValue(3).ToString());
                long actualCredit = long.Parse(dataReader.GetValue(4).ToString());

                Console.WriteLine($"You currently have {actualCredit} of credit and {actualBalance} of balance available");

                dataReader.Close();

                if (WithdrawValue > actualBalance + actualCredit)
                {
                    Console.WriteLine("You don't have enough funds to perform the operation. Please enter any key to go back to the home page.");
                    Console.ReadLine();
                    sqlConnection.Close();
                    return;
                }

                if (WithdrawValue > actualBalance && WithdrawValue <= actualBalance + actualCredit)
                {
                    // Choosing to use credits or not
                    Console.WriteLine("You don't have enough funds in your account. This operation may use your available credit. Do you want to continue? Enter 1 to yes or any other number to go back to home menu:");

                    int number;
                    bool verification = int.TryParse(Console.ReadLine(), out number);
                    if (!verification || number != 1)
                    {
                        Console.WriteLine("You did not confirm the operation. You will return to home menu.");
                        sqlConnection.Close();
                        return;
                    }
                    actualCredit -= WithdrawValue - actualBalance;
                    actualBalance -= WithdrawValue;

                    Console.WriteLine($"Operation completed successfully. You have now {actualBalance} of balance and {actualCredit} of remaining credit available in your account.");
                }
                if (WithdrawValue <= actualBalance)
                {
                    // Choosing to use credits
                    Console.WriteLine($"Are you sure to withdraw {WithdrawValue} from your account? Enter 1 to continue or any other number to go back to home menu:");
                    int number;
                    bool verification = int.TryParse(Console.ReadLine(), out number);
                    if (!verification || number != 1)
                    {
                        Console.WriteLine("You did not confirm the operation. You will return to home menu.");
                        sqlConnection.Close();
                        return;
                    }

                    actualBalance -= WithdrawValue;
                    Console.WriteLine("The operation has been completed successfully!");
                    Console.WriteLine($"Your account has now {actualBalance} of balance and you have {actualCredit} of remaining credit available.");
                }
                string updateClientBalance = $"UPDATE clients SET Balance = '{actualBalance}' WHERE Agency='{Agency}' AND AccountNumber='{AccountNumber}';";
                SqlCommand updateCommand = new SqlCommand(updateClientBalance, sqlConnection);
                updateCommand.ExecuteNonQuery();

                string updateClientCredit = $"UPDATE clients SET Credit = '{actualCredit}' WHERE Agency='{Agency}' AND AccountNumber='{AccountNumber}';";
                SqlCommand updateCommandCredit = new SqlCommand(updateClientCredit, sqlConnection);
                updateCommandCredit.ExecuteNonQuery();

                sqlConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("It was not possible to update the client information.");
                Console.WriteLine();
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("Press any key to return to Main Menu");
                Console.ReadKey();
            }
        }

        public static void Deposit(long DepositAmount)
        {
            SqlConnection sqlConnection;
            // Insert the connectionString
            string connectionString = "";
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                Console.WriteLine("Insert the agency number: ");
                string Agency = Console.ReadLine();
                Console.WriteLine("Insert the account number: ");
                string AccountNumber = Console.ReadLine();

                string balanceInfo = $"SELECT Balance, Credit, MaxCredit FROM clients WHERE AccountNumber = '{AccountNumber}' AND Agency = '{Agency}';";
                SqlCommand verifyBalance = new SqlCommand(balanceInfo, sqlConnection);
                SqlDataReader dataReader = verifyBalance.ExecuteReader();
                if (!dataReader.Read())
                {
                    Console.WriteLine("Client does not exist. Returning to main menu.");
                    sqlConnection.Close();
                    return;
                }

                long actualBalance = long.Parse(dataReader.GetValue(0).ToString());
                long actualCredit = long.Parse(dataReader.GetValue(1).ToString());
                long maxCredit = long.Parse(dataReader.GetValue(2).ToString());

                Console.WriteLine($"Are you sure you want to deposit {DepositAmount} into the account {AccountNumber} of the agency {Agency}? Press 1 to continue or any other keyword to cancel and return to Main Menu: ");

                dataReader.Close();

                int number;
                bool verification = int.TryParse(Console.ReadLine(), out number);
                if (!verification || number != 1)
                {
                    Console.WriteLine("You did not confirm the operation. You will return to home menu.");
                    sqlConnection.Close();
                    return;
                }

                long diference = maxCredit - actualCredit;
                long remainder = DepositAmount - diference;
                if (DepositAmount <= diference)
                {
                    actualCredit += DepositAmount;
                }
                if (DepositAmount > diference)
                {
                    actualCredit += diference;
                    actualBalance += remainder;
                }

                string updateClientBalance = $"UPDATE clients SET Balance = '{actualBalance}' WHERE Agency='{Agency}' AND AccountNumber='{AccountNumber}';";
                SqlCommand updateCommand = new SqlCommand(updateClientBalance, sqlConnection);
                updateCommand.ExecuteNonQuery();

                string updateClientCredit = $"UPDATE clients SET Credit = '{actualCredit}' WHERE Agency='{Agency}' AND AccountNumber='{AccountNumber}';";
                SqlCommand updateCommandCredit = new SqlCommand(updateClientCredit, sqlConnection);
                updateCommandCredit.ExecuteNonQuery();

                Console.WriteLine($"Operation completed successfully. You have now {actualBalance} of balance and {actualCredit} of remaining credit available in your account.");

                sqlConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("It was not possible to update the client information.");
                Console.WriteLine();
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("Press any key to return to Main Menu");
                Console.ReadKey();
            }
        }
    }
}