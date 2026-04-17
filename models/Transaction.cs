namespace BuggyBackend.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public bool IsReturned()
        {
            return ReturnDate != null;
        }
    }
}
