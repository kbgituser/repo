using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MallRoof.Models
{
    public class ProposalCreateModel
    {
        public string DemandId { get; set; }
        public IEnumerable<Premise> Premises { get; set; }
        public bool CanCreate { get; set; }
        public Proposal Proposal { get; set; }
    }
}