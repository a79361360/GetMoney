using GetMoney.Data.Order;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace GetMoney.Dal.Configuration
{
    class OrderConfiguration: EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            this.HasKey(key => key.id);
            this.Property(p => p.OrderNo)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(p => p.ExtraDate)
                .IsRequired()
                .HasMaxLength(1000);
            this.Property(p => p.Remark)
            .HasMaxLength(250);
        }
    }
}
