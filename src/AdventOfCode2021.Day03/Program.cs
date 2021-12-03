var lines = await File.ReadAllLinesAsync("input.txt");

int[] CountOneBits(string[] lines)
{
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

    return counts;
}

var counts = CountOneBits(lines);

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

var candidates = lines;

for (var i = 0; i < counts.Length; i++)
{
    counts = CountOneBits(candidates);

    var keep =
        counts[i] >= (candidates.Length + 1) / 2 ?
        '1' :
        '0';

    candidates = candidates
        .Where(line => line[i] == keep)
        .ToArray();

    if (candidates.Length == 1)
    {
        break;
    }
}

var oxygen = Convert.ToInt32(candidates[0], 2);

candidates = lines;

for (var i = 0; i < counts.Length; i++)
{
    counts = CountOneBits(candidates);

    var keep =
        counts[i] < (candidates.Length + 1) / 2 ?
        '1' :
        '0';

    candidates = candidates
        .Where(line => line[i] == keep)
        .ToArray();

    if (candidates.Length == 1)
    {
        break;
    }
}

var co2 = Convert.ToInt32(candidates[0], 2);

var solution2 = oxygen * co2;

Console.WriteLine($"Day 3 - Puzzle 2: {solution2}");
