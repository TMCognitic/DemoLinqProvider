using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DemoLinqProvider
{
    public class CustomQueryProvider : QueryProvider
    {
        public override object? Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public override string GetQueryText(Expression expression)
        {
            return new SqlBuilder().Translate(expression);
        }
    }
}
