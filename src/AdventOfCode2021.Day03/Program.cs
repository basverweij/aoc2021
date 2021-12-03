var lines = await File.ReadAllLinesAsync("input.txt");

var counts = new int[lines[0].Length];

foreach (var line in lines)
{
    for (var i = 0; i < line.Length; i++)
    {
        if (line[i] == '1')
        {
            counts[i]++;
        }
    }
}

var gamma = 0;

for (var i = 0; i < counts.Length; i++)
{
    if (counts[i] == lines.Length / 2)
    {
        throw new InvalidOperationException("count is exactly half of the occurrences");
    }

    if (counts[i] > lines.Length / 2)
    {
        gamma |= 1 << (counts.Length - 1 - i);
    }
}

var epsilon = ((1 << counts.Length) - 1) ^ gamma;

var solution1 = gamma * epsilon;

Console.WriteLine($"Day 3 - Puzzle 1: {solution1}");
