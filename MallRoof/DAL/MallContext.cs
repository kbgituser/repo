using MallRoof.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MallRoof.DAL
{
    public class MallContext : IdentityDbContext<ApplicationUser>
    {
        public MallContext() : base("MallRoofConnection")
        {

        }

        public static MallContext Create()
        {
            return new MallContext();
        }

        public DbSet<Mall> Malls { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Premise> Premises { get; set; }
        public DbSet<PremiseCalendar> PremiseCalendars { get; set; }
        public System.Data.Entity.DbSet<MallRoof.Models.User> IdentityUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // configures one-to-many relationship
            modelBuilder.Entity<Premise>()
                .HasRequired<Mall>(s => s.Mall)
                .WithMany(g => g.Premises)
                .HasForeignKey(s => s.MallId);

            modelBuilder.Entity<Photo>()                
                .HasRequired<Premise>(s => s.Premise)
                .WithMany(g => g.Photos)                
                .HasForeignKey(s => s.PremiseId)                
                ;

            modelBuilder.Entity<Mall>()
                //.HasOptional<User>(s => s.User)
                .HasRequired<User>(s => s.User)
                .WithMany(g => g.Malls)
                .HasForeignKey(s => s.UserId);
        }
    }
}
