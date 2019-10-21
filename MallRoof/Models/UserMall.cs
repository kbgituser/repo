using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public class UserMall
    {
        public Guid UserMallId { get; set; }
        public Guid UserId { get; set; }
        public Guid MallId { get; set; }
        public virtual Mall Mall { get; set; }
        public virtual User User { get; set; }
    }
}