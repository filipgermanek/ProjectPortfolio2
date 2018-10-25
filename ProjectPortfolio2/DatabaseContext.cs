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
            optionsBuilder.UseNpgsql("host=rawdata.ruc.dk;db=raw8;uid=raw8;pwd=OriPUmyf");
            // you only need this if you want to see the SQL statments created by EF
            optionsBuilder.UseLoggerFactory(MyLoggerFactory)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new OwnerConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new CommentMarkedConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new SearchHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());


            // CommentMarked composite key.
            modelBuilder.Entity<CommentMarked>()
                        .HasKey(x => new { x.CommentId, x.UserId });
            // PostLink composite key.
            modelBuilder.Entity<PostLink>()
                        .HasKey(x => new { x.LinkId, x.PostId });
            // PostMarked composite key.
            modelBuilder.Entity<PostMarked>()
                        .HasKey(x => new { x.PostId, x.UserId });
            // PostTag composite key.
            modelBuilder.Entity<PostTag>()
                        .HasKey(x => new { x.PostId, x.TagId });

            //modelBuilder.Entity<Post>()
                        //.HasDiscriminator<int?>("parent_id")
                        //.HasValue<Question>(null)
                        //.HasValue<Answer>();

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

    //OWNER CONFIG
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

    //Comments Congif
    class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comment");
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Score).HasColumnName("score");
            builder.Property(x => x.Text).HasColumnName("text");
            builder.Property(x => x.CreationDate).HasColumnName("creation_date");
            builder.Property(x => x.PostId).HasColumnName("post_id");
            builder.Property(x => x.OwnerId).HasColumnName("owner_id");
        }
    }

    //Comments Marked Config
    class CommentMarkedConfiguration : IEntityTypeConfiguration<CommentMarked>
    {
        public void Configure(EntityTypeBuilder<CommentMarked> builder)
        {
            builder.ToTable("comment_marked");
            builder.Property(x => x.CommentId).HasColumnName("comment_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.AnnotationText).HasColumnName("annotation_text");
        }
    }

    class PostLinkConfiguration : IEntityTypeConfiguration<PostLink>
    {
        public void Configure(EntityTypeBuilder<PostLink> builder)
        {
            builder.ToTable("post_link");
            builder.Property(x => x.PostId).HasColumnName("post_id");
            builder.Property(x => x.LinkId).HasColumnName("link_id");
        }
    }

    class PostMarkedConfiguration : IEntityTypeConfiguration<PostMarked>
    {
        public void Configure(EntityTypeBuilder<PostMarked> builder)
        {
            builder.ToTable("post_marked");
            builder.Property(x => x.PostId).HasColumnName("post_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.AnnotationText).HasColumnName("annotation_text");
        }
    }


    //SEARCH HISTORY CONFIG
    class SearchHistoryConfiguration : IEntityTypeConfiguration<SearchHistory>
    {
        public void Configure(EntityTypeBuilder<SearchHistory> builder)
        {
            builder.ToTable("search_history");
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.Searchtext).HasColumnName("search_text");
            builder.Property(x => x.CreationDate).HasColumnName("creation_date");
        }
    }

    //TAG CONFIG
    class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("tag");
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name");
        }
    }

    //USER CONFIGURATION
    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Email).HasColumnName("email");
            builder.Property(x => x.Password).HasColumnName("password");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.Location).HasColumnName("location");
            builder.Property(x => x.CreationDate).HasColumnName("creation_date");
        }
    }
}
