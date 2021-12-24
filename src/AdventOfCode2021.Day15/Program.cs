var input = await File.ReadAllLinesAsync("input.txt");

//var input = new string[]
//{
//    "1163751742",
//    "1381373672",
//    "2136511328",
//    "3694931569",
//    "7463417111",
//    "1319128137",
//    "1359912421",
//    "3125421639",
//    "1293138521",
//    "2311944581",
//};

var levels = input.Select(line => line.Select(c => c - '0').ToArray()).ToArray();

var solution1 = FindLowestTotalRiskPath(levels).TotalRisk;

Console.WriteLine($"Day 15 - Puzzle 1: {solution1}");

var expandedLevels = ExpandLevels(levels, 5, 5);

var solution2 = FindLowestTotalRiskPath(expandedLevels).TotalRisk;

Console.WriteLine($"Day 15 - Puzzle 2: {solution2}");

static int[][] ExpandLevels(
    int[][] levels,
    int expandX,
    int expandY)
{
    var expandedLevels = new int[levels.Length * expandY][];

    for (var y = 0; y < levels.Length; y++)
    {
        for (var yy = 0; yy < expandY; yy++)
        {
            expandedLevels[yy * levels.Length + y] = new int[levels[y].Length * expandX];

            for (var x = 0; x < levels[y].Length; x++)
            {
                for (var xx = 0; xx < expandX; xx++)
                {
                    expandedLevels[yy * levels.Length + y][xx * levels[y].Length + x] = (levels[y][x] - 1 + yy + xx) % 9 + 1;
                }
            }
        }
    }

    return expandedLevels;
}

static Path FindLowestTotalRiskPath(
    int[][] levels)
{
    var totalRisks = Enumerable
        .Range(0, levels.Length)
        .Select(y => Enumerable.Range(0, levels[y].Length).Select(_ => int.MaxValue).ToArray())
        .ToArray();

    // starting position has zero total risk

    totalRisks[0][0] = 0;

    var queue = new Queue<Path>();

    queue.Enqueue(new Path((0, 0), 0));

    return InternalFindLowestTotalRiskPath(
        levels,
        totalRisks,
        queue);
}

static Path InternalFindLowestTotalRiskPath(
    int[][] levels,
    int[][] totalRisks,
    Queue<Path> partialPaths)
{
    var path = new Path((0, 0), int.MaxValue);

    while (partialPaths.TryDequeue(out var partialPath))
    {
        if (partialPath.Last.y == levels.Length - 1 &&
            partialPath.Last.x == levels[partialPath.Last.y].Length - 1 &&
            partialPath.TotalRisk < path.TotalRisk)
        {
            // reached the end w/ a lower total risk path

            path = partialPath;

            continue;
        }

        var candidates = GetCandidates(levels, partialPath.Last);

        var nexts = GetLowerTotalRiskCandidates(levels, totalRisks, partialPath.TotalRisk, candidates);

        foreach (var (x, y, totalRisk) in nexts)
        {
            var nextPath = new Path(
                (x, y),
                totalRisk);

            partialPaths.Enqueue(nextPath);
        }
    }

    return path;
}

static IEnumerable<(int x, int y)> GetCandidates(
    int[][] levels,
    (int x, int y) last)
{
    if (last.y > 0)
    {
        yield return (last.x, last.y - 1); // top
    }

    if (last.y < levels.Length - 1)
    {
        yield return (last.x, last.y + 1); // bottom
    }

    if (last.x > 0)
    {
        yield return (last.x - 1, last.y); // left
    }

    if (last.x < levels[last.y].Length - 1)
    {
        yield return (last.x + 1, last.y); // right
    }
}

static IEnumerable<(int x, int y, int totalRisk)> GetLowerTotalRiskCandidates(
    int[][] levels,
    int[][] totalRisks,
    int totalRisk,
    IEnumerable<(int x, int y)> candidates)
{
    foreach (var (x, y) in candidates)
    {
        var candidateTotalRisk = totalRisk + levels[y][x];

        if (totalRisks[y][x] <= candidateTotalRisk)
        {
            continue;
        }

        totalRisks[y][x] = candidateTotalRisk;

        yield return (x, y, candidateTotalRisk);
    }
}

sealed record Path(
    (int x, int y) Last,
    int TotalRisk);
