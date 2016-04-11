using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildnReleaseWP
{
    class ItemDetails
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public ItemDetails() { }


        public ItemDetails(string name)
        {
            this.Name = name;
        }
        public ItemDetails(string id, string name)
        {
            this.Name = name;
            this.Id = id;
        }
    }
}
