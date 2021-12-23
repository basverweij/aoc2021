var input = await File.ReadAllLinesAsync("input.txt");

var graph = BuildGraph(input);

var solution1 = FindPaths(graph);

Console.WriteLine($"Day 12 - Puzzle 1: {solution1}");

var solution2 = FindPaths(graph, 1);

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
    int allowedDuplicateSmallCaves = 0)
{
    return InternalFindPaths(
        graph,
        (nodes: new(), duplicateSmallCaves: new()),
        "start",
        allowedDuplicateSmallCaves);
}

static int InternalFindPaths(
    Dictionary<string, HashSet<string>> graph,
    (HashSet<string> nodes, HashSet<string> duplicateSmallCaves) path,
    string next,
    int allowedDuplicateSmallCaves = 0)
{
    var count = 0;

    foreach (var link in graph[next])
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

        if (char.IsLower(link[0]) &&
            path.nodes.Contains(link))
        {
            // small cave: check allowed duplicates

            if (path.duplicateSmallCaves.Count >= allowedDuplicateSmallCaves ||
                path.duplicateSmallCaves.Contains(link))
            {
                // small caves can only be visited once

                continue;
            }

            // allowed duplicate small cave

            count += InternalFindPaths(
                graph,
                (new(path.nodes) { next }, new(path.duplicateSmallCaves) { link }),
                link,
                allowedDuplicateSmallCaves);

            continue;
        }

        count += InternalFindPaths(
            graph,
            (new(path.nodes) { next }, new(path.duplicateSmallCaves)),
            link,
            allowedDuplicateSmallCaves);
    }

    return count;
}
