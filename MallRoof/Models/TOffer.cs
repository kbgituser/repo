using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public class TOffer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TOfferId { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Пользователь")]

        public virtual User User { get; set; }

        public Guid TReqId { get; set; }
        public virtual TReq TRequest { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,#}")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Можно вводить только цифры")]

        [Display(Name = "Цена")]
        public double Price { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [Display(Name = "Дата создания")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreateDate { get; set; }

        [ValidateDateRange]
        public DateTime EndDate { get; set; }

        [Display(Name = "Статус")]
        public TOfferStatus TOfferStatus { get; set; }

    }
}