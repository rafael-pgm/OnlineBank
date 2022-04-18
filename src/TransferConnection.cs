using System;
using System.Data.SqlClient;

namespace OnlineBank
{
    public class TransferConnection
    {
        public static void Transfer(long transferValue)
        {
            Console.WriteLine("Please insert the source account information for the transfer:");
            SqlConnection sqlConnection;
            // Insert the connectionString
            string connectionString = "";
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                Console.WriteLine("Insert the agency number: ");
                string sourceAgency = Console.ReadLine();

                Console.WriteLine("Insert the account number: ");
                string sourceAccountNumber = Console.ReadLine();

                string numbersQuery = $"SELECT Name, Agency, AccountNumber, Balance, Credit, Email, Adress, Phone, ID, MaxCredit FROM clients WHERE Agency = '{sourceAgency}' AND AccountNumber = '{sourceAccountNumber}';";
                SqlCommand sourceListCommand = new SqlCommand(numbersQuery, sqlConnection);

                SqlDataReader sourceDataReader = sourceListCommand.ExecuteReader();
                if (!sourceDataReader.Read())
                {
                    Console.WriteLine("Client does not exist. Returning to main menu.");
                    sqlConnection.Close();
                    return;
                }

                long sourceActualBalance = long.Parse(sourceDataReader.GetValue(3).ToString());
                long sourceActualCredit = long.Parse(sourceDataReader.GetValue(4).ToString());

                Console.WriteLine($"You currently have {sourceActualBalance} of balance and {sourceActualCredit} of credit available.");

                sourceDataReader.Close();

                if (transferValue > sourceActualBalance + sourceActualCredit)
                {
                    Console.WriteLine("You don't have enough funds to perform the operation. Please enter any key to go back to the home page.");
                    Console.ReadLine();
                    sqlConnection.Close();
                    return;
                }
                if (transferValue > sourceActualBalance && transferValue <= sourceActualBalance + sourceActualCredit)
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
                    sourceActualCredit -= transferValue - sourceActualBalance;
                    sourceActualBalance -= transferValue;
                }
                if (transferValue <= sourceActualBalance)
                {
                    // Choosing to use credits
                    Console.WriteLine($"Are you sure to use {transferValue} from your account to perform the transfer? Enter 1 to continue or any other number to go back to home menu:");
                    int number;
                    bool verification = int.TryParse(Console.ReadLine(), out number);
                    if (!verification || number != 1)
                    {
                        Console.WriteLine("You did not confirm the operation. You will return to home menu.");
                        sqlConnection.Close();
                        return;
                    }
                    sourceActualBalance -= transferValue;
                }
                // End of withdraw without connect the information
                Console.WriteLine();
                Console.WriteLine("Please insert the target account information for the transfer.");
                Console.WriteLine("Insert the agency number: ");
                string targetAgency = Console.ReadLine();
                Console.WriteLine("Insert the account number: ");
                string targetAccountNumber = Console.ReadLine();

                if (sourceAgency == targetAgency && sourceAccountNumber == targetAccountNumber)
                {
                    Console.WriteLine("You can't transfer to the same acoount. Returning to main menu.");
                    sqlConnection.Close();
                    return;
                }

                string targetInfo = $"SELECT Balance, Credit, MaxCredit FROM clients WHERE AccountNumber = '{targetAccountNumber}' AND Agency = '{targetAgency}';";
                SqlCommand verifyBalance = new SqlCommand(targetInfo, sqlConnection);
                SqlDataReader targetDataReader = verifyBalance.ExecuteReader();
                if (!targetDataReader.Read())
                {
                    Console.WriteLine("Client does not exist. Returning to main menu.");
                    sqlConnection.Close();
                    return;
                }

                long targetActualBalance = long.Parse(targetDataReader.GetValue(0).ToString());
                long targetActualCredit = long.Parse(targetDataReader.GetValue(1).ToString());
                long targetMaxCredit = long.Parse(targetDataReader.GetValue(2).ToString());

                Console.WriteLine($"Are you sure you want to deposit {transferValue} into the account {targetAccountNumber} of the agency {targetAgency}? Press 1 to continue or any other keyword to cancel and return to Main Menu: ");

                targetDataReader.Close();

                int targetNumber;
                bool targetVerification = int.TryParse(Console.ReadLine(), out targetNumber);
                if (!targetVerification || targetNumber != 1)
                {
                    Console.WriteLine("You did not confirm the operation. You will return to home menu.");
                    sqlConnection.Close();
                    return;
                }

                long diference = targetMaxCredit - targetActualCredit;
                long remainder = transferValue - diference;
                if (transferValue <= diference)
                {
                    targetActualCredit += transferValue;
                }
                if (transferValue > diference)
                {
                    targetActualCredit += diference;
                    targetActualBalance += remainder;
                }

                string updateSourceBalance = $"UPDATE clients SET Balance = '{sourceActualBalance}' WHERE Agency='{sourceAgency}' AND AccountNumber='{sourceAccountNumber}';";
                SqlCommand updateSourceBalanceCommand = new SqlCommand(updateSourceBalance, sqlConnection);
                updateSourceBalanceCommand.ExecuteNonQuery();

                string updateSourceCredit = $"UPDATE clients SET Credit = '{sourceActualCredit}' WHERE Agency='{sourceAgency}' AND AccountNumber='{sourceAccountNumber}';";
                SqlCommand updateSourceCommandCredit = new SqlCommand(updateSourceCredit, sqlConnection);
                updateSourceCommandCredit.ExecuteNonQuery();

                string updateTargetBalance = $"UPDATE clients SET Balance = '{targetActualBalance}' WHERE Agency='{targetAgency}' AND AccountNumber='{targetAccountNumber}';";
                SqlCommand updateTargetBalanceCommand = new SqlCommand(updateTargetBalance, sqlConnection);
                updateTargetBalanceCommand.ExecuteNonQuery();

                string updateClientCredit = $"UPDATE clients SET Credit = '{targetActualCredit}' WHERE Agency='{targetAgency}' AND AccountNumber='{targetAccountNumber}';";
                SqlCommand updateCommandCredit = new SqlCommand(updateClientCredit, sqlConnection);
                updateCommandCredit.ExecuteNonQuery();

                Console.WriteLine($"Operation completed successfully. You have now {sourceActualBalance} of balance and {sourceActualCredit} of remaining credit available in your account.");

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