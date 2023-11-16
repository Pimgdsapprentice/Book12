using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Locations
{
    public class NamedLocation : Location
    {
        public string LocationName;

        public NamedLocation(int id, int x_Cord, int y_Cord, string locationName) : base(id, x_Cord, y_Cord)
        {
            this.LocationName = locationName;
        }
    }
}

