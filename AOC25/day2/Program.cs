// https://adventofcode.com/2025/day/2

namespace day_2
{
    public class Program
    {
        internal static void Main()
        {
            string[] z = File.ReadAllLines(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\input.txt");
            // Normal

            var ranges = z[0].Split(',').Select(x =>
            {
                var parts = x.Split('-');
                return new Range
                {
                    start = long.Parse(parts[0]),
                    end = long.Parse(parts[1])
                };
            }).OrderBy(r => r.start).ToList();

            long sum = 0;
            foreach (Range range in ranges)
            {
                for (long i = range.start; i <= range.end; i++)
                {
                    sum += Pattern(i);
                }
            }
            Console.WriteLine(sum);

            //Golfed char count: 398
            {
                Console.WriteLine(z[0].Split(',').Select(x => { var t = x.Split('-').Select(long.Parse).ToList(); return Enumerable.Range(0, (int)(t[1] - t[0] + 1)).Select(n => { List<int> c, p, l = [.. (n + t[0]).ToString().Select(c => c - 48)]; for (int i = 1, g, j; ++i <= l.Count;) { c = []; p = c; g = 1; for (j = 0; j < i; p = c) { c = l[(l.Count * j / i)..(l.Count * (++j) / i)]; if (p.Count < 1) p = c; if (!p.SequenceEqual(c)) g = 0; } if (g > 0) return n + t[0]; } return 0; }).Sum(); }).Sum());
            }
        }
        static long Pattern(long n)
        {
            List<int> nlist = n.ToString().Select(c => c - '0').ToList();

            for (int i = 2; i <= nlist.Count; i++)
            {
                List<int> currentSeq = [], previousSeq = [];
                bool correct = true;
                for ( int j = 0; j < i; j++)
                {
                    currentSeq = nlist[(nlist.Count * j / i)..(nlist.Count * (j + 1) / i)];
                    if(previousSeq.Count == 0)
                    {
                        previousSeq = currentSeq;
                    }
                    if (!previousSeq.SequenceEqual(currentSeq))
                    {
                        correct = false;
                    }
                    previousSeq = currentSeq;
                }
                if(correct)
                    return n;
            }
            return 0;
        }

    }
    public struct Range
    {
        public long start;
        public long end;
    }
}