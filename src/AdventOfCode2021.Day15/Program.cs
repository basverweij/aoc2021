var input = await File.ReadAllLinesAsync("input.txt");

var levels = input.Select(line => line.Select(c => c - '0').ToArray()).ToArray();

var path = FindLowestTotalRiskPath(levels);

var solution1 = TotalRisk(levels, path);

Console.WriteLine($"Day 15 - Puzzle 1: {solution1}");

static (int x, int y)[] FindLowestTotalRiskPath(
    int[][] levels)
{
    var totalRisks = Enumerable
        .Range(0, levels.Length)
        .Select(y => Enumerable.Range(0, levels[y].Length).Select(_ => int.MaxValue).ToArray())
        .ToArray();

    // starting position has zero total risk

    totalRisks[0][0] = 0;

    return InternalFindLowestTotalRiskPath(
        levels,
        totalRisks,
        new Queue<(int x, int y)[]>(new[] { new[] { (0, 0) } }));
}

static (int x, int y)[] InternalFindLowestTotalRiskPath(
    int[][] levels,
    int[][] totalRisks,
    Queue<(int x, int y)[]> partialPaths)
{
    var lowestTotalRisk = int.MaxValue;

    var path = Array.Empty<(int x, int y)>();

    while (partialPaths.TryDequeue(out var partialPath))
    {
        var totalRisk = TotalRisk(levels, partialPath);

        if (totalRisk < lowestTotalRisk &&
            partialPath[^1].y == levels.Length - 1 &&
            partialPath[^1].x == levels[partialPath[^1].y].Length - 1)
        {
            // reached the end, check if path has lower cost

            lowestTotalRisk = totalRisk;

            path = partialPath;

            continue;
        }

        var candidates = GetCandidates(levels, partialPath[^1]);

        var nexts = candidates.Where(c => IsLowerTotalRiskCandidate(levels, totalRisks, totalRisk, c));

        foreach (var next in nexts)
        {
            var nextPath = partialPath.Concat(new[] { next }).ToArray();

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

static bool IsLowerTotalRiskCandidate(
    int[][] levels,
    int[][] totalRisks,
    int totalRisk,
    (int x, int y) candidate)
{
    var candidateTotalRisk = totalRisk + levels[candidate.y][candidate.x];

    if (totalRisks[candidate.y][candidate.x] <= candidateTotalRisk)
    {
        return false;
    }

    totalRisks[candidate.y][candidate.x] = candidateTotalRisk;

    return true;
}

static int TotalRisk(
    int[][] levels,
    IEnumerable<(int x, int y)> path)
{
    return path.Skip(1).Sum(p => levels[p.y][p.x]);
}
