using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CityId { get; set; }
        [Display(Name = "Наименование города")]
        [Required(ErrorMessage = "Наименование города обязательно")]
        public string Name { get; set; }
        [Display(Name = "Центры")]
        public virtual ICollection<Mall> Malls { get; set; }
        [Display(Name = "Запросы")]
        public virtual ICollection<Demand> Demands { get; set; }
    }
}