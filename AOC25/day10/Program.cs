// https://adventofcode.com/2025/day/10

namespace day_10
{
    public class Program
    {
        internal static void Main()
        {
            string[] z = File.ReadAllLines(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\test.txt");
            // Normal
            {
                List<List<int>> goals = z.Select(l => l.Split(" ")[0][1..^1].Select(c => c == '#' ? 1 : 0).ToList()).ToList();
                List<List<int>> joltages = z.Select(l => l.Split(" ")[^1][1..^1].Split(",").Select(int.Parse).ToList()).ToList();
                List<List<List<int>>> combos = z.Select(l => l.Split(" ")[1..^1].Select(n => n[1..^1].Split(",").Select(int.Parse).ToList()).ToList()).ToList();
                List<List<List<int>>> joltageComboList = [];
                List<List<int>> joltageComboListSub = [];
                List<int> sequences = [];

                int sum = 0;
                for (int i = 0; i < z.Length; i++)
                {
                    sequences = [];
                    tryNewCombo(i);
                    joltageComboList.Add(joltageComboListSub);

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

                //dp
                void tryNewCombo(int i)
                {
                    //base case
                    if (sequences.Count == combos[i].Count)
                    {
                        if(isJoltageCorrect(sequences, i) == 0)
                        {
                            joltageComboListSub.Add(sequences);
                        }
                        sequences.RemoveAt(sequences.Count - 1);
                    }
                    else
                    {
                        int maxJolt = 0;
                        for (int j = 0; j < combos[i][sequences.Count].Count; j++)
                        {
                            if (joltages[i][combos[i][sequences.Count][j]] > maxJolt)
                                maxJolt = joltages[i][combos[i][sequences.Count][j]];
                        }
                        bool ovf = false;
                        for (int j = 0; j < maxJolt && !ovf; j++)
                        {
                            sequences.Add(j);
                            if (isJoltageCorrect(sequences, i) != 1)
                            {
                                tryNewCombo(i);
                            }
                            else
                            {
                                sequences.RemoveAt(sequences.Count - 1);
                            }
                        }
                    }
                }
                int isJoltageCorrect(List<int> sequences, int i)
                {
                    List<int> joltList = Enumerable.Range(0, joltages[i].Count).ToList();
                    for (int j = 0; j < sequences.Count; j++)
                    {
                        for (int k = 0; k < combos[i][j].Count; k++)
                        {
                            joltList[combos[i][j][k]] += sequences[j];
                            if (joltList[combos[i][j][k]] > joltages[i][k])
                                return 1;
                        }
                    }
                    for (int j = 0; j < joltages[i].Count; j++)
                    {
                        if (joltList[j] != joltages[i][j])
                            return -1;
                    }
                    return 0;
                }
            }

            //
            {

            }

            
        }
    }
}
