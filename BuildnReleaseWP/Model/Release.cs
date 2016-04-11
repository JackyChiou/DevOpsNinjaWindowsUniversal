using System;
using System.Collections.Generic;
using System.Text;

namespace BuildnReleaseWP.Model
{
    class Release
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public Identity CreatedBy { get; set; }
        public Identity ModifiedBy { get; set; }
        public string Reason { get; set; }
        public string SourceBranch { get; set; }
        public ReleaseDefinition ReleaseDefinition { get; set; }
        public string Description { get; set; }
        public string ReleaseNameFormat { get; set; }

        public List<REnvironment> Environments { get; set; }
        public List<RArtifact> Artifacts { get; set; }

        public class REnvironment
        {

            public string Id { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public string Rank { get; set; }
            public DateTime CreatedOn { get; set; }

            public List<Approval> PreDeployApprovals { get; set; }
            public List<Approval> PostDeployApprovals { get; set; }

            public DeployStep DeploySteps { get; set; }           
        }

        public class DeployStep
        {
            public List<DTask> Tasks { get; set; }
            public DJob Job { get; set; }
        }

        public class DJob
        {
            public string Status { get; set; }
        }
        public class DTask
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public int Rank { get; set; }
            public string AgentName { get; set; }
            public string LogUrl { get; set; }
            public DateTime DateStarted { get; set; }
            public DateTime DateEnded { get; set; }
            public double Duration { get; set; }
        }

        public class RArtifact
        {
            public string Alias { get; set; }
            public string Id { get; set; }
            public string Type { get; set; }
            public ArtifactDefinitionReference  DefinitionReference { get; set; }

            public class ArtifactDefinitionReference
            {
                public string DefinitionId { get; set; }
                public string DefintionName { get; set; }
                public string projectId { get; set; }
                public string projectName { get; set; }
                public string VersionId { get; set; }
                public string VersionName { get; set; }
                public string BranchId { get; set; }
                public string   BranchName { get; set; }
            }
        }
    }
}
