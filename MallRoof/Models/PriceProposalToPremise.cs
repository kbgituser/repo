using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public class PriceProposalToPremise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PriceProposalToPremiseId { get; set; }
        public string ProposingUserId { get; set; }
        [Display(Name = "Предлагающий пользователь")]
        public virtual User ProposingUser { get; set; }
        [Display(Name = "Помещение")]
        public Guid PremiseId { get; set; }
        [Display(Name = "Помещение")]
        public virtual Premise Premise { get; set; }
        [Display(Name = "Цена предложения")]
        public float Price { get; set; }
        [Display(Name = "Комментарии")]
        public string Comments { get; set; }
    }
}