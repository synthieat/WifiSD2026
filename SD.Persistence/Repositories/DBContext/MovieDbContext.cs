using Microsoft.EntityFrameworkCore;
using SD.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Persistence.Repositories.DBContext
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext() { }

        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
            Database.SetCommandTimeout(90);
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<MediumType> MediumTypes { get; set; }

        /* Fluent API-Konfigurationen für die Entitäten */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable(nameof(Movie) + "s");
                /* Redundant, da EF Core automatisch die Id-Eigenschaft als 
                   Primärschlüssel erkennt 

                   entity.HasKey(e => e.Id);
                */

                entity.Property(p => p.Title).HasMaxLength(128).IsRequired();
                entity.Property(p => p.ReleaseDate).HasColumnType("date");
                entity.Property(p => p.Price).HasPrecision(18, 2).HasDefaultValue(0M);

                /* Indizes für häufig abgefragte Spalten und Fremdschlüssel */
                entity.HasIndex(i => i.Title).HasDatabaseName("IX_" + nameof(Movie) + "s_" + nameof(Movie.Title));
                entity.HasIndex(i => i.GenreId).HasDatabaseName("IX_" + nameof(Movie) + "s_" + nameof(Movie.GenreId));
                entity.HasIndex(i => i.MediumTypeCode).HasDatabaseName("IX_" + nameof(Movie) + "s_" + nameof(Movie.MediumTypeCode));


                /* Beziehungen und Navigationseigenschaften */
                entity.HasOne(m => m.MediumType)
                    .WithMany(mt => mt.Movies)
                    .HasForeignKey(m => m.MediumTypeCode)
                    .OnDelete(DeleteBehavior.SetNull); /* Beim Löschen eines MediumType werden die zugehörigen Filme nicht gelöscht,
                                                          sondern die MediumTypeCode-Fremdschlüssel auf NULL gesetzt. */

                entity.HasOne(m => m.Genre)
                      .WithMany(mt => mt.Movies)
                      .HasForeignKey(m => m.GenreId)
                      .OnDelete(DeleteBehavior.Restrict); /* Genre kann nicht gelöscht werden, solange es Filme gibt,
                                                           * die diesem Genre zugeordnet sind => Referentielle Integrität. */
            });

            /* Alternative Möglichkeit, direkt über Entity
            modelBuilder.Entity<Movie>().HasOne(m => m.MediumType)
                                        .WithMany(mt => mt.Movies)
                                        .HasForeignKey(m => m.MediumTypeCode)
                                        .OnDelete(DeleteBehavior.SetNull);
            
            /* Alternative: Fremdschlüsselbeziehung zwischen Movie und Genre aus der Genre -Entität heraus konfigurieren */
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable(nameof(Genre) + "s");
                entity.Property(p => p.Name).HasMaxLength(64).IsRequired();

                /* Redundant, da EF Core automatisch die Id-Eigenschaft als Primärschlüssel erkennt 
                   entity.HasKey(e => e.Id) und weil bereits in Movie konfiguriert, aber hier zur Verdeutlichung der bidirektionalen Beziehung */

                entity.HasMany(mt => mt.Movies)
                      .WithOne(g => g.Genre)
                      .HasForeignKey(m => m.GenreId)
                      .OnDelete(DeleteBehavior.Restrict);

            });

            /* Seed Methode zum Vorbefüllen der Datenbank mit Testdaten bzw. Stammdaten */
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Comedy" },
                new Genre { Id = 3, Name = "Drama" },
                new Genre { Id = 4, Name = "Horror" },
                new Genre { Id = 5, Name = "Science Fiction" }
            );

            modelBuilder.Entity<MediumType>().HasData(
                new MediumType { Code = "CD", Name = "Compact Disc" },
                new MediumType { Code = "DVD", Name = "Digital Versatile Disc" },
                new MediumType { Code = "BD", Name = "Blu-ray Disc" },
                new MediumType { Code = "VHS", Name = "Video Home System" },
                new MediumType { Code = "STREAM", Name = "Streaming" }
            );

            modelBuilder.Entity<Movie>().HasData(
                new Movie { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Title = "Die Hard", GenreId = 1, MediumTypeCode = "DVD", Price = 9.99M, ReleaseDate = new DateTime(1988, 7, 15) },
                new Movie { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Title = "Mad Max: Fury Road", GenreId = 1, MediumTypeCode = "BD", Price = 14.99M, ReleaseDate = new DateTime(2015, 5, 15) },
                new Movie { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Title = "Groundhog Day", GenreId = 2, MediumTypeCode = "DVD", Price = 7.99M, ReleaseDate = new DateTime(1993, 2, 12) },
                new Movie { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Title = "The Grand Budapest Hotel", GenreId = 2, MediumTypeCode = "BD", Price = 12.99M, ReleaseDate = new DateTime(2014, 3, 28) },
                new Movie { Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), Title = "The Shawshank Redemption", GenreId = 3, MediumTypeCode = "DVD", Price = 8.99M, ReleaseDate = new DateTime(1994, 9, 23) },
                new Movie { Id = Guid.Parse("00000000-0000-0000-0000-000000000006"), Title = "Forrest Gump", GenreId = 3, MediumTypeCode = "BD", Price = 11.99M, ReleaseDate = new DateTime(1994, 7, 6) },
                new Movie { Id = Guid.Parse("00000000-0000-0000-0000-000000000007"), Title = "The Shining", GenreId = 4, MediumTypeCode = "DVD", Price = 9.99M, ReleaseDate = new DateTime(1980, 5, 23) },
                new Movie { Id = Guid.Parse("00000000-0000-0000-0000-000000000008"), Title = "A Quiet Place", GenreId = 4, MediumTypeCode = "STREAM", Price = 4.99M, ReleaseDate = new DateTime(2018, 4, 6) },
                new Movie { Id = Guid.Parse("00000000-0000-0000-0000-000000000009"), Title = "Blade Runner", GenreId = 5, MediumTypeCode = "BD", Price = 13.99M, ReleaseDate = new DateTime(1982, 6, 25) },
                new Movie { Id = Guid.Parse("00000000-0000-0000-0000-000000000010"), Title = "The Matrix", GenreId = 5, MediumTypeCode = "DVD", Price = 10.99M, ReleaseDate = new DateTime(1999, 3, 31) }
            );

        }
    }
}