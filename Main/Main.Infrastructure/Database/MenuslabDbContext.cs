using Main.Application.Enums;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using Scrypt;

namespace Main.Infrastructure.Database
{
    internal class MenuslabDbContext : DbContext
    {
        public MenuslabDbContext(DbContextOptions<MenuslabDbContext> options) : base(options)
        {
            var dbCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (!dbCreator.CanConnect())
                dbCreator.Create();

            if (!dbCreator.HasTables())
            {
                var users = new List<User>
                {
                    new() { Email = "sirkellyc@gmail.com", Role = Role.SuperAdmin.ToString(), PasswordHash = new ScryptEncoder().Encode("admin"), CountryId="Nigeria", Name="Kelly Osoba",PhoneNo="080",UserDevice="", AuthenticationType=AuthenticationType.EmailPassWord.ToString()}
                };
                dbCreator.CreateTables();

                Users.AddRange(users);
                SaveChangesAsync();
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantStaff> RestaurantStaff { get; set; }
        public DbSet<Dispatcher> Dispatchers { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderedItem> OrderedItems { get; set; }
        public DbSet<Fund> Funds { get; set; }
        public DbSet<SystemAdmin> SystemStaff { get; set; }
        public DbSet<Withdrawal> WithdrewFunds { get; set; }
        public DbSet<FeatureOfRestaurant> FeatureOfRestaurants { get; set; }
        public DbSet<OfferOrder> OfferOrders { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<PaymentGateway> PaymentGateways { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<RestaurantRating> RestaurantRatings { get; set; }
        public DbSet<DispatcherRating> DispatcherRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserEntityConfig().Configure(modelBuilder.Entity<User>());
            new PermissionEntityConfig().Configure(modelBuilder.Entity<Permission>());
            new RestaurantEntityConfig().Configure(modelBuilder.Entity<Restaurant>());
            new RestaurantStaffEntityConfig().Configure(modelBuilder.Entity<RestaurantStaff>());
            new DispatcherEntityConfig().Configure(modelBuilder.Entity<Dispatcher>());
            new MenuCategoryEntityConfig().Configure(modelBuilder.Entity<MenuCategory>());
            new MenuItemEntityConfig().Configure(modelBuilder.Entity<MenuItem>());
            new OrderEntityConfig().Configure(modelBuilder.Entity<Order>());
            new OrderedItemEntityConfig().Configure(modelBuilder.Entity<OrderedItem>());
            new FundEntityConfig().Configure(modelBuilder.Entity<Fund>());
            new SystemAdminEntityConfig().Configure(modelBuilder.Entity<SystemAdmin>());
            new WithdrawalEntityConfig().Configure(modelBuilder.Entity<Withdrawal>());
            new OfferOrderEntityConfig().Configure(modelBuilder.Entity<OfferOrder>());
            new CountryEntityConfig().Configure(modelBuilder.Entity<Country>());
            new BankEntityConfig().Configure(modelBuilder.Entity<Bank>());
            new RestaurantRatingEntityConfig().Configure(modelBuilder.Entity<RestaurantRating>());
            new DispatcherRatingEntityConfig().Configure(modelBuilder.Entity<DispatcherRating>());
        }

        public async Task<ActionResponse> CompleteAsync(object? response = null, CancellationToken cancellationToken = default)
        {
            try
            {
                await SaveChangesAsync(cancellationToken);
                return new ActionResponse { PayLoad=response};
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresException)
            {
                return postgresException.SqlState switch
                {
                    "23505" => BadRequestResult("The record already exists in the database."),
                    "23503" => BadRequestResult("Operation failed due to a foreign key constraint violation."),
                    _ => ServerExceptionError(postgresException),
                };
            }
            catch (PostgresException ex)
            {
                return ServerExceptionError(ex);
            }
            catch (Exception ex)
            {
                return ServerExceptionError(ex);
            }
        }

    }
}
