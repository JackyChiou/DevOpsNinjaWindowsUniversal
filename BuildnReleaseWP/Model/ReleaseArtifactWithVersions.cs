using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildnReleaseWP.Model
{
    class ReleaseArtifactWithVersions
    {
        public ReleaseDefinition.RDArtifact Artifact { get; set; }
        public List<ReleaseArtifactVersion> ArtifactVersions { get; set; }

        public ReleaseArtifactVersion SelectedArtifactVersion { get; set; }
    }
}
