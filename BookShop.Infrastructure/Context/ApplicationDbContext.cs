using BookShop.DataAccess.Entities;
using BookShop.DataAccess.Entities.Identity;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace BookShop.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, int>
    {
        private readonly IEncryptionProvider _encryptionProvider;
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            _encryptionProvider = new GenerateEncryptionProvider("8a4dcaaec64d412380fe4b02193cd26f");
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubSubject> SubSubjects { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order_State> Order_States { get; set; }

        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Book_Discount> Book_Discounts { get; set; }

        public DbSet<Card_Type> Card_Types { get; set; }
        public DbSet<Payment_Methods> Payment_Methods { get; set; }
        public DbSet<Shipping_Methods> Shipping_Methods { get; set; }

        public DbSet<Book_Image> Book_Images { get; set; }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<User_Reviews> User_Reviews { get; set; }
        public DbSet<UserRefreshToken> UserRefreshToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //modelBuilder.UseEncryption(_encryptionProvider);


            // Index on product name
            //modelBuilder.Entity<Book>().HasIndex(p => p.ISBN13);
            // Book Configuration
            modelBuilder.Entity<Book>(entity =>
            {
                // Navigation properties
                entity.HasOne(b => b.Subject)
                    .WithMany(s => s.Books)
                    .HasForeignKey(b => b.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.SubSubject)
                    .WithMany(ss => ss.Books)
                    .HasForeignKey(b => b.SubSubjectId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Subject Configuration
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                // Navigation properties
                entity.HasMany(s => s.Books)
                    .WithOne(b => b.Subject)
                    .HasForeignKey(b => b.SubjectId);
            });

            // SubSubject Configuration
            modelBuilder.Entity<SubSubject>(entity =>
            {
                entity.HasKey(ss => ss.Id);
                entity.Property(ss => ss.Name).IsRequired().HasMaxLength(100);

                // Navigation properties
                entity.HasMany(ss => ss.Books)
                    .WithOne(b => b.SubSubject)
                    .HasForeignKey(b => b.SubSubjectId);
            });


            // Discount Configuration
            modelBuilder.Entity<Discount>()
                .HasMany(p => p.Book_Discounts)
                .WithOne(p => p.discount)
                .HasForeignKey(p => p.DiscountId);

            // Review Configuration
            modelBuilder.Entity<Review>()
                .HasOne(o => o.Book)
                .WithMany(o => o.Reviews)
                .HasForeignKey(o => o.BookId);

            // Order Configuration => OrderItems
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Orders)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);


            // Order Configuration => shipping_Methods
            modelBuilder.Entity<Order>()
                .HasOne(o => o.shipping_Methods)
                .WithMany()
                .HasForeignKey(o => o.ShippingMethodsID);


            // Order Configuration => payment_Methods
            modelBuilder.Entity<Order>()
                .HasOne(o => o.payment_Methods)
                .WithMany()
                .HasForeignKey(o => o.PaymentMethodsID)
                .OnDelete(DeleteBehavior.NoAction);

            // Order Configuration => ApplicationUser
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ApplicationUser)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.ApplicationUserId);

            // Order Configuration => order_State
            modelBuilder.Entity<Order>()
                .HasOne(o => o.order_State)
                .WithMany()
                .HasForeignKey(o => o.OrderStateID)
                .OnDelete(DeleteBehavior.Restrict);


            // Payment_Methods Configuration => Card_type
            modelBuilder.Entity<Payment_Methods>()
                .HasOne(p => p.Card_type)
                .WithMany()
                .HasForeignKey(p => p.Card_TypeId)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.payment_Methods)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.Payment_MethodsID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Ignore<BaseEntity<int>>();

            // Set index on email to enhance search and make unique
            modelBuilder.Entity<ApplicationUser>().HasIndex(u => u.Email).IsUnique();











        }
    }
}
