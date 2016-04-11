using System;
using System.Collections.Generic;
using System.Text;

namespace BuildnReleaseWP.Model
{
    class BuildDefinition
    {
        public Repository Repository { get; set; }

        public string Name { get; set; }
        public string Id { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public Identity AuthoredBy { get; set; }
        public string BuildNumberFormat { get; set; }
        public Queue Queue { get; set; }
    }
}
