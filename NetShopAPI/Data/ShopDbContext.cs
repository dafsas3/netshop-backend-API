using Microsoft.EntityFrameworkCore;
using NetShopAPI.Models;
using NetShopAPI.Models.CartModel;
using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;
using NetShopAPI.Models.OrderModel;
using NetShopAPI.Models.SupplyModel;

namespace NetShopAPI.Data
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options) { }
     
        public DbSet<User> Users => Set<User>();
        public DbSet<Category> ProductCategories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Position> Positions => Set<Position>();
        public DbSet<SupplyLog> SupplyLogs => Set<SupplyLog>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedOnAdd();

                entity.Property(u => u.FirstName).HasMaxLength(20).IsRequired();
                entity.Property(u => u.SurName).HasMaxLength(30).IsRequired();
                entity.Property(u => u.LastName).HasMaxLength(40);

                entity.Property(u => u.Email).HasMaxLength(320).IsRequired();
                entity.HasIndex(u => u.Email).IsUnique().HasDatabaseName("UX_Users_Email");

                entity.Property(u => u.Phone).HasMaxLength(30);
                entity.HasIndex(u => u.Phone).IsUnique().HasDatabaseName("UX_Users_Phone");

                entity.Property(u => u.NickName).HasMaxLength(20).IsRequired();
                entity.HasIndex(u => u.NickName).IsUnique().HasDatabaseName("UX_Users_NickName");

                entity.Property(u => u.PasswordHash).HasMaxLength(255).IsRequired();

                entity.Property(u => u.Role).HasMaxLength(20).IsRequired();

                entity.Property(u => u.CreatedAtUtc).IsRequired().HasColumnType("datetime(6)");
                entity.HasIndex(u => u.CreatedAtUtc);
            });

            modelBuilder.Entity<Order>(e =>
            {
                e.Property(x => x.Status)
                .HasColumnType("tinyint unsigned");

                e.HasIndex(x => x.Status);
            });
        }
    }
}
