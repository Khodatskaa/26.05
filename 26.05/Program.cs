using System.Data.SqlClient;

namespace _26._05
{
    public class Program
    {
        static string connectionString = "Server=localhost;Database=YourDatabaseName;Integrated Security=True;";

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Exit");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    Login();
                }
                else if (choice == "2")
                {
                    break;
                }
            }
        }

        static void Login()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    bool isAdmin = (bool)reader["IsAdmin"];
                    if (isAdmin)
                    {
                        AdminMenu();
                    }
                    else
                    {
                        UserMenu();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid username or password");
                }
            }
        }

        static void AdminMenu()
        {
            while (true)
            {
                Console.WriteLine("\nAdmin Menu:");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. Delete Book");
                Console.WriteLine("3. Edit Book");
                Console.WriteLine("4. Sell Book");
                Console.WriteLine("5. Write Off Book");
                Console.WriteLine("6. Add Book to Promotion");
                Console.WriteLine("7. Reserve Book");
                Console.WriteLine("8. Exit");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddBook();
                        break;
                    case "2":
                        DeleteBook();
                        break;
                    case "3":
                        EditBook();
                        break;
                    case "4":
                        SellBook();
                        break;
                    case "5":
                        WriteOffBook();
                        break;
                    case "6":
                        AddBookToPromotion();
                        break;
                    case "7":
                        ReserveBook();
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void UserMenu()
        {
            while (true)
            {
                Console.WriteLine("\nUser Menu:");
                Console.WriteLine("1. Search Book");
                Console.WriteLine("2. View New Books");
                Console.WriteLine("3. View Popular Books");
                Console.WriteLine("4. View Popular Authors");
                Console.WriteLine("5. View Popular Genres");
                Console.WriteLine("6. Exit");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        SearchBook();
                        break;
                    case "2":
                        ViewNewBooks();
                        break;
                    case "3":
                        ViewPopularBooks();
                        break;
                    case "4":
                        ViewPopularAuthors();
                        break;
                    case "5":
                        ViewPopularGenres();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void AddBook()
        {
            try
            {
                Console.WriteLine("\nAdding a New Book:");
                Console.Write("Enter book title: ");
                string title = Console.ReadLine();
                Console.Write("Enter author's name: ");
                string author = Console.ReadLine();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Books (Title, Author) VALUES (@Title, @Author)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Author", author);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Book added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add book.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding book: " + ex.Message);
            }
        }

        static void DeleteBook()
        {
            try
            {
                Console.WriteLine("\nDeleting a Book:");
                Console.Write("Enter book ID to delete: ");
                int bookId = int.Parse(Console.ReadLine());

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Books WHERE BookId = @BookId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Book deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Book not found with the given ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting book: " + ex.Message);
            }
        }

        static void EditBook()
        {
            try
            {
                Console.WriteLine("\nEditing a Book:");
                Console.Write("Enter book ID to edit: ");
                int bookId = int.Parse(Console.ReadLine());

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Books WHERE BookId = @BookId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Console.WriteLine("Current Book Details:");
                        Console.WriteLine($"Title: {reader["Title"]}, Author: {reader["Author"]}");

                        Console.WriteLine("Enter new values (press Enter to keep current value):");
                        Console.Write("New Title: ");
                        string newTitle = Console.ReadLine();
                        if (string.IsNullOrEmpty(newTitle))
                            newTitle = (string)reader["Title"]; 

                        Console.Write("New Author: ");
                        string newAuthor = Console.ReadLine();
                        if (string.IsNullOrEmpty(newAuthor))
                            newAuthor = (string)reader["Author"];

                        reader.Close();

                        string updateQuery = "UPDATE Books SET Title = @Title, Author = @Author WHERE BookId = @BookId";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@BookId", bookId);
                        updateCommand.Parameters.AddWithValue("@Title", newTitle);
                        updateCommand.Parameters.AddWithValue("@Author", newAuthor);
                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Book updated successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to update book.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Book not found with the given ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error editing book: " + ex.Message);
            }
        }

        static void SellBook()
        {
            try
            {
                Console.WriteLine("\nSelling a Book:");
                Console.Write("Enter book ID to sell: ");
                int bookId = int.Parse(Console.ReadLine());

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Books WHERE BookId = @BookId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        int availableCopies = (int)reader["StockCount"];
                        if (availableCopies > 0)
                        {
                            Console.Write("Enter quantity to sell: ");
                            int quantity = int.Parse(Console.ReadLine());

                            if (quantity <= availableCopies)
                            {
                                int newStockCount = availableCopies - quantity;
                                reader.Close();

                                string updateQuery = "UPDATE Books SET StockCount = @NewStockCount WHERE BookId = @BookId";
                                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                                updateCommand.Parameters.AddWithValue("@BookId", bookId);
                                updateCommand.Parameters.AddWithValue("@NewStockCount", newStockCount);
                                int rowsAffected = updateCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    RecordTransaction(bookId, quantity);
                                    Console.WriteLine($"{quantity} copies sold successfully!");
                                }
                                else
                                {
                                    Console.WriteLine("Failed to update stock count.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Not enough copies available to sell.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No copies available to sell.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Book not found with the given ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error selling book: " + ex.Message);
            }
        }

        static void WriteOffBook()
        {
            try
            {
                Console.WriteLine("\nWriting Off a Book:");
                Console.Write("Enter book ID to write off: ");
                int bookId = int.Parse(Console.ReadLine());

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Books WHERE BookId = @BookId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Book written off successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Book not found with the given ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing off book: " + ex.Message);
            }
        }

        static void AddBookToPromotion()
        {
            try
            {
                Console.WriteLine("\nAdding Book to Promotion:");
                Console.Write("Enter book ID to add to promotion: ");
                int bookId = int.Parse(Console.ReadLine());
                Console.Write("Enter promotion type (e.g., New Year Sale): ");
                string promotionType = Console.ReadLine();
                Console.Write("Enter discount percentage: ");
                int discountPercentage = int.Parse(Console.ReadLine());

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Promotions (BookId, PromotionType, DiscountPercentage) VALUES (@BookId, @PromotionType, @DiscountPercentage)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    command.Parameters.AddWithValue("@PromotionType", promotionType);
                    command.Parameters.AddWithValue("@DiscountPercentage", discountPercentage);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Book added to promotion successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add book to promotion.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding book to promotion: " + ex.Message);
            }
        }

        static void ReserveBook()
        {
            try
            {
                Console.WriteLine("\nReserving a Book for a Buyer:");
                Console.Write("Enter book ID to reserve: ");
                int bookId = int.Parse(Console.ReadLine());
                Console.Write("Enter buyer's name: ");
                string buyerName = Console.ReadLine();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Reservations (BookId, BuyerName) VALUES (@BookId, @BuyerName)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    command.Parameters.AddWithValue("@BuyerName", buyerName);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Book reserved successfully for {buyerName}");
                    }
                    else
                    {
                        Console.WriteLine("Failed to reserve book.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reserving book: " + ex.Message);
            }
        }

        static void SearchBook()
        {
            try
            {
                Console.WriteLine("\nSearching for a Book:");
                Console.Write("Enter book title, author, or genre to search: ");
                string searchTerm = Console.ReadLine();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Books WHERE Title LIKE @SearchTerm OR Author LIKE @SearchTerm OR Genre LIKE @SearchTerm";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"Book ID: {reader["BookId"]}, Title: {reader["Title"]}, Author: {reader["Author"]}, Genre: {reader["Genre"]}");
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error searching for book: " + ex.Message);
            }
        }

        static void ViewNewBooks()
        {
            try
            {
                Console.WriteLine("\nViewing New Books:");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT TOP 10 * FROM Books ORDER BY ReleaseDate DESC";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"Book ID: {reader["BookId"]}, Title: {reader["Title"]}, Author: {reader["Author"]}, Release Date: {reader["ReleaseDate"]}");
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error viewing new books: " + ex.Message);
            }
        }

        static void ViewPopularBooks()
        {
            try
            {
                Console.WriteLine("\nViewing Popular Books:");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT TOP 10 b.*, COUNT(s.SaleId) AS SalesCount " +
                                   "FROM Books b " +
                                   "LEFT JOIN Sales s ON b.BookId = s.BookId " +
                                   "GROUP BY b.BookId, b.Title, b.Author, b.Genre " +
                                   "ORDER BY SalesCount DESC";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"Book ID: {reader["BookId"]}, Title: {reader["Title"]}, Author: {reader["Author"]}, Genre: {reader["Genre"]}, Sales Count: {reader["SalesCount"]}");
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error viewing popular books: " + ex.Message);
            }
        }

        static void ViewPopularAuthors()
        {
            try
            {
                Console.WriteLine("\nViewing Popular Authors:");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT TOP 10 Author, COUNT(*) AS BookCount " +
                                   "FROM Books " +
                                   "GROUP BY Author " +
                                   "ORDER BY BookCount DESC";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"Author: {reader["Author"]}, Book Count: {reader["BookCount"]}");
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error viewing popular authors: " + ex.Message);
            }
        }

        static void ViewPopularGenres()
        {
            try
            {
                Console.WriteLine("\nViewing Popular Genres:");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT TOP 5 Genre, COUNT(*) AS BookCount " +
                                   "FROM Books " +
                                   "GROUP BY Genre " +
                                   "ORDER BY BookCount DESC";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"Genre: {reader["Genre"]}, Book Count: {reader["BookCount"]}");
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error viewing popular genres: " + ex.Message);
            }
        }

        static void RecordTransaction(int bookId, int quantity)
        {
        }
    }
}
