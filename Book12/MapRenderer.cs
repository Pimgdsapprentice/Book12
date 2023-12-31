﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using Engine;



namespace Book12
{
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
            Random rnd = new Random();
            int randomX = Randomer.Instance.Next(xbuffer, mapX_Max - xbuffer);
            int randomY = Randomer.Instance.Next(ybuffer, mapY_Max - ybuffer);
            return Tuple.Create(randomX, randomY);
            
        }

        public void RenderMapInitial()
        {
            Bitmap bmp_Map = new Bitmap(size.Width, size.Height);
            Bitmap bmp_is_Land = new Bitmap(size.Width, size.Height);
            for (int x = 0; x < mapX_Max; x++)
            {
                for (int y = 0; y < mapY_Max; y++)
                {
                    float CalcE = (float)(Engine.SimplexNoise.GenerateO(x, y, (int)mapESet[0], mapESet[1], mapESet[2], mapESet[3], mapESet[4]));
                    float CalcM = (float)(Engine.SimplexNoise.GenerateO(x, y, (int)mapMSet[0], mapMSet[1], mapMSet[2], mapMSet[3], mapMSet[4]));

                    // shading
                    if (CalcE < mapEVal[0])
                    {
                        //Ocean
                        bmp_Map.SetPixel(x, y, Color.FromArgb(67, 67, 122));
                    }
                    else
                    {
                        bmp_is_Land.SetPixel(x, y, Color.FromArgb(34, 139, 34));
                        if (CalcE < mapEVal[1])
                        {
                            //Beach
                            bmp_Map.SetPixel(x, y, Color.FromArgb(160, 145, 119));
                        }
                        else if (CalcE > mapEVal[2])
                        {
                            if (CalcM < mapMVal[0])
                            {
                                //return SCORCHED;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(85, 85, 85));
                            }
                            else if (CalcM < mapMVal[1])
                            {
                                //return BARE;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(136, 136, 136));
                            }
                            else if (CalcM < mapMVal[2])
                            {
                                //return TUNDRA;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(188, 188, 171));
                            }
                            else
                            {
                                //return SNOW;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(221, 221, 228));
                            }
                        }
                        else if (CalcE > mapEVal[3])
                        {
                            if (CalcM < mapMVal[3])
                            {
                                //return TEMPERATE_DESERT;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(201, 210, 155));
                            }
                            else if (CalcM < mapMVal[4])
                            {
                                // RETURN SHRUBLAND;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(136, 153, 119));
                            }
                            else
                            {
                                // RETURN TAIGA;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(153, 171, 119));
                            }
                        }
                        else if (CalcE > mapEVal[4])
                        {
                            if (CalcM < mapMVal[5])
                            {
                                // RETURN TEMPERATE_DESERT;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(201, 210, 155));
                            }
                            else if (CalcM < mapMVal[6])
                            {
                                // RETURN GRASSLAND;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(136, 171, 86));
                            }
                            else if (CalcM < mapMVal[7])
                            {
                                // RETURN TEMPERATE_DECIDUOUS_FOREST;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(103, 147, 89));
                            }
                            else
                            {
                                // RETURN TEMPERATE_RAIN_FOREST;
                                //bmp_Map.SetPixel(x, y, Color.FromArgb(201, 210, 155);
                                bmp_Map.SetPixel(x, y, Color.FromArgb(67, 136, 85));
                            }
                        }
                        else
                        {
                            if (CalcM < mapMVal[8])
                            {
                                // RETURN SUBTROPICAL_DESERT;
                                //bmp_Map.SetPixel(x, y, Color.FromArgb(136, 171, 86);
                                bmp_Map.SetPixel(x, y, Color.FromArgb(210, 186, 139));
                            }
                            else if (CalcM < mapMVal[9])
                            {
                                // RETURN GRASSLAND;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(136, 171, 85));
                            }

                            else if (CalcM < mapMVal[10])
                            {
                                // RETURN TROPICAL_SEASONAL_FOREST;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(86, 153, 68));
                            }
                            else
                            {
                                // RETURN TROPICAL_RAIN_FOREST;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(51, 119, 65));
                            }
                        }
                    }

                }
            }
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
