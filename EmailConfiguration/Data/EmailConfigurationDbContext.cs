using System;
using EmailConfiguration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EmailConfiguration.Data;

    public class EmailConfigurationDbContext : DbContext
    {
        public EmailConfigurationDbContext(DbContextOptions<EmailConfigurationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ClientEmailConfiguration> EmailConfigurations { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientEmailConfiguration>()
                .Property(e => e.SmtpServer)
                .IsRequired()
                .HasMaxLength(255); 

            modelBuilder.Entity<ClientEmailConfiguration>()
                .Property(e => e.SenderEmail)
                .IsRequired()
                .HasMaxLength(255); 

            modelBuilder.Entity<ClientEmailConfiguration>()
                .Property(e => e.SmtpPassword)
                .IsRequired()
                .HasMaxLength(255); 

            modelBuilder.Entity<ClientEmailConfiguration>()
                .Property(e => e.SmtpPort)
                .IsRequired();

            modelBuilder.Entity<ClientEmailConfiguration>()
                .Property(e => e.UseSsl)
                .IsRequired();

            modelBuilder.Entity<ClientEmailConfiguration>()
                .Property(e => e.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<ClientEmailConfiguration>()
                .Property(e => e.UpdatedAt)
                .IsRequired();

            modelBuilder.Entity<ClientEmailConfiguration>()
                .Property(e => e.ClientId)
                .IsRequired();

			modelBuilder.Entity<ClientEmailConfiguration>()
				.HasKey(e => e.Id);
        }
    }

