// https://adventofcode.com/2025/day/3

using System.Numerics;

namespace day_3
{
    public class Program
    {
        internal static void Main()
        {
            string[] z = File.ReadAllLines(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\input.txt");
            // Normal
            {

                //Console.WriteLine(z.Select(l => l.Select(c => c - '0').ToList()[..(l.Length - 1)].Select((value, index) => (value, index)).MaxBy(p => p.value).value * 10 + l.Select(c => c - '0').ToList()[(l.Select(c => c - '0').ToList()[..(l.Length - 1)].Select((value, index) => (value, index)).MaxBy(p => p.value).index + 1)..].Select((value, index) => (value, index)).MaxBy(p => p.value).value).Sum());
                long sum = 0;

                foreach (string l in z)
                {
                    long n = 0;
                    List<int> list = l.Select(c => c - '0').ToList();
                    int pos = 0;

                    for (int i = 11; i >= 0; i--)
                    {
                        var max = list[pos..(list.Count - i)].Select((v, idx) => (v, idx)).MaxBy(x => x.v);

                        n += (long)Math.Pow(10, i) * max.v;
                        pos += max.idx + 1;
                    }
                    sum += n;
                }
                Console.WriteLine(sum);
            }

            // pure LINQed: 
            {
                Console.WriteLine(
                    z.Sum(
                    l => Enumerable.Range(0, 12).Reverse().Aggregate(
                        (n: 0L, pos: 0),
                        (a, i) => (a.n + (long)Math.Pow(10, i) * l
                            .Select(c => c - '0').ToList()[
                                a.pos
                                ..
                                (l.Length - i)
                            ]
                            .Select((v, idx) => (v, idx))
                            .MaxBy(x => x.v).v, 
                            a.pos + l
                            .Select(c => c - '0').ToList()[
                                a.pos
                                ..
                                (l.Length - i)
                            ]
                            .Select((v, idx) => (v, idx))
                            .MaxBy(x => x.v).idx + 1)
                    ).n
                ));
                //Console.WriteLine(z.Sum(l => Enumerable.Range(0, 12).Reverse().Aggregate((n: 0L, pos: 0), (a, i) => (a.n + (long)Math.Pow(10, i) * l.Select(c => c - '0').ToList()[a.pos..(l.Length - i)].Select((v, idx) => (v, idx)).MaxBy(x => x.v).v, a.pos + l.Select(c => c - '0').ToList()[a.pos..(l.Length - i)].Select((v, idx) => (v, idx)).MaxBy(x => x.v).idx + 1)).n));
            }
        }
    }
}
