using Microsoft.EntityFrameworkCore;
using NetShopAPI.Models;
using NetShopAPI.Models.CartModel;
using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;
using NetShopAPI.Models.OrderModel;
using NetShopAPI.Models.SupplyModel;
using NetShopAPI.Models.TestInfo;

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

        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<Position> Positions => Set<Position>();
        public DbSet<SupplyLog> SupplyLogs => Set<SupplyLog>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();

            modelBuilder.Entity<User>().HasIndex(x => x.Phone).IsUnique().HasFilter("[Phone] IS NOT NULL");

            modelBuilder.Entity<Order>(e =>
            {
                e.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(32);
                e.HasIndex(x => x.Status);
            });
        }
    }
}
