// https://adventofcode.com/2025/day/6

namespace day_6
{
    public class Program
    {
        internal static void Main()
        {
            string[] z = File.ReadAllLines(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\input.txt");
            // Normal
            {
                long sum = 0;
                List<List<long>> nums = Enumerable.Range(0, z[0].Length).Select(_ => new List<long>()).ToList();
                for (int i = 0; i < z.Length - 1; i++)
                {
                    var nl = z[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                    for (int j = 0; j < nl.Count; j++)
                    {
                        nums[j].Add(nl[j]);
                    }
                }

                List<bool> isMult = z.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c => c[0] == '*').ToList();
                nums = nums.Where(n => n.Count > 0).ToList();
                for (int i = 0; i < nums.Count; i++)
                {
                    Func<long, long, long> operation = isMult[i] ? ((x, y) => x * y) : ((x, y) => x + y);
                    long result = isMult[i] ? 1 : 0;
                    foreach (long num in nums[i])
                    {
                        result = operation(result, num);
                    }
                    sum += result;
                }
                Console.WriteLine(sum);
            }

            //
            {
                long sum = 0;
                List<List<long>> nums = [];
                List<List<string>> full = [];
                List<int> spacing = [];

                string last = z.Last();

                for (int i = 1, c = 0; i < last.Length; i++)
                {
                    if (last[i] != ' ')
                    {
                        spacing.Add(c);
                        c = 0;
                    }
                    else if (i == last.Length - 1)
                    {
                        spacing.Add(c + 1);
                    }
                    else
                    {
                        c++;
                    }
                }

                for(int i = 0; i < spacing.Count; i++)
                {
                    List<string> grid = [];
                    foreach (string l in z[..^1])
                    {
                        int offset = spacing[..i].Sum() + i;
                        grid.Add(l[(spacing[..i].Sum() + i)..(spacing[..(i+1)].Sum() + i)]);
                    }
                    full.Add(grid);
                }

                for(int g = 0; g < full.Count; g++)
                {
                    List<List<char>> realNums = Enumerable.Range(0, spacing[g]).Select(_ => new List<char>()).ToList();

                    for (int i = 0; i < full[g].Count; i++)
                    {
                        for (int j = 0; j < spacing[g]; j++)
                        {
                            realNums[j].Add(full[g][i][j]);
                            var t = realNums;
                        }
                    }
                    nums.Add(realNums.Select(r => long.Parse(string.Join("", r))).ToList());
                }


                List<bool> isMult = z.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c => c[0] == '*').ToList();
                nums = nums.Where(n => n.Count > 0).ToList();
                for (int i = 0; i < nums.Count; i++)
                {
                    Func<long, long, long> operation = isMult[i] ? ((x, y) => x * y) : ((x, y) => x + y);
                    long result = isMult[i] ? 1 : 0;
                    foreach (long num in nums[i])
                    {
                        result = operation(result, num);
                    }
                    sum += result;
                }
                Console.WriteLine(sum);
            }
        }
    }
}
