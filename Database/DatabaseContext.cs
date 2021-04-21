using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
        public class DatabaseContext : DbContext
        {
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (optionsBuilder.IsConfigured == false)
                {
                    optionsBuilder.UseSqlServer(@"Data Source=PC;Initial Catalog=PIlabs;Integrated Security=True;MultipleActiveResultSets=True;");
                }
                base.OnConfiguring(optionsBuilder);
            }
            public virtual DbSet<User> Users { set; get; }
            public virtual DbSet<Dogovor> Dogovors { set; get; }
            public virtual DbSet<Product> Products { set; get; }
            public virtual DbSet<ProductSklad> ProductSklads { set; get; }
            public virtual DbSet<Sklad> Sklads { set; get; }
            public virtual DbSet<DogovorProduct> DogovorProducts { set; get; }
        }

}
