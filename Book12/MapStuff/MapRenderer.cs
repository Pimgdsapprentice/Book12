using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Book12.GameData;
using Book12.MapStuff;
using DelaunayVoronoi;
using Engine;
using Engine.Language;
using Engine.Language.Examples;
using Engine.LanguageB;
using Engine.Locations;



namespace Book12.MapStuff
{
    public class MapRenderer
    {
        DnC_Screen dnC_scrn;
        DVPrinter dvPrint = new DVPrinter();
        public static Tuple<int, int> randomCords(int xbuffer, int ybuffer)
        {
            Random rnd = new Random();
            int randomX = Randomer.Instance.Next(xbuffer, MapSettings.mapX_Max - xbuffer);
            int randomY = Randomer.Instance.Next(ybuffer, MapSettings.mapY_Max - ybuffer);
            return Tuple.Create(randomX, randomY);

        }
        public void RenderMapInitial()
        {
            Bitmap bmp_Map = new Bitmap(MapSettings.map_size.Width, MapSettings.map_size.Height);
            Bitmap bmp_is_Land = new Bitmap(MapSettings.map_size.Width, MapSettings.map_size.Height);
            for (int x = 0; x < MapSettings.mapX_Max; x++)
            {
                for (int y = 0; y < MapSettings.mapY_Max; y++)
                {
                    float CalcE = (float)SimplexNoise.GenerateO(x, y, (int)MapSettings.mapESet[0], MapSettings.mapESet[1], MapSettings.mapESet[2], MapSettings.mapESet[3], MapSettings.mapESet[4]);
                    float CalcM = (float)SimplexNoise.GenerateO(x, y, (int)MapSettings.mapMSet[0], MapSettings.mapMSet[1], MapSettings.mapMSet[2], MapSettings.mapMSet[3], MapSettings.mapMSet[4]);

                    // shading
                    if (CalcE < MapSettings.mapEVal[0])
                    {
                        //Ocean
                        bmp_Map.SetPixel(x, y, Color.FromArgb(67, 67, 122));
                    }
                    else
                    {
                        bmp_is_Land.SetPixel(x, y, MapSettings.isLandColor);
                        if (CalcE < MapSettings.mapEVal[1])
                        {
                            //Beach
                            bmp_Map.SetPixel(x, y, Color.FromArgb(160, 145, 119));
                        }
                        else if (CalcE > MapSettings.mapEVal[2])
                        {
                            if (CalcM < MapSettings.mapMVal[0])
                            {
                                //return SCORCHED;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(85, 85, 85));
                            }
                            else if (CalcM < MapSettings.mapMVal[1])
                            {
                                //return BARE;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(136, 136, 136));
                            }
                            else if (CalcM < MapSettings.mapMVal[2])
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
                        else if (CalcE > MapSettings.mapEVal[3])
                        {
                            if (CalcM < MapSettings.mapMVal[3])
                            {
                                //return TEMPERATE_DESERT;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(201, 210, 155));
                            }
                            else if (CalcM < MapSettings.mapMVal[4])
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
                        else if (CalcE > MapSettings.mapEVal[4])
                        {
                            if (CalcM < MapSettings.mapMVal[5])
                            {
                                // RETURN TEMPERATE_DESERT;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(201, 210, 155));
                            }
                            else if (CalcM < MapSettings.mapMVal[6])
                            {
                                // RETURN GRASSLAND;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(136, 171, 86));
                            }
                            else if (CalcM < MapSettings.mapMVal[7])
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
                            if (CalcM < MapSettings.mapMVal[8])
                            {
                                // RETURN SUBTROPICAL_DESERT;
                                //bmp_Map.SetPixel(x, y, Color.FromArgb(136, 171, 86);
                                bmp_Map.SetPixel(x, y, Color.FromArgb(210, 186, 139));
                            }
                            else if (CalcM < MapSettings.mapMVal[9])
                            {
                                // RETURN GRASSLAND;
                                bmp_Map.SetPixel(x, y, Color.FromArgb(136, 171, 85));
                            }

                            else if (CalcM < MapSettings.mapMVal[10])
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
            World.map_Dict[MapName.World_Map] = bmp_Map;
            World.map_Dict[MapName.Is_Land_Map] = bmp_is_Land;
        }
        public void RenderCities()
        {
            //Placeholder Language
            LanguageGenerator lg = new LanguageGenerator();
            //Found in PLGL.Examples. Set this to your own language (derived from Language).
            Qen qen = new Qen();
            lg.Language = qen;

            int citiescount = 50;
            int generatedCities = 0;
            int maxAttempts = 1000; // Set a maximum number of attempts to generate a city

            Bitmap bmp_Cities = CopyBitmap(World.map_Dict[MapName.World_Map]);
            World.map_Dict[MapName.Cities_Map] = bmp_Cities;
            World.map_Dict[MapName.Cities_Ex_Map] = CopyBitmap(World.map_Dict[MapName.Is_Land_Map]);
            Bitmap cities_Ex_Map = World.map_Dict[MapName.Cities_Ex_Map];

            while (generatedCities < citiescount)
            {
                if (maxAttempts <= 0)
                {
                    // Break the loop if we can't generate more cities
                    break;
                }
                var coords = randomCords(MapSettings.cityDotBorder, MapSettings.cityDotBorder);
                int x = coords.Item1;
                int y = coords.Item2;
                Color pixelColor = cities_Ex_Map.GetPixel(x, y);
                if (pixelColor.ToArgb() == MapSettings.isLandColor.ToArgb())
                {
                    NL_Settlement nL_Settlement = new NL_Settlement(World.locationIndex, x, y, lg.GenerateClean(LanguageReferences.realCityNames[generatedCities]));

                    World.w_settlements.Add(World.locationIndex, nL_Settlement);
                    World.locationIndex++;
                    AddDot(x, y, Color.Gray, MapSettings.cityDotBorder, bmp_Cities);
                    AddDot(x, y, Color.Yellow, MapSettings.cityDotInner, bmp_Cities);
                    AddDot(x, y, Color.Black, MapSettings.cityExclusion + Randomer.Instance.Next(MapSettings.cityExclusionflux), cities_Ex_Map);
                    generatedCities++;
                }
                maxAttempts--;
            }
            if (maxAttempts <= 0)
            {
                MessageBox.Show("Unable to place all cities. Some areas may be too crowded.");
            }
        }
        public void RenderProvincalBorders()
        {
            dvPrint.Intialize();
            var centerPoints = World.w_settlements.Values.Select(settlement => new DVPoint(settlement.X_Cord, settlement.Y_Cord));
            dvPrint.GenerateAndDrawWithCenters(centerPoints);

            //Gets the Locations of the Cities
            List<Point> points = new List<Point>();
            foreach (var settlement in World.w_settlements.Values)
            {
                points.Add(new Point(settlement.X_Cord, settlement.Y_Cord));
            }

            Bitmap uncolouredMap = MapRenderer.CopyBitmap(World.map_Dict[MapName.DVMapWithCenters]);
            Bitmap borderedBitmap = DrawBorder(uncolouredMap, MapSettings.mapOuterBorder, MapSettings.mapBordersColor);
            World.map_Dict[MapName.Provincal_Colors_Map] = borderedBitmap;
            foreach (var seedPoint in points)
            {
                FloodFill(World.map_Dict[MapName.Provincal_Colors_Map], seedPoint, Color.Black, RandomColour());
            }
        }
        public void TintWorldMap()
        {
            Bitmap worldMap = World.map_Dict[MapName.World_Map];
            Bitmap isLandMap = World.map_Dict[MapName.Is_Land_Map];
            Bitmap provincialColorsMap = World.map_Dict[MapName.Provincal_Colors_Map];

            int width = worldMap.Width;
            int height = worldMap.Height;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color landColor = isLandMap.GetPixel(x, y);

                    // Check if the pixel in Is_Land_Map is the isLandColor
                    if (landColor.ToArgb() == MapSettings.isLandColor.ToArgb())
                    {
                        Color provincialColor = provincialColorsMap.GetPixel(x, y);

                        // Apply tint to the pixel in World_Map
                        Color originalColor = worldMap.GetPixel(x, y);
                        Color tintedColor = TintColor(originalColor, provincialColor, MapSettings.mapTintLevel);
                        worldMap.SetPixel(x, y, tintedColor);
                    }
                }
            }

            // Update the modified World_Map in the map dictionary
            World.map_Dict[MapName.World_Map] = worldMap;
        }
        public void PlaceCities()
        {
            Bitmap worldMap = World.map_Dict[MapName.World_Map];
            List<Point> points = new List<Point>();
            foreach (var settlement in World.w_settlements.Values)
            {
                points.Add(new Point(settlement.X_Cord, settlement.Y_Cord));
            }
            foreach (var city in points)
            {
                AddDot(city.X, city.Y, Color.Gray, MapSettings.cityDotBorder, worldMap);
                AddDot(city.X, city.Y, Color.Yellow, MapSettings.cityDotInner, worldMap);
            }
        }

        private Color TintColor(Color originalColor, Color tintColor, float tintLevel)
        {
            int r = (int)(originalColor.R + (tintColor.R - originalColor.R) * tintLevel);
            int g = (int)(originalColor.G + (tintColor.G - originalColor.G) * tintLevel);
            int b = (int)(originalColor.B + (tintColor.B - originalColor.B) * tintLevel);

            // Ensure RGB values are in the valid range (0-255)
            r = Math.Max(0, Math.Min(255, r));
            g = Math.Max(0, Math.Min(255, g));
            b = Math.Max(0, Math.Min(255, b));

            return Color.FromArgb(r, g, b);
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
        public static Color RandomColour()
        {
            int red = Randomer.Instance.Next(0, 256);
            int green = Randomer.Instance.Next(0, 256);
            int blue = Randomer.Instance.Next(0, 256);

            // Clamp values to valid RGB range (0-255)
            red = Math.Min(255, Math.Max(0, red));
            green = Math.Min(255, Math.Max(0, green));
            blue = Math.Min(255, Math.Max(0, blue));

            return Color.FromArgb(red, green, blue);
        }
        public static Bitmap DrawBorder(Bitmap originalBitmap, int borderWidth, Color borderColor)
        {
            Bitmap borderedBitmap = new Bitmap(originalBitmap);

            using (Graphics g = Graphics.FromImage(borderedBitmap))
            {
                using (Pen borderPen = new Pen(borderColor, borderWidth))
                {
                    g.DrawRectangle(borderPen, new Rectangle(0, 0, borderedBitmap.Width, borderedBitmap.Height));
                }
            }

            return borderedBitmap;
        }
        private static void FloodFill(Bitmap bmp, Point pt, Color targetColor, Color replacementColor)
        {
            Stack<Point> pixels = new Stack<Point>();
            targetColor = bmp.GetPixel(pt.X, pt.Y);
            pixels.Push(pt);

            while (pixels.Count > 0)
            {
                Point a = pixels.Pop();
                if (a.X < bmp.Width && a.X >= 0 &&
                        a.Y < bmp.Height && a.Y >= 0) // Make sure we stay within bounds
                {
                    if (bmp.GetPixel(a.X, a.Y) == targetColor)
                    {
                        bmp.SetPixel(a.X, a.Y, replacementColor);
                        pixels.Push(new Point(a.X - 1, a.Y));
                        pixels.Push(new Point(a.X + 1, a.Y));
                        pixels.Push(new Point(a.X, a.Y - 1));
                        pixels.Push(new Point(a.X, a.Y + 1));
                    }
                }
            }
        }
        public static Bitmap CopyBitmap(Bitmap sourceBitmap)
        {
            if (sourceBitmap == null)
                return null;
            Bitmap copiedBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            using (Graphics g = Graphics.FromImage(copiedBitmap))
            {
                g.DrawImage(sourceBitmap, 0, 0, sourceBitmap.Width, sourceBitmap.Height);
            }
            return copiedBitmap;
        }      
    }
}
