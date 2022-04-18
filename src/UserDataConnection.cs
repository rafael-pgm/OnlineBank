using System;
using System.Data.SqlClient;

namespace OnlineBank
{
    public class UserDataConnection
    {
        public static void SQLInsertClient()
        {
            SqlConnection sqlConnection;
            // Insert the connectionString
            string connectionString = "";
            try
            {
                sqlConnection = new SqlConnection(connectionString);

                sqlConnection.Open();

                Console.WriteLine("Insert name: ");
                string userName = Console.ReadLine();
                Console.WriteLine("Insert agency number with four digits: ");
                string agency = Console.ReadLine();
                Console.WriteLine("Insert account number with 5 digits: ");
                string accountNumber = Console.ReadLine();
                Console.WriteLine("Insert account actual balance: ");
                long balance = long.Parse(Console.ReadLine());
                Console.WriteLine("Insert account available credit: ");
                long credit = long.Parse(Console.ReadLine());
                Console.WriteLine("Insert email: ");
                string email = Console.ReadLine();
                Console.WriteLine("Insert client's adress: ");
                string adress = Console.ReadLine();
                Console.WriteLine("Insert phone number: ");
                string phone = Console.ReadLine();
                Console.WriteLine("Insert account ID: ");
                string id = Console.ReadLine();
                Console.WriteLine("Insert account maximum credit: ");
                string maxCredit = Console.ReadLine();

                string insertQuery = $"INSERT INTO clients VALUES('{userName}', '{agency}', '{accountNumber}', {balance}, {credit}, '{email}', '{adress}', '{phone}', '{id}', '{maxCredit}')";

                SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);
                insertCommand.ExecuteNonQuery();
                Console.WriteLine("Successfull data insertion!");

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
        public static void listClients()
        {
            SqlConnection sqlConnection;
            // Insert the connectionString
            string connectionString = "";
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                Console.WriteLine("List of clients accounts in our database: ");
                Console.WriteLine();

                string clientsListQuery = $"SELECT Name, Agency, AccountNumber, Balance, Credit, Email, Adress, Phone, ID, MaxCredit FROM clients ORDER BY ID ASC;";

                SqlCommand listCommand = new SqlCommand(clientsListQuery, sqlConnection);
                SqlDataReader dataReader = listCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    Console.WriteLine($"Id: {dataReader.GetValue(8).ToString()}");
                    Console.WriteLine($"Name: {dataReader.GetValue(0).ToString()}");
                    Console.WriteLine($"Agency: {dataReader.GetValue(1).ToString()}");
                    Console.WriteLine($"Account Number: {dataReader.GetValue(2).ToString()}");
                    Console.WriteLine($"Balance: {dataReader.GetValue(3).ToString()}");
                    Console.WriteLine($"Credit: {dataReader.GetValue(4).ToString()}");
                    Console.WriteLine($"Email: {dataReader.GetValue(5).ToString()}");
                    Console.WriteLine($"Address: {dataReader.GetValue(6).ToString()}");
                    Console.WriteLine($"Phone: {dataReader.GetValue(7).ToString()}");
                    Console.WriteLine($"Max Credit: {dataReader.GetValue(9).ToString()}");
                    Console.WriteLine();
                    Console.WriteLine("--------------------------------------------------------------------------");
                }
                dataReader.Close();
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

        public static void updateAndDeleteClient()
        {
            SqlConnection sqlConnection;
            // Insert the connectionString
            string connectionString = "";
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                Console.WriteLine("Choose the client id to update: ");
                string id = Console.ReadLine();

                string clientsListQuery = $"SELECT Name, Agency, AccountNumber, Balance, Credit, Email, Adress, Phone, ID, MaxCredit FROM clients WHERE ID = '{id}';";
                SqlCommand listCommand = new SqlCommand(clientsListQuery, sqlConnection);

                SqlDataReader dataReader = listCommand.ExecuteReader();
                if (!dataReader.Read())
                {
                    Console.WriteLine("Client does not exist.");
                    sqlConnection.Close();
                    return;
                }
                while (dataReader.Read())
                {
                    string readID = dataReader.GetValue(8).ToString();
                    Console.WriteLine(readID);
                }
                dataReader.Close();

                //End of data reading

                string userName;
                string email;
                string agency;
                string accountNumber;
                string address;
                string phone;
                string updateClientQuery;

                Console.WriteLine(
                @"Choose the user information you wanto to update or press 0 to delete the user information:
            
                0 - Delete the user information
                1 - Name 
                2 - Agency
                3 - Account Number
                4 - Email
                5 - Address
                6 - Phone                    
                ");
                updateClientQuery = Console.ReadLine();
                switch (updateClientQuery)
                {
                    case "1":
                        Console.WriteLine("Enter the correct user name: ");
                        userName = Console.ReadLine();
                        updateClientQuery = $"UPDATE clients SET Name = '{userName}' WHERE ID = '{id}';";
                        break;
                    case "2":
                        Console.WriteLine("Enter the correct user agency number with four digits: ");
                        agency = Console.ReadLine();
                        updateClientQuery = $"UPDATE clients SET Agency = '{agency}' WHERE ID = '{id}';";
                        break;
                    case "3":
                        Console.WriteLine("Enter the correct user account number with five digits: ");
                        accountNumber = Console.ReadLine();
                        updateClientQuery = $"UPDATE clients SET AccountNumber = '{accountNumber}' WHERE ID = '{id}';";
                        break;
                    case "4":
                        Console.WriteLine("Enter the correct user email: ");
                        email = Console.ReadLine();
                        updateClientQuery = $"UPDATE clients SET Email = '{email}' WHERE ID = '{id}';";
                        break;
                    case "5":
                        Console.WriteLine("Enter the correct user adress: ");
                        address = Console.ReadLine();
                        updateClientQuery = $"UPDATE clients SET Adress = '{address}' WHERE ID = '{id}';";
                        break;
                    case "6":
                        Console.WriteLine("Enter the correct user phone: ");
                        phone = Console.ReadLine();
                        updateClientQuery = $"UPDATE clients SET Phone = '{phone}' WHERE ID = '{id}';";
                        break;
                    case "0":
                        Console.WriteLine(@$"Are you sure you want to delete the user ID {id} data? Press 1 to continue or any other button to cancel and return to main menu:");
                        int number;
                        bool verification = int.TryParse(Console.ReadLine(), out number);
                        if (!verification || number != 1)
                        {
                            Console.WriteLine("You did not confirm the operation.");
                            sqlConnection.Close();
                            return;
                        }
                        string deleteUser = $"DELETE FROM clients WHERE ID = '{id}';";
                        SqlCommand deleteCommand = new SqlCommand(deleteUser, sqlConnection);
                        deleteCommand.ExecuteNonQuery();
                        Console.WriteLine("Data deleted successfully");
                        sqlConnection.Close();
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        sqlConnection.Close();
                        return;
                }
                SqlCommand updateCommand = new SqlCommand(updateClientQuery, sqlConnection);
                updateCommand.ExecuteNonQuery();
                Console.WriteLine("Data updated successfully!");
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