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
