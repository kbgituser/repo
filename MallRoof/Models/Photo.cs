using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public class Photo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PhotoId { get; set; }
        [Display(Name = "Путь")]
        public string Path { get; set; }
        public Guid PremiseId { get; set; }
        [Display(Name = "Помещение")]
        public Premise Premise { get; set; }
        [Display(Name = "Загружено")]
        public bool IsPhotoUploaded(string path) {            
            return File.Exists(path + @"\" + Path);
        }

    }
}