using BookStore.DTOs.AuthorDTOs;
using BookStore.DTOs.BookDTOs;
using BookStore.DTOs.OrderDTOs;
using BookStore.Models;

namespace BookStore.Repository
{
    public class AuthorFuncRepository
    {
        public List<AuthorDTO> convertAuthorsToAuthorDTO(List<Author> authors)
        {
            List<AuthorDTO> authorDTOs = new List<AuthorDTO>();
            foreach (var author in authors)
            {
                AuthorDTO authorDTO = new AuthorDTO()
                {
                    Id = author.id,
                    Name = author.fullname,
                    BIO = author.bio,
                    NumberOfBooks = author.numberOfBooks,
                    Age = author.age,
                    books = author.Books.Select(b => b.title).ToList(),
                };
                authorDTOs.Add(authorDTO);
            }
            return authorDTOs.ToList();
        }
        public AuthorDTO convertAuthorToAuthorDTO(Author author)
        {
            AuthorDTO authorDTO = new AuthorDTO()
            {
                Id = author.id,
                Name = author.fullname,
                NumberOfBooks = author.numberOfBooks,
                Age = author.age,
                BIO = author.bio,
                books = author.Books.Select(b => b.title).ToList(),
            };
            return authorDTO;
        }

    }
}
