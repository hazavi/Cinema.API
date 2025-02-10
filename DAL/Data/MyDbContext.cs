using DAL.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace DAL.Data
{
    // DbContext klasse der håndterer forbindelsen mellem app og db
    // Bro mellem kode og databasen   
    public class MyDbContext : DbContext
    {
        private readonly DbContextOptions dbContextOptions; // En variabel, der holder de konfigurationer, der bruges af DbContext
        

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        // DbSets
        // Repræsenterer tabeller i databasen

        public DbSet<User> Users { get; set; }
        public DbSet<PostalCode> PostalCodes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }


        // OnConfiguring-metoden bruges til at konfigurere forbindelsen til databasen
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=tcp:saicoapidbserver.database.windows.net,1433;Initial Catalog=bioma;Persist Security Info=False;User ID=haz;Password=anH3272G4U4(wb\\*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
                //"Data Source=LAPTOP-DMFQLTGK;Initial Catalog=NewCinemaDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

        }
        // OnModelCreating-metoden bruges til at konfigurere databasens struktur og forhold mellem tabellerne
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // USER konfiguration 
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.LastName)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.Email)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.PasswordHash)
                      .IsRequired();

                entity.Property(e => e.PasswordSalt)
                      .IsRequired();

                entity.Property(e => e.CreateDate)
                      .HasDefaultValueSql("GETDATE()");

                // Relation mellem User og PostalCode (Many-to-One) 
                // En user har en postal code, men en postal code kan have mange users.
                entity.HasOne(e => e.PostalCode)
                      .WithMany()
                      .HasForeignKey(e => e.PostalCodeId);
            });


            // POSTALCODE   
            modelBuilder.Entity<PostalCode>()
                .Property(e => e.PostalCodeId)
                .ValueGeneratedNever();  // PostalCodeId skal ikke genereres automatisk af databasen
            modelBuilder.Entity<PostalCode>()
                .HasIndex(p => p.PostalCodeId)
                .IsUnique(); // PostalCodeId skal være unik

            // MOVIE
            modelBuilder.Entity<Movie>()
                .Property(e => e.ReleaseDate)
                .HasColumnType("date");

            // Movie og MovieGenres (One-to-Many) 
            // En film kan have mange genre (MovieGenres), men hver MovieGenre hører kun til én film.
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.MovieGenres)
                .WithOne(mg => mg.Movie)
                .HasForeignKey(mg => mg.MovieId);

            // Genre og MovieGenres (One-to-Many) 
            // En genre kan have mange MovieGenres, men hver MovieGenre hører kun til én genre.
            modelBuilder.Entity<Genre>()
                .HasMany(g => g.MovieGenres)
                .WithOne(mg => mg.Genre)
                .HasForeignKey(mg => mg.GenreId);

            // Mange-til-mange relation mellem Movie og Genre gennem MovieGenre
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId }); // Den sammensatte primærnøgle

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieId);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreId);

            // ADDRESS
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.StreetName)
                      .HasMaxLength(100)
                      .IsRequired();
                entity.Property(e => e.StreetNumber)
                      .IsRequired();
            });

            // THEATER
            modelBuilder.Entity<Theater>(entity =>
            {
                entity.Property(e => e.Name)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.Capacity)
                      .IsRequired();

                entity.Property(e => e.Location)
                      .HasMaxLength(200)
                      .IsRequired();
            });


            // SEAT
            modelBuilder.Entity<Seat>(entity =>
            {
                entity.Property(e => e.RowNumber)
                      .IsRequired();
                entity.Property(e => e.SeatNumber)
                      .IsRequired();
            });

            // SHOWTIME
            modelBuilder.Entity<Showtime>(entity =>
            {
                entity.Property(e => e.StartTime)
                      .IsRequired();
            });

            // TICKET
            modelBuilder.Entity<Ticket>(entity =>
            {
                // Ticket og Showtime, Seat og User (One-to-Many)
                // En ticket er knyttet til en Showtime, én Seat og én User.
                entity.HasOne(t => t.Showtime)
                      .WithMany()
                      .HasForeignKey(t => t.ShowtimeId)
                      .OnDelete(DeleteBehavior.NoAction); 

                entity.HasOne(t => t.Seat)
                      .WithMany()
                      .HasForeignKey(t => t.SeatId)
                      .OnDelete(DeleteBehavior.NoAction);  

                entity.HasOne(t => t.User)
                      .WithMany()
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
        }

    }
}
