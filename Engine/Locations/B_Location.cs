using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class B_Location
    {
        protected string name;
        protected int x_Cord;
        protected int y_Cord;

        public B_Location(string name, int x_Cord, int y_Cord)
        {
            this.name = name;
            this.x_Cord = x_Cord;
            this.y_Cord = y_Cord;
        }
    }
}
