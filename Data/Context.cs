using Microsoft.EntityFrameworkCore;  

namespace website.Data
{
    public class Context : DbContext
    {
        public Context (DbContextOptions<Context> options)  
            : base(options)  
        {  
        }  
  
        public DbSet<website.Models.IdeaModel> Idea { get; set; } 
        public DbSet<website.Models.DonationModel> Donation { get; set; } 
    }
}