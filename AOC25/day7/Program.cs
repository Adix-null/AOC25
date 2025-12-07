// https://adventofcode.com/2025/day/7

namespace day_7
{
    public class Program
    {
        internal static void Main()
        {
            string[] z = File.ReadAllLines(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\input.txt");
            // Normal
            {
                int splits = 0;
                List<bool> beams = [];
                for(int i = 0; i < z.Length; i++)
                {
                    for(int j = 0; j < z[i].Length; j++)
                    {
                        if(i == 0)
                        {
                            beams.Add(z[i][j] == 'S');
                        }

                        if(z[i][j] == '^' && beams[j])
                        {
                            beams[j] = false;
                            beams[j - 1] = true;
                            beams[j + 1] = true;
                            splits++;
                        }
                    }
                }
                Console.WriteLine(splits);

                List<long> paths = Enumerable.Repeat(0L, z[0].Length).ToList();
                paths[z[0].IndexOf('S')] = 1;

                for (int i = 3; i < z.Length; i += 2)
                {
                    List<long> newPaths = [.. paths];
                    for (int j = 0; j < z[0].Length; j++)
                    {
                        if (j > 0 && z[i - 1][j - 1] == '^')
                            newPaths[j] += paths[j - 1];

                        if (j < z[0].Length - 1 && z[i - 1][j + 1] == '^')
                            newPaths[j] += paths[j + 1];

                        if (z[i - 1][j] == '^')
                            newPaths[j] = 0;
                    }
                    paths = newPaths;
                }
                Console.WriteLine(paths.Sum());
            }

            //golfed
            {
                Console.WriteLine(Enumerable.Range(3, z.Length - 3)
                    .Where(i => i % 2 == 1)
                    .Aggregate(Enumerable.Repeat(0L, z[0].Length).Select((x, idx) => x = z[0].IndexOf('S') == idx ? 1 : 0).ToList(), (acc, i) =>
                        acc.Select((c, j) => z[i - 1][Math.Max(0, j - 1)] == '^' & z[i - 1][Math.Min(z[0].Length - 1, j + 1)] == '^' ? acc[j] + acc[Math.Max(0, j - 1)] + acc[j + 1] :
                                   z[i - 1][Math.Max(0, j - 1)] == '^' & z[i - 1][Math.Min(z[0].Length - 1, j + 1)] != '^' ? acc[j] + acc[Math.Max(0, j - 1)] :
                                   z[i - 1][Math.Max(0, j - 1)] != '^' & z[i - 1][Math.Min(z[0].Length - 1, j + 1)] == '^' ? acc[j] + acc[Math.Min(z[0].Length - 1, j + 1)] :
                                   z[i - 1][j] == '^' ? 0 :
                                   acc[j]).ToList()
                    ).Sum());
                //Console.WriteLine(Enumerable.Range(3,z.Length-3).Where(i=>i%2==1).Aggregate(Enumerable.Repeat(0L,z[0].Length).Select((x,idx)=>x=z[0].IndexOf('S')==idx?1:0).ToList(),(acc,i)=>acc.Select((c,j)=>z[i-1][Math.Max(0,j-1)]=='^'&z[i-1][Math.Min(z[0].Length-1,j+1)]=='^'?acc[j]+acc[Math.Max(0,j-1)]+acc[j+1]:z[i-1][Math.Max(0,j-1)]=='^'&z[i-1][Math.Min(z[0].Length-1,j+1)]!='^'?acc[j]+acc[Math.Max(0,j-1)]:z[i-1][Math.Max(0,j-1)]!='^'&z[i-1][Math.Min(z[0].Length-1,j+1)]=='^'?acc[j]+acc[Math.Min(z[0].Length-1,j+1)]:z[i-1][j]=='^'?0:acc[j]).ToList()).Sum());
            }
        }
    }
}
