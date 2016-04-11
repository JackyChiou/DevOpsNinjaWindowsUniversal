using System;
using System.Collections.Generic;
using System.Text;

namespace BuildnReleaseWP.Model
{
    class QueuedBuild
    {
        public string Id { get; set; }
        public string BuildNumber { get; set; }
        public string DefinitionName { get; set; }
        public DateTime QueuedDate { get; set; }
        public string QueuedDateFormatted { get; set; }
        public string Controller { get; set; }
        public string RequestedForName { get; set; }
        public string SourceBranch { get; set; }
        public string Status { get; set; }
        public string RequestedForUName { get; set; }
        public string Priotiy { get; set; }
        public string Url { get; set; }
    }
}
