using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DemoLinqProvider
{
    public class Query<T> : IQueryable<T>
    {
        private readonly QueryProvider _queryProvider;
        private readonly Expression _expression;

        public Query(QueryProvider queryProvider)
        {
            _queryProvider = queryProvider;
            _expression = Expression.Constant(this);
        }

        public Query(QueryProvider queryProvider, Expression expression)
        {
            _queryProvider = queryProvider;
            _expression = expression;
        }

        public Type ElementType
        {
            get
            {
                return typeof(T);
            }
        }

        public Expression Expression
        {
            get
            {
                return _expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return _queryProvider;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            IEnumerable<T>? enumerable = (IEnumerable<T>?)_queryProvider.Execute(_expression);

            if (enumerable is null)
                throw new InvalidOperationException();

            return enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return _queryProvider.GetQueryText(_expression);
        }
    }
}
