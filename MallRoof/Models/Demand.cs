using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public class ValidateDateRange : ValidationAttribute
    {
        //public DateTime FirstDate { get; set; }
        //public DateTime SecondDate { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // your validation logic
            int demandDuration = Int32.Parse(ConfigurationManager.AppSettings["maxDemandDuration"]);
            if ((DateTime) value >= DateTime.Today && (DateTime)value <= DateTime.Today.AddDays(demandDuration))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Дата окончания приема предложений должно быть между " + DateTime.Today.ToString("dd.MM.yyyy") + " и " + DateTime.Now.AddDays(demandDuration).ToString("dd.MM.yyyy"));
            }
        }
    }

    public class Demand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DemandId { get; set; }
        [Display(Name = "Пользователь")]
        public string UserId { get; set; }
        [Display(Name = "Пользователь")]
        public virtual User TenantUser { get; set; }

        [Display(Name = "Город")]
        public Guid CityId { get; set; }
        [Display(Name = "Город")]
        public virtual City City { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#}")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Можно вводить только цифры")]
        [Display(Name = "Цена от")]
        public double PriceFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#}")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Можно вводить только цифры")]
        [Display(Name = "Цена до")]
        public double PriceTo { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#}")]
        [RegularExpression("([1-9][0-9]*.?[0-9]*)", ErrorMessage = "Можно вводить только целое или дробное число")]
        [Display(Name = "Площадь от")]        
        public double AreaFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#}")]
        [RegularExpression("([1-9][0-9]*.?[0-9]*)", ErrorMessage = "Можно вводить только целое или дробное число")]
        [Display(Name = "Площадь до")]
        public double AreaTo { get; set; }
                
        [Display(Name = "Есть окно")]
        public bool HasWindow { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [Required(ErrorMessage = "Укажите пожалуйста номер телефона")]
        [Display(Name = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([+7]{2})\)?([(]{1})\)?([0-9]{3})\)?([)]{1})\)?([0-9]{3})[-. ]?([0-9]{2})[-. ]?([0-9]{2})$", ErrorMessage = "Not a valid phone number")]
        //[RegularExpression(@"^[+7]*[(]{1}[0-9]{3}[)]{1}[-\s\./0-9]*$", ErrorMessage = "Номер не соответсвует формату")]
        
        public string Phone { get; set; }
        [Display(Name = "Район, Адрес")]
        public string PossibleAddress{ get; set; }

        public virtual ICollection<Proposal> Proposals { get; set; }
        [Display(Name = "Дата создания")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreateDate { get; set; }

        [Display(Name =  "Дата окончания приема предложений")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[ValidateDateRange( FirstDate = DateTime.Now, SecondDate = DateTime.Now.AddDays(7))]
        [ValidateDateRange]
        public DateTime EndDate { get; set; }
        [Display(Name = "Статус")]
        public DemandStatus DemandStatus { get;set;}


        [Display(Name = "Тип помещения")]
        public Guid? PremiseTypeId { get; set; }
        [Display(Name = "Тип помещения")]
        public virtual PremiseType PremiseType { get; set; }
    }
}