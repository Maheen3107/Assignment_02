using Microsoft.Data.SqlClient;
using System.Net;

namespace DAL
{
    public class DataAccess
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Library_Data;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public void AddBook(Book book)
        {
            string query = "INSERT INTO Books (Title, Author, Genre, IsAvailable) " +
                           "VALUES (@Title, @Author, @Genre, @IsAvailable);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.Parameters.AddWithValue("@Genre", book.Genre);
                    command.Parameters.AddWithValue("@IsAvailable", book.IsAvailable);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Book added successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding book: {ex.Message}");
                    }
                }
            }
        }

        public void RemoveBook(int bookId)
        {
            var books = GetAllBooks();
            var bookToRemove = books.SingleOrDefault(b => b.BookId == bookId);

            if (bookToRemove == null)
            {
                throw new InvalidOperationException("The book with the specified ID does not exist.");
            }

            string query = "DELETE FROM Books WHERE BookId = @BookId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", bookId);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Book removed successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error removing book: {ex.Message}");
                    }
                }
            }
        }

        public void UpdateBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author) || string.IsNullOrWhiteSpace(book.Genre))
            {
                throw new ArgumentException("Book details cannot be empty.");
            }

            var books = GetAllBooks();
            var existingBook = books.SingleOrDefault(b => b.BookId == book.BookId);

            if (existingBook == null)
            {
                throw new InvalidOperationException("The book with the specified ID does not exist.");
            }

            string query = "UPDATE Books SET Title = @Title, Author = @Author, Genre = @Genre, IsAvailable = @IsAvailable " +
                           "WHERE BookId = @BookId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", book.BookId);
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.Parameters.AddWithValue("@Genre", book.Genre);
                    command.Parameters.AddWithValue("@IsAvailable", book.IsAvailable);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Book updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating book: {ex.Message}");
                    }
                }
            }
        }
        public List<Book> SearchBooks(string query)
        {
            List<Book> books = new List<Book>();

            string sqlQuery = "SELECT BookId, Title, Author, Genre, IsAvailable FROM Books " +
                              "WHERE Title LIKE @Query OR Author LIKE @Query OR Genre LIKE @Query;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Query", "%" + query + "%");

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Book book = new Book
                                {
                                    BookId = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Author = reader.GetString(2),
                                    Genre = reader.GetString(3),
                                    IsAvailable = reader.GetBoolean(4)
                                };
                                books.Add(book);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error searching books: {ex.Message}");
                        // Handle exception (log, throw, etc.)
                    }
                }
            }

            return books;
        }

        public Book GetBookById(int bookId)
        {
            string query = "SELECT BookId, Title, Author, Genre, IsAvailable FROM Books WHERE BookId = @BookId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", bookId);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Book
                                {
                                    BookId = (int)reader["BookId"],
                                    Title = reader["Title"].ToString(),
                                    Author = reader["Author"].ToString(),
                                    Genre = reader["Genre"].ToString(),
                                    IsAvailable = (bool)reader["IsAvailable"]
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error retrieving book: {ex.Message}");
                    }
                }
            }

            return null;
        }

        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            string query = "SELECT BookId, Title, Author, Genre, IsAvailable FROM Books;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Book book = new Book
                                {
                                    BookId = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Author = reader.GetString(2),
                                    Genre = reader.GetString(3),
                                    IsAvailable = reader.GetBoolean(4)
                                };
                                books.Add(book);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error retrieving books: {ex.Message}");
                    }
                }
            }

            return books;
        }

        public void RegisterBorrower(Borrower borrower)
        {
            List<Borrower> borrowers = GetAllBorrowers();

            if (string.IsNullOrWhiteSpace(borrower.Name) || string.IsNullOrWhiteSpace(borrower.Email))
            {
                throw new ArgumentException("Borrower details cannot be empty.");
            }

            if (borrowers.Any(b => string.Equals(b.Email, borrower.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Borrower already exists");
            }

            string query = "INSERT INTO Borrower (Name, Email) VALUES (@Name, @Email)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", borrower.Name);
                    command.Parameters.AddWithValue("@Email", borrower.Email);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Borrower added successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding borrower: {ex.Message}");
                    }
                }
            }
        }

        public void UpdateBorrower(Borrower borrower)
        {
            List<Borrower> borrowers = GetAllBorrowers();

            if (!borrowers.Any(b => b.BorrowerId == borrower.BorrowerId))
            {
                throw new InvalidOperationException("Borrower not found.");
            }

            string query = "UPDATE Borrower SET Name = @Name, Email = @Email WHERE BorrowerId = @BorrowerId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", borrower.Name);
                    command.Parameters.AddWithValue("@Email", borrower.Email);
                    command.Parameters.AddWithValue("@BorrowerId", borrower.BorrowerId);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Borrower updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating borrower: {ex.Message}");
                    }
                }
            }
        }

        public  List<Borrower> GetAllBorrowers()
        {
            List<Borrower> borrowers = new List<Borrower>();

            string query = "SELECT BorrowerId, Name, Email FROM Borrower";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Borrower borrower = new Borrower
                                {
                                    BorrowerId = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Email = reader.GetString(2)
                                };
                                borrowers.Add(borrower);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error retrieving borrowers: {ex.Message}");
                    }
                }
            }

            return borrowers;
        }


        public void DeleteBorrower(int borrowerId)
        {
            Borrower borrower = getBorrowerById(borrowerId);

            if (borrower == null)
            {
                throw new InvalidOperationException("The borrower with the specified ID does not exist.");
            }

            string query = "DELETE FROM Borrower WHERE BorrowerId = @BorrowerId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BorrowerId", borrowerId);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Borrower removed successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error removing borrower: {ex.Message}");
                        // Handle exception (log, throw, etc.)
                    }
                }
            }
        }

        private Borrower getBorrowerById(int id)
        {
            string query = "SELECT BorrowerId, Name, Email FROM Borrower WHERE BorrowerId = @BorrowerId;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BorrowerId", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Borrower
                                {
                                    BorrowerId = reader.GetInt32(reader.GetOrdinal("BorrowerId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Email = reader.GetString(reader.GetOrdinal("Email"))
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error retrieving borrower: {ex.Message}");
                        // Handle exception (log, throw, etc.)
                    }
                }
            }

            return null; // Return null if no borrower is found
        }

        public void RecordTransaction(Transaction transaction)
        {
            string query = "INSERT INTO Transactions (BookId, BorrowerId, Date, IsBorrowed) " +
                           "VALUES (@BookId, @BorrowerId, @Date, @IsBorrowed);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", transaction.BookId);
                    command.Parameters.AddWithValue("@BorrowerId", transaction.BorrowerId);
                    command.Parameters.AddWithValue("@Date", transaction.Date);
                    command.Parameters.AddWithValue("@IsBorrowed", transaction.IsBorrowed);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Transaction recorded successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error recording transaction: {ex.Message}");
                        // Handle exception (log, throw, etc.)
                    }
                }
            }
        }

        public List<Transaction> GetBorrowedBooksByBorrower(int borrowerId)
        {
            List<Transaction> transactions = new List<Transaction>();

            string query = "SELECT TransactionId, BookId, BorrowerId, Date, IsBorrowed " +
                           "FROM Transactions " +
                           "WHERE BorrowerId = @BorrowerId AND IsBorrowed = 1;"; // Assuming IsBorrowed is 1 for borrowed books

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BorrowerId", borrowerId);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Transaction transaction = new Transaction
                                {
                                    TransactionId = reader.GetInt32(reader.GetOrdinal("TransactionId")),
                                    BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                    BorrowerId = reader.GetInt32(reader.GetOrdinal("BorrowerId")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                    IsBorrowed = reader.GetBoolean(reader.GetOrdinal("IsBorrowed"))
                                };
                                transactions.Add(transaction);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error retrieving borrowed books: {ex.Message}");
                        // Handle exception (log, throw, etc.)
                    }
                }
            }

            return transactions;
        }
        public List<Transaction> GetAllTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();

            string query = "SELECT TransactionId, BookId, BorrowerId, Date, IsBorrowed FROM Transactions";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Transaction transaction = new Transaction
                                {
                                    TransactionId = reader.GetInt32(0),
                                    BookId = reader.GetInt32(1),
                                    BorrowerId = reader.GetInt32(2),
                                    Date = reader.GetDateTime(3),
                                    IsBorrowed = reader.GetBoolean(4)
                                };
                                transactions.Add(transaction);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error retrieving transactions: {ex.Message}");
                    }
                }
            }

            return transactions;
        }
        public bool BorrowerExists(int borrowerId)
        {
            bool exists = false;
            string query = "SELECT COUNT(1) FROM Borrower WHERE BorrowerId = @BorrowerId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BorrowerId", borrowerId);
                    try
                    {
                        connection.Open();
                        exists = (int)command.ExecuteScalar() > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error checking borrower existence: {ex.Message}");
                    }
                }
            }

            return exists;
        }
    }
}