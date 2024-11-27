using BookStore.Models;
using BookStore.Repository;

namespace BookStore.UnitOfWorks
{
    public class UnitOfWork
    {
        BookStoreContext _context;
        GenericRepository<Book> bookReps;
        public UnitOfWork(BookStoreContext context)
        {
            _context = context;
        }
        public GenericRepository<Book> BookReps
        {
            get {
                if (bookReps == null)
                    bookReps = new GenericRepository<Book>(_context);
                return bookReps;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }


    }
}
