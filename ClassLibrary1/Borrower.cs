

namespace DAL
{
    public class Borrower
    {
        public int BorrowerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    
    public Borrower()
    {
        Name = ""; // Initialize with empty string or appropriate default value
        Email = "";  // Initialize with empty string or appropriate default value
        
    }
    }
}
