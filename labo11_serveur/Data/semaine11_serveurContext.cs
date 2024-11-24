using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using semaine11_serveur.Models;

namespace semaine11_serveur.Data
{
    public class semaine11_serveurContext : DbContext
    {
        public semaine11_serveurContext (DbContextOptions<semaine11_serveurContext> options)
            : base(options)
        {
        }

        public DbSet<semaine11_serveur.Models.Picture> Picture { get; set; } = default!;
    }
}
