var lines = await File.ReadAllLinesAsync("input.txt");

var draws = lines[0].Split(',').Select(int.Parse).ToArray();

var boards = lines.Skip(2).Chunk(6).Select(Board.Parse).ToList();

var (winner, draw) = Play(draws, boards);

var solution1 = winner.Numbers.Keys.Sum() * draw;

Console.WriteLine($"Day 4 - Puzzle 1: {solution1}");

boards.Remove(winner);

var (last, finalDraw) = ContinueToLast(draws.SkipWhile(d => d != draw), boards);

var solution2 = last.Numbers.Keys.Sum() * finalDraw;

Console.WriteLine($"Day 4 - Puzzle 2: {solution2}");

static (Board, int) Play(
    IEnumerable<int> draws,
    IReadOnlyList<Board> boards)
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

static (Board, int) ContinueToLast(
    IEnumerable<int> draws,
    IList<Board> boards)
{
    foreach (var draw in draws)
    {
        foreach (var board in boards.ToArray()) // copy to allow removing boards
        {
            if (board.Wins(draw))
            {
                boards.Remove(board);

                if (!boards.Any())
                {
                    return (board, draw);
                }
            }
        }
    }

    throw new InvalidOperationException("no loser");
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