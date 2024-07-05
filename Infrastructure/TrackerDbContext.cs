﻿using Infrastructure.Models.Bug;
using Infrastructure.Models.User;
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
        public DbSet<BugUser> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BugConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
