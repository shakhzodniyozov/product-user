﻿using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Email).IsRequired();

        builder.HasMany(x => x.Products)
                .WithOne(x => x.CreatedUser)
                .HasForeignKey(x => x.CreatedUserId);
        
    }
}
