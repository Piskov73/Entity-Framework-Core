﻿namespace TeisterMask.Data
{
    using Microsoft.EntityFrameworkCore;

    using Models;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class TeisterMaskContext : DbContext
    {
        public TeisterMaskContext() 
        {
        }

        public TeisterMaskContext(DbContextOptions options)
            : base(options) 
        {
        }

        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Task> Tasks { get; set; } = null!;
        public DbSet<EmployeeTask> EmployeesTasks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeTask>()
                .HasKey(et => new { et.EmployeeId, et.TaskId });
        }
    }
}