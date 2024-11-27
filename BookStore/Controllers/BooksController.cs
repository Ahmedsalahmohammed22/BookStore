using BookStore.DTOs.BookDTOs;
using BookStore.Models;
using BookStore.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class BooksController : ControllerBase
    {
        UnitOfWork _unit;
        public BooksController(UnitOfWork unit)
        {
            _unit = unit;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Book> books = await _unit.BookReps.GetAll();
            List<BookDTO> bookDTOs = new List<BookDTO>();
            if(books.Count == 0) return NotFound();
            foreach(var book in books)
            {
                BookDTO bookDTO = new BookDTO()
                {
                    id = book.id,
                    Title = book.title,
                    Price = book.price,
                    stock = book.stock,
                    photo = book.photoPath,
                    publishDate = book.publishDate,
                    author_name = book.author == null ? "" :book.author.fullname,
                    catalog_name = book.catalog == null ? "" :book.catalog.Name
                };
                bookDTOs.Add(bookDTO);

            }
            return Ok(bookDTOs.ToList());
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            Book book = await (_unit.BookReps.Get(id));
            if(book == null) return NotFound();
            else
            {
                BookDTO bookDTO = new BookDTO()
                {
                    id = id,
                    Title = book.title,
                    Price = book.price,
                    stock = book.stock,
                    photo = book.photoPath,
                    publishDate = book.publishDate,
                    author_name = book.author != null ? book.author.fullname : "",
                    catalog_name = book.catalog != null ? book.catalog.Name : ""
                };   
                return Ok(bookDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(AddBookDTO addBook)
        {
           if(!ModelState.IsValid) return BadRequest(ModelState);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "UploadPhoto", addBook.photoPath.FileName);
            FileStream file = new FileStream(path, FileMode.Create);
            addBook.photoPath.CopyTo(file);
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
            if(book.catalog_id == 0) 
                book.catalog_id = null;
            if(book.author_id == 0)
                book.author_id = null;
             _unit.BookReps.Add(book);
             _unit.Save();
            BookDTO bookDTO = new BookDTO()
            {
                Title = book.title,
                Price = book.price,
                stock = book.stock,
                photo = path,
                author_name = book.author == null ? "" : book.author.fullname,
                catalog_name = book.catalog == null ? "" : book.catalog.Name,

            };
            return CreatedAtAction("GetBook", new { id = book.id }, bookDTO);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditBook(int id , AddBookDTO updateBook)
        {
            if (id <= 0) return BadRequest("Invalid book ID.");
            Book book = await _unit.BookReps.Get(id);
            if (book == null) return NotFound($"Book with ID {id} not found.");
            string newPhotoPath = book.photoPath;
            if(updateBook.photoPath != null)
            {
                newPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadPhoto", updateBook.photoPath.FileName);
                using(FileStream file = new FileStream(newPhotoPath, FileMode.Create))
                {
                    await updateBook.photoPath.CopyToAsync(file);
                }

            }
            
            book.title = updateBook.title;
            book.price = updateBook.price;
            book.stock = updateBook.stock;
            book.author_id = updateBook.author_id == 0 ? null : updateBook.author_id;
            book.catalog_id = updateBook.catalog_id == 0 ? null : updateBook.catalog_id;
            book.publishDate = updateBook.publishDate;
            book.photoPath = newPhotoPath;
            _unit.BookReps.Save();
            return NoContent();

        }
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid book ID.");
            Book book = await _unit.BookReps.Get(id);
            if (book == null)
                return NotFound($"Book with ID {id} not found.");
            _unit.BookReps.Delete(id);
            _unit.Save();
            List<Book> books = await _unit.BookReps.GetAll();
            return Ok(books);
        }
    }
}
