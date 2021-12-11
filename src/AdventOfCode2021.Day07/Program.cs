var input = await File.ReadAllTextAsync("input.txt");

var crabs = input.Split(',').Select(int.Parse).ToArray();

var solution1 = GetMinFuel(crabs, i => i);

Console.WriteLine($"Day 7 - Puzzle 1: {solution1}");

var solution2 = GetMinFuel(crabs, i => (i * i + i) / 2);

Console.WriteLine($"Day 7 - Puzzle 2: {solution2}");

int GetMinFuel(
    int[] crabs,
    Func<int, int> cost)
{
    var max = crabs.Max();

    var minFuel = int.MaxValue;

    for (var i = 0; i < max; i++)
    {
        var fuel = crabs.Sum(c => cost(Math.Abs(c - i)));

        if (fuel < minFuel)
        {
            minFuel = fuel;
        }
    }

    return minFuel;
}