var input = await File.ReadAllLinesAsync("input.txt");

var lines = input.Select(ParseLine).ToArray();

var cells = new Dictionary<(int x, int y), int>();

foreach (var line in lines)
{
    if (line.x1 != line.x2 &&
        line.y1 != line.y2)
    {
        // ignore diagonal lines

        continue;
    }

    if (line.x1 == line.x2)
    {
        // vertical line

        var from = Math.Min(line.y1, line.y2);

        var to = Math.Max(line.y1, line.y2);

        for (var y = from; y <= to; y++)
        {
            HitCell(cells, line.x1, y);
        }
    }
    else
    {
        // horizontal line

        var from = Math.Min(line.x1, line.x2);

        var to = Math.Max(line.x1, line.x2);

        for (var x = from; x <= to; x++)
        {
            HitCell(cells, x, line.y1);
        }
    }
}

var solution1 = cells.Values.Count(hits => hits > 1);

Console.WriteLine($"Day 5 - Puzzle 1: {solution1}");

static (int x1, int y1, int x2, int y2) ParseLine(
    string input)
{
    var parts = input
        .Split(
            new string[] { ",", " -> " },
            StringSplitOptions.None)
        .Select(int.Parse)
        .ToArray();

    return (parts[0], parts[1], parts[2], parts[3]);
}

static void HitCell(
    Dictionary<(int x, int y), int> @this,
    int x,
    int y)
{
    if (!@this.TryGetValue(
        (x, y),
        out var hits))
    {
        hits = 0;
    }

    @this[(x, y)] = hits + 1;
}
