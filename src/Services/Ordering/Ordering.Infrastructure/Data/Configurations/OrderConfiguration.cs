using Mapster.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> orderBuilder)
        {
            orderBuilder.HasKey(o => o.Id);

            orderBuilder.Property(o => o.Id).HasConversion(
                            orderId => orderId.Value,
                            dbId => OrderId.Of(dbId));

            // Configure the CustomerId property with a value converter

            // One Order has one Customer, and each Customer can have many Orders
            orderBuilder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .IsRequired();


            // One Order has many OrderItems, and each OrderItem belongs to one Order
            orderBuilder.HasMany(o => o.OrderItems)
                .WithOne()
                .HasForeignKey(oi => oi.OrderId);


            orderBuilder.ComplexProperty(
                o => o.OrderName, 
                nameBuilder =>
                {
                    nameBuilder.Property(n => n.Value)
                        .HasColumnName(nameof(Order.OrderName))
                        .HasMaxLength(100)
                        .IsRequired();
                });

            orderBuilder.ComplexProperty(
               o => o.ShippingAddress, 
               addressBuilder =>
               {
                   addressBuilder.Property(a => a.FirstName)
                       .HasMaxLength(50)
                       .IsRequired();

                   addressBuilder.Property(a => a.LastName)
                        .HasMaxLength(50)
                        .IsRequired();

                   addressBuilder.Property(a => a.EmailAddress)
                       .HasMaxLength(50);

                   addressBuilder.Property(a => a.AddressLine)
                       .HasMaxLength(180)
                       .IsRequired();

                   addressBuilder.Property(a => a.Country)
                       .HasMaxLength(50);

                   addressBuilder.Property(a => a.State)
                       .HasMaxLength(50);

                   addressBuilder.Property(a => a.ZipCode)
                       .HasMaxLength(5)
                       .IsRequired();
               });


            orderBuilder.ComplexProperty(
              o => o.BillingAddress, 
              addressBuilder =>
              {
                  addressBuilder.Property(a => a.FirstName)
                       .HasMaxLength(50)
                       .IsRequired();

                  addressBuilder.Property(a => a.LastName)
                       .HasMaxLength(50)
                       .IsRequired();

                  addressBuilder.Property(a => a.EmailAddress)
                      .HasMaxLength(50);

                  addressBuilder.Property(a => a.AddressLine)
                      .HasMaxLength(180)
                      .IsRequired();

                  addressBuilder.Property(a => a.Country)
                      .HasMaxLength(50);

                  addressBuilder.Property(a => a.State)
                      .HasMaxLength(50);

                  addressBuilder.Property(a => a.ZipCode)
                      .HasMaxLength(5)
                      .IsRequired();
              });

            orderBuilder.ComplexProperty(
               o => o.Payment, paymentBuilder =>
               {
                   paymentBuilder.Property(p => p.CardName)
                       .HasMaxLength(50);

                   paymentBuilder.Property(p => p.CardNumber)
                       .HasMaxLength(24)
                       .IsRequired();

                   paymentBuilder.Property(p => p.Expiration)
                       .HasMaxLength(10);

                   paymentBuilder.Property(p => p.CVV)
                       .HasMaxLength(3);

                   paymentBuilder.Property(p => p.PaymentMethod);
               });


            orderBuilder.Property(o => o.Status)
                .HasDefaultValue(OrderStatus.Draft)
                .HasSentinel(OrderStatus.Unspecified)
                .HasConversion(
                    s => s.ToString(),
                    dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

            orderBuilder.Property(o => o.TotalPrice).HasPrecision(18, 2);
        }

    }
}