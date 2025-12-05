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

                } while (moveable.Count > 0);

                Console.WriteLine(sum);
            }

            // pure LINQed: 
            {
                z = File.ReadAllLines(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\input.txt");

                Console.WriteLine(Enumerable.Range(0, 100)
                .Aggregate(
                    (sum: 0, z),
                    (acc, _) => Enumerable.Range(0, acc.z.Length)
                        .SelectMany(i => Enumerable.Range(0, acc.z[i].Length)
                            .Where(j => Enumerable.Range(-1, 3)
                                .SelectMany(x => Enumerable.Range(-1, 3)
                                    .Where(y => x != 0 || y != 0)
                                    .Select(y => (x, y)))
                                .Where(t =>
                                    i + t.y >= 0 && i + t.y < acc.z.Length &&
                                    j + t.x >= 0 && j + t.x < acc.z[i + t.y].Length &&
                                    acc.z[i + t.y][j + t.x] == '@')
                                .Count() < 4 && acc.z[i][j] == '@')
                            .Select(j => (i, j))
                        )
                        .ToList().Aggregate(
                        acc,
                        (state, coords) => (
                            sum: state.sum + 1,
                            z: state.z.Select((row, i) =>
                                i == coords.i
                                    ? row[..coords.j] + '.' + row[(coords.j + 1)..]
                                    : row
                            ).ToArray()
                        )
                    )
                ).sum);
                //Console.WriteLine(Enumerable.Range(0,100).Aggregate((sum:0,z),(acc,_)=>Enumerable.Range(0,acc.z.Length).SelectMany(i=>Enumerable.Range(0,acc.z[i].Length).Where(j=>Enumerable.Range(-1,3).SelectMany(x=>Enumerable.Range(-1,3).Where(y=>x!=0||y!=0).Select(y=>(x,y))).Where(t=>i+t.y>=0&&i+t.y<acc.z.Length&&j+t.x>=0&&j+t.x<acc.z[i+t.y].Length&&acc.z[i+t.y][j+t.x]=='@').Count()<4&&acc.z[i][j]=='@').Select(j=>(i,j))).ToList().Aggregate(acc,(state,coords)=>(sum:state.sum+1,z:state.z.Select((row,i)=>i==coords.i?row[..coords.j]+'.'+row[(coords.j+1)..]:row).ToArray()))).sum);
            }
        }
    }
}
