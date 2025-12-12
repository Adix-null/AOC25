// https://adventofcode.com/2025/day/11

namespace day_11
{
    public class Program
    {

        internal static void Main()
        {
            string[] z = File.ReadAllLines(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\input.txt");
            // Normal
            {
                List<string> ind = z.Select(l => l.Split(" ")[0][..^1]).ToList();
                List<Node> nodes = [];
                for (int i = 0; i < z.Length; i++)
                {
                    nodes.Add(new Node { val = ind[i], conns = z[i].Split(" ")[1..].ToList() });
                }
                
                Console.WriteLine(CountSpecial(nodes.First(n => n.val == "svr"), [], nodes));
            }
        }

        //dp black magic
        static Dictionary<(string, bool, bool), long> memo = [];
        static long CountSpecial(Node n, HashSet<string> visiting, List<Node> nodes)
        {
            bool h1 = n.val == "dac" || visiting.Contains("dac");
            bool h2 = n.val == "fft" || visiting.Contains("fft");

            var key = (n.val, h1, h2);
            if (memo.TryGetValue(key, out var v)) 
                return v;

            if (!visiting.Add(n.val)) 
                return 0;

            long sum = 0;

            foreach (string c in n.conns)
            {
                if (c == "out")
                {
                    if (h1 && h2) 
                        sum++;
                }
                else
                {
                    sum += CountSpecial(nodes.Find(x => x.val == c), visiting, nodes);
                }
            }

            visiting.Remove(n.val);
            memo[key] = sum;
            return sum;
        }


        public struct Node
        {
            public string val;
            public List<string> conns;
        }
    }
}
