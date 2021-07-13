using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHR
{
    class EntityContext : DbContext
    {
        public EntityContext(string name) : base(name)
        {
            Database.SetInitializer(new DataBaseInitializer());
        }

        public DbSet<Position> Positions { get; set; }

        //public DbSet<Order> Orders { get; set; }

        public DbSet<Vacancy> Vacancies { get; set; }

        public DbSet<Сandidate> Сandidates { get; set; }

    }
}
