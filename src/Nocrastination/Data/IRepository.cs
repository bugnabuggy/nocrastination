using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nocrastination.Data
{
    public interface IRepository<T>
    {
        IQueryable<T> Data { get; }

        IEnumerable<T> Get(Expression<Func<T, bool>> query);

        T Add(T item);
        T Update(T item);
        T Delete(T item);

        IEnumerable<T> Add(IEnumerable<T> items);
        IEnumerable<T> Update(IEnumerable<T> items);
        IEnumerable<T> Delete(IEnumerable<T> items);
    }
}
