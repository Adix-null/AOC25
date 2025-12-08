// https://adventofcode.com/2025/day/8

namespace day_8
{
    public class Program
    {
        internal static void Main()
        {
            string[] z = File.ReadAllLines(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\input.txt");
            // Normal
            {
                var positions = z.Select(l => l.Split(',').Select(int.Parse).ToArray()).ToArray();

                int n = positions.Length;

                var edges =
                    Enumerable.Range(0, n)
                    .SelectMany(i =>
                        Enumerable.Range(i + 1, n - (i + 1)).Select(j =>
                        {
                            var a = positions[i];
                            var b = positions[j];
                            long d =
                                (long)(a[0] - b[0]) * (a[0] - b[0]) +
                                (long)(a[1] - b[1]) * (a[1] - b[1]) +
                                (long)(a[2] - b[2]) * (a[2] - b[2]);
                            return (i, j, d);
                        })
                    )
                    .OrderBy(e => e.d)
                    .Take(1000)
                    .ToArray();

                var parent = Enumerable.Range(0, n).ToArray();
                var size = Enumerable.Repeat(1, n).ToArray();

                int Find(int x)
                {
                    if (parent[x] == x) return x;
                    parent[x] = Find(parent[x]);
                    return parent[x];
                }

                void Union(int a, int b)
                {
                    int ra = Find(a);
                    int rb = Find(b);
                    if (ra == rb) return;

                    if (size[ra] < size[rb])
                    {
                        (rb, ra) = (ra, rb);
                    }
                    parent[rb] = ra;
                    size[ra] += size[rb];
                }

                foreach (var (i, j, _) in edges)
                {
                    Union(i, j);
                }

                var counts =
                    Enumerable.Range(0, n)
                    .Select(Find)
                    .GroupBy(r => r)
                    .Select(g => g.Count())
                    .OrderByDescending(x => x)
                    .ToArray();

                long answer = 1;
                foreach (var s in counts.Take(3))
                {
                    answer *= s;
                }

                Console.WriteLine(answer);
            }

            //part 2
            {
                var positions = z.Select(l => l.Split(',').Select(int.Parse).ToArray()).ToArray();

                int n = positions.Length;

                var edges =
                    Enumerable.Range(0, n)
                    .SelectMany(i =>
                        Enumerable.Range(i + 1, n - (i + 1)).Select(j =>
                        {
                            var a = positions[i];
                            var b = positions[j];
                            long d =
                                (long)(a[0] - b[0]) * (a[0] - b[0]) +
                                (long)(a[1] - b[1]) * (a[1] - b[1]) +
                                (long)(a[2] - b[2]) * (a[2] - b[2]);
                            return (i, j, d);
                        })
                    )
                    .OrderBy(e => e.d)
                    .ToArray();

                var parent = Enumerable.Range(0, n).ToArray();
                var size = Enumerable.Repeat(1, n).ToArray();

                // find w/ union
                int Find(int x)
                {
                    if (parent[x] == x) return x;
                    parent[x] = Find(parent[x]);
                    return parent[x];
                }

                (int a, int b)? lastMerge = null;

                void Union(int a, int b)
                {
                    int ra = Find(a);
                    int rb = Find(b);
                    if (ra == rb) return;

                    lastMerge = (a, b);

                    if (size[ra] < size[rb])
                    {
                        (rb, ra) = (ra, rb);
                    }

                    parent[rb] = ra;
                    size[ra] += size[rb];
                }

                int sets = n;

                foreach (var (i, j, _) in edges)
                {
                    int ri = Find(i);
                    int rj = Find(j);

                    if (ri != rj)
                    {
                        Union(i, j);
                        sets--;
                        if (sets == 1) break;
                    }
                }

                var (aIdx, bIdx) = lastMerge!.Value;
                long result = (long)positions[aIdx][0] * positions[bIdx][0];

                Console.WriteLine(result);
            }
        }
    }
}
