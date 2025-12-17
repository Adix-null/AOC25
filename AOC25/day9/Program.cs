// https://adventofcode.com/2025/day/9

using Microsoft.VisualBasic;

namespace day_9
{
    public class Program
    {
        internal static void Main()
        {
            string[] z = File.ReadAllLines(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\input.txt");
            // Normal
            {
                List<Pt> coords = z.Select(x => new Pt { X = int.Parse(x.Split(",")[0]), Y = int.Parse(x.Split(",")[1]) }).ToList();

                List <Edge> conn = [];
                for (int i = 0; i < coords.Count - 1; i++)
                {
                    conn.Add(new Edge{ A = coords[i], B = coords[i+1] });
                }
                conn.Add(new Edge { A = coords[^1], B = coords[0] });


                long bestArea = 0;
                foreach (Pt p1 in coords)
                {
                    foreach (Pt p2 in coords)
                    {
                        if (p1.X != p2.X && p1.Y != p2.Y)
                        {
                            long area = (long.Abs(p1.X - p2.X) + 1) * (long.Abs(p1.Y - p2.Y) + 1);
                            Pt tl = new() { X = Math.Min(p1.X, p2.X), Y = Math.Min(p1.Y, p2.Y) };
                            Pt br = new() { X = Math.Max(p1.X, p2.X), Y = Math.Max(p1.Y, p2.Y) };

                            if (IsRectangleInsidePolygon(coords, conn, tl, br))
                                bestArea = Math.Max(bestArea, area);
                        }
                    }
                }
                Console.WriteLine(bestArea);
            }

            //
            {

            }
        }


        public static bool IsRectangleInsidePolygon(List<Pt> polygon, List<Edge> conn, Pt topLeft, Pt bottomRight)
        {
            //check if any cornerrs inside inner rect
            if (polygon.Any(p => p.X > topLeft.X && p.X < bottomRight.X && p.Y > topLeft.Y && p.Y < bottomRight.Y))
                return false;

            //check if edges present in inner rect
            int left = topLeft.X + 1;
            int right = bottomRight.X - 1;
            int top = topLeft.Y + 1;
            int bottom = bottomRight.Y - 1;

            bool Inside(Pt p) => p.X > left && p.X < right &&p.Y > top && p.Y < bottom;

            bool SegmentIntersectsInner(Pt a, Pt b)
            {
                // one endpoint strictly inside
                if (Inside(a) || Inside(b))
                    return true;

                // reject if segment is completely on one side
                if (a.X < left && b.X < left) return false;
                if (a.X > right && b.X > right) return false;
                if (a.Y < top && b.Y < top) return false;
                if (a.Y > bottom && b.Y > bottom) return false;

                return true;
            }

            if (conn.Any(e => SegmentIntersectsInner(e.A, e.B)))
                return false;

            return true;
        }
    }

    public struct Edge
    {
        public Pt A;
        public Pt B;
    }

    public struct Pt
    {
        public int X;
        public int Y;
    }
}
