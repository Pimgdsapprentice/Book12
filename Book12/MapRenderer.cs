using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using Engine;



namespace Book12
{
    /// <summary>
    /// Generates and caches bitmap layers for the main form.
    /// MapRenderer relies on Engine.SimplexNoise to create terrain
    /// and exposes the results via the static map_Dict dictionary.
    /// </summary>
    public class MapRenderer
    {
        //Dictionary for bmps of Map
        public static Dictionary<string, Bitmap> map_Dict = new Dictionary<string, Bitmap>();
        //"Map", "is_Land_Map"
        //Map Perlin Settings
        public static float[] mapESet = new float[] { 10, 0.0036F, 1.5F, 2400, 10 };
        public static float[] mapMSet = new float[] { 10, 0.005F, 1.7F, 0, 100 };
        public static float[] mapEVal = new float[] { 0.4F, 0.42F, 0.8F, 0.7F, 0.6F };
        public static float[] mapMVal = new float[] { 0.1F, 0.2F, 0.5F, 0.33F, 0.66F, 0.16F, 0.50F, 0.83F, 0.16F, 0.33F, 0.66F };
        
        //Map Settings
        public static int mapX_Max = 1300;
        public static int mapY_Max = 700;
        public static Size size = new Size(mapX_Max, mapY_Max);

        public static int cityDotBorder = 8;
        public static int cityDotInner = 6;

        Randomer randomer = new Randomer();
        public static Tuple<int, int> randomCords(int xbuffer, int ybuffer)
        {
            int randomX = Randomer.Instance.Next(xbuffer, mapX_Max - xbuffer);
            int randomY = Randomer.Instance.Next(ybuffer, mapY_Max - ybuffer);
            return Tuple.Create(randomX, randomY);

        }

        public void RenderMapInitial()
        {
            Bitmap bmp_Map = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            Bitmap bmp_is_Land = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);

            Rectangle rect = new Rectangle(0, 0, size.Width, size.Height);
            BitmapData mapData = bmp_Map.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData landData = bmp_is_Land.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* mapPtr = (byte*)mapData.Scan0;
                byte* landPtr = (byte*)landData.Scan0;

                for (int y = 0; y < mapY_Max; y++)
                {
                    byte* rowMap = mapPtr + (y * mapData.Stride);
                    byte* rowLand = landPtr + (y * landData.Stride);
                    for (int x = 0; x < mapX_Max; x++)
                    {
                        float CalcE = (float)Engine.SimplexNoise.GenerateO(x, y, (int)mapESet[0], mapESet[1], mapESet[2], mapESet[3], mapESet[4]);
                        float CalcM = (float)Engine.SimplexNoise.GenerateO(x, y, (int)mapMSet[0], mapMSet[1], mapMSet[2], mapMSet[3], mapMSet[4]);

                        bool isLand = false;
                        Color pixelColor;

                        if (CalcE < mapEVal[0])
                        {
                            pixelColor = Color.FromArgb(67, 67, 122); // ocean
                        }
                        else
                        {
                            isLand = true;
                            if (CalcE < mapEVal[1])
                            {
                                pixelColor = Color.FromArgb(160, 145, 119); // beach
                            }
                            else if (CalcE > mapEVal[2])
                            {
                                if (CalcM < mapMVal[0]) pixelColor = Color.FromArgb(85, 85, 85);
                                else if (CalcM < mapMVal[1]) pixelColor = Color.FromArgb(136, 136, 136);
                                else if (CalcM < mapMVal[2]) pixelColor = Color.FromArgb(188, 188, 171);
                                else pixelColor = Color.FromArgb(221, 221, 228);
                            }
                            else if (CalcE > mapEVal[3])
                            {
                                if (CalcM < mapMVal[3]) pixelColor = Color.FromArgb(201, 210, 155);
                                else if (CalcM < mapMVal[4]) pixelColor = Color.FromArgb(136, 153, 119);
                                else pixelColor = Color.FromArgb(153, 171, 119);
                            }
                            else if (CalcE > mapEVal[4])
                            {
                                if (CalcM < mapMVal[5]) pixelColor = Color.FromArgb(201, 210, 155);
                                else if (CalcM < mapMVal[6]) pixelColor = Color.FromArgb(136, 171, 86);
                                else if (CalcM < mapMVal[7]) pixelColor = Color.FromArgb(103, 147, 89);
                                else pixelColor = Color.FromArgb(67, 136, 85);
                            }
                            else
                            {
                                if (CalcM < mapMVal[8]) pixelColor = Color.FromArgb(210, 186, 139);
                                else if (CalcM < mapMVal[9]) pixelColor = Color.FromArgb(136, 171, 85);
                                else if (CalcM < mapMVal[10]) pixelColor = Color.FromArgb(86, 153, 68);
                                else pixelColor = Color.FromArgb(51, 119, 65);
                            }
                        }

                        int idx = x * 4;
                        rowMap[idx] = pixelColor.B;
                        rowMap[idx + 1] = pixelColor.G;
                        rowMap[idx + 2] = pixelColor.R;
                        rowMap[idx + 3] = pixelColor.A;

                        if (isLand)
                        {
                            rowLand[idx] = 34;
                            rowLand[idx + 1] = 139;
                            rowLand[idx + 2] = 34;
                            rowLand[idx + 3] = 255;
                        }
                        else
                        {
                            rowLand[idx] = rowLand[idx + 1] = rowLand[idx + 2] = rowLand[idx + 3] = 0;
                        }
                    }
                }
            }

            bmp_Map.UnlockBits(mapData);
            bmp_is_Land.UnlockBits(landData);

            map_Dict["Map"] = bmp_Map;
            map_Dict["is_Land_Map"] = bmp_is_Land;
        }

        public Bitmap AddDot(int x, int y, Color dotColor, int dotRad, Bitmap bmp_map)
        {
            using (Graphics graphics = Graphics.FromImage(bmp_map))
            {
                // Set the dot coordinates
                int dotX = x - dotRad; // X-coordinate
                int dotY = y - dotRad; // Y-coordinate

                // Draw the dot on the bitmap
                using (SolidBrush brush = new SolidBrush(dotColor))
                {
                    graphics.FillEllipse(brush, dotX, dotY, dotRad * 2, dotRad * 2);
                }
            }
            return bmp_map;
        }


        public void RenderCities()
        {
            int citiescount = 10;

            Bitmap bmp_Cities = map_Dict["Map"];
            map_Dict["cities_Map"] = bmp_Cities;

            Bitmap bmp_isLand = map_Dict["is_Land_Map"];
            for (int i = 0; i < citiescount; i++)
            {
                var coords = randomCords(cityDotBorder, cityDotBorder);
                int x = coords.Item1;
                int y = coords.Item2;
                Color pixelColor = bmp_isLand.GetPixel(x, y);
                if (pixelColor == Color.FromArgb(34, 139, 34))
                {
                    AddDot(x, y, Color.Gray, cityDotBorder, bmp_Cities);
                    AddDot(x, y, Color.Yellow, cityDotInner, bmp_Cities);
                }
                else
                {
                    i--;
                }
            }
            
            

        }
    }
}
