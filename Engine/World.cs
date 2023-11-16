using Engine.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class World
    {
        public static string World_Name;
        public int seed = 123;
        public static int locationIndex;
        public static Dictionary<int, NL_Settlement> w_settlements;
    }
}
