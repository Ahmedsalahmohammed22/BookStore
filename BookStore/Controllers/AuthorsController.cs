using BookStore.DTOs.AuthorDTOs;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AuthorsController : ControllerBase
    {
        BookStoreContext _context;
        public AuthorsController(BookStoreContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            List<Author> authors = await _context.Authors.ToListAsync();
            if(authors.Count == 0) return NotFound();
            return Ok(authors);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            if(id <= 0) return BadRequest("Invalid book ID.");
            Author author = await _context.Authors.FindAsync(id);
            if(author == null) return NotFound();
            AuthorDTO authorDTO = new AuthorDTO()
            {
                Id = id,
                Name = author.fullname,
                NumberOfBooks = author.numberOfBooks,
                Age = author.age,
                BIO = author.bio,
                books = author.Books.Select(b => b.title).ToList(),
            }; 
            return Ok(authorDTO);
        }
        [HttpPost("author")]
        public async Task<IActionResult> AddAuthor(AddAuthorDTO addAuthorDTO) 
        {
            if (addAuthorDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Author author = new Author()
            {
                fullname = addAuthorDTO.Author_name,
                bio = addAuthorDTO.Author_Bio,
                age = addAuthorDTO.Author_age,
                numberOfBooks = addAuthorDTO.numberOfBooks 
            };
            await _context.Authors.AddAsync(author);
            if (await _context.SaveChangesAsync() > 0) 
                return CreatedAtAction("GetAuthor", new { id = author.id }, author);

            else return BadRequest("Author doesn't save");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditAuthor(int id ,  EditAuthorDTO editauthorDTO)
        {
            if (id <= 0) return BadRequest("Invalid book ID.");
            Author author = await _context.Authors.FindAsync(id);
            if(author == null) return NotFound();
            if (ModelState.IsValid)
            {
                author.id = id;
                author.fullname = editauthorDTO.Author_name;
                author.bio = editauthorDTO.Author_Bio;
                author.numberOfBooks = editauthorDTO.numberOfBooks;
                author.age = editauthorDTO.Author_age;
                if (await _context.SaveChangesAsync() > 0)
                    return NoContent();
                else return BadRequest("Author did not edit");

            }
            else return BadRequest(ModelState);


        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid book ID.");
            Author author = await _context.Authors.FindAsync(id);
            if (author == null)
                return NotFound($"Book with ID {id} not found.");
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            List<Author> authors = await _context.Authors.ToListAsync();
            return Ok(authors);
        }

    }
}
