using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public class PremiseType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PremiseTypeId { get; set; }
        [Display(Name = "Наименование типа помещения")]
        [Required(ErrorMessage = "Наименование типа обязательно")]
        public string Name { get; set; }        
    }
}