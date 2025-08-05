using Microsoft.EntityFrameworkCore;
using System;
using WebApplicationAPI.Model;

namespace WebApplicationAPI.Context
{
    public class WebApiContext:DbContext
    {
        public WebApiContext(DbContextOptions<WebApiContext> options)
           : base(options)
        {

        }

        public DbSet<Produto> Produtos { get; set; }
    }
}
