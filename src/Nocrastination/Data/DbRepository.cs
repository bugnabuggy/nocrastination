using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Nocrastination.Data
{
    public class DbRepository<T> : IRepository<T> where T : class
    {
        public IQueryable<T> Data { get; }
        private DbSet<T> _table;
        private ApplicationDbContext _ctx;

        public DbRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
            _table = _ctx.Set<T>();
            Data = _table;
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> query)
        {
            IQueryable<T> data = this.Data.AsNoTracking();

            if (query != null)
            {
                data = data.Where(query);
            }

            return data;
        }

        public T Add(T item)
        {
            _table.Add(item);
            _ctx.SaveChanges();
            return item;
        }

        public IEnumerable<T> Add(IEnumerable<T> items)
        {
            _table.AddRange(items);
            _ctx.SaveChanges();
            return items;
        }

        public T Delete(T item)
        {
            _table.Remove(item);
            _ctx.SaveChanges();
            return item;
        }

        public IEnumerable<T> Delete(IEnumerable<T> items)
        {
            _table.RemoveRange(items);
            _ctx.SaveChanges();
            return items;
        }

        public T Update(T item)
        {
            _table.Update(item);
            _ctx.SaveChanges();
            return item;
        }

        public IEnumerable<T> Update(IEnumerable<T> items)
        {
            _table.UpdateRange(items);
            _ctx.SaveChanges();
            return items;
        }
    }
}
