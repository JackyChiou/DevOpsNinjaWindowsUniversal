using BuildnReleaseWP.Model;
using System.Collections.Generic;

namespace BuildnReleaseWP.Service
{
   
        public class ProjectsList
        {
            public int count { get; set; }
            public IEnumerable<Project> value { get; set; }
        }
  
}