using miniapp.EntityFrameworkCore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace miniapp.EntityFrameworkCore.Context
{
    public class EntityContext: IdentityDbContext<AppUser>
    {
        public EntityContext(DbContextOptions<EntityContext> options): base(options)
        {

        }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<SubMenu> SubMenus { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<ToDo> ToDos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
