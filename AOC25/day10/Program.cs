// https://adventofcode.com/2025/day/10

using System.Numerics;
using System.Reflection.Emit;

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
                List<List<List<int>>> joltageComboList = [];
                List<List<int>> joltageComboListSub = [];
                List<Fraction> smallest = [];

                int sum = 0;
                for (int i = 0; i < z.Length; i++)
                {
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

                    Fraction[,] matrix = new Fraction[joltages[i].Count, combos[i].Count + 1];

                    for (int j = 0; j < joltages[i].Count; j++)
                    {
                        for (int k = 0; k < combos[i].Count + 1; k++)
                        {
                            if (k == combos[i].Count)
                            {
                                matrix[j, k] = new (joltages[i][j]);
                            }
                            else
                            {
                                matrix[j, k] = new(combos[i][k].Contains(j) ? 1 : 0);
                            }
                        }
                    }
                    //printMatrix(matrix);

                    List<int> ConstrainedColumns = [], eliminatedRows = [];
                    for (int col = 0; col < matrix.GetLength(1) - 1; col++)
                    {
                        // Find pivot row
                        int pivotRow = -1;
                        for (int row = 0; row < matrix.GetLength(0); row++)
                        {
                            if (matrix[row, col] != new Fraction(0) && !eliminatedRows.Contains(row))
                            {
                                pivotRow = row;
                                eliminatedRows.Add(row);
                                ConstrainedColumns.Add(col);
                                break;
                            }
                        }

                        if (pivotRow == -1)
                            continue; //every non eliminated row is 0, move to next one

                        // Normalize pivot to be 1
                        Fraction pivotFactor = matrix[pivotRow, col];
                        for (int x = 0; x < matrix.GetLength(1); x++)
                        {
                            matrix[pivotRow, x] /= pivotFactor;
                        }

                        // Eliminate rows
                        for (int row = 0; row < matrix.GetLength(0); row++)
                        {
                            if (row == pivotRow) 
                                continue;

                            Fraction factor = matrix[row, col];
                            for (int x = 0; x < matrix.GetLength(1); x++)
                            {
                                matrix[row, x] -= factor * matrix[pivotRow, x];
                            }
                        }

                        //Console.WriteLine($"row: {pivotRow} col: {col}");
                        //printMatrix(matrix);
                    }
                    for (int y = 0; y < matrix.GetLength(1) - 1; y++)
                    {
                        //Console.WriteLine("Column " + y + ": " + (ConstrainedColumns.Contains(y) ? "C" : "F"));
                    }
                    for (int x = 0; x < matrix.GetLength(0); x++)
                    {
                        //Console.WriteLine("Row " + x + ": " + (eliminatedRows.Contains(x) ? "E" : "0"));
                    }
                    //printMatrix(matrix);
                    //Console.WriteLine("Gaussian elimination complete");

                    List<int> allColumns = Enumerable.Range(0, matrix.GetLength(1) - 1).ToList();
                    List<int> freeColumns = allColumns.Except(ConstrainedColumns).ToList();

                    List<int> attempts = Enumerable.Repeat(0, freeColumns.Count).ToList();
                    int attMax = joltages[i].Max();

                    bool end = false;
                    List<Fraction> buttonSums = [];
                    List<Fraction> prevSums = Enumerable.Repeat(new Fraction(-999), eliminatedRows.Count).ToList();
                    while (!end && freeColumns.Count != 0)
                    {
                        Fraction buttonSum = new(0);
                        for (int l = 0; l < attempts.Count; l++)
                        {
                            //Console.Write(attempts[l].ToString().PadLeft(4));
                            buttonSum += new Fraction(attempts[l]);
                        }
                        //Console.WriteLine();
                        bool badSum = false;
                        for (int k = 0; k < eliminatedRows.Count; k++)
                        {
                            Fraction cellSum = new(0);
                            for (int l = 0; l < freeColumns.Count; l++)
                            {
                                cellSum += matrix[eliminatedRows[k], freeColumns[l]] * new Fraction(-attempts[l]);
                            }
                            cellSum += matrix[eliminatedRows[k], matrix.GetLength(1) - 1];
                            buttonSum += cellSum;
                            //Console.WriteLine(cellSum);

                            if (cellSum < new Fraction(0) || cellSum.Den != 1)
                            {
                                badSum = true;
                                if (prevSums[k] > cellSum)
                                {
                                    //attempts[0] = attMax + 1;
                                    //break;
                                }
                            }
                            else
                            {
                                prevSums[k] = cellSum;
                            }
                        }
                        if (!badSum)
                        {
                            buttonSums.Add(buttonSum);
                        }
                        attempts[0]++;
                        for (int j = 0; j < attempts.Count - 1; j++)
                        {
                            if (attempts[j] > attMax)
                            {
                                attempts[j] = 0;
                                attempts[j + 1]++;
                                prevSums = Enumerable.Repeat(new Fraction(-999), eliminatedRows.Count).ToList();
                            }                            
                        }
                        if (attempts[^1] > attMax)
                        {
                            end = true;
                        }
                    }
                    if (freeColumns.Count == 0)
                    {
                        Fraction trivialsum = new(0);
                        for (int j = 0; j < matrix.GetLength(0); j++)
                        {
                            trivialsum += matrix[j, matrix.GetLength(1) - 1];
                        }
                        smallest.Add(trivialsum);
                    }
                    else
                    {
                        var tt = attempts;
                        smallest.Add(buttonSums.Min());
                    }
                    Console.WriteLine("Smallest sum = " + smallest[^1]);
                }
                Console.WriteLine(sum);
                Fraction final = new(0);
                foreach (var fr in smallest)
                {
                    final += fr;
                }
                Console.WriteLine(final.ToString());
            }

            void printMatrix(Fraction[,] matrix)
            {
                for (int y = 0; y < matrix.GetLength(0); y++)
                {
                    for (int x = 0; x < matrix.GetLength(1); x++)
                    {
                        Console.Write(matrix[y, x].ToString().PadLeft(8));
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            //
            {

            }

            
        }

        public readonly struct Fraction : IComparable<Fraction>
        {
            public long Num { get; }
            public long Den { get; }

            public Fraction(long num, long den = 1)
            {
                if (den == 0)
                    throw new DivideByZeroException();

                if (den < 0)
                {
                    num = -num;
                    den = -den;
                }

                long g = Gcd(Math.Abs(num), den);
                Num = num / g;
                Den = den / g;
            }

            private static long Gcd(long a, long b)
            {
                while (b != 0)
                {
                    long t = a % b;
                    a = b;
                    b = t;
                }
                return a;
            }

            public static Fraction operator +(Fraction a, Fraction b)
            {
                return new Fraction(a.Num * b.Den + b.Num * a.Den, a.Den * b.Den);
            }

            public static Fraction operator -(Fraction a, Fraction b)
            {
                return new Fraction(a.Num * b.Den - b.Num * a.Den, a.Den * b.Den);
            }

            public static Fraction operator *(Fraction a, Fraction b)
            {
                return new Fraction(a.Num * b.Num, a.Den * b.Den);
            }

            public static Fraction operator /(Fraction a, Fraction b)
            {
                if (b.Num == 0)
                    throw new DivideByZeroException();

                return new Fraction(a.Num * b.Den, a.Den * b.Num);
            }

            public static bool operator ==(Fraction a, Fraction b)
            {
                return a.Num == 0 && b.Num == 0 || a.Num == b.Num && a.Den == b.Den;
            }

            public static bool operator !=(Fraction a, Fraction b)
            {
                return !(a == b);
            }

            public static bool operator <(Fraction a, Fraction b)
            {
                return a.Num * b.Den < b.Num * a.Den;
            }

            public static bool operator >(Fraction a, Fraction b)
            {
                return a.Num * b.Den > b.Num * a.Den;
            }

            public static bool operator <=(Fraction a, Fraction b)
            {
                return a.Num * b.Den <= b.Num * a.Den;
            }

            public static bool operator >=(Fraction a, Fraction b)
            {
                return a.Num * b.Den >= b.Num * a.Den;
            }
            public override bool Equals(object? obj)
            {
                return obj is Fraction f && this == f;
            }
            public int CompareTo(Fraction other)
            {
                long lhs = Num * other.Den;
                long rhs = other.Num * Den;
                return lhs.CompareTo(rhs);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Num, Den);
            }

            public override string ToString()
            {
                return Den == 1 ? Num.ToString() : $"{Num}/{Den}";
            }
        }

    }
}
