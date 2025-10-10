﻿using GymManagementDAL.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class HealthRecordConfiguration :IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members")
           .HasKey(X => X.Id);
            builder.HasOne<Member>()
                .WithOne(M => M.HealthRecord)
                .HasForeignKey<HealthRecord>(X => X.Id);

            builder.Ignore(X => X.CreatedAt);
            builder.Ignore(X => X.UpdatedAt);

        }
    }
    
}
  