// https://adventofcode.com/2025/day/4

namespace day_4
{
    public class Program
    {
        internal static void Main()
        {
            string[] z = File.ReadAllLines(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\input.txt");
            // Normal
            {                
                int sum = 0;
                List<(int, int)> moveable = [];
                do
                {
                    moveable.Clear();
                    for (int i = 0; i < z.Length; i++)
                    {
                        for (int j = 0; j < z[i].Length; j++)
                        {
                            int neighbors = 0;
                            for (int x = -1; x <= 1; x++)
                            {
                                for (int y = -1; y <= 1; y++)
                                {
                                    if (x == 0 && y == 0)
                                        continue;
                                    if (i + y >= 0 && i + y < z.Length && j + x >= 0 && j + x < z[i].Length && z[i + y][j + x] == '@')
                                        neighbors++;
                                }
                            }
                            if (neighbors < 4 && z[i][j] == '@')
                            {
                                moveable.Add((i, j));
                                sum++;
                            }
                        }
                    }

                    foreach (var coords in moveable)
                    {
                        z[coords.Item1] = z[coords.Item1][..coords.Item2] + '.' + z[coords.Item1][(coords.Item2 + 1)..];
                    }

                } while(moveable.Count > 0);

                Console.WriteLine(sum);
            }

            // pure LINQed: 
            {
                
            }
        }
    }
}
