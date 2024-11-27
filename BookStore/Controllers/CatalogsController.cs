using BookStore.DTOs.BookDTOs;
using BookStore.DTOs.CatalogDTOs;
using BookStore.DTOs.OrderDTOs;
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
    public class CatalogsController : ControllerBase
    {
        BookStoreContext _context;
        public CatalogsController(BookStoreContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllCatalog()
        {
            var catalogs = await _context.Catalogs.ToListAsync();
            if(!catalogs.Any()) return NotFound();
            List<CatalogDTO> catalogDTOs = new List<CatalogDTO>();
            foreach (var catalog in catalogs) 
            {
                List<BookDetailsDTOs> booksDTO = new List<BookDetailsDTOs>();
                foreach (var book in catalog.Books)
                {
                    BookDetailsDTOs bookDTO = new BookDetailsDTOs()
                    {
                        Book_title = book.title,
                        Book_photo = book.photoPath,
                        Book_price = book.price,
                        Book_publishDate = book.publishDate,
                    };
                    booksDTO.Add(bookDTO);
                }
                CatalogDTO catalogDTO = new CatalogDTO()
                {
                    Id = catalog.Id,
                    Name = catalog.Name,
                    Description = catalog.Description,
                    Books = booksDTO
                };
                catalogDTOs.Add(catalogDTO);
            }
            return Ok(catalogDTOs);

        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCatalog(int id)
        {
            Catalog catalog = await _context.Catalogs.FindAsync(id);
            if (catalog == null) return NotFound();
            List<BookDetailsDTOs> booksDtoDetail = new List<BookDetailsDTOs>();
            foreach (var book in catalog.Books)
            {
                BookDetailsDTOs bookDTO = new BookDetailsDTOs()
                {
                    Book_title = book.title,
                    Book_photo = book.photoPath,
                    Book_price = book.price,
                    Book_publishDate = book.publishDate,
                };
                booksDtoDetail.Add(bookDTO);
            }
            CatalogDTO catalogDTO = new CatalogDTO()
            {
                Id = catalog.Id,
                Name = catalog.Name,
                Description = catalog.Description,
                Books = booksDtoDetail
            };
            return Ok(catalogDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddCatalog(AddCatalogDTO addCatalogDTO)
        {
            if(addCatalogDTO == null) return BadRequest();
            if(!ModelState.IsValid) return BadRequest(ModelState);
            Catalog catalog = new Catalog()
            {
                Name = addCatalogDTO.Catalog_name,
                Description = addCatalogDTO.Catalog_description,
            };
            await _context.Catalogs.AddAsync(catalog);
            if(await  _context.SaveChangesAsync() > 0 ) return Ok(new { Message = "Catalog added successfully", CatalogId = catalog.Id });
            else return BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCatalog(int id , AddCatalogDTO addCatalogDTO)
        {
            if (id <= 0) return BadRequest();
            Catalog catalog = await _context.Catalogs.FindAsync(id);
            if (catalog == null) return NotFound($"Catalog with ID {id} not found.");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            catalog.Name = addCatalogDTO.Catalog_name;
            catalog.Description = addCatalogDTO.Catalog_description;
            if (await _context.SaveChangesAsync() > 0) return NoContent();
            else return BadRequest();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCatalog(int id)
        {
            if(id <= 0) return BadRequest();
            Catalog catalog = _context.Catalogs.Find(id);
            if (catalog == null) return NotFound($"Catalog with ID {id} not found.");
            _context.Catalogs.Remove(catalog);
            List<Catalog> catalogs = await _context.Catalogs.ToListAsync();
            List<CatalogDTO> catalogDTOs = new List<CatalogDTO>();
            foreach (var cat in catalogs)
            {
                List<BookDetailsDTOs> bookDetailsDTOs = new List<BookDetailsDTOs>();
                foreach(var book in cat.Books)
                {
                    BookDetailsDTOs bkDetails = new BookDetailsDTOs()
                    {
                        Book_title = book.title,
                        Book_photo = book.photoPath,
                        Book_price = book.price,
                        Book_publishDate = book.publishDate,
                    };
                    bookDetailsDTOs.Add(bkDetails);
                }
                CatalogDTO catalogsDTO = new CatalogDTO()
                {
                    Name = cat.Name,
                    Description = cat.Description,
                    Books = bookDetailsDTOs
                };
                catalogDTOs.Add(catalogsDTO);
            }
            if (await _context.SaveChangesAsync() > 0) return Ok(catalogDTOs);
            else return BadRequest();
        }
    }
}
