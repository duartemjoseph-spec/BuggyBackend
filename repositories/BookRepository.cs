using BuggyBackend.Models;

namespace BuggyBackend.Repositories
{
    public class BookRepository : IBookRepository
    {
        private List<Book> _books;
        private int _nextId;

        public BookRepository()
        {
            _books = new List<Book>();
            _nextId = 1;
            InitializeData();
        }

        private void InitializeData()
        {
            _books = new List<Book>
            {
                new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "978-0743273565", PublishedYear = 1925, AvailableCopies = 3 },
                new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", ISBN = "978-0061120084", PublishedYear = 1960, AvailableCopies = 2 },
                new Book { Id = 3, Title = "1984", Author = "George Orwell", ISBN = "978-0451524935", PublishedYear = 1949, AvailableCopies = 4 },
                new Book { Id = 4, Title = "Pride and Prejudice", Author = "Jane Austen", ISBN = "978-0141439518", PublishedYear = 1813, AvailableCopies = 1 },
                new Book { Id = 5, Title = "The Catcher in the Rye", Author = "J.D. Salinger", ISBN = "978-0316769488", PublishedYear = 1951, AvailableCopies = 0 }
            };
            _nextId = 6;
        }

        public List<Book> GetAll()
        {
            return _books;
        }

        public Book GetById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }

        public List<Book> SearchByTitle(string title)
        {
            return _books.Where(b => b.Title.ToLower().Contains(title.ToLower())).ToList();
        }

        public Book Create(Book book)
        {
            book.Id = _nextId++;
            _books.Add(book);
            return book;
        }

        public Book Update(int id, Book book)
        {
            var existingBook = GetById(id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.ISBN = book.ISBN;
                existingBook.PublishedYear = book.PublishedYear;
                existingBook.AvailableCopies = book.AvailableCopies;
                return existingBook;
            }
            return null;
        }

        public bool Delete(int id)
        {
            var book = GetById(id);
            if (book != null)
            {
                _books.Remove(book);
                return true;
            }
            return false;
        }

        public bool DecrementCopies(int id)
        {
            var book = GetById(id);
            if (book != null && book.AvailableCopies > 0)
            {
                book.AvailableCopies--;
                return true;
            }
            return false;
        }

        public bool IncrementCopies(int id)
        {
            var book = GetById(id);
            if (book != null)
            {
                book.AvailableCopies++;
                return true;
            }
            return false;
        }
    }
}
