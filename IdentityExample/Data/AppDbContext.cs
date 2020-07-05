using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityExample.Data {
    //userTable
    public class AppDbContext : IdentityDbContext {
        public AppDbContext (DbContextOptions<AppDbContext> options) 
        : base (options) {

        }
    }
}