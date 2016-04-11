using System;
using System.Collections.Generic;
using System.Text;

namespace BuildnReleaseWP.Model
{
    class ReleaseDefinition
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Url { get; set; }
        public Identity CreatedBy { get; set; }
        public List<RDArtifact> Artifacts { get; internal set; }
        public string ReleaseNameFormat { get; set; }

        public List<RDEnvironment> Environments { get; set; }

        public class RDArtifact
        {
            public string Id { get; set; }
            public string Alias { get; set; }
            public string Type { get; set; }
            public DefinitionRef DefinitionReference { get; set; }

            public class DefinitionRef
            {
                public ItemDetails Project { get; set; }
                public ItemDetails Definition { get; set; }

            }
        }

        public class RDEnvironment
        {
            public string Name { get; set; }
        }
    }
}
