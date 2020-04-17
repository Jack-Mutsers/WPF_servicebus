using Microsoft.EntityFrameworkCore;
using Database.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options) { }

        public DbSet<Session> sessions { get; set; }
        public DbSet<Player> players { get; set; }
    }
}
