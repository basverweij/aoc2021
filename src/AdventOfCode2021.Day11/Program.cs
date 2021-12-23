var input = await File.ReadAllLinesAsync("input.txt");

var octopuses = input.Select(row => row.Select(c => c - '0').ToArray()).ToArray();

var solution1 = Enumerable.Range(1, 100).Sum(_ => Step(octopuses));

Console.WriteLine($"Day 11 - Puzzle 1: {solution1}");

octopuses = input.Select(row => row.Select(c => c - '0').ToArray()).ToArray();

var count = octopuses.Length * octopuses[0].Length;

var solution2 = Enumerable.Range(1, int.MaxValue).First(_ => Step(octopuses) == count);

Console.WriteLine($"Day 11 - Puzzle 2: {solution2}");

static int Step(
    int[][] octopuses)
{
    var flashes = new HashSet<(int x, int y)>();

    for (var y = 0; y < octopuses.Length; y++)
    {
        for (var x = 0; x < octopuses[y].Length; x++)
        {
            IncreaseEnergy(octopuses, x, y, flashes);
        }
    }

    foreach (var (x, y) in flashes)
    {
        octopuses[y][x] = 0;
    }

    return flashes.Count();
}

static void IncreaseEnergy(
    int[][] octopuses,
    int x,
    int y,
    HashSet<(int x, int y)> flashes)
{
    if (flashes.Contains((x, y)))
    {
        // octopus can only flash at most once per step

        return;
    }

    if (++octopuses[y][x] > 9)
    {
        flashes.Add((x, y));

        // increase adjacent octopuses

        if (x > 0)
        {
            if (y > 0)
            {
                IncreaseEnergy(octopuses, x - 1, y - 1, flashes);
            }

            IncreaseEnergy(octopuses, x - 1, y, flashes);

            if (y < octopuses.Length - 1)
            {
                IncreaseEnergy(octopuses, x - 1, y + 1, flashes);
            }
        }

        if (y > 0)
        {
            IncreaseEnergy(octopuses, x, y - 1, flashes);
        }

        if (y < octopuses.Length - 1)
        {
            IncreaseEnergy(octopuses, x, y + 1, flashes);
        }

        if (x < octopuses[y].Length - 1)
        {
            if (y > 0)
            {
                IncreaseEnergy(octopuses, x + 1, y - 1, flashes);
            }

            IncreaseEnergy(octopuses, x + 1, y, flashes);

            if (y < octopuses.Length - 1)
            {
                IncreaseEnergy(octopuses, x + 1, y + 1, flashes);
            }
        }
    }
}