using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DemoLinqProvider
{
    public abstract class QueryProvider : IQueryProvider
    {
        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new Query<TElement>(this, expression);
        }

        object? IQueryProvider.Execute(Expression expression)
        {
            return Execute(expression);
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }

        public abstract string GetQueryText(Expression expression);
        public abstract object? Execute(Expression expression);
    }
}
