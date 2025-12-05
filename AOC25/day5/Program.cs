// https://adventofcode.com/2025/day/5

namespace day_5
{
    public class Program
    {
        internal static void Main()
        {
            string[] z = File.ReadAllLines(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\input.txt");
            // Normal
            {
                int fresh = 0;
                long sum = 0;

                List<(long, long)> ranges = [];
                List<long> ids = [];

                foreach (var line in z.Where(s => !string.IsNullOrWhiteSpace(s)))
                {
                    if (line.Contains('-'))
                    {
                        var parts = line.Split('-');
                        ranges.Add((long.Parse(parts[0]), long.Parse(parts[1])));
                    }
                    else
                    {
                        ids.Add(long.Parse(line));
                    }
                }

                foreach (long n in ids)
                {
                    bool valid = false;
                    foreach (var range in ranges)
                    {
                        if (n >= range.Item1 && n <= range.Item2)
                        {
                            valid = true;
                            break;
                        }
                    }
                    if (valid)
                    {
                        fresh++;
                    }
                }

                ranges = ranges.OrderBy(r => r.Item1).ToList();
                List<(long, long)> merged = [];

                foreach (var range in ranges)
                {
                    if (merged.Count == 0)
                    {
                        merged.Add(range);
                        continue;
                    }
                    if (range.Item1 > merged.Last().Item2)
                    {
                        merged.Add(range);
                    }
                    else
                    {
                        if (range.Item2 > merged.Last().Item2)
                        {
                            merged[^1] = (merged[^1].Item1, range.Item2);
                        }
                    }
                }
                foreach (var range in merged)
                {
                    sum += range.Item2 - range.Item1 + 1;
                }


                Console.WriteLine(sum);
            }

            //LINQed
            {
                Console.WriteLine(z.Where(s => s.Contains('-'))
                    .Select(l => l.Split('-').Select(long.Parse).ToList())
                    .OrderBy(r => r[0])
                    .Aggregate(Enumerable.Empty<List<long>>().ToList(), (acc, r) =>
                        acc.Count == 0 || r[0] > acc[^1][1]
                            ? acc.Append(r).ToList()
                            : acc.Take(acc.Count - 1)
                                 .Append([acc[^1][0], Math.Max(acc[^1][1], r[1])])
                                 .ToList()
                    ).Aggregate(0L, (acc, r) => acc + r[1] - r[0] + 1));
                //Console.WriteLine(z.Where(s=>s.Contains('-')).Select(l=>l.Split('-').Select(long.Parse).ToList()).OrderBy(r=>r[0]).Aggregate(Enumerable.Empty<List<long>>().ToList(),(acc,r)=>acc.Count==0||r[0]>acc[^1][1]?acc.Append(r).ToList():acc.Take(acc.Count-1).Append([acc[^1][0],Math.Max(acc[^1][1],r[1])]).ToList()).Aggregate(0L,(acc,r)=>acc+r[1]-r[0]+1));
            }
        }
    }
}
