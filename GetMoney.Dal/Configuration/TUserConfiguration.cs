using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using GetMoney.Data.TUser;

namespace GetMoney.Dal.Configuration
{
    public class TUserConfiguration : EntityTypeConfiguration<TUser>
    {
        public TUserConfiguration()
        {
            this.HasKey(key => key.Userid);
            this.Property(p => p.UserName)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(p => p.IdentityNum)
                .IsRequired()
                .HasMaxLength(18);
            this.Property(p => p.Phone)
                .HasMaxLength(16);
        }
    }
}
