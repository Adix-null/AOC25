// https://adventofcode.com/2025/day/10

using System.Numerics;

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
                    printMatrix(matrix);

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
                        Console.WriteLine("Column " + y + ": " + (ConstrainedColumns.Contains(y) ? "C" : "F"));
                    }
                    for (int x = 0; x < matrix.GetLength(0); x++)
                    {
                        Console.WriteLine("Row " + x + ": " + (eliminatedRows.Contains(x) ? "E" : "0"));
                    }
                    printMatrix(matrix);
                    Console.WriteLine("Gaussian elimination complete");

                    List<int> allColumns = Enumerable.Range(0, matrix.GetLength(1) - 1).ToList();
                    List<int> freeColumns = allColumns.Except(ConstrainedColumns).ToList();



                    //int numFree = freeColumns.Count;
                    //var solutions = new List<Dictionary<int, long>>(); // store integer solutions

                    //void Enumerate(int freeIndex, Dictionary<int, long> current)
                    //{
                    //    if (freeIndex == numFree)
                    //    {
                    //        // All free variables assigned, compute constrained variables
                    //        var solution = new Dictionary<int, long>(current);

                    //        foreach (int row in eliminatedRows)
                    //        {
                    //            // pivot column
                    //            int pivotCol = ConstrainedColumns[eliminatedRows.IndexOf(row)];

                    //            Fraction rhs = matrix[row, matrix.GetLength(1) - 1];
                    //            Fraction sum = new Fraction(0);

                    //            for (int c = 0; c < matrix.GetLength(1) - 1; c++)
                    //            {
                    //                if (c == pivotCol) continue;
                    //                if (freeColumns.Contains(c))
                    //                    sum += matrix[row, c] * new Fraction(current[c]);
                    //                else
                    //                    sum += matrix[row, c] * new Fraction(0); // should be 0
                    //            }

                    //            Fraction val = rhs - sum;

                    //            // Check non-negative integer
                    //            if (val < new Fraction(0) || val.Den != 1)
                    //                return; // invalid solution

                    //            solution[pivotCol] = val.Num;
                    //        }

                    //        solutions.Add(solution);
                    //        return;
                    //    }

                    //    int freeCol = freeColumns[freeIndex];

                    //    // Compute bounds for this free variable from all rows
                    //    Fraction lower = new Fraction(0);
                    //    Fraction upper = new Fraction(long.MaxValue); // start large

                    //    foreach (int row in eliminatedRows)
                    //    {
                    //        Fraction rhs = matrix[row, matrix.GetLength(1) - 1];
                    //        Fraction coeff = matrix[row, freeCol];
                    //        if (coeff == new Fraction(0)) continue;

                    //        Fraction sum = new Fraction(0);
                    //        for (int c = 0; c < matrix.GetLength(1) - 1; c++)
                    //        {
                    //            if (c == freeCol) continue;
                    //            if (freeColumns.Contains(c) && current.ContainsKey(c))
                    //                sum += matrix[row, c] * new Fraction(current[c]);
                    //            // else unknown free variable not yet assigned, treat as 0 for upper bound
                    //        }

                    //        Fraction bound = (rhs - sum) / coeff;

                    //        if (coeff > new Fraction(0))
                    //        {
                    //            if (bound < upper) upper = bound;
                    //        }
                    //        else
                    //        {
                    //            if (bound > lower) lower = bound;
                    //        }
                    //    }

                    //    // Enumerate all integer values within [lower, upper]
                    //    long start = (long)Math.Ceiling((double)lower.Num / lower.Den);
                    //    long end = (long)Math.Floor((double)upper.Num / upper.Den);

                    //    for (long val = start; val <= end; val++)
                    //    {
                    //        current[freeCol] = val;
                    //        Enumerate(freeIndex + 1, current);
                    //        current.Remove(freeCol);
                    //    }
                    //}

                    //// Call with empty assignment
                    //Enumerate(0, []);
                    //var maxValues = new Dictionary<int, long>();

                    //foreach (int freeCol in freeColumns)
                    //{
                    //    long maxVal = solutions.Max(sol => sol[freeCol]);
                    //    maxValues[freeCol] = maxVal;
                    //}

                    //// 'solutions' now contains all valid integer solutions
                    //foreach (var kv in maxValues)
                    //{
                    //    Console.WriteLine($"x[{kv.Key}] max = {kv.Value}");
                    //}
                    for (int a1 = 0; a1 < 500; a1++)
                    {
                        for (int a2 = 0; a2 < 500; a2++)
                        {
                            Fraction buttonSum = new(0);
                            for (int k = 0; k < eliminatedRows.Count; k++)
                            {
                                for (int l = 0; l < freeColumns.Count; l++)
                                {
                                    buttonSum += (matrix[k, l] * new Fraction(-a1)) + matrix[k, matrix.GetLength(1) - 1];

                                }
                            }
                        }
                    }

                }
                Console.WriteLine(sum);


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

        public readonly struct Fraction
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
