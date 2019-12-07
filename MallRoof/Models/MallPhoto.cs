using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public class MallPhoto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MallPhotoId { get; set; }
        [Display(Name = "Путь")]
        public string Path { get; set; }
        public Guid MallId { get; set; }
        [Display(Name = "Центр")]
        public Mall Mall { get; set; }
        [Display(Name = "Загружено")]
        public bool IsPhotoUploaded(string path)
        {
            return File.Exists(path + @"\" + Path);
        }

    }
}