using EfApi1.Models.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfApi1.Models.EntityMapping;

public class MovieMapping : IEntityTypeConfiguration<Movie> {
  public void Configure(EntityTypeBuilder<Movie> builder) {
    builder.ToTable("Films");
    builder.HasKey(m => m.Identifier);
    builder.HasQueryFilter(m => m.ReleaseDate >= new DateTime(2000, 1, 1));

    builder.Property(m => m.Title)
      .HasColumnType("VARCHAR")
      .HasMaxLength(256)
      .IsRequired();

    builder.Property(m => m.ReleaseDate)
      .HasColumnType("CHAR(8)")
      .HasConversion(new DateTimeToChar8Converter());

    builder.Property(m => m.Synopsis)
      .HasColumnType("VARCHAR(MAX)")
      .HasColumnName("Plot");

    builder.Property(m => m.AgeRating)
      .HasColumnType("VARCHAR(32)")
      .HasConversion<string>();

    builder.OwnsOne(m => m.Director)
      .ToTable("Movie_Directors");

    builder.OwnsMany(m => m.Actors)
      .ToTable("Movie_Actors");

    builder
      .HasOne(movie => movie.Genre)
      .WithMany(genre => genre.Movies)
      .HasPrincipalKey(genre => genre.Id)
      .HasForeignKey(movie => movie.MainGenreId);

    builder.HasData(new Movie {
      Identifier = 1,
      Title = "Fight Club",
      ReleaseDate = new DateTime(1999, 9, 10),
      Synopsis = "Ed Norton and Brad Pitt have a couple of fist fights with each other.",
      MainGenreId = 1,
      AgeRating = AgeRating.Adolescent,
    });

    builder.OwnsOne(m => m.Director)
      .HasData(new { MovieIdentifier = 1, FirstName = "David", LastName = "Fincher" });

    builder.OwnsMany(m => m.Actors)
      .HasData(
        new { MovieIdentifier = 1, Id = 1, FirstName = "Edward", LastName = "Norton" },
        new { MovieIdentifier = 1, Id = 2, FirstName = "Brad", LastName = "Pitt" }
      );
  }
}
