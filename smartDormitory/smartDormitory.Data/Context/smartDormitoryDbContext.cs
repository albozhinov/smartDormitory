using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using smartDormitory.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartDormitory.Data.Context
{
    public class smartDormitoryDbContext : IdentityDbContext<User>
    {
        public smartDormitoryDbContext()
        {

        }

        public smartDormitoryDbContext(DbContextOptions<smartDormitoryDbContext> options)
            : base(options)
        {

        }

        public DbSet<Sensor> Sensors { get; set; }

        public DbSet<UserSensors> UserSensors { get; set; }

        public DbSet<MeasureType> MeasureTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
