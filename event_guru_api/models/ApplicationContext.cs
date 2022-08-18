using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace event_guru_api.models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<BudgetVendor> BudgetVendors { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<EventAttendance> EventAttendances { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Contribution> Contributions { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelling many to many relationship for Budget and vendor
            modelBuilder.Entity<BudgetVendor>()
                .HasKey(bt => new { bt.BudgetID, bt.VendorID });

            modelBuilder.Entity<BudgetVendor>()
                .HasOne(bt => bt.Vendor)
                .WithMany(bt => bt.BudgetVendors)
                .HasForeignKey(bt => bt.VendorID);

            modelBuilder.Entity<BudgetVendor>()
                .HasOne(bt => bt.Budget)
                .WithMany(e => e.BudgetVendors)
                .HasForeignKey(ev => ev.BudgetID);
            //Modelling the relationship between Budget and Event
            modelBuilder.Entity<Budget>()
                .HasOne(b => b.Event)
                .WithMany(e => e.Budgets)
                .HasForeignKey(b => b.EventID);

            //Modelling many to many relationship between event and attendance
            modelBuilder.Entity<EventAttendance>()
               .HasKey(ea => new { ea.EventID, ea.AttendeeID });

            modelBuilder.Entity<EventAttendance>()
                .HasOne(ea => ea.Attendee)
                .WithMany(a => a.EventAttendances)
                .HasForeignKey(ea => ea.AttendeeID);

            modelBuilder.Entity<EventAttendance>()
               .HasOne(ea => ea.Event)
               .WithMany(e => e.EventAttendances)
               .HasForeignKey(ea => ea.EventID);

            //modelling the one to many relationship between event and user
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organizer)
                .WithMany(o => o.Events)
                .HasForeignKey(e => e.OrganizerID);

            //modelling the event & user contribution relationships
            modelBuilder.Entity<Contribution>()
                .HasOne(c => c.Event)
                .WithMany(e => e.Contributions)
                .HasForeignKey(c => c.EventID);

            modelBuilder.Entity<Contribution>()
                .HasOne(c => c.Attendee)
                .WithMany(a => a.Contributions)
                .HasForeignKey(c => c.AttendeeID);

            //modelling the event and invitation relationships
            modelBuilder.Entity<Invitation>()
               .HasOne(c => c.Event)
               .WithMany(e => e.Invitations)
               .HasForeignKey(c => c.EventID);

            modelBuilder.Entity<Invitation>()
                .HasOne(c => c.Attendee)
                .WithMany(a => a.Invitations)
                .HasForeignKey(c => c.AttendeeID);

            //fixing the length of the identity databases
            // Shorten key length for Identity 
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(m => m.Email).HasMaxLength(127);
                entity.Property(m => m.NormalizedEmail).HasMaxLength(127);
                entity.Property(m => m.NormalizedUserName).HasMaxLength(127);
                entity.Property(m => m.UserName).HasMaxLength(127);
            });
            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.Property(m => m.Name).HasMaxLength(127);
                entity.Property(m => m.NormalizedName).HasMaxLength(127);
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.Property(m => m.LoginProvider).HasMaxLength(127);
                entity.Property(m => m.ProviderKey).HasMaxLength(127);
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.RoleId).HasMaxLength(127);
            });
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.LoginProvider).HasMaxLength(127);
                entity.Property(m => m.Name).HasMaxLength(127);

            });
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added) || (e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                };

            };
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
               .Entries()
               .Where(e => e.Entity is BaseEntity && ((e.State == EntityState.Added) || (e.State == EntityState.Modified)));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                };

            };
            return await base.SaveChangesAsync(true, cancellationToken).ConfigureAwait(false);
        }
    }
}

