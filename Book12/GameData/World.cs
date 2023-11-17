using Engine;
using Engine.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book12.GameData
{
    public class World
    {
        public static string World_Name;
        public static int seed;
        public static int World_Scale;

        public static int locationIndex;
        public static Dictionary<int, NL_Settlement> w_settlements;
        public static Dictionary<string, Bitmap> map_Dict = new Dictionary<string, Bitmap>();

    }
}
