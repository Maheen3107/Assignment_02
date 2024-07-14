using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DAL;
namespace DataBase
{
    public class Program
    {
        static void Main(string[] args)
        {
            DataAccess dataAccess = new DataAccess();
            Menu menu = new Menu(dataAccess);
            menu.ShowMenu();
        }
    }
    public class Menu
    {
        public DataAccess _dataAccess;

        public Menu(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=========================================");
                Console.WriteLine("       Library Management System          ");
                Console.WriteLine("=========================================");
                Console.ResetColor();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1. Add a new book");
                Console.WriteLine("2. Remove a book");
                Console.WriteLine("3. Update a book");
                Console.WriteLine("4. Register a new borrower");
                Console.WriteLine("5. Update a borrower");
                Console.WriteLine("6. Delete a borrower");
                Console.WriteLine("7. Borrow a book");
                Console.WriteLine("8. Return a book");
                Console.WriteLine("9. Search for books by title, author, or genre");
                Console.WriteLine("10. View all books");
                Console.WriteLine("11. View borrowed books by a specific borrower");
                Console.WriteLine("12. View All Borrowers");
                Console.WriteLine("13. View All Transactions");
                Console.WriteLine("14. Exit the application");
                Console.ResetColor();

                Console.WriteLine();
                Console.Write("Choose an option: ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            AddBook();
                            break;
                        case 2:
                            RemoveBook();
                            break;
                        case 3:
                            UpdateBook();
                            break;
                        case 4:
                            RegisterNewBorrower();
                            break;
                        case 5:
                            UpdateBorrower();
                            break;
                        case 6:
                            DeleteBorrower();
                            break;
                        case 7:
                            BorrowBook();
                            break;
                        case 8:
                            ReturnBook();
                            break;
                        case 9:
                            SearchBooks();
                            break;
                        case 10:
                            ViewAllBooks();
                            break;
                        case 11:
                            ViewBorrowedBooksByBorrower();
                            break;

                        case 12:
                            ViewAllBorrowers();
                            break;

                        case 13:
                            ViewAllTransactions();
                            break; 

                        case 14:
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private void AddBook()
        {
            while (true)  // Loop until valid input is provided
            {
                Console.Clear();
                Console.WriteLine("Add a New Book");
                Console.WriteLine("===============");
                Console.Write("Enter book title: ");
                string title = Console.ReadLine();

                Console.Write("Enter book author: ");
                string author = Console.ReadLine();

                Console.Write("Enter book genre: ");
                string genre = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(genre))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Title, author, and genre cannot be empty.");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to retry...");
                    Console.ReadKey();
                }
                else
                {
                    Book newBook = new Book
                    {
                        Title = title,
                        Author = author,
                        Genre = genre
                    };

                    _dataAccess.AddBook(newBook);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Book added successfully.");
                    Console.ResetColor();
                    List<Book> books = _dataAccess.GetAllBooks();

                    if (books.Count > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Books in library:");
                        Console.ResetColor();
                        foreach (var book in books)
                        {
                            Console.WriteLine($"ID: {book.BookId}, Title: {book.Title}, Author: {book.Author}, Genre: {book.Genre}, Available: {book.IsAvailable}");
                        }
                    }
                    
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.ReadKey();
                    break;  // Exit the loop once book is added successfully
                }
            }
        }


        private void RemoveBook()
        {
            Console.Clear();
            Console.WriteLine("Remove a Book");
            Console.WriteLine("===============");
            List<Book> books = _dataAccess.GetAllBooks();

            if (books.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Books in library:");
                Console.ResetColor();
                foreach (var book in books)
                {
                    Console.WriteLine($"ID: {book.BookId}, Title: {book.Title}, Author: {book.Author}, Genre: {book.Genre}, Available: {book.IsAvailable}");
                }
            }
            Console.Write("Enter book ID to remove: ");
           
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int bookId))
                {
                    if (_dataAccess.GetAllBooks().Any(b => b.BookId == bookId))
                    {
                        _dataAccess.RemoveBook(bookId);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Book removed successfully.");
                        Console.ResetColor();
                        break; // Exit the loop if book removal is successful
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Book with ID {0} does not exist.", bookId);
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid book ID.");
                    Console.ResetColor();
                }

                Console.WriteLine("Press any key to retry...");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("Remove a Book");
                Console.WriteLine("===============");
                Console.Write("Enter book ID to remove: ");
            }

            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }
        private void UpdateBook()
        {
            Console.Clear();
            Console.WriteLine("Update a Book");
            Console.WriteLine("===============");
           
            
            while (true)
            {
                Console.WriteLine("All Books:");
                List<Book> books = _dataAccess.GetAllBooks();

                if (books.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Books in library:");
                    Console.ResetColor();
                    foreach (var book in books)
                    {
                        Console.WriteLine($"ID: {book.BookId}, Title: {book.Title}, Author: {book.Author}, Genre: {book.Genre}, Available: {book.IsAvailable}");
                    }
                }
                Console.Write("Enter book ID to update: ");

                if (int.TryParse(Console.ReadLine(), out int bookId))
                {
                    Book book = _dataAccess.GetBookById(bookId);
                    if (book != null)
                    {
                        Console.Write("Enter new title (leave blank to keep current): ");
                        string title = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(title))
                            book.Title = title;

                        Console.Write("Enter new author (leave blank to keep current): ");
                        string author = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(author))
                            book.Author = author;

                        Console.Write("Enter new genre (leave blank to keep current): ");
                        string genre = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(genre))
                            book.Genre = genre;

                        _dataAccess.UpdateBook(book);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Book updated successfully.");
                        Console.ResetColor();
                        break; // Exit the loop if book update is successful
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Book with ID {0} not found.", bookId);
                        Console.ResetColor();
                       
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid book ID.");
                    Console.ResetColor();

                }

                Console.WriteLine("Press any key to retry...");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("Update a Book");
                Console.WriteLine("===============");
                Console.Write("Enter book ID to update: ");
            }

            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private void RegisterNewBorrower()
        {
            while (true)  // Loop until valid input is provided
            {
                Console.Clear();
                Console.WriteLine("Register a New Borrower");
                Console.WriteLine("=========================");
                Console.Write("Enter borrower name: ");
                string name = Console.ReadLine();
                
                Console.Write("Enter borrower email: ");
                string email = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Name cannot be empty.");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to retry...");
                    Console.ReadKey();
                }
                else if (!IsValidEmail(email))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid email format. Please enter a valid email address.");
                    Console.ResetColor();

                    Console.WriteLine("Press any key to retry...");
                    Console.ReadKey();
                }
                else if (IsEmailAlreadyRegistered(email))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("This email is already registered. Please use a different email address.");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to retry...");
                    Console.ReadKey();
                }
                else
                {
                    Borrower newBorrower = new Borrower
                    {
                        Name = name,
                        Email = email
                    };

                    _dataAccess.RegisterBorrower(newBorrower);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Borrower registered successfully.");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.ReadKey();
                    break;  // Exit the loop once borrower is registered successfully
                }
            }
        }

        private bool IsEmailAlreadyRegistered(string email)
        {
            // Implement this method to check if the email already exists in the database
            var existingBorrowers = _dataAccess.GetAllBorrowers();
            return existingBorrowers.Any(borrower => borrower.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private void UpdateBorrower()
        {
            Console.Clear();
            Console.WriteLine("Update a Borrower");
            Console.WriteLine("===================");
            List<Borrower> borrowers = _dataAccess.GetAllBorrowers();
            if (borrowers.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Borrowers:");
                Console.ResetColor();
                foreach (var borrower in borrowers)
                {
                    Console.WriteLine($"ID: {borrower.BorrowerId}, Name: {borrower.Name}, Email: {borrower.Email}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No borrowers available.");
                return;
            }

            Console.Write("Enter borrower ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int borrowerId))
            {
               // List<Borrower> borrowers = _dataAccess.GetAllBorrowers();
                Borrower borrower = borrowers.Find(b => b.BorrowerId == borrowerId);
                if (borrower == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Borrower not found.");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    string name = null;
                    string email = null;

                    // Validate name input
                    while (string.IsNullOrWhiteSpace(name))
                    {
                        Console.Write("Enter new name (leave blank to keep current): ");
                        name = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(name))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Name cannot be empty.");
                            Console.ResetColor();
                        }
                    }

                    // Validate email input
                    while (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                    {
                        Console.Write("Enter new email (leave blank to keep current): ");
                        email = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(email))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Email cannot be empty.");
                            Console.ResetColor();
                            continue;
                        }

                        if (!IsValidEmail(email))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid email format. Please enter a valid email.");
                            Console.ResetColor();
                        }
                    }

                    // Update borrower with validated inputs
                    borrower.Name = name;
                    borrower.Email = email;

                    _dataAccess.UpdateBorrower(borrower);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Borrower updated successfully.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid borrower ID.");
                Console.ResetColor();
            }

            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private void DeleteBorrower()
        {
            Console.Clear();
            Console.WriteLine("Delete a Borrower");
            Console.WriteLine("===================");
            List<Borrower> borrowers = _dataAccess.GetAllBorrowers();
            if (borrowers.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Borrowers:");
                Console.ResetColor();
                foreach (var borrower in borrowers)
                {
                    Console.WriteLine($"ID: {borrower.BorrowerId}, Name: {borrower.Name}, Email: {borrower.Email}");
                }
            }
            int borrowerId;
            bool validInput = false;

            do
            {
                Console.Write("Enter borrower ID to delete: ");
                if (int.TryParse(Console.ReadLine(), out borrowerId))
                {
                   // List<Borrower> borrowers = _dataAccess.GetAllBorrowers();
                    Borrower borrower = borrowers.Find(b => b.BorrowerId == borrowerId);
                    if (borrower == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Borrower not found.");
                        Console.ResetColor();
                        Console.WriteLine("Press any key to return to the main menu...");
                        Console.ReadKey();
                        return;
                    }
                    validInput = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid borrower ID. Please enter a valid integer ID.");
                    Console.ResetColor();
                }
            } while (!validInput);

            _dataAccess.DeleteBorrower(borrowerId);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Borrower deleted successfully.");
            Console.ResetColor();
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }
        private void BorrowBook()
        {
            Console.Clear();
            Console.WriteLine("Borrow a Book");
            Console.WriteLine("===================");

            // Display all borrowers
            ViewAllBorrowers();

            int bookId, borrowerId;
            bool validBookId = false, validBorrowerId = false;

            do
            {
                Console.Write("Enter book ID to borrow: ");
                if (int.TryParse(Console.ReadLine(), out bookId))
                {
                    validBookId = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid book ID. Please enter a valid integer ID.");
                    Console.ResetColor();
                }
            } while (!validBookId);

            do
            {
                Console.Write("Enter borrower ID: ");
                if (int.TryParse(Console.ReadLine(), out borrowerId))
                {
                    validBorrowerId = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid borrower ID. Please enter a valid integer ID.");
                    Console.ResetColor();
                }
            } while (!validBorrowerId);

            // Check if the book exists
            Book book = _dataAccess.GetBookById(bookId);
            if (book == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Book not found.");
                Console.ResetColor();
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey();
                return;
            }

            // Check if the book is available
            if (!book.IsAvailable)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The book is currently not available for borrowing.");
                Console.ResetColor();
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey();
                return;
            }

            // Create a new transaction object
            Transaction transaction = new Transaction
            {
                BookId = bookId,
                BorrowerId = borrowerId,
                Date = DateTime.Now,
                IsBorrowed = true
            };

            try
            {
                // Record the transaction using your data access method
                _dataAccess.RecordTransaction(transaction);

                // Update book availability
                book.IsAvailable = false;
                _dataAccess.UpdateBook(book);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Book borrowed successfully.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error recording transaction: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey();
            }
        }




        private void ReturnBook()
        {
            Console.Clear();
            Console.WriteLine("Return a Book");
            Console.WriteLine("===================");

            int bookId, borrowerId;
            bool validBookId = false, validBorrowerId = false;

            do
            {
                Console.Write("Enter book ID to return: ");
                if (int.TryParse(Console.ReadLine(), out bookId))
                {
                    validBookId = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid book ID. Please enter a valid integer ID.");
                    Console.ResetColor();
                }
            } while (!validBookId);

            do
            {
                Console.Write("Enter borrower ID: ");
                if (int.TryParse(Console.ReadLine(), out borrowerId))
                {
                    validBorrowerId = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid borrower ID. Please enter a valid integer ID.");
                    Console.ResetColor();
                }
            } while (!validBorrowerId);

            // Create a new transaction object
            Transaction transaction = new Transaction
            {
                BookId = bookId,
                BorrowerId = borrowerId,
                Date = DateTime.Now,
                IsBorrowed = false // Marking the book as returned
            };

            // Record the transaction using your data access method
            _dataAccess.RecordTransaction(transaction);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Book returned successfully.");
            Console.ResetColor();
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }
        private void SearchBooks()
        {
            Console.Clear();
            Console.WriteLine("Search for Books");
            Console.WriteLine("=================");

            Console.Write("Enter search query (title, author, or genre): ");
            string query = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(query))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Search query cannot be empty.");
                Console.ResetColor();
            }
            else
            {
                List<Book> books = _dataAccess.SearchBooks(query);

                if (books.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Search results:");
                    Console.ResetColor();
                    foreach (var book in books)
                    {
                        Console.WriteLine($"ID: {book.BookId}, Title: {book.Title}, Author: {book.Author}, Genre: {book.Genre}, Available: {book.IsAvailable}");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No books found matching the search query.");
                }
            }

            Console.ResetColor();
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }
        private void ViewAllBooks()
        {
            Console.Clear();
            Console.WriteLine("View All Books");
            Console.WriteLine("=================");

            List<Book> books = _dataAccess.GetAllBooks();

            if (books.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Books in library:");
                Console.ResetColor();
                foreach (var book in books)
                {
                    Console.WriteLine($"ID: {book.BookId}, Title: {book.Title}, Author: {book.Author}, Genre: {book.Genre}, Available: {book.IsAvailable}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No books available in the library.");
            }

            Console.ResetColor();
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }
        private void ViewBorrowedBooksByBorrower()
        {
            Console.Clear();
            Console.WriteLine("View Borrowed Books by Borrower");
            Console.WriteLine("================================");
            ViewAllBorrowers();
            int borrowerId;
            bool validBorrowerId = false;

            do
            {
                Console.Write("Enter borrower ID: ");
                if (int.TryParse(Console.ReadLine(), out borrowerId))
                {
                    validBorrowerId = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid borrower ID.");
                    Console.ResetColor();
                }
            } while (!validBorrowerId);

            if (validBorrowerId)
            {
                List<Transaction> transactions = _dataAccess.GetBorrowedBooksByBorrower(borrowerId);

                if (transactions.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Borrowed books:");
                    Console.ResetColor();
                    foreach (var transaction in transactions)
                    {
                        if (transaction.IsBorrowed)
                        {
                            var book = _dataAccess.GetBookById(transaction.BookId);
                            if (book != null)
                            {
                                Console.WriteLine($"ID: {book.BookId}, Title: {book.Title}, Author: {book.Author}");
                            }
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No borrowed books found for this borrower.");
                }
            }

            Console.ResetColor();
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }
        private void ViewAllTransactions()
        {
            Console.Clear();
            Console.WriteLine("View All Transactions");
            Console.WriteLine("=====================");

            List<Transaction> transactions = _dataAccess.GetAllTransactions();

            if (transactions.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Transactions:");
                Console.ResetColor();
                foreach (var transaction in transactions)
                {
                    Console.WriteLine($"ID: {transaction.TransactionId}, Book ID: {transaction.BookId}, Borrower ID: {transaction.BorrowerId}, Date: {transaction.Date}, Is Borrowed: {transaction.IsBorrowed}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No transactions found.");
            }

            Console.ResetColor();
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private void ViewAllBorrowers()
        {
            Console.Clear();
            Console.WriteLine("View All Borrowers");
            Console.WriteLine("===================");

            List<Borrower> borrowers = _dataAccess.GetAllBorrowers();

            if (borrowers.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Borrowers:");
                Console.ResetColor();
                foreach (var borrower in borrowers)
                {
                    Console.WriteLine($"ID: {borrower.BorrowerId}, Name: {borrower.Name}, Email: {borrower.Email}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No borrowers available.");
            }

            Console.ResetColor();
        }

    }
}
