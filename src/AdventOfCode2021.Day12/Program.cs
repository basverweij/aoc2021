var input = await File.ReadAllLinesAsync("input.txt");

var graph = BuildGraph(input);

var solution1 = FindPaths(graph, new() { "start" });

Console.WriteLine($"Day 12 - Puzzle 1: {solution1}");

var solution2 = FindPaths(graph, new() { "start" }, 1);

Console.WriteLine($"Day 12 - Puzzle 2: {solution2}");

static Dictionary<string, HashSet<string>> BuildGraph(
    string[] input)
{
    var graph = new Dictionary<string, HashSet<string>>();

    foreach (var link in input)
    {
        var parts = link.Split('-');

        UpdateGraph(graph, parts[0], parts[1]);

        UpdateGraph(graph, parts[1], parts[0]);
    }

    return graph;
}

static void UpdateGraph(
    Dictionary<string, HashSet<string>> graph,
    string from,
    string to)
{
    if (graph.TryGetValue(
        from,
        out var links))
    {
        links.Add(to);
    }
    else
    {
        graph[from] = new() { to };
    }
}

static int FindPaths(
    Dictionary<string, HashSet<string>> graph,
    List<string> path,
    int allowedDuplicateSmallCaves = 0)
{
    var count = 0;

    foreach (var link in graph[path.Last()])
    {
        if (link == "start")
        {
            // not allowed to go back to the start cave

            continue;
        }

        if (link == "end")
        {
            // found path to end

            count++;

            continue;
        }

        if ((link.ToLowerInvariant() == link) &&
            path.Contains(link) &&
            DuplicateSmallCaves(path) >= allowedDuplicateSmallCaves)
        {
            // small caves can only be visited once, and
            // the path can contain up to allowedDuplicateSmallCaves
            // duplicates

            continue;
        }

        count += FindPaths(graph, new(path) { link }, allowedDuplicateSmallCaves);
    }

    return count;
}

static int DuplicateSmallCaves(
    IEnumerable<string> path)
{
    return path
        .Where(n => n.ToLowerInvariant() == n)
        .GroupBy(n => n)
        .Count(g => g.Count() > 1);
}
