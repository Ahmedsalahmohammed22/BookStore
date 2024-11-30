using BookStore.DTOs.BookDTOs;
using BookStore.Models;
using BookStore.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

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
        [SwaggerOperation(Summary = "select all books" , Description = "exampleP: http/localhost/api/books")]
        [SwaggerResponse(200 , "Return all books " , typeof(List<BookDTO>))]
        [SwaggerResponse(404 , "if not found any books")]
        public async Task<IActionResult> GetAll()
        {
            List<Book> books = await _unit.BookReps.GetAll();
            if (books.Count == 0) return NotFound();
            return Ok(_unit.BookFuncRepository.convertBooksToBookDTO(books));
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "can search on book by book id", 
            Description = "Example: http//localhost/api/books/id"
        )]
        [SwaggerResponse(200, "Return book data ", typeof(BookDTO))]
        [SwaggerResponse(404, "if no book founded")]

        public async Task<IActionResult> GetBook(int id)
        {
            Book book = await (_unit.BookReps.Get(id));
            if(book == null) return NotFound();
            else
                return Ok(_unit.BookFuncRepository.convertBookToBookDTO(book));
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Add a new book to the library",
            Description = "Adds a new book using the provided details. Example: http://localhost/api/books"
        )]
        [SwaggerResponse(201, "The book was successfully created", typeof(BookDTO))]
        [SwaggerResponse(400, "Invalid input data")]
        public async Task<IActionResult> AddBook(AddBookDTO addBook)
        {
           if(!ModelState.IsValid) return BadRequest(ModelState);
            Book book = _unit.BookFuncRepository.CreateBook(addBook);
            _unit.BookReps.Add(book);
            if (await _unit.Save() > 0)
            {

                return CreatedAtAction("GetBook", new { id = book.id }, _unit.BookFuncRepository.convertBookToBookDTO(book));
            }
            else return BadRequest("Book did not add");
        }
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Edit an existing book",
            Description = "Updates the details of an existing book by its ID. If a new photo is provided, it replaces the old photo."
        )]
        [SwaggerResponse(204, "The book was successfully updated.")]
        [SwaggerResponse(400, "Invalid input or book ID.")]
        [SwaggerResponse(404, "Book not found.")]
        public async Task<IActionResult> EditBook(int id , AddBookDTO updateBook)
        {
            if (id <= 0) return BadRequest("Invalid book ID.");
            Book book = await _unit.BookReps.Get(id);
            if (book == null) return NotFound($"Book with ID {id} not found.");
            string newPhotoPath = book.photoPath;
            if(updateBook.photoPath != null)
            {
                newPhotoPath = _unit.BookFuncRepository.UploadBookPhoto(updateBook.photoPath);

            }
            _unit.BookFuncRepository.UpdateSpecificBook(book , updateBook, newPhotoPath);
            await _unit.Save();
            return NoContent();

        }
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a book",
            Description = "Deletes a book by its ID. Returns the updated list of books if successful."
        )]
        [SwaggerResponse(200, "Book deleted successfully and returns the updated list of books.", typeof(List<BookDTO>))]
        [SwaggerResponse(400, "Invalid book ID or deletion failed.")]
        [SwaggerResponse(404, "Book with the specified ID not found.")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid book ID.");
            Book book = await _unit.BookReps.Get(id);
            if (book == null)
                return NotFound($"Book with ID {id} not found.");
            _unit.BookReps.Delete(id);
            if (await _unit.Save() > 0)
            {
                List<Book> books = await _unit.BookReps.GetAll();
                
                return Ok(_unit.BookFuncRepository.convertBooksToBookDTO(books));
            }
            else return BadRequest("Book did not delete");
        }
    }
}
