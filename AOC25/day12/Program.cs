// https://adventofcode.com/2025/day/12

namespace day_12
{
    public class Program
    {
        internal static void Main()
        {
            string[] z = File.ReadAllLines(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\input.txt");
            // Normal
            {
                List<int> areas = z.Select(x => x.Split(" ")[0][..^1].Split("x").Select(int.Parse).ToList()[0] * 
                    x.Split(" ")[0][..^1].Split("x").Select(int.Parse).ToList()[1]).ToList();
                List<int> sums = z.Select(x => x.Split(" ")[1..].Sum(int.Parse)).ToList();
                int sum = 0;
                for (int i = 0; i < areas.Count; i++)
                {
                    if (areas[i] >= sums[i] * 9)
                        sum++;
                }
                Console.WriteLine(sum);
            }

            //
            {

            }
        }
    }
}
