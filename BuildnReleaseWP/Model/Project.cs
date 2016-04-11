using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildnReleaseWP.Model
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public State ProjectState { get; set; }
    }

    public enum State
    {
        WellFormed,
        CreatePending,
        Deleting,
        New,
        All
    }
}
