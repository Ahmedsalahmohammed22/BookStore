using BookStore.DTOs.BookDTOs;
using BookStore.DTOs.CatalogDTOs;
using BookStore.DTOs.OrderDTOs;
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
    public class CatalogsController : ControllerBase
    {
        UnitOfWork _unit;
        public CatalogsController(UnitOfWork unit)
        {
            _unit = unit;
        }
        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve all catalogs",
                  Description = "Returns a list of all catalog entries in the system.")]
        [SwaggerResponse(200, "List of catalogs found.", typeof(IEnumerable<CatalogDTO>))]
        [SwaggerResponse(404, "No catalogs found.")]
        public async Task<IActionResult> GetAllCatalog()
        {
            var catalogs = await _unit.CatalogReps.GetAll();
            if(!catalogs.Any()) return NotFound();
        
            return Ok(_unit.CatalogFuncRepository.convertcatalogsTocatalogDTO(catalogs));

        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Retrieve a specific catalog by ID",
                  Description = "Returns a catalog entry if found.")]
        [SwaggerResponse(200, "Catalog found", typeof(CatalogDTO))]
        [SwaggerResponse(404, "Catalog not found")]
        public async Task<IActionResult> GetCatalog(int id)
        {
            Catalog catalog = await _unit.CatalogReps.Get(id);
            if (catalog == null) return NotFound();

            return Ok(_unit.CatalogFuncRepository.convertCatalogToCatalogDTO(catalog));
        }
        [HttpPost]
        [SwaggerOperation(Summary = "Add a new catalog",
                  Description = "Adds a new catalog with a name and description to the system.")]
        [SwaggerResponse(200, "Catalog added successfully", typeof(object))]
        [SwaggerResponse(400, "Bad request due to invalid data")]
        public async Task<IActionResult> AddCatalog(AddCatalogDTO addCatalogDTO)
        {
            if(addCatalogDTO == null) return BadRequest();
            if(!ModelState.IsValid) return BadRequest(ModelState);
            Catalog catalog = new Catalog()
            {
                Name = addCatalogDTO.Catalog_name,
                Description = addCatalogDTO.Catalog_description,
            };
            _unit.CatalogReps.Add(catalog);
            if(await _unit.Save() > 0 ) return Ok(new { Message = "Catalog added successfully", CatalogId = catalog.Id });
            else return BadRequest();
        }
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Edit an existing catalog",
                  Description = "Updates a catalog's name and description based on the provided ID.")]
        [SwaggerResponse(204, "Catalog updated successfully")]
        [SwaggerResponse(400, "Bad request due to invalid data")]
        [SwaggerResponse(404, "Catalog not found")]
        public async Task<IActionResult> EditCatalog(int id , AddCatalogDTO addCatalogDTO)
        {
            if (id <= 0) return BadRequest();
            Catalog catalog = await _unit.CatalogReps.Get(id);
            if (catalog == null) return NotFound($"Catalog with ID {id} not found.");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            catalog.Name = addCatalogDTO.Catalog_name;
            catalog.Description = addCatalogDTO.Catalog_description;
            if (await _unit.Save() > 0) return NoContent();
            else return BadRequest();
        }
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an existing catalog",
                  Description = "Deletes a catalog and retrieves the updated list of catalogs along with their books.")]
        [SwaggerResponse(200, "Catalog deleted successfully", typeof(List<CatalogDTO>))]
        [SwaggerResponse(400, "Bad request due to invalid data or failed deletion")]
        [SwaggerResponse(404, "Catalog not found")]
        public async Task<IActionResult> DeleteCatalog(int id)
        {
            if(id <= 0) return BadRequest();
            Catalog catalog = await _unit.CatalogReps.Get(id);
            if (catalog == null) return NotFound($"Catalog with ID {id} not found.");
            _unit.CatalogReps.Delete(id);
            if (await _unit.Save() > 0) {
                List<Catalog> catalogs = await _unit.CatalogReps.GetAll();
                List<CatalogDTO> catalogDTOs = new List<CatalogDTO>();
                foreach (var cat in catalogs)
                {
                    List<BookDetailsDTOs> bookDetailsDTOs = new List<BookDetailsDTOs>();
                    foreach (var book in cat.Books)
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
                        Id = cat.Id,
                        Name = cat.Name,
                        Description = cat.Description,
                        Books = bookDetailsDTOs
                    };
                    catalogDTOs.Add(catalogsDTO);
                }
                return Ok(catalogDTOs);
            }
            else return BadRequest();
        }
    }
}
