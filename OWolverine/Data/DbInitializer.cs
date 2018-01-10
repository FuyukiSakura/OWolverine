using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWolverine.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // TODO: Use migration
            context.Database.EnsureCreated();

            //Look for any universe
            if (context.Universes.Any())
            {
                return; // DB has been seeded
            }
        }
    }
}
