using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        BookStoreContext _context;
        public GenericRepository(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<List<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
        public async Task<TEntity> Get(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public async void Add(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }
        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

        }
        public async void Delete(int id)
        {
            TEntity entity = await _context.Set<TEntity>().FindAsync(id);
             _context.Set<TEntity>().Remove(entity);
        }
        public async void Save()
        {

            await _context.SaveChangesAsync();
        }
    }
}
