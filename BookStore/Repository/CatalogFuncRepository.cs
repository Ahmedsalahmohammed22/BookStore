using BookStore.DTOs.BookDTOs;
using BookStore.DTOs.CatalogDTOs;
using BookStore.DTOs.OrderDTOs;
using BookStore.Models;

namespace BookStore.Repository
{
    public class CatalogFuncRepository
    {
        public List<CatalogDTO> convertcatalogsTocatalogDTO(List<Catalog> catalogs)
        {
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
            return catalogDTOs.ToList();
        }
        public CatalogDTO convertCatalogToCatalogDTO(Catalog catalog)
        {
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
            return catalogDTO;
        }
    }
}
