using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Webstore_MyElectronics.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Webstore_MyElectronics.Models
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet <Product> Products{get; set;}
        public DbSet <Category> Categories {get; set;}
        public DbSet <ShoppingCartItem> ShoppingCartItems {get; set;}
        public DbSet <UserWishList> UserWishLists{get; set;}


        public DbSet <Order> Orders{ get; set; }

        
        public DatabaseContext(DbContextOptions<DatabaseContext> opt): base(opt)
        {
            
        }

    }
}
