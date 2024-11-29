using BookStore.DTOs.BookDTOs;
using BookStore.Models;

namespace BookStore.Repository
{
    public class BookFuncRepository
    {
        public BookFuncRepository()
        {
            
        }
        public List<BookDTO> convertBooksToBookDTO(List<Book> books)
        {
            List<BookDTO> bookDTOs = new List<BookDTO>();
            foreach (var book in books)
            {
                BookDTO bookDTO = new BookDTO()
                {
                    id = book.id,
                    Title = book.title,
                    Price = book.price,
                    stock = book.stock,
                    photo = book.photoPath,
                    publishDate = book.publishDate,
                    author_name = book.author == null ? "" : book.author.fullname,
                    catalog_name = book.catalog == null ? "" : book.catalog.Name
                };
                bookDTOs.Add(bookDTO);
            }
            return bookDTOs.ToList();
        }
        public BookDTO convertBookToBookDTO(Book book)
        {
            BookDTO bookDTO = new BookDTO()
            {
                id = book.id,
                Title = book.title,
                Price = book.price,
                stock = book.stock,
                photo = book.photoPath,
                publishDate = book.publishDate,
                author_name = book.author != null ? book.author.fullname : "",
                catalog_name = book.catalog != null ? book.catalog.Name : ""
            };
            return bookDTO;
        }
        public string UploadBookPhoto(IFormFile photoFile)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "UploadPhoto", photoFile.FileName);
            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                photoFile.CopyTo(file);
            }
                return path;
        }
        public Book CreateBook(AddBookDTO addBook)
        {
            string path = UploadBookPhoto(addBook.photoPath);

            Book book = new Book()
            {
                title = addBook.title,
                price = addBook.price,
                stock = addBook.stock,
                photoPath = path,
                author_id = addBook.author_id,
                catalog_id = addBook.catalog_id,
                publishDate = addBook.publishDate,
            };
            if (book.catalog_id == 0)
                book.catalog_id = null;
            if (book.author_id == 0)
                book.author_id = null;
            return book;
        }
        public void UpdateSpecificBook(Book book , AddBookDTO updateBook , string newPhotoPath)
        {
            book.title = updateBook.title;
            book.price = updateBook.price;
            book.stock = updateBook.stock;
            book.author_id = updateBook.author_id == 0 ? null : updateBook.author_id;
            book.catalog_id = updateBook.catalog_id == 0 ? null : updateBook.catalog_id;
            book.publishDate = updateBook.publishDate;
            book.photoPath = newPhotoPath;
        }

    }
}
