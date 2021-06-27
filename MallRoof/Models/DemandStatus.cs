using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public enum DemandStatus
    {
        [Description("Созданый")]
        Created = 0,
        [Description("Активный")]
        Active = 1,
        [Description("Отказаный")]
        Rejected = 2,
        [Description("Выбранный")]
        Selected = 3,
        [Description("Выполненный")]
        Done = 4
    }

    public enum OfferStatus
    {
        [Description("Созданый")]
        Created = 0,
        [Description("Активный")]
        Active = 1,
        [Description("Отказаный")]
        Rejected = 2,
        [Description("Выбранный")]
        Selected = 3,
        [Description("Выполненный")]
        Done = 4
    }

    public enum TReqStatus
    {
        [Description("Созданый")]
        Created = 0,
        [Description("Активный")]
        Active = 1,
        [Description("Отказаный")]
        Rejected = 2,
        [Description("Выбранный")]
        Selected = 3,
        [Description("Выполненный")]
        Done = 4
    }

    public enum TOfferStatus
    {
        [Description("Созданый")]
        Created = 0,
        [Description("Активный")]
        Active = 1,
        [Description("Отказаный")]
        Rejected = 2,
        [Description("Выбранный")]
        Selected = 3,
        [Description("Выполненный")]
        Done = 4
    }


}