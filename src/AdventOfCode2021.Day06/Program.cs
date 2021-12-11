var input = await File.ReadAllTextAsync("input.txt");

var counts = input.Split(',').Select(int.Parse).GroupBy(age => age).ToDictionary(g => g.Key, g => g.Count());

var fish = Enumerable.Range(0, 9).Select(i => counts.TryGetValue(i, out var count) ? (long)count : 0L).ToArray();

Simulate(fish, 80);

var solution1 = fish.Sum();

Console.WriteLine($"Day 6 - Puzzle 1: {solution1}");

Simulate(fish, 256 - 80);

var solution2 = fish.Sum();

Console.WriteLine($"Day 6 - Puzzle 2: {solution2}");

static void Simulate(
    long[] fish,
    int days)
{
    for (var i = 0; i < days; i++)
    {
        var newCount = fish[0];

        for (var j = 1; j < 9; j++)
        {
            fish[j - 1] = fish[j];
        }

        fish[6] += newCount;

        fish[8] = newCount;
    }
}
