using System;
using System.Collections.Generic;
using System.Text;

namespace BuildnReleaseWP.Model
{
    class Build
    {
        public string Id { get; set; }
        public string BuildNumber { get; set; }
        public string Status { get; set; }
        public string Result { get; set; }
        public string Url { get; set; }
        public DateTime QueueTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public BuildDefinition Definition { get; set; }
        public Queue Queue { get; set; }
        public string SourceBranch { get; set; }
        public string SourceVersion { get; set; }
        public string Priority { get; set; }
        public string Reason { get; set; }
        public Identity RequestedFor { get; set; }
        public Identity RequestedBy { get; set; }
        //public Identity LastChangedBy { get; set; }
        public string LogsUrl { get; set; }
    }

    public class TimelineRecord
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public string State { get; set; }
        public string Result { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public double Duration { get; set; }
        public string LogUrl { get; set; }
        public string Type { get; set; }
        public string WorkerName { get; set; }
        public string ParentId { get; set; }

    }

    public class Artifact
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string DownloadUrl { get; set; }

    }
}
