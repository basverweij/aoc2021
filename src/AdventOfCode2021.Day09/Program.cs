var input = await File.ReadAllLinesAsync("input.txt");

var area = input.Select(s => s.Select(c => c - '0').ToArray()).ToArray();

var lowPoints = GetLowPoints(area);

var solution1 = lowPoints.Select(p => area[p.y][p.x] + 1).Sum();

Console.WriteLine($"Day 9 - Puzzle 1: {solution1}");

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