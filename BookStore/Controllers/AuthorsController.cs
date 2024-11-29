using BookStore.DTOs.AuthorDTOs;
using BookStore.DTOs.BookDTOs;
using BookStore.Models;
using BookStore.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AuthorsController : ControllerBase
    {
        UnitOfWork _unit;
        public AuthorsController(BookStoreContext context , UnitOfWork unit)
        {
            _unit = unit;
        }
        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(Summary = "select all authors", Description = "example: http/localhost/api/authors")]
        [SwaggerResponse(200, "Return all books ", typeof(List<AuthorDTO>))]
        [SwaggerResponse(404, "if not found any authors")]
        public async Task<IActionResult> GetAuthors()
        {
            List<Author> authors = await _unit.AuthorReps.GetAll();
            if(authors.Count == 0) return NotFound();
            return Ok(_unit.AuthorFuncRepository.convertAuthorsToAuthorDTO(authors));
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "can search on author by author id",
            Description = "Example: http//localhost/api/authors/id"
        )]
        [SwaggerResponse(200, "Return author data ", typeof(AuthorDTO))]
        [SwaggerResponse(404, "if no author founded")]
        [SwaggerResponse(400, "Invalid author id.")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            if(id <= 0) return BadRequest("Invalid author ID.");
            Author author = await _unit.AuthorReps.Get(id);
            if(author == null) return NotFound();

            return Ok(_unit.AuthorFuncRepository.convertAuthorToAuthorDTO(author));
        }
        [HttpPost("author")]
        [SwaggerOperation(
            Summary = "Add a new author to the library",
            Description = "Adds a new author using the provided details. Example: http://localhost/api/books"
        )]
        [SwaggerResponse(201, "The author was successfully created", typeof(Author))]
        [SwaggerResponse(400, "Invalid input data")]
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
            _unit.AuthorReps.Add(author);
            if (await _unit.Save() > 0) 
                return CreatedAtAction("GetAuthor", new { id = author.id }, author);

            else return BadRequest("Author doesn't save");
        }
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Edit an existing author",
            Description = "Updates the details of an existing author by its ID"
        )]
        [SwaggerResponse(204, "The author was successfully updated.")]
        [SwaggerResponse(400, "Invalid input or author ID.")]
        [SwaggerResponse(404, "Author not found.")]
        public async Task<IActionResult> EditAuthor(int id ,  EditAuthorDTO editauthorDTO)
        {
            if (id <= 0) return BadRequest("Invalid book ID.");
            Author author = await _unit.AuthorReps.Get(id);
            if(author == null) return NotFound();
            if (ModelState.IsValid)
            {
                author.id = id;
                author.fullname = editauthorDTO.Author_name;
                author.bio = editauthorDTO.Author_Bio;
                author.numberOfBooks = editauthorDTO.numberOfBooks;
                author.age = editauthorDTO.Author_age;
                if (await _unit.Save() > 0)
                    return NoContent();
                else return BadRequest("Author did not edit");

            }
            else return BadRequest(ModelState);


        }
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete an author",
            Description = "Deletes an author by its ID. Returns the updated list of author if successful."
        )]
        [SwaggerResponse(200, "Author deleted successfully and returns the updated list of authors.", typeof(List<AuthorDTO>))]
        [SwaggerResponse(400, "Invalid author ID or deletion failed.")]
        [SwaggerResponse(404, "Author with the specified ID not found.")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid book ID.");
            Author author = await _unit.AuthorReps.Get(id);
            if (author == null)
                return NotFound($"Book with ID {id} not found.");
            _unit.AuthorReps.Delete(id);
            if (await _unit.Save() > 0)
            {
                List<Author> authors = await _unit.AuthorReps.GetAll();
                return Ok(_unit.AuthorFuncRepository.convertAuthorsToAuthorDTO(authors));
            }else return BadRequest("Author did not delete");
        }

    }
}
