using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Book12.MapStuff
{
    public class MapSettings
    {
        //Base Map Settings
        public static int mapX_Max = 1300;
        public static int mapY_Max = 700;
        public static Size map_size = new Size(mapX_Max, mapY_Max);

        //Map Generation Settings
        public static float[] mapESet = new float[] { 10, 0.0036F, 1.5F, 2400, 10 };
        public static float[] mapMSet = new float[] { 10, 0.005F, 1.7F, 0, 100 };
        public static float[] mapEVal = new float[] { 0.4F, 0.42F, 0.8F, 0.7F, 0.6F };
        public static float[] mapMVal = new float[] { 0.1F, 0.2F, 0.5F, 0.33F, 0.66F, 0.16F, 0.50F, 0.83F, 0.16F, 0.33F, 0.66F };

        //public static Color isLandColor = Color.FromArgb(34, 139, 34);
        public static Color isLandColor = Color.Red;

        public static int cityDotBorder = 9;
        public static int cityDotInner = 7;

        public static int cityExclusion = 50;
        public static int cityExclusionflux = 30;

        public static int mapOuterBorder = 2;
        public static Color mapBordersColor = Color.DarkViolet;

        //Range 0.0 to 1.0
        public static float mapTintLevel = 0.9f;
        
    }
}
