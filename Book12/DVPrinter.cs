using DelaunayVoronoi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Engine.DelaunayVoronoi
{
    public class DVPrinter
    {

        public static Dictionary<string, Bitmap> map_Dict2 = new Dictionary<string, Bitmap>();

        private DelaunayTriangulator delaunay = new DelaunayTriangulator();
        private Voronoi voronoi = new Voronoi();
        public int PointCount { get; set; } = 2000;

        // Map Settings
        public static int mapX_Max = 1300;
        public static int mapY_Max = 700;
        public static Size size = new Size(mapX_Max, mapY_Max);

        public double DiagramWidth => mapX_Max;
        public double DiagramHeight => mapY_Max;

        public ICommand DrawCommand { get; set; }

        public DVPrinter()
        {
            DrawCommand = new Command(param => GenerateAndDraw());
        }

        public void GenerateAndDraw()
        {
            var points = delaunay.GeneratePoints(PointCount, DiagramWidth, DiagramHeight);
            var triangulation = delaunay.BowyerWatson(points);
            var voronoiEdges = voronoi.GenerateEdgesFromDelaunay(triangulation);

            Bitmap bitmap = new Bitmap(mapX_Max, mapY_Max);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                DrawPoints(g, points);
                DrawTriangulation(g, triangulation);
                DrawVoronoi(g, voronoiEdges);
            }

            map_Dict2["Map"] = bitmap;
        }

        private void DrawPoints(Graphics g, IEnumerable<DVPoint> points)
        {
            foreach (var point in points)
            {
                g.FillEllipse(System.Drawing.Brushes.Red, (float)point.X, (float)point.Y, 1, 1);
            }
        }
        private void DrawTriangulation(Graphics g, IEnumerable<Triangle> triangulation)
        {
            var edges = new List<Edge>();
            foreach (var triangle in triangulation)
            {
                edges.Add(new Edge(triangle.Vertices[0], triangle.Vertices[1]));
                edges.Add(new Edge(triangle.Vertices[1], triangle.Vertices[2]));
                edges.Add(new Edge(triangle.Vertices[2], triangle.Vertices[0]));
            }

            foreach (var edge in edges)
            {
                g.DrawLine(
                    new Pen(System.Drawing.Color.LightSteelBlue, 0.5f),
                    Convert.ToInt32(edge.Point1.X),
                    Convert.ToInt32(edge.Point1.Y),
                    Convert.ToInt32(edge.Point2.X),
                    Convert.ToInt32(edge.Point2.Y)
                );
            }
        }

        private void DrawVoronoi(Graphics g, IEnumerable<Edge> voronoiEdges)
        {
            foreach (var edge in voronoiEdges)
            {
                g.DrawLine(
                    new Pen(System.Drawing.Color.DarkViolet, 1),
                    Convert.ToInt32(edge.Point1.X),
                    Convert.ToInt32(edge.Point1.Y),
                    Convert.ToInt32(edge.Point2.X),
                    Convert.ToInt32(edge.Point2.Y)
                );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Command : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public Command(Action<object> execute)
            : this(execute, param => true)
        {
        }

        public Command(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return (_canExecute == null) || _canExecute(parameter);
        }
    }
}
