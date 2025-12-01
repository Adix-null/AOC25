string baseDir = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName + "\\";
string[] z = File.ReadAllLines(baseDir + "input.txt");

/// https://adventofcode.com/2025/day/1https://adventofcode.com/2025/day/1
/// Normal
{
    int dial = 50;
    int count = 0;
    foreach (string line in z)
    {
        string cleanLine = line.Replace("R", "").Replace("L", "");
        int k = int.Parse(cleanLine);
        int prev = dial;
        if (line[0] == 'L')
        {
            dial = ((dial - k) + 100 * k) % 100;
            if ((prev <= dial && prev != 0) || dial == 0)
                count++;
        }
        if (line[0] == 'R')
        {
            dial = (dial + k) % 100;
            if (prev >= dial || dial == 0)
                count++;
        }
        count += k / 100;
    }
    Console.WriteLine(count);
}

///Golfed char count: 159
{ 
int a=50,c=0,h=100,k,p;foreach(var l in z){k=int.Parse(l[1..]);p=a;a=(l[0]<77?a-k+h*k:a+k)%h;c+=l[0]<77?(p<a&p>0|a<1?1:0):p>a?1:0;c+=k/h;}Console.WriteLine(c);
}