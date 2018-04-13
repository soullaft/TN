using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class DataService : IDataService
    {
        public IList<User> GetUsers()
        {
            return new List<User>();
        }
    }
}
