using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Location
    {
        public int Id { get; private set; }
        public int X_Cord { get; private set; }
        public int Y_Cord { get; private set; }

        public Location(int id, int x_Cord, int y_Cord)
        {
            Id = id;
            X_Cord = x_Cord;
            Y_Cord = y_Cord;
        }
    }

}
