﻿using Infrastructure.Models.BugEntity;
using Infrastructure.Models.CommentEntity;
using Infrastructure.Models.ReplyEntity;
using Infrastructure.Models.UserEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class TrackerDbContext : IdentityDbContext<BugUser>
    {
        public TrackerDbContext(DbContextOptions<TrackerDbContext> options) : base(options) 
        {
        }

        public DbSet<Bug> Bugs { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Reply> Replies { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BugConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new ReplyConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
