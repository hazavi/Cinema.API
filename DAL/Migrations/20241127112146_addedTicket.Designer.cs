﻿// <auto-generated />
using System;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20241127112146_addedTicket")]
    partial class addedTicket
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DAL.Models.Domain.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressId"));

                    b.Property<int>("PostalCodeId")
                        .HasColumnType("int");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("StreetNumber")
                        .HasColumnType("int");

                    b.HasKey("AddressId");

                    b.HasIndex("PostalCodeId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("DAL.Models.Domain.Genre", b =>
                {
                    b.Property<int>("GenreID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GenreID"));

                    b.Property<string>("GenreName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GenreID");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("DAL.Models.Domain.Movie", b =>
                {
                    b.Property<int>("MovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MovieId"));

                    b.Property<int>("DurationMinutes")
                        .HasColumnType("int");

                    b.Property<decimal>("Rating")
                        .HasPrecision(3, 1)
                        .HasColumnType("decimal(3,1)");

                    b.Property<DateOnly>("ReleaseDate")
                        .HasColumnType("date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MovieId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("DAL.Models.Domain.PostalCode", b =>
                {
                    b.Property<int>("PostalCodeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PostalCodeId");

                    b.ToTable("PostalCodes");
                });

            modelBuilder.Entity("DAL.Models.Domain.Seat", b =>
                {
                    b.Property<int>("SeatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SeatId"));

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<int>("RowNumber")
                        .HasColumnType("int");

                    b.Property<int>("SeatNumber")
                        .HasColumnType("int");

                    b.Property<int>("TheaterId")
                        .HasColumnType("int");

                    b.HasKey("SeatId");

                    b.HasIndex("TheaterId");

                    b.ToTable("Seats");
                });

            modelBuilder.Entity("DAL.Models.Domain.Showtime", b =>
                {
                    b.Property<int>("ShowtimeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShowtimeId"));

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TheaterId")
                        .HasColumnType("int");

                    b.HasKey("ShowtimeId");

                    b.HasIndex("MovieId");

                    b.HasIndex("TheaterId");

                    b.ToTable("Showtimes");
                });

            modelBuilder.Entity("DAL.Models.Domain.Theater", b =>
                {
                    b.Property<int>("TheaterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TheaterId"));

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TheaterId");

                    b.HasIndex("AddressId");

                    b.ToTable("Theaters");
                });

            modelBuilder.Entity("DAL.Models.Domain.Ticket", b =>
                {
                    b.Property<int>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketId"));

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("SeatId")
                        .HasColumnType("int");

                    b.Property<int>("ShowtimeId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TicketId");

                    b.HasIndex("SeatId");

                    b.HasIndex("ShowtimeId");

                    b.HasIndex("UserId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("DAL.Models.Domain.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PostalCodeId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("PostalCodeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GenreMovie", b =>
                {
                    b.Property<int>("GenresGenreID")
                        .HasColumnType("int");

                    b.Property<int>("MoviesMovieId")
                        .HasColumnType("int");

                    b.HasKey("GenresGenreID", "MoviesMovieId");

                    b.HasIndex("MoviesMovieId");

                    b.ToTable("GenreMovie");
                });

            modelBuilder.Entity("DAL.Models.Domain.Address", b =>
                {
                    b.HasOne("DAL.Models.Domain.PostalCode", "PostalCode")
                        .WithMany()
                        .HasForeignKey("PostalCodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PostalCode");
                });

            modelBuilder.Entity("DAL.Models.Domain.Seat", b =>
                {
                    b.HasOne("DAL.Models.Domain.Theater", "Theater")
                        .WithMany()
                        .HasForeignKey("TheaterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Theater");
                });

            modelBuilder.Entity("DAL.Models.Domain.Showtime", b =>
                {
                    b.HasOne("DAL.Models.Domain.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.Domain.Theater", "Theater")
                        .WithMany()
                        .HasForeignKey("TheaterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("Theater");
                });

            modelBuilder.Entity("DAL.Models.Domain.Theater", b =>
                {
                    b.HasOne("DAL.Models.Domain.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("DAL.Models.Domain.Ticket", b =>
                {
                    b.HasOne("DAL.Models.Domain.Seat", "Seat")
                        .WithMany()
                        .HasForeignKey("SeatId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DAL.Models.Domain.Showtime", "Showtime")
                        .WithMany()
                        .HasForeignKey("ShowtimeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DAL.Models.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Seat");

                    b.Navigation("Showtime");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DAL.Models.Domain.User", b =>
                {
                    b.HasOne("DAL.Models.Domain.PostalCode", "PostalCode")
                        .WithMany()
                        .HasForeignKey("PostalCodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PostalCode");
                });

            modelBuilder.Entity("GenreMovie", b =>
                {
                    b.HasOne("DAL.Models.Domain.Genre", null)
                        .WithMany()
                        .HasForeignKey("GenresGenreID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.Domain.Movie", null)
                        .WithMany()
                        .HasForeignKey("MoviesMovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
