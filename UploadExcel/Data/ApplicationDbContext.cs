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
        public DbSet<Seller> sellers { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<CreditReview> Credit_Review { get; set; }
        public DbSet<DocumentSubmitted> document_submitted { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // BussinessPartners is keyless (probably from a view)
            modelBuilder.Entity<BussinessPartners>().HasNoKey();

            // Seller has a primary key `Id`, no need for .HasNoKey()
            modelBuilder.Entity<Seller>().HasKey(s => s.Id);
            modelBuilder.Entity<Property>().HasKey(s => s.Id);
            modelBuilder.Entity<CreditReview>().HasNoKey();
            modelBuilder.Entity<DocumentSubmitted>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }
    }
}



