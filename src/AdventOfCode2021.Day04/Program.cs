var lines = await File.ReadAllLinesAsync("input.txt");

var draws = lines[0].Split(',').Select(int.Parse).ToArray();

var boards = lines.Skip(2).Chunk(6).Select(Board.Parse).ToArray();

var (winner, draw) = Play(draws, boards);

var solution1 = winner.Numbers.Keys.Sum() * draw;

Console.WriteLine($"Day 4 - Puzzle 1: {solution1}");

static (Board, int) Play(
    int[] draws,
    Board[] boards)
{
    foreach (var draw in draws)
    {
        foreach (var board in boards)
        {
            if (board.Wins(draw))
            {
                return (board, draw);
            }
        }
    }

    throw new InvalidOperationException("no winner");
}

record Board(
    IDictionary<int, (int x, int y)> Numbers)
{
    public static Board Parse(
        string[] lines)
    {
        var numbers = new Dictionary<int, (int x, int y)>();

        for (var y = 0; y < 5; y++)
        {
            var row = lines[y].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            for (var x = 0; x < 5; x++)
            {
                numbers.Add(row[x], (x, y));
            }
        }

        return new(numbers);
    }

    #region Internal State

    private readonly int[] _columnHits = new int[5];

    private readonly int[] _rowHits = new int[5];

    #endregion

    public bool Wins(int draw)
    {
        if (!Numbers.TryGetValue(
            draw,
            out var number))
        {
            return false;
        }

        Numbers.Remove(draw); // only keep unmarked numbers

        _columnHits[number.x]++;

        _rowHits[number.y]++;

        return
            _columnHits[number.x] == 5 ||
            _rowHits[number.y] == 5;
    }
}