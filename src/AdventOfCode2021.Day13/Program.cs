var input = await File.ReadAllLinesAsync("input.txt");

var (dots, folds) = ParseInput(input);

Fold(dots, folds[0]);

var solution1 = dots.Count;

Console.WriteLine($"Day 13 - Puzzle 1: {solution1}");

static (HashSet<(int x, int y)> dots, (int x, int y)[] folds) ParseInput(
    string[] input)
{
    var dots = new HashSet<(int x, int y)>();

    var i = 0;

    for (; i < input.Length; i++)
    {
        if (input[i] == string.Empty)
        {
            break;
        }

        dots.Add(ParseDot(input[i]));
    }

    var folds = input
        .Skip(i + 1)
        .Select(ParseFold)
        .ToArray();

    return (dots, folds);
}

static (int x, int y) ParseDot(
    string line)
{
    var dot = line
        .Split(',')
        .Select(int.Parse)
        .ToArray();

    return (dot[0], dot[1]);
}

static (int x, int y) ParseFold(
    string line)
{
    var fold = line.Split('=');

    var value = int.Parse(fold[1]);

    return
        fold[0][^1] == 'x' ?
        (value, 0) :
        (0, value);
}

static void Fold(
    HashSet<(int x, int y)> dots,
    (int x, int y) fold)
{
    foreach (var (x, y) in dots.ToArray())
    {
        if (fold.x > 0 &&
            x > fold.x)
        {
            // fold left

            dots.Add((2 * fold.x - x, y));
        }
        else if (fold.y > 0 &&
            y > fold.y)
        {
            // fold up

            dots.Add((x, 2 * fold.y - y));
        }
        else
        {
            // 'above' the fold

            continue;
        }

        dots.Remove((x, y));
    }
}