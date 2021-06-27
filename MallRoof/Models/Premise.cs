using MallRoof.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public class Premise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PremiseId { get; set; }
        [Display(Name = "Номер помещения")]
        public string Number { get; set; }
        [Display(Name = "Этаж")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Можно вводить только цифры")]
        public int Floor { get; set; }
        public Guid MallId { get; set; }
        [Display(Name = "Торговый центр")]
        public virtual Mall Mall { get; set; }

        public virtual ICollection<PriceProposalToPremise> PriceProposalToPremises { get; set; }

        [RegularExpression("([1-9][0-9]*.?[0-9]*)", ErrorMessage = "Можно вводить только целое или дробное число")]
        [Display(Name = "Площадь")]
        public double Area { get; set; }
        [Display(Name = "Последний этаж?")]
        public bool IsLastFloor { get; set; }
        public string IsLastFloorString
        {
            get { return IsLastFloor ? "Да" : "Нет"; }
        }
        [Display(Name = "Есть ли окно?")]
        public bool HasWindow { get; set; }
        public string HasWindowString
        {
            get { return HasWindow ? "Да" : "Нет"; }
        }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Цена")]
        public double Price { get; set; }
        //        public bool Free { get; set; }

        [Display(Name = "Фотография с инстаграмма")]
        [DataType(DataType.MultilineText)]
        [System.Web.Mvc.AllowHtml]
        public string InstaPhoto {
            get; set;
        }

        //public string GetInstaPhoto
        //{
        //    get {
        //        return GetInstaPhotoProcessed();
        //    }
        //}

        //public string GetInstaPhotoProcessed()
        //{
        //    string res = InstaPhoto.Replace("(@", "(@@");
        //    return res;
        //}

        public virtual ICollection<PremiseCalendar> PremiseCalendars { get; set; }
        [Display(Name = "Фотографии")]
        public virtual ICollection<Photo> Photos { get; set; }
        [Display(Name = "Предложения")]
        public virtual ICollection<Proposal> Proposals { get; set; }
        [Display(Name = "Видимость")]
        public bool IsSeen { get; set; }

        public bool IsFree()
        {
            var result = !PremiseCalendars.Where(c => c.DateFrom <= DateTime.Now && DateTime.Now <= c.DateTo).Any();
            return result;
        }
        [Display(Name = "Тип помещения")]
        public Guid? PremiseTypeId { get; set; }
        [Display(Name = "Тип помещения")]
        public virtual PremiseType PremiseType { get; set; }

        public string FirstPhoto()
        {
            if (Photos.Any())
            {
                return Photos.FirstOrDefault().Path;
            }
            return "";
        }
    }
}