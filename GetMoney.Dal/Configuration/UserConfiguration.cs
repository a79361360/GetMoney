using GetMoney.Data.User;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace GetMoney.Dal.Configuration
{
    class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            this.HasKey(key => key.Userid);
            this.Property(p => p.UserName)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(p => p.Identity)
                .IsRequired()
                .HasMaxLength(18);
            this.Property(p => p.Phone)
                .HasMaxLength(16);
        }
    }
}
