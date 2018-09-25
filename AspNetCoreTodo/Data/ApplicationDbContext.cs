using System;
using System.Collections.Generic;
using System.Text;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> Items {get;set;}

        public DbSet<Album> Albums {get;set;}

        public DbSet<Post> Posts {get;set;}

    }
}
