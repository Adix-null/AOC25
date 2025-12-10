// https://adventofcode.com/2025/day/10

namespace day_10
{
    public class Program
    {
        internal static void Main()
        {
            string[] z = File.ReadAllLines(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\input.txt");
            // Normal
            {
                List<List<int>> goals = z.Select(l => l.Split(" ")[0][1..^1].Select(c => c == '#' ? 1 : 0).ToList()).ToList();
                List<List<int>> joltages = z.Select(l => l.Split(" ")[^1][1..^1].Split(",").Select(int.Parse).ToList()).ToList();
                List<List<List<int>>> combos = z.Select(l => l.Split(" ")[1..^1].Select(n => n[1..^1].Split(",").Select(int.Parse).ToList()).ToList()).ToList();

                int sum = 0;
                for (int i = 0; i < z.Length; i++)
                {
                    int lowest = combos[i].Count;
                    for (int j = 0; j < Math.Pow(2, combos[i].Count); j++)
                    {
                        List<bool> bits = Convert.ToString(j, 2).PadLeft(combos[i].Count, '0').Select(ch => ch == '1').ToList();
                        List<bool> lights = Enumerable.Repeat(false, goals[i].Count).ToList();
                        for (int k = 0; k < combos[i].Count; k++)
                        {
                            if (bits[k])
                            {
                                for (int i1 = 0; i1 < combos[i][k].Count; i1++)
                                {
                                    lights[combos[i][k][i1]] = !lights[combos[i][k][i1]];
                                }
                            }
                        }
                        bool cor = true;
                        for (int k = 0; k < lights.Count; k++)
                        {
                            if (!((lights[k] == true && goals[i][k] == 1) || (lights[k] == false && goals[i][k] == 0)))
                            {
                                cor = false;
                                break;
                            }
                        }

                        if (cor)
                        {
                            int bitcount = bits.Where(b => b).Count();
                            if (lowest > bitcount)
                            {
                                lowest = bitcount;
                            }
                        }
                    }
                    sum += lowest;
                }
                Console.WriteLine(sum);
            }

            //
            {

            }
        }
    }
}
