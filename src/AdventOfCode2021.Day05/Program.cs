var input = await File.ReadAllLinesAsync("input.txt");

var lines = input.Select(ParseLine).ToArray();

var cells1 = new Dictionary<(int x, int y), int>();

var cells2 = new Dictionary<(int x, int y), int>();

foreach (var line in lines)
{
    if (line.x1 == line.x2)
    {
        // vertical line

        var from = Math.Min(line.y1, line.y2);

        var to = Math.Max(line.y1, line.y2);

        for (var y = from; y <= to; y++)
        {
            HitCell(cells1, line.x1, y);

            HitCell(cells2, line.x1, y);
        }
    }
    else if (line.y1 == line.y2)
    {
        // horizontal line

        var from = Math.Min(line.x1, line.x2);

        var to = Math.Max(line.x1, line.x2);

        for (var x = from; x <= to; x++)
        {
            HitCell(cells1, x, line.y1);

            HitCell(cells2, x, line.y1);
        }
    }
    else
    {
        // diagonal line

        var length = Math.Abs(line.x1 - line.x2) + 1;

        var deltaX = line.x1 < line.x2 ? 1 : -1;

        var deltaY = line.y1 < line.y2 ? 1 : -1;

        for (var i = 0; i < length; i++)
        {
            HitCell(cells2, line.x1 + i * deltaX, line.y1 + i * deltaY);
        }
    }
}

var solution1 = cells1.Values.Count(hits => hits > 1);

Console.WriteLine($"Day 5 - Puzzle 1: {solution1}");

var solution2 = cells2.Values.Count(hits => hits > 1);

Console.WriteLine($"Day 5 - Puzzle 2: {solution2}");

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
