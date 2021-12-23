var input = await File.ReadAllLinesAsync("input.txt");

var area = input.Select(s => s.Select(c => c - '0').ToArray()).ToArray();

var lowPoints = GetLowPoints(area);

var solution1 = lowPoints.Select(p => area[p.y][p.x] + 1).Sum();

Console.WriteLine($"Day 9 - Puzzle 1: {solution1}");

var solution2 = lowPoints
    .Select(p => GetBasinSize(area, p.x, p.y))
    .OrderByDescending(s => s)
    .Take(3)
    .Aggregate(1, (a, b) => a * b);

Console.WriteLine($"Day 9 - Puzzle 2: {solution2}");

static IEnumerable<(int x, int y)> GetLowPoints(
    int[][] area)
{
    for (var y = 0; y < area.Length; y++)
    {
        for (var x = 0; x < area[y].Length; x++)
        {
            if (IsLowPoint(area, x, y))
            {
                yield return (x, y);
            }
        }
    }
}

static bool IsLowPoint(
    int[][] area,
    int x,
    int y)
{
    var height = area[y][x];

    return
        (y == 0 || height < area[y - 1][x]) && // top
        (y == area.Length - 1 || height < area[y + 1][x]) && // bottom
        (x == 0 || height < area[y][x - 1]) && // left
        (x == area[y].Length - 1 || height < area[y][x + 1]); // right
}

static int GetBasinSize(
    int[][] area,
    int x,
    int y)
{
    var basin = new HashSet<(int x, int y)>();

    FillBasin(basin, area, x, y);

    return basin.Count;
}

static void FillBasin(
    HashSet<(int x, int y)> basin,
    int[][] area,
    int x,
    int y)
{
    var height = area[y][x];

    if (height == 9)
    {
        return;
    }

    if (basin.Contains((x, y)))
    {
        return;
    }

    basin.Add((x, y));

    if (y > 0 && height < area[y - 1][x])
    {
        FillBasin(basin, area, x, y - 1); // top
    }

    if (y < area.Length - 1 && height < area[y + 1][x])
    {
        FillBasin(basin, area, x, y + 1); // bottom
    }

    if (x > 0 && height < area[y][x - 1])
    {
        FillBasin(basin, area, x - 1, y); // left
    }

    if (x < area[y].Length - 1 && height < area[y][x + 1])
    {
        FillBasin(basin, area, x + 1, y); // right
    }
}
