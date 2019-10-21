using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public class PremiseCalendar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PremiseCalendarId { get; set; }
        [Display(Name = "С")]
        public DateTime DateFrom { get; set; }
        [Display(Name = "По")]
        public DateTime DateTo { get; set; }
        public Guid PremiseId { get; set; }
        [Display(Name = "Помещение")]
        public Premise Premise { get; set; }
        [Display(Name = "Съемщик")]
        public User Tenant { get; set; }
    }
}