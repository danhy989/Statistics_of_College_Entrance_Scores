using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.Dto
{
    public class Province
    {
        private int id { get; set; }
        private string name { get; set; }
        public Province()
        {
        }

        public Province(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }

    public class JsonProvince
    {
        private int id { get; set; }
        private string name { get; set; }
        public JsonProvince()
        {
        }

        public JsonProvince(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
