using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLinqProvider
{
    public class Service
    {
        private readonly QueryProvider _queryProvider;
        public Query<Contact> Contacts { get; init; }

        public Service()
        {
            _queryProvider = new CustomQueryProvider();
            Contacts = new Query<Contact>(_queryProvider);
        }
    }
}
