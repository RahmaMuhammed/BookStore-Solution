using BookStore.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Book> builder)
        {
            builder.HasMany(B => B.genres)
                .WithMany(G => G.Books);

            builder.HasMany(B => B.Formats)
               .WithMany(G => G.Books);

            builder.HasOne(B => B.Author)
                .WithMany()
                .HasForeignKey(B => B.AuthorId);

            builder.Property(B => B.Title).HasMaxLength(100);
        }
    }
}
