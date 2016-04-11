using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildnReleaseWP.Model
{
    class ReleaseArtifactVersion
    {
        public string ArtifactSourceId { get; internal set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string SourceBranch { get; set; }
    }
}
