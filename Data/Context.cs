using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore; 
using website.Models;

namespace website.Data
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public Context (DbContextOptions<Context> options)  
            : base(options)  
        {  
        }  
        public DbSet<IdeaModel> Idea { get; set; } 
        public DbSet<DonationModel> Donation { get; set; } 
    }
}