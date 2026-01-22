using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.DAL.Entities;

namespace UrlShortener.DAL.Configuration;

public class AboutContentConfiguration : IEntityTypeConfiguration<AboutContent>
{
    public void Configure(EntityTypeBuilder<AboutContent> builder)
    {
        builder.ToTable("AboutContent", "App");

        builder.HasKey(a => a.Id);
    }
}