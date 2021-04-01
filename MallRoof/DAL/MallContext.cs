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
        public DbSet<MallPhoto> MallPhotos { get; set; }
        public DbSet<Premise> Premises { get; set; }
        public DbSet<PremiseCalendar> PremiseCalendars { get; set; }
        public System.Data.Entity.DbSet<MallRoof.Models.User> IdentityUsers { get; set; }
        public DbSet<City> Cities { get; set; }
        public System.Data.Entity.DbSet<MallRoof.Models.Demand> Demands { get; set; }
        public System.Data.Entity.DbSet<MallRoof.Models.Proposal> Proposals { get; set; }        
        public System.Data.Entity.DbSet<MallRoof.Models.PremiseType> PremiseTypes { get; set; }
        public System.Data.Entity.DbSet<MallRoof.Models.PriceProposalToPremise> PriceProposalToPremises { get; set; }

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

            modelBuilder.Entity<Mall>()
                //.HasOptional<City>(s => s.City)
                .HasRequired<City>(s => s.City)
                .WithMany(g => g.Malls)
                .HasForeignKey(s => s.CityId);
                       
            modelBuilder.Entity<Demand>()                
                .HasRequired<User>(u => u.TenantUser)
                .WithMany(u=>u.Demands)
                .HasForeignKey(d=>d.UserId)
                ;

            modelBuilder.Entity<Demand>()
                //.HasOptional<City>(s => s.City)
                .HasRequired<City>(s => s.City)
                .WithMany(g => g.Demands)
                .HasForeignKey(s => s.CityId)
                .WillCascadeOnDelete(false)
                ;

            modelBuilder.Entity<Proposal>()
                .HasRequired<Demand>(p => p.Demand)
                .WithMany(p => p.Proposals)
                .HasForeignKey(p => p.DemandId)
                ;

            modelBuilder.Entity<Proposal>()
                .HasOptional<User>(u => u.LandLordUser)
                //.HasRequired<User>(u => u.LandLordUser)
                .WithMany(u => u.Proposals)                
                .WillCascadeOnDelete(false)
                ;

            modelBuilder.Entity<Proposal>()
                //.HasOptional<Premise>(u => u.Premise)
                .HasRequired<Premise>(u => u.Premise)
                .WithMany(u => u.Proposals)
                //.HasForeignKey(p => p.PremiseId)
                .WillCascadeOnDelete(false)
                ;

            modelBuilder.Entity<PriceProposalToPremise>()
                //.HasOptional<Premise>(u => u.Premise)
                .HasRequired<Premise>(u => u.Premise)
                .WithMany(u => u.PriceProposalToPremises)
                .HasForeignKey(p => p.PremiseId)
                .WillCascadeOnDelete(false)
                ;
        }        
    }
}
