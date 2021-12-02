var lines = await File.ReadAllLinesAsync("input.txt");

var measurements = lines.Select(int.Parse).ToArray();

var solution1 = 0;

for (var i = 1; i < measurements.Length; i++)
{
    if (measurements[i] > measurements[i - 1])
    {
        solution1++;
    }
}

Console.WriteLine($"Day 1 - Puzzle 1: {solution1}");

var solution2 = 0;

for (var i = 1; i < measurements.Length - 2; i++)
{
    if (measurements[i..(i + 3)].Sum() > measurements[(i - 1)..(i + 2)].Sum())
    {
        solution2++;
    }
}

Console.WriteLine($"Day 1 - Puzzle 2: {solution2}");
