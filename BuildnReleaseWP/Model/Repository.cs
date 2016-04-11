using System;
using System.Collections.Generic;
using System.Text;

namespace BuildnReleaseWP.Model
{
    public class Repository
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string projectName { get; set; }
        public string projectId { get; set; }
        public string Url { get; set; }
        public string DefaultBranch { get; set; }
        public string projectUrl { get; set; }
        public string Type { get; set; }
    }
}
