using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public class Mall
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MallId { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Наименование центра")]
        [Required(ErrorMessage = "Наименование обязательно")]
        public string Name { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Пользователь")]
        public virtual User User { get; set; }
        [Display(Name = "Помещения")]
        public virtual ICollection<Premise> Premises { get; set; }
        [Display(Name = "Количество этажей")]
        public int NumberOfFloors { get; set; }
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
    }
}