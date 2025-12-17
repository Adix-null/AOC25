// https://adventofcode.com/2025/day/9

using System.ComponentModel;
using System.Drawing;

namespace day_9
{
    public class Program
    {
        internal static void Main()
        {
            string[] z = File.ReadAllLines(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\test.txt");
            // Normal
            {
                List<Pt> coords = z.Select(x => new Pt { X = int.Parse(x.Split(",")[0]), Y = int.Parse(x.Split(",")[1]) }).ToList();

                coords = coords.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

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

                            if (IsRectangleInsidePolygon(coords, conn, p1, p2))
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

        public static bool IsPointInPolygon(List<Pt> coords, List<Edge> conn, Pt p)
        {
            //Check if p is a corner 
            if(coords.Any(x => x.X == p.X && x.Y == p.Y))
                return false;

            //Check if p is on edge
            if (conn.Any(e => 
                (e.A.Y == e.B.Y && e.A.Y == p.Y && Math.Min(e.A.X, e.B.X) < p.X && Math.Max(e.A.X, e.B.X) > p.X) ||
                (e.A.X == e.B.X && e.A.X == p.X && Math.Min(e.A.Y, e.B.Y) < p.Y && Math.Max(e.A.Y, e.B.Y) > p.Y)
            )) 
                return true;

            int[] hits = new int[4];
            for (int i = 0; i < 4; i++) 
            {
                int inter = IntersectPointConn(conn, p, i) + IntersectPointCoord(coords, p, i) / 2;
                if (inter > 0)
                    return inter % 2 == 0;
                else
                {
                    inter = IntersectPointCoord(coords, p, i);
                    if (inter > 0)
                        return inter % 2 == 0;
                }
            }
            return false;

        }

        public static int IntersectPointConn(List<Edge> conn, Pt p, int dir)
        {
            Func<Pt, bool> pred = dir switch
            {
                0 => c => c.Y > p.Y,
                1 => c => c.X > p.X,
                2 => c => c.Y < p.Y,
                3 => c => c.X < p.X
            };

            Func<Pt, bool> align = dir switch
            {
                0 or 2 => c => c.X == p.X,
                1 or 3 => c => c.Y == p.Y
            };

            int hits = 0;
            foreach (Edge e in conn)
            {
                bool horizontal = e.A.Y == e.B.Y;
                if(horizontal)
                {
                    if ((dir == 0 && p.Y > e.A.Y || dir == 2 && p.Y < e.A.Y) && Math.Min(e.A.X, e.B.X) < p.X && Math.Max(e.A.X, e.B.X) > p.X)
                        hits++;
                }
                else
                {
                    if ((dir == 1 && p.X > e.A.X || dir == 3 && p.X < e.A.X) && Math.Min(e.A.Y, e.B.Y) < p.Y && Math.Max(e.A.Y, e.B.Y) > p.Y)
                        hits++;
                }
            }

            return hits;
        }

        public static int IntersectPointCoord(List<Pt> coord, Pt p, int dir)
        {
            Func<Pt, bool> pred = dir switch
            {
                0 => c => c.Y > p.Y,
                1 => c => c.X > p.X,
                2 => c => c.Y < p.Y,
                3 => c => c.X < p.X
            };

            Func<Pt, bool> eq = dir switch
            {
                0 or 2 => c => c.X == p.X,
                1 or 3 => c => c.Y == p.Y
            };

            return coord.Where(c => pred(c) && eq(c)).Count();
        }

        public static bool IsRectangleInsidePolygon(List<Pt> polygon, List<Edge> conn, Pt topLeft, Pt bottomRight)
        {
            //check if any cornerrs inside inner rect
            if (polygon.Any(p => p.X > topLeft.X && p.X < bottomRight.X && p.Y > topLeft.Y && p.Y < bottomRight.Y))
                return false;

            //check if edges present in inner rect
            bool isToBottom(Pt p) => p.Y > topLeft.Y || p.Y > bottomRight.Y;
            bool isToTop(Pt p) => p.Y < topLeft.Y || p.Y < bottomRight.Y;
            bool isToRight(Pt p) => p.X > topLeft.X || p.X > bottomRight.X;
            bool isToLeft(Pt p) => p.X < topLeft.X || p.X < bottomRight.X;

            if (conn.Any(e => isToBottom(e.A) && isToTop(e.B) || isToBottom(e.B) && isToTop(e.A) ||
            isToLeft(e.A) && isToRight(e.B) || isToLeft(e.B) && isToRight(e.A)))
                return false;

            //check if point inside in the middle is inside
            Pt mid = new() { X = topLeft.X + bottomRight.X / 2, Y = topLeft.Y + bottomRight.Y / 2 };

            for(int dir = 0; dir < 4; dir++)
            {
                int con = IntersectPointConn(conn, mid, dir);
                if(con != 0)
                    return con % 2 != 0;
            }

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
