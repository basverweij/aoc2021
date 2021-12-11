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
    return Enumerable
        .Range(0, crabs.Max())
        .Min(i => crabs.Sum(c => cost(Math.Abs(c - i))));
}
