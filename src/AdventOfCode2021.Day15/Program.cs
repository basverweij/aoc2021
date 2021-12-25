var input = await File.ReadAllLinesAsync("input.txt");

var levels = input.Select(line => line.Select(c => c - '0').ToArray()).ToArray();

var solution1 = FindShortestPath(levels);

Console.WriteLine($"Day 15 - Puzzle 1: {solution1}");

var expandedLevels = ExpandLevels(levels, 5, 5);

var solution2 = FindShortestPath(expandedLevels);

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

static int FindShortestPath(
    int[][] levels)
{
    var nodes = Enumerable
        .Range(0, levels.Length)
        .Select(y => Enumerable.Range(0, levels[y].Length).Select(x => new Node(x, y)).ToArray())
        .ToArray();

    var visited = Enumerable
        .Range(0, levels.Length)
        .Select(y => Enumerable.Range(0, levels[y].Length).Select(_ => false).ToArray())
        .ToArray();

    // starting position has zero distance

    nodes[0][0].Distance = 0;

    var sortedNodes = new SortedSet<Node>(
        nodes.SelectMany(n => n),
        new ByDistance());

    // run Dijkstra's shortest path algorithm

    while (true)
    {
        var current = sortedNodes.First();

        if (current.Y == levels.Length - 1 &&
            current.X == levels[current.Y].Length - 1)
        {
            // reached the end

            return current.Distance;
        }

        var unvisitedNeighbours = GetNeighbours(levels, current).Where(nb => !visited[nb.y][nb.x]);

        foreach (var (x, y) in unvisitedNeighbours)
        {
            var node = nodes[y][x];

            var distance = current.Distance + levels[y][x];

            if (distance < node.Distance)
            {
                sortedNodes.Remove(node);

                node.Distance = distance;

                sortedNodes.Add(node);
            }
        }

        // remove current node and mark as visited

        sortedNodes.Remove(current);

        visited[current.Y][current.X] = true;
    }
}

static IEnumerable<(int x, int y)> GetNeighbours(
    int[][] levels,
    Node current)
{
    if (current.Y > 0)
    {
        yield return (current.X, current.Y - 1); // top
    }

    if (current.Y < levels.Length - 1)
    {
        yield return (current.X, current.Y + 1); // bottom
    }

    if (current.X > 0)
    {
        yield return (current.X - 1, current.Y); // left
    }

    if (current.X < levels[current.Y].Length - 1)
    {
        yield return (current.X + 1, current.Y); // right
    }
}

sealed record Node(
    int X,
    int Y)
{
    public int Distance { get; set; } = int.MaxValue;
}

sealed class ByDistance :
    IComparer<Node>
{
    public int Compare(
        Node? x,
        Node? y)
    {
        if (x!.Distance != y!.Distance)
        {
            return x!.Distance - y!.Distance;
        }

        if (x.X != y.X)
        {
            return x.X - y.X;
        }

        return x.Y - y.Y;
    }
}
