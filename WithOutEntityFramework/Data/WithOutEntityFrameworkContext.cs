using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WithOutEntityFramework.Models;

namespace WithOutEntityFramework.Data
{
    public class WithOutEntityFrameworkContext : DbContext
    {
        public WithOutEntityFrameworkContext (DbContextOptions<WithOutEntityFrameworkContext> options)
            : base(options)
        {
        }

        public DbSet<WithOutEntityFramework.Models.MovieViewModel> MovieViewModel { get; set; }
    }
}
