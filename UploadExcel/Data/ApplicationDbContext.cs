//using Microsoft.EntityFrameworkCore;
//using UploadExcel.Models;

//namespace UploadExcel.Data
//{
//    public class ApplicationDbContext : DbContext
//    {
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)  { }

//        public DbSet<BussinessPartners> BussinessPartners { get; set; }
//    }
//}

using Microsoft.EntityFrameworkCore;
using UploadExcel.Models;

namespace UploadExcel.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<BussinessPartners> business_partners { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BussinessPartners>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }
    }
}


