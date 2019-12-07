using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public class PremisesMallListModel
    {
        public List<Premise> Premises { get; set;}
        public List<Mall> Malls { get; set; }        
        public string MallId { get; set; }
        public string Price { get; set; }
        public string Area { get; set; }
        [Display(Name = "Есть окно")]
        public string Haswindow { get; set; }
        public string Priceorder { get; set; }
        public string Order { get; set; }
        public string GetMine { get; set; }
    }
}