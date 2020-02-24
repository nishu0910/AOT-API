using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApi.Models.DBModels
{
    public partial class assignmentContext : DbContext
    {
        public assignmentContext()
        {
        }

        public assignmentContext(DbContextOptions<assignmentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Groupmembers> Groupmembers { get; set; }
        public virtual DbSet<Usergroup> Usergroup { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("Server=localhost;port=3306;User=root;Password=Nishu@123;Database=assignment");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Groupmembers>(entity =>
            {
                entity.ToTable("groupmembers");

                entity.HasIndex(e => e.GroupId)
                    .HasName("Group_FK_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("User_FK_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.GroupId).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Groupmembers)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("Group_GroupMembers_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Groupmembers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("User_GroupMembers_FK");
            });

            modelBuilder.Entity<Usergroup>(entity =>
            {
                entity.ToTable("usergroup");

                entity.HasIndex(e => e.AdminId)
                    .HasName("Id_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AdminId).HasColumnType("int(11)");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasColumnType("varchar(45)");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Usergroup)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_FK");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasColumnName("emailId")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Token).HasColumnType("varchar(45)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnType("varchar(45)");
            });
        }
    }
}
