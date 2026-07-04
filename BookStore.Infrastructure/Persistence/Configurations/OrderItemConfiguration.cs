using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Persistence.Configurations;

/// <summary>
/// OrderItem cədvəlinin Fluent API konfiqurasiyası — Order ↔ Book N-N körpüsü.
/// </summary>
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.UnitPrice)
            .HasPrecision(18, 2);

        // Order 1-N OrderItem (sifariş silinsə sətirləri də silinir)
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Book 1-N OrderItem (sifarişdə istifadə olunan kitab silinə bilməz)
        builder.HasOne(oi => oi.Book)
            .WithMany(b => b.OrderItems)
            .HasForeignKey(oi => oi.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        // Eyni sifarişdə eyni kitab yalnız bir sətir ola bilər
        builder.HasIndex(oi => new { oi.OrderId, oi.BookId })
            .IsUnique();
    }
}
