using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildnReleaseWP.Model
{
    class Approval
    {
        public string Id { get; set; }
        public List<Approval> History;
        public Identity Approver { get; set; }
        public string ApprovalType { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public bool IsAutomated { get; set; }
        public int Rank { get; set; }
        public Release Release { get; set; }
        public ReleaseDefinition  ReleaseDefinition { get; set; }
        public Release.REnvironment ReleaseEnvironment { get; set; }
    }
}
