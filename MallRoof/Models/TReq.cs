using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public class TReq
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TReqId { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Пользователь")]
        public virtual User User { get; set; }

        [Display(Name = "Город")]
        public Guid CityId { get; set; }
        [Display(Name = "Город")]
        public virtual City City { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#}")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Можно вводить только цифры")]
        [Display(Name = "Цена")]
        public double Price { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }


        public virtual ICollection<Proposal> Proposals { get; set; }
        [Display(Name = "Дата создания")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Дата окончания приема предложений")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[ValidateDateRange( FirstDate = DateTime.Now, SecondDate = DateTime.Now.AddDays(7))]
        [ValidateDateRange]
        public DateTime EndDate { get; set; }

        [Display(Name = "Статус")]
        public TReqStatus TReqStatus { get; set; }

    }
}