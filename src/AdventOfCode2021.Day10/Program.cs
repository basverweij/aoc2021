var input = await File.ReadAllLinesAsync("input.txt");

var solution1 = input.Select(GetSyntaxErrorScore).Sum();

Console.WriteLine($"Day 10 - Puzzle 1: {solution1}");

static int GetSyntaxErrorScore(
    string line)
{
    var syntax = new Dictionary<char, char>
    {
        ['('] = ')',
        ['['] = ']',
        ['{'] = '}',
        ['<'] = '>',
    };

    var scores = new Dictionary<char, int>
    {
        [')'] = 3,
        [']'] = 57,
        ['}'] = 1197,
        ['>'] = 25137,
    };

    var chunks = new Stack<char>();

    foreach (var c in line)
    {
        if (syntax.TryGetValue(c, out var close))
        {
            chunks.Push(close);

            continue;
        }

        if (!chunks.Any() ||
            c != chunks.Pop())
        {
            return scores[c];
        }
    }

    return 0;
}