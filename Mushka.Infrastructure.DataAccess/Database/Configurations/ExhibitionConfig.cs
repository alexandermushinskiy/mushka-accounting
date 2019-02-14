using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Database.Configurations
{
    internal class ExhibitionConfig : IEntityTypeConfiguration<Exhibition>
    {
        public void Configure(EntityTypeBuilder<Exhibition> builder)
        {
            builder.ToTable("Exhibitions");

            builder
                .Property(exh => exh.Id)
                .HasColumnName("Id");

            builder.HasKey(exh => exh.Id);

            builder
                .Property(exh => exh.Name)
                .HasColumnName("Name")
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(exh => exh.City)
                .HasColumnName("City")
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(exh => exh.FromDate)
                .HasColumnName("FromDate")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property(exh => exh.ToDate)
                .HasColumnName("ToDate")
                .HasColumnType("Date")
                .IsRequired();

            builder
                .Property(exh => exh.ParticipationCost)
                .HasColumnName("ParticipationCost")
                .HasColumnType("Money")
                .IsRequired();

            builder
                .Property(exh => exh.ParticipationCostMethod)
                .HasColumnName("ParticipationCostMethod")
                .IsRequired();

            builder
                .Property(exh => exh.AccommodationCost)
                .HasColumnName("AccommodationCost")
                .HasColumnType("Money");

            builder
                .Property(exh => exh.AccommodationCostMethod)
                .HasColumnName("AccommodationCostMethod");

            builder
                .Property(exh => exh.FareCost)
                .HasColumnName("FareCost")
                .HasColumnType("Money");

            builder
                .Property(exh => exh.FareCostMethod)
                .HasColumnName("FareCostMethod");

            builder
                .Property(exh => exh.Profit)
                .HasColumnName("Profit")
                .HasColumnType("Money")
                .IsRequired();

            builder
                .Property(exh => exh.Notes)
                .HasColumnName("Notes");
        }
    }
}