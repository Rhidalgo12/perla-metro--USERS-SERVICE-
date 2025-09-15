using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerlaMetro.data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        
    }
}