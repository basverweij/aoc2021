var input = await File.ReadAllTextAsync("input.txt");

var crabs = input.Split(',').Select(int.Parse).ToArray();

var max = crabs.Max();

var solution1 = int.MaxValue;

for (var i = 0; i < max; i++)
{
    var fuel = crabs.Sum(c => Math.Abs(c - i));

    if (fuel < solution1)
    {
        solution1 = fuel;
    }
}

Console.WriteLine($"Day 7 - Puzzle 1: {solution1}");
