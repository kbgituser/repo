using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MallRoof.Models;

namespace MallRoof.Models
{
    public class Proposal
    {
        public Guid ProposalId {get; set;}
        public Guid DemandId {get;set;}
        public virtual Demand Demand { get; set; }
        [Required(ErrorMessage = "Выберите пожалуйста помещение")]
        public Guid PremiseId {get; set;}
        public virtual Premise Premise {get;set;}
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Можно вводить только цифры")]
        //[DisplayFormat(DataFormatString = "{0:C0}")]
        [DisplayFormat(DataFormatString = "{0:0,0}")]
        [Display(Name = "Цена")]
        public double Price { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        public string Description {get;set;}
        public string UserId { get; set; }
        public virtual User LandLordUser { get; set; }
        [Display(Name = "Дата создания")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreateDate { get; set; }
    }
}