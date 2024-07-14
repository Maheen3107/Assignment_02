using System;


namespace DAL
{
     public class Transaction
    {
        private static int _nextTransactionId = 1;

        public int TransactionId { get; set; } // Use int type
        public int BookId { get; set; }
        public int BorrowerId { get; set; }
        public DateTime Date { get; set; }
        public bool IsBorrowed { get; set; }
        // Add other properties as needed

        public Transaction()
        {
            TransactionId = GetNextTransactionId(); // Assign next available int
        }

        private static int GetNextTransactionId()
        {
            return _nextTransactionId++;
        }
    }
}
