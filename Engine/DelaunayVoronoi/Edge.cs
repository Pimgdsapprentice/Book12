namespace DelaunayVoronoi
{
    public class Edge
    {
        public DVPoint Point1 { get; }
        public DVPoint Point2 { get; }

        public DVPoint Midpoint => new DVPoint((Point1.X + Point2.X) / 2, (Point1.Y + Point2.Y) / 2);

        public Edge(DVPoint point1, DVPoint point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            var edge = obj as Edge;

            var samePoints = Point1 == edge.Point1 && Point2 == edge.Point2;
            var samePointsReversed = Point1 == edge.Point2 && Point2 == edge.Point1;
            return samePoints || samePointsReversed;
        }

        public override int GetHashCode()
        {
            int hCode = (int)Point1.X ^ (int)Point1.Y ^ (int)Point2.X ^ (int)Point2.Y;
            return hCode.GetHashCode();
        }
    }
}
