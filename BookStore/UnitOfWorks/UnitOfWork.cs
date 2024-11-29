using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace BookStore.UnitOfWorks
{
    public class UnitOfWork
    {
        //DataBase
        BookStoreContext _context;
        UserManager<IdentityUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        SignInManager<IdentityUser> _signInManager;
        //Repositories
        GenericRepository<Book> bookReps;
        GenericRepository<Author> authorReps;
        GenericRepository<Catalog> catalogReps;
        GenericRepository<Order> orderReps;
        UserRepository userReps;
        BookFuncRepository bookFuncReps;
        OrderFuncRepository orderFuncRepository;
        AuthorFuncRepository authorFuncRepository;
        CatalogFuncRepository catalogFuncRepository;

        public UnitOfWork(BookStoreContext context , UserManager<IdentityUser> userManager , SignInManager<IdentityUser> signInManager , RoleManager<IdentityRole> roleManager )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public CatalogFuncRepository CatalogFuncRepository
        {
            get
            {
                if(catalogFuncRepository == null)
                    catalogFuncRepository = new CatalogFuncRepository();
                return catalogFuncRepository;
            }
        }
        public AuthorFuncRepository AuthorFuncRepository
        {
            get
            {
                if(authorFuncRepository == null)
                    authorFuncRepository = new AuthorFuncRepository();
                return authorFuncRepository;
            }
        }
        public OrderFuncRepository OrderFuncRepository
        {
            get
            {
                if(orderFuncRepository == null)
                    orderFuncRepository = new OrderFuncRepository(_context);
                return orderFuncRepository;
            }
        }
        public BookFuncRepository BookFuncRepository
        {
            get
            {
                if(bookFuncReps == null)
                    bookFuncReps = new BookFuncRepository();
                return bookFuncReps;
            }
        }
        public UserRepository UserReps
        {
            get
            {
                if(userReps == null) 
                    userReps = new UserRepository(_userManager , _signInManager);
                return userReps;
            }
        }
        public GenericRepository<Author> AuthorReps
        {
            get
            {
                if (authorReps == null)
                    authorReps = new GenericRepository<Author>(_context);
                return authorReps;
            }
        }
        public GenericRepository<Order> OrderReps
        {
            get
            {
                if (orderReps == null)
                    orderReps = new GenericRepository<Order>(_context);
                return orderReps;
            }
        }
        public GenericRepository<Catalog> CatalogReps
        {
            get
            {
                if (catalogReps == null)
                    catalogReps = new GenericRepository<Catalog>(_context);
                return catalogReps;
            }
        }
        public GenericRepository<Book> BookReps
        {
            get {
                if (bookReps == null)
                    bookReps = new GenericRepository<Book>(_context);
                return bookReps;
            }
        }

        public async Task<int> Save()
        {
           return await _context.SaveChangesAsync();
        }


    }
}
