using System;
using Microsoft.EntityFrameworkCore;
using ProjectPortfolio2.DatabaseModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ProjectPortfolio2
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Comment> Comments {get; set; }
        public DbSet<CommentMarked> CommentsMarked {get; set; }
        public DbSet<Post> Posts {get; set; }
        public DbSet<PostLink> PostLinks { get; set; }
        public DbSet<PostMarked> PostsMarked { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("host=localhost;db=stackoverflow;uid=filipgermanek;pwd=GRuby123");
            // you only need this if you want to see the SQL statments created by EF
            optionsBuilder.UseLoggerFactory(MyLoggerFactory)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new OwnerConfiguration());
        }

        // you only need this if you want to see the SQL statments created
        // by EF. See https://docs.microsoft.com/en-us/ef/core/miscellaneous/logging
        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[]
            {
                new ConsoleLoggerProvider((category, level)
                    => category == DbLoggerCategory.Database.Command.Name
                       && level == LogLevel.Information, true)
            });
    }

    //CATEGORY CONFIG
    class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.ToTable("owner");
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.DisplayName).HasColumnName("displayname");
            builder.Property(x => x.Location).HasColumnName("location");
            builder.Property(x => x.CreationDate).HasColumnName("creation_date");
            builder.Property(x => x.Age).HasColumnName("age");
        }
    }
}
