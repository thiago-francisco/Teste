using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteAPI.Controllers;
using TesteAPI.Models;

namespace TesteAPI
{
    public class Contexto : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>().ToTable("Produto");
        }
    }
}
