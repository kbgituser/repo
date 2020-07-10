using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MallRoof.Models
{
    public class User : ApplicationUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        [Display(Name ="Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string SurName { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Display(Name = "Подтверждена почта")]
        public override bool EmailConfirmed { get; set; }

        [Display(Name = "Центры")]
        public virtual ICollection<Mall> Malls { get; set; }


        [Display(Name = "Запросы")]
        public virtual ICollection<Demand> Demands { get; set; }

        [Display(Name = "Предложения")]
        public virtual ICollection<Proposal> Proposals { get; set; }
    }
}