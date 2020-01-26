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
        [Display(Name = "Адрес центра")]
        public string Address { get; set; }
        [Display(Name = "Пользователь")]
        public virtual User User { get; set; }
        [Display(Name = "Помещения")]
        public virtual ICollection<Premise> Premises { get; set; }
        [Display(Name = "Количество этажей")]
        public int NumberOfFloors { get; set; }
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Есть ли парковка")]
        public bool ParkingExists { get; set; }
        [Display(Name = "Есть ли паркинг")]
        public bool ParkingInsideExists { get; set; }
        [Display(Name = "Платная ли парковка")]
        public bool ParkingPayment { get; set; }
        [Display(Name = "Платный ли паркинг")]
        public bool ParkingInsidePayment { get; set; }

        [Display(Name = "Фотографии")]
        public virtual ICollection<MallPhoto> MallPhotos { get; set; }

        public Guid CityId { get; set; }
        [Display(Name = "Город")]
        public virtual City City { get; set; }
    }
}