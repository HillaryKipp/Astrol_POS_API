using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AstrolPOSAPI.Domain.Common;
using AstrolPOSAPI.Domain.Entities.Audit;
using AstrolPOSAPI.Domain.Entities.Core;
using AstrolPOSAPI.Domain.Entities.Identity;
using AstrolPOSAPI.Domain.Entities.POS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Persistence.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        private readonly AstrolPOSAPI.Application.Interfaces.Services.ICurrentUserService _currentUserService;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            AstrolPOSAPI.Application.Interfaces.Services.ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;
            var now = DateTimeOffset.UtcNow;

            // ---------------------------
            // 1. FIX: Materialize entries before looping
            // ---------------------------
            var entries = ChangeTracker
                .Entries<BaseAuditableEntity>()
                .ToList();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = now.DateTime;
                        entry.Entity.CreatedBy = userId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = now.DateTime;
                        entry.Entity.UpdatedBy = userId;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.DeletedDate = now.DateTime;
                        entry.Entity.DeletedBy = userId;
                        break;
                }
            }

            // ---------------------------
            // 2. FIX: Materialize audit entries before looping
            // ---------------------------
            var auditEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added
                            || e.State == EntityState.Modified
                            || e.State == EntityState.Deleted)
                .Where(e => e.Entity is not AuditLog)
                .ToList();

            foreach (var e in auditEntries)
            {
                var log = new AuditLog
                {
                    TableName = e.Metadata.GetTableName() ?? e.Entity.GetType().Name,
                    Action = e.State.ToString(),
                    OccurredAt = now,
                    UserId = userId // Capture the user ID here
                };

                // Primary Key
                var key = e.Properties
                    .Where(p => p.Metadata.IsPrimaryKey())
                    .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue?.ToString()); // Ensure string conversion

                log.KeyValues = JsonSerializer.Serialize(key);

                // OLD VALUES
                if (e.State == EntityState.Modified || e.State == EntityState.Deleted)
                {
                    var oldVals = e.Properties
                        .ToDictionary(p => p.Metadata.Name, p => p.OriginalValue?.ToString());
                    log.OldValues = JsonSerializer.Serialize(oldVals);
                }

                // NEW VALUES
                if (e.State == EntityState.Added || e.State == EntityState.Modified)
                {
                    var newVals = e.Properties
                        .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue?.ToString());
                    log.NewValues = JsonSerializer.Serialize(newVals);
                }

                // THIS modifies ChangeTracker → MUST run AFTER .ToList()
                AuditLogs.Add(log);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Store> Stores => Set<Store>();
        public DbSet<StoreType> StoreTypes => Set<StoreType>();
        public DbSet<NoSeries> NoSeries => Set<NoSeries>();
        public DbSet<DrawerGroup> DrawerGroups => Set<DrawerGroup>();
        public DbSet<Terminal> Terminals => Set<Terminal>();
        public DbSet<DefaultScreen> DefaultScreens => Set<DefaultScreen>();
        public DbSet<Drawer> Drawers => Set<Drawer>();
        public DbSet<AssignedDrawer> AssignedDrawers => Set<AssignedDrawer>();
        public DbSet<TouchScreen> TouchScreens => Set<TouchScreen>();
        public DbSet<TouchScreenButton> TouchScreenButtons => Set<TouchScreenButton>();

        // POS Item and Sales
        public DbSet<ItemCategory> ItemCategories => Set<ItemCategory>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
        public DbSet<SalesOrderLine> SalesOrderLines => Set<SalesOrderLine>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Receipt> Receipts => Set<Receipt>();

        // Identity/Auth related

        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<Domain.Entities.Identity.OTP> OTPs => Set<Domain.Entities.Identity.OTP>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Identity.UserStore> UserStores => Set<AstrolPOSAPI.Domain.Entities.Identity.UserStore>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Core.GeneralSettings> GeneralSettings => Set<AstrolPOSAPI.Domain.Entities.Core.GeneralSettings>();

        // Accounting & Purchasing
        public DbSet<AstrolPOSAPI.Domain.Entities.Accounting.GLAccount> GLAccounts => Set<AstrolPOSAPI.Domain.Entities.Accounting.GLAccount>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Accounting.VendorPostingGroup> VendorPostingGroups => Set<AstrolPOSAPI.Domain.Entities.Accounting.VendorPostingGroup>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Accounting.GenBusPostingGroup> GenBusPostingGroups => Set<AstrolPOSAPI.Domain.Entities.Accounting.GenBusPostingGroup>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor> Vendors => Set<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Purchasing.PurchaseHeader> PurchaseHeaders => Set<AstrolPOSAPI.Domain.Entities.Purchasing.PurchaseHeader>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Purchasing.PurchaseLine> PurchaseLines => Set<AstrolPOSAPI.Domain.Entities.Purchasing.PurchaseLine>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Purchasing.PurchInvHeader> PurchInvHeaders => Set<AstrolPOSAPI.Domain.Entities.Purchasing.PurchInvHeader>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Purchasing.PurchInvLine> PurchInvLines => Set<AstrolPOSAPI.Domain.Entities.Purchasing.PurchInvLine>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Accounting.GLEntry> GLEntries => Set<AstrolPOSAPI.Domain.Entities.Accounting.GLEntry>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Accounting.VendorLedgerEntry> VendorLedgerEntries => Set<AstrolPOSAPI.Domain.Entities.Accounting.VendorLedgerEntry>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Accounting.Customer> Customers => Set<AstrolPOSAPI.Domain.Entities.Accounting.Customer>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Accounting.CustomerPostingGroup> CustomerPostingGroups => Set<AstrolPOSAPI.Domain.Entities.Accounting.CustomerPostingGroup>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Accounting.GenProdPostingGroup> GenProdPostingGroups => Set<AstrolPOSAPI.Domain.Entities.Accounting.GenProdPostingGroup>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Accounting.GeneralPostingSetup> GeneralPostingSetups => Set<AstrolPOSAPI.Domain.Entities.Accounting.GeneralPostingSetup>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Accounting.BankAccount> BankAccounts => Set<AstrolPOSAPI.Domain.Entities.Accounting.BankAccount>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Accounting.BankLedgerEntry> BankLedgerEntries => Set<AstrolPOSAPI.Domain.Entities.Accounting.BankLedgerEntry>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Accounting.CustomerLedgerEntry> CustomerLedgerEntries => Set<AstrolPOSAPI.Domain.Entities.Accounting.CustomerLedgerEntry>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Accounting.VATPostingSetup> VATPostingSetups => Set<AstrolPOSAPI.Domain.Entities.Accounting.VATPostingSetup>();
        public DbSet<AstrolPOSAPI.Domain.Entities.Purchasing.PaymentVoucher> PaymentVouchers => Set<AstrolPOSAPI.Domain.Entities.Purchasing.PaymentVoucher>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AstrolPOSAPI.Domain.Entities.Purchasing.PaymentVoucher>(e =>
            {
                e.Property(p => p.No).IsRequired().HasMaxLength(32);
                e.Property(p => p.Amount).HasPrecision(18, 4);
                e.HasIndex(p => new { p.CompanyId, p.No }).IsUnique();
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Accounting.VATPostingSetup>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.Property(p => p.VATPercentage).HasPrecision(18, 4);
                e.HasIndex(p => new { p.CompanyId, p.Code }).IsUnique();
            });

            builder.Entity<Company>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.Property(p => p.Name).IsRequired().HasMaxLength(256);
                e.HasIndex(p => p.Code).IsUnique();
            });

            builder.Entity<StoreType>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.Property(p => p.Description).IsRequired().HasMaxLength(256);
                e.HasIndex(p => p.Code).IsUnique();
            });

            builder.Entity<NoSeries>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.Property(p => p.Description).IsRequired().HasMaxLength(128);
            });

            builder.Entity<DrawerGroup>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.Property(p => p.Name).IsRequired().HasMaxLength(128);
                e.Property(p => p.Description).IsRequired().HasMaxLength(128);
                e.Property(p => p.CompanyId).IsRequired();
                e.Property(p => p.StoreOfOperationId).IsRequired();

                e.HasOne(dg => dg.Company)
                    .WithMany()
                    .HasForeignKey(dg => dg.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(dg => dg.StoreOfOperation)
                    .WithMany()
                    .HasForeignKey(dg => dg.StoreOfOperationId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasQueryFilter(dg => dg.DeletedDate == null);
            });

            builder.Entity<Terminal>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.Property(p => p.Description).IsRequired().HasMaxLength(128);
                e.Property(p => p.CompanyId).IsRequired();
                e.Property(p => p.StoreOfOperationId).IsRequired();

                e.HasOne(t => t.Company)
                    .WithMany()
                    .HasForeignKey(t => t.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(t => t.StoreOfOperation)
                    .WithMany()
                    .HasForeignKey(t => t.StoreOfOperationId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasQueryFilter(t => t.DeletedDate == null);
            });

            builder.Entity<DefaultScreen>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.Property(p => p.Description).IsRequired().HasMaxLength(128);
                e.Property(p => p.CompanyId).IsRequired();
                e.Property(p => p.StoreOfOperationId).IsRequired();

                e.HasOne(ds => ds.Company)
                    .WithMany()
                    .HasForeignKey(ds => ds.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ds => ds.StoreOfOperation)
                    .WithMany()
                    .HasForeignKey(ds => ds.StoreOfOperationId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasQueryFilter(ds => ds.DeletedDate == null);
            });

            builder.Entity<Drawer>(e =>
            {
                e.Property(p => p.Status).HasConversion<int>();

                // Foreign Keys

                e.Property(p => p.CompanyId).IsRequired();
                e.Property(p => p.StoreOfOperationId).IsRequired();
                e.Property(p => p.DrawerGroupId).IsRequired();
                e.Property(p => p.TerminalId).IsRequired();

                // Relationships
                e.HasOne(d => d.Company)
                    .WithMany()
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(d => d.StoreOfOperation)
                    .WithMany()
                    .HasForeignKey(d => d.StoreOfOperationId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(d => d.DrawerGroup)
                    .WithMany()
                    .HasForeignKey(d => d.DrawerGroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(d => d.Terminal)
                    .WithMany()
                    .HasForeignKey(d => d.TerminalId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(d => d.DefaultScreen)
                    .WithMany()
                    .HasForeignKey(d => d.DefaultScreenId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasQueryFilter(d => d.DeletedDate == null);
                e.HasIndex(d => d.StoreOfOperationId);
            });

            builder.Entity<AssignedDrawer>(e =>
            {
                e.Property(p => p.OpenCash).HasColumnType("decimal(18,2)");

                // Foreign Keys

                e.Property(p => p.CompanyId).IsRequired();
                e.Property(p => p.StoreOfOperationId).IsRequired();
                e.Property(p => p.DrawerId).IsRequired();
                e.Property(p => p.UserId).IsRequired();

                // Relationships
                e.HasOne(ad => ad.Company)
                    .WithMany()
                    .HasForeignKey(ad => ad.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ad => ad.StoreOfOperation)
                    .WithMany()
                    .HasForeignKey(ad => ad.StoreOfOperationId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ad => ad.Drawer)
                    .WithMany()
                    .HasForeignKey(ad => ad.DrawerId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ad => ad.User)
                    .WithMany()
                    .HasForeignKey(ad => ad.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ad => ad.DefaultScreen)
                    .WithMany()
                    .HasForeignKey(ad => ad.DefaultScreenId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasQueryFilter(ad => ad.DeletedDate == null);
                e.HasIndex(ad => ad.StoreOfOperationId);
                e.HasIndex(ad => ad.UserId);
                e.HasIndex(ad => ad.DrawerId);
            });

            builder.Entity<TouchScreen>(e =>
            {
                e.Property(p => p.ScreenName).IsRequired().HasMaxLength(100);
                e.Property(p => p.Description).HasMaxLength(500);
                e.Property(p => p.CompanyId).IsRequired();
                e.Property(p => p.StoreOfOperationId).IsRequired();

                e.HasOne(ts => ts.Company)
                    .WithMany()
                    .HasForeignKey(ts => ts.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ts => ts.StoreOfOperation)
                    .WithMany()
                    .HasForeignKey(ts => ts.StoreOfOperationId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(ts => ts.Buttons)
                    .WithOne(b => b.TouchScreen)
                    .HasForeignKey(b => b.TouchScreenId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasQueryFilter(ts => ts.DeletedDate == null);
                e.HasIndex(ts => ts.StoreOfOperationId);
            });

            builder.Entity<TouchScreenButton>(e =>
            {
                e.Property(p => p.ItemName).IsRequired().HasMaxLength(100);
                e.Property(p => p.BackgroundColor).IsRequired().HasMaxLength(7);
                e.Property(p => p.TextColor).IsRequired().HasMaxLength(7);
                e.Property(p => p.ImageUrl).HasMaxLength(512);
                e.Property(p => p.CompanyId).IsRequired();
                e.Property(p => p.StoreOfOperationId).IsRequired();
                e.Property(p => p.TouchScreenId).IsRequired();

                // Enum conversions
                e.Property(p => p.ButtonType).HasConversion<int>();
                e.Property(p => p.Shape).HasConversion<int>();

                e.HasOne(b => b.Company)
                    .WithMany()
                    .HasForeignKey(b => b.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(b => b.StoreOfOperation)
                    .WithMany()
                    .HasForeignKey(b => b.StoreOfOperationId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(b => b.TouchScreen)
                    .WithMany(ts => ts.Buttons)
                    .HasForeignKey(b => b.TouchScreenId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasQueryFilter(b => b.DeletedDate == null);
                e.HasIndex(b => b.TouchScreenId);
                e.HasIndex(b => b.StoreOfOperationId);
                e.HasIndex(b => new { b.Row, b.Column });
            });

            builder.Entity<AuditLog>(e =>
            {
                e.Property(p => p.TableName).IsRequired().HasMaxLength(256);
                e.Property(p => p.Action).IsRequired().HasMaxLength(32);
            });

            builder.Entity<AppUser>(e =>
            {
                e.Property(p => p.EmpNo).HasMaxLength(32);
                e.Property(p => p.Name).HasMaxLength(256);
                e.Property(p => p.NationalID).HasMaxLength(64);

                // Relationships

                e.HasOne(u => u.Company)
                    .WithMany()
                    .HasForeignKey(u => u.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);


                e.HasOne(u => u.StoreOfOperation)
                    .WithMany()
                    .HasForeignKey(u => u.StoreOfOperationId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes for performance

                e.HasIndex(u => u.CompanyId);
                e.HasIndex(u => u.StoreOfOperationId);
            });

            // UserStore (junction table for many-to-many User-Store relationship)
            builder.Entity<AstrolPOSAPI.Domain.Entities.Identity.UserStore>(e =>
            {
                // Composite primary key
                e.HasKey(us => new { us.UserId, us.StoreId });

                // Relationships
                e.HasOne(us => us.User)
                    .WithMany(u => u.UserStores)
                    .HasForeignKey(us => us.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(us => us.Store)
                    .WithMany()
                    .HasForeignKey(us => us.StoreId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                e.HasIndex(us => us.UserId);
                e.HasIndex(us => us.StoreId);
                e.HasIndex(us => us.IsPrimary);
            });

            // Store entity configuration

            builder.Entity<Store>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.Property(p => p.Name).IsRequired().HasMaxLength(256);
                e.Property(p => p.CompanyId).IsRequired();


                e.HasOne(s => s.Company)
                    .WithMany(c => c.Stores)
                    .HasForeignKey(s => s.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);


                e.HasOne(s => s.StoreType)
                    .WithMany()
                    .HasForeignKey(s => s.StoreTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Query filter for soft deletes

                e.HasQueryFilter(s => s.DeletedDate == null);

                // Indexes

                e.HasIndex(s => s.CompanyId);
                e.HasIndex(s => s.Code);
            });

            // Permission entity configuration

            builder.Entity<Permission>(e =>
            {
                e.Property(p => p.ResourceName).IsRequired().HasMaxLength(128);


                e.HasOne(p => p.User)
                    .WithMany()
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);


                e.HasOne(p => p.Role)
                    .WithMany()
                    .HasForeignKey(p => p.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Query filter for soft deletes

                e.HasQueryFilter(p => p.DeletedDate == null);

                // Indexes

                e.HasIndex(p => new { p.UserId, p.ResourceName });
                e.HasIndex(p => new { p.RoleId, p.ResourceName });
            });

            // OTP entity configuration

            builder.Entity<Domain.Entities.Identity.OTP>(e =>
            {
                e.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(20);
                e.Property(p => p.OTPCode).IsRequired().HasMaxLength(10);
                e.Property(p => p.Purpose).IsRequired().HasMaxLength(64);


                e.HasOne(o => o.User)
                    .WithMany()
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes for quick lookups

                e.HasIndex(o => new { o.UserId, o.Purpose, o.IsVerified });
                e.HasIndex(o => o.ExpiresAt);
            });

            // GeneralSettings entity configuration
            builder.Entity<AstrolPOSAPI.Domain.Entities.Core.GeneralSettings>(e =>
            {
                e.Property(p => p.CompanyId).IsRequired();
                e.Property(p => p.LogoUrl).HasMaxLength(512);
                e.Property(p => p.CompanyName).HasMaxLength(256);
                e.Property(p => p.CompanySlogan).HasMaxLength(512);


                e.Property(p => p.PrimaryColor).HasMaxLength(20);
                e.Property(p => p.SecondaryColor).HasMaxLength(20);
                e.Property(p => p.TertiaryColor).HasMaxLength(20);
                e.Property(p => p.AccentColor).HasMaxLength(20);
                e.Property(p => p.BackgroundColor).HasMaxLength(20);
                e.Property(p => p.BackgroundImageUrl).HasMaxLength(512);


                e.Property(p => p.Currency).HasMaxLength(10);
                e.Property(p => p.CurrencySymbol).HasMaxLength(5);
                e.Property(p => p.DateFormat).HasMaxLength(20);
                e.Property(p => p.TimeFormat).HasMaxLength(5);
                e.Property(p => p.Timezone).HasMaxLength(100);


                e.Property(p => p.TaxNumber).HasMaxLength(50);
                e.Property(p => p.ReceiptFooter).HasMaxLength(500);
                e.Property(p => p.SupportEmail).HasMaxLength(256);
                e.Property(p => p.SupportPhone).HasMaxLength(20);

                // One-to-one relationship with Company
                e.HasOne(gs => gs.Company)
                    .WithOne()
                    .HasForeignKey<AstrolPOSAPI.Domain.Entities.Core.GeneralSettings>(gs => gs.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Query filter for soft deletes
                e.HasQueryFilter((AstrolPOSAPI.Domain.Entities.Core.GeneralSettings gs) => gs.DeletedDate == null);

                // Index on CompanyId for quick lookups
                e.HasIndex(gs => gs.CompanyId).IsUnique();
            });

            // ItemCategory configuration
            builder.Entity<ItemCategory>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.Property(p => p.Name).IsRequired().HasMaxLength(128);
                e.Property(p => p.Description).HasMaxLength(500);
                e.Property(p => p.CompanyId).IsRequired();
                e.Property(p => p.StoreOfOperationId).IsRequired();

                e.HasOne(c => c.ParentCategory)
                    .WithMany()
                    .HasForeignKey(c => c.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(c => c.Company)
                    .WithMany()
                    .HasForeignKey(c => c.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(c => c.StoreOfOperation)
                    .WithMany()
                    .HasForeignKey(c => c.StoreOfOperationId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasQueryFilter(c => c.DeletedDate == null);
                e.HasIndex(c => new { c.CompanyId, c.Code }).IsUnique();
            });

            // Item configuration
            builder.Entity<Item>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.Property(p => p.Name).IsRequired().HasMaxLength(128);
                e.Property(p => p.Description).HasMaxLength(500);
                e.Property(p => p.UnitOfMeasure).HasMaxLength(16);
                e.Property(p => p.UnitPrice).HasPrecision(18, 4);
                e.Property(p => p.CostPrice).HasPrecision(18, 4);
                e.Property(p => p.QuantityOnHand).HasPrecision(18, 4);
                e.Property(p => p.ReorderLevel).HasPrecision(18, 4);
                e.Property(p => p.TaxRate).HasPrecision(5, 2);
                e.Property(p => p.Barcode).HasMaxLength(100);
                e.Property(p => p.ImageUrl).HasMaxLength(512);
                e.Property(p => p.CompanyId).IsRequired();
                e.Property(p => p.StoreOfOperationId).IsRequired();

                e.HasOne(i => i.Category)
                    .WithMany(c => c.Items)
                    .HasForeignKey(i => i.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);

                e.HasOne(i => i.Company)
                    .WithMany()
                    .HasForeignKey(i => i.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(i => i.StoreOfOperation)
                    .WithMany()
                    .HasForeignKey(i => i.StoreOfOperationId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(i => i.GenProdPostingGroup)
                    .WithMany()
                    .HasForeignKey(i => i.GenProdPostingGroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasQueryFilter(i => i.DeletedDate == null);
                e.HasIndex(i => new { i.CompanyId, i.Code }).IsUnique();
                e.HasIndex(i => i.Barcode);
            });

            // SalesOrder configuration
            builder.Entity<SalesOrder>(e =>
            {
                e.Property(p => p.OrderNo).IsRequired().HasMaxLength(50);
                e.Property(p => p.CustomerName).HasMaxLength(256);
                e.Property(p => p.Status).HasConversion<int>();
                e.Property(p => p.Subtotal).HasPrecision(18, 4);
                e.Property(p => p.DiscountAmount).HasPrecision(18, 4);
                e.Property(p => p.TaxAmount).HasPrecision(18, 4);
                e.Property(p => p.TotalAmount).HasPrecision(18, 4);
                e.Property(p => p.AmountPaid).HasPrecision(18, 4);
                e.Property(p => p.ChangeGiven).HasPrecision(18, 4);
                e.Property(p => p.CompanyId).IsRequired();
                e.Property(p => p.StoreOfOperationId).IsRequired();
                e.Property(p => p.CashierId).IsRequired();
                e.Property(p => p.DrawerId).IsRequired();

                e.HasOne(o => o.Cashier)
                    .WithMany()
                    .HasForeignKey(o => o.CashierId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(o => o.Drawer)
                    .WithMany()
                    .HasForeignKey(o => o.DrawerId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(o => o.Company)
                    .WithMany()
                    .HasForeignKey(o => o.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(o => o.StoreOfOperation)
                    .WithMany()
                    .HasForeignKey(o => o.StoreOfOperationId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(o => o.Lines)
                    .WithOne(l => l.SalesOrder)
                    .HasForeignKey(l => l.SalesOrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(o => o.Payments)
                    .WithOne(p => p.SalesOrder)
                    .HasForeignKey(p => p.SalesOrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasQueryFilter(o => o.DeletedDate == null);
                e.HasIndex(o => o.OrderNo).IsUnique();
                e.HasIndex(o => new { o.DrawerId, o.Status });
                e.HasIndex(o => o.OrderDate);
            });

            // SalesOrderLine configuration
            builder.Entity<SalesOrderLine>(e =>
            {
                e.Property(p => p.ItemCode).IsRequired().HasMaxLength(32);
                e.Property(p => p.ItemName).IsRequired().HasMaxLength(128);
                e.Property(p => p.UnitOfMeasure).HasMaxLength(16);
                e.Property(p => p.Quantity).HasPrecision(18, 4);
                e.Property(p => p.UnitPrice).HasPrecision(18, 4);
                e.Property(p => p.DiscountAmount).HasPrecision(18, 4);
                e.Property(p => p.TaxRate).HasPrecision(5, 2);
                e.Property(p => p.TaxAmount).HasPrecision(18, 4);
                e.Property(p => p.LineTotal).HasPrecision(18, 4);
                e.Property(p => p.SalesOrderId).IsRequired();
                e.Property(p => p.ItemId).IsRequired();

                e.HasOne(l => l.Item)
                    .WithMany()
                    .HasForeignKey(l => l.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasQueryFilter(l => l.DeletedDate == null);
            });

            // Payment configuration
            builder.Entity<Payment>(e =>
            {
                e.Property(p => p.PaymentMethod).HasConversion<int>();
                e.Property(p => p.Amount).HasPrecision(18, 4);
                e.Property(p => p.ReferenceNo).HasMaxLength(100);
                e.Property(p => p.PhoneNumber).HasMaxLength(20);
                e.Property(p => p.CardLastFour).HasMaxLength(4);
                e.Property(p => p.Status).HasConversion<int>();
                e.Property(p => p.ResponseMessage).HasMaxLength(500);
                e.Property(p => p.SalesOrderId).IsRequired();

                e.HasQueryFilter(p => p.DeletedDate == null);
                e.HasIndex(p => p.ReferenceNo);
            });

            // Receipt configuration
            builder.Entity<Receipt>(e =>
            {
                e.Property(p => p.ReceiptNo).IsRequired().HasMaxLength(50);
                e.Property(p => p.TotalAmount).HasPrecision(18, 4);
                e.Property(p => p.AmountPaid).HasPrecision(18, 4);
                e.Property(p => p.ChangeGiven).HasPrecision(18, 4);
                e.Property(p => p.EmailAddress).HasMaxLength(256);
                e.Property(p => p.PhoneNumber).HasMaxLength(20);
                e.Property(p => p.SalesOrderId).IsRequired();

                e.HasOne(r => r.SalesOrder)
                    .WithMany()
                    .HasForeignKey(r => r.SalesOrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasQueryFilter(r => r.DeletedDate == null);
                e.HasIndex(r => r.ReceiptNo).IsUnique();
            });

            // Accounting & Purchasing Configurations
            builder.Entity<AstrolPOSAPI.Domain.Entities.Accounting.GLAccount>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.Property(p => p.Name).IsRequired().HasMaxLength(256);
                e.HasIndex(p => new { p.CompanyId, p.Code }).IsUnique();
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Accounting.VendorPostingGroup>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.HasIndex(p => new { p.CompanyId, p.Code }).IsUnique();
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Accounting.GenBusPostingGroup>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.HasIndex(p => new { p.CompanyId, p.Code }).IsUnique();
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Purchasing.Vendor>(e =>
            {
                e.Property(p => p.No).IsRequired().HasMaxLength(32);
                e.Property(p => p.Name).IsRequired().HasMaxLength(256);
                e.HasIndex(p => new { p.CompanyId, p.No }).IsUnique();
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Purchasing.PurchaseHeader>(e =>
            {
                e.Property(p => p.No).IsRequired().HasMaxLength(32);
                e.HasIndex(p => new { p.CompanyId, p.No }).IsUnique();
                e.HasMany(h => h.Lines).WithOne(l => l.PurchaseHeader).HasForeignKey(l => l.PurchaseHeaderId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Purchasing.PurchaseLine>(e =>
            {
                e.Property(p => p.Quantity).HasPrecision(18, 4);
                e.Property(p => p.DirectUnitCost).HasPrecision(18, 4);
                e.Property(p => p.LineAmount).HasPrecision(18, 4);
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Purchasing.PurchInvHeader>(e =>
            {
                e.Property(p => p.No).IsRequired().HasMaxLength(32);
                e.HasIndex(p => new { p.CompanyId, p.No }).IsUnique();
                e.HasMany(h => h.Lines).WithOne(l => l.PurchInvHeader).HasForeignKey(l => l.PurchInvHeaderId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Purchasing.PurchInvLine>(e =>
            {
                e.Property(p => p.Quantity).HasPrecision(18, 4);
                e.Property(p => p.DirectUnitCost).HasPrecision(18, 4);
                e.Property(p => p.LineAmount).HasPrecision(18, 4);
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Accounting.GLEntry>(e =>
            {
                e.Property(p => p.Amount).HasPrecision(18, 4);
                e.Property(p => p.DebitAmount).HasPrecision(18, 4);
                e.Property(p => p.CreditAmount).HasPrecision(18, 4);
                e.HasIndex(p => new { p.CompanyId, p.GLAccountNo, p.PostingDate });
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Accounting.VendorLedgerEntry>(e =>
            {
                e.Property(p => p.Amount).HasPrecision(18, 4);
                e.Property(p => p.RemainingAmount).HasPrecision(18, 4);
                e.HasIndex(p => new { p.CompanyId, p.VendorNo, p.Open });
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Accounting.Customer>(e =>
            {
                e.Property(p => p.No).IsRequired().HasMaxLength(32);
                e.Property(p => p.Name).IsRequired().HasMaxLength(256);
                e.HasIndex(p => new { p.CompanyId, p.No }).IsUnique();
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Accounting.CustomerPostingGroup>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.HasIndex(p => new { p.CompanyId, p.Code }).IsUnique();
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Accounting.GenProdPostingGroup>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.HasIndex(p => new { p.CompanyId, p.Code }).IsUnique();
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Accounting.GeneralPostingSetup>(e =>
            {
                e.Property(p => p.GenBusPostingGroupCode).IsRequired().HasMaxLength(32);
                e.Property(p => p.GenProdPostingGroupCode).IsRequired().HasMaxLength(32);
                e.HasIndex(p => new { p.CompanyId, p.GenBusPostingGroupCode, p.GenProdPostingGroupCode }).IsUnique();
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Accounting.BankAccount>(e =>
            {
                e.Property(p => p.Code).IsRequired().HasMaxLength(32);
                e.Property(p => p.Name).IsRequired().HasMaxLength(256);
                e.Property(p => p.Balance).HasPrecision(18, 4);
                e.HasIndex(p => new { p.CompanyId, p.Code }).IsUnique();
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Accounting.BankLedgerEntry>(e =>
            {
                e.Property(p => p.Amount).HasPrecision(18, 4);
                e.HasIndex(p => new { p.CompanyId, p.BankAccountCode, p.PostingDate });
            });

            builder.Entity<AstrolPOSAPI.Domain.Entities.Accounting.CustomerLedgerEntry>(e =>
            {
                e.Property(p => p.Amount).HasPrecision(18, 4);
                e.Property(p => p.RemainingAmount).HasPrecision(18, 4);
                e.HasIndex(p => new { p.CompanyId, p.CustomerNo, p.Open });
            });
        }




    }
}
