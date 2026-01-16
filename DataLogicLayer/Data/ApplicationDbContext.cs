using Microsoft.EntityFrameworkCore;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogicLayer.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<Collaborator> Collaborators { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.LastName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_User_Email_Unique");



                entity.Property(e => e.Password)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.ChangedAt)
                      .IsRequired(false);
            });
            modelBuilder.Entity<Note>(entity =>
            {
                entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255);

                entity.Property(e => e.Description)
                .IsRequired();

                entity.Property(e => e.Colour)
                .HasMaxLength(7)
                .HasDefaultValue("#FFFFFF");

                entity.Property(e => e.Image);

                entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");


                //One to Many 
                entity.HasOne(e=>e.User)
                .WithMany(n=>n.Notes)
                .HasForeignKey(e=>e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

                //many to one
                entity.HasMany(e => e.Collaborators)
                .WithOne(n => n.Note)
                .HasForeignKey(e => e.CollaboratorId)
                .OnDelete(DeleteBehavior.Cascade);

                //Many to many
                entity.HasMany(e => e.Labels)
                .WithMany(n => n.Notes)
                .UsingEntity("NoteLabelEntity");

            });
            modelBuilder.Entity<Label>(entity =>
            {
                entity.Property(e=>e.LabelName)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e=>e.User)
                .WithMany(n=>n.Labels)
                .HasForeignKey(e=>e.UserId) 
                .OnDelete(DeleteBehavior.Cascade);

            });
            modelBuilder.Entity<Collaborator>(entity =>
            {
                entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

                entity.HasIndex(e => e.Email)
                .IsUnique()
                .HasDatabaseName("IX_User_Email_Unique");
            });
        }
    }
}
