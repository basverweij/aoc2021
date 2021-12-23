var input = await File.ReadAllLinesAsync("input.txt");

var graph = BuildGraph(input);

var solution1 = FindPaths(graph, new() { "start" });

Console.WriteLine($"Day 12 - Puzzle 1: {solution1}");

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
    List<string> path)
{
    var count = 0;

    foreach (var link in graph[path.Last()])
    {
        if (link == "end")
        {
            // found path to end

            count++;

            continue;
        }

        if ((link.ToLowerInvariant() == link) &&
            path.Contains(link))
        {
            // small caves can only be visited once

            continue;
        }

        count += FindPaths(graph, new(path) { link });
    }

    return count;
}
