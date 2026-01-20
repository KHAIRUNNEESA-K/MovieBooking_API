using Microsoft.EntityFrameworkCore;
using MovieBooking_API.Models;

namespace MovieBooking_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Show> Shows { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Show>(e =>
            {
                e.HasKey(s => s.ShowId);

                e.Property(s => s.ShowName)
                    .IsRequired()
                    .HasMaxLength(100);
            });


            modelBuilder.Entity<Seat>(e =>
            {
                e.HasKey(s => s.SeatId);

                e.Property(s => s.SeatNumber)
                    .IsRequired()
                    .HasMaxLength(10);

                e.Property(s => s.Status)
                    .IsRequired();

                e.Property(s => s.RowVersion)
                    .IsRowVersion();

                e.HasOne(s => s.Show)
                    .WithMany(sh => sh.Seats)
                    .HasForeignKey(s => s.ShowId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(s => s.Booking)
                    .WithMany(b => b.Seats)
                    .HasForeignKey(s => s.BookingId)
                    .OnDelete(DeleteBehavior.SetNull);

                e.HasIndex(s => new { s.ShowId, s.SeatNumber })
                    .IsUnique();
            });

            modelBuilder.Entity<Booking>(e =>
            {
                e.HasKey(b => b.BookingId);

                e.Property(b => b.IsConfirmed)
                    .IsRequired();

                e.Property(b => b.BookingTime)
                    .IsRequired();

                e.HasOne(b => b.Show)
                    .WithMany()
                    .HasForeignKey(b => b.ShowId)
                    .OnDelete(DeleteBehavior.Restrict); 
            });

        }
    }
}
