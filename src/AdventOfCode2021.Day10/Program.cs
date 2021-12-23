var input = await File.ReadAllLinesAsync("input.txt");

var scores = input.Select(GetLineScores).ToArray();

var solution1 = scores.Sum(s => s.syntaxError);

Console.WriteLine($"Day 10 - Puzzle 1: {solution1}");

var incomplete = scores.Where(s => s.syntaxError == 0).OrderBy(s => s.autoComplete).ToArray();

var solution2 = incomplete[incomplete.Length / 2].autoComplete;

Console.WriteLine($"Day 10 - Puzzle 2: {solution2}");

static (int syntaxError, long autoComplete) GetLineScores(
    string line)
{
    var syntax = new Dictionary<char, char>
    {
        ['('] = ')',
        ['['] = ']',
        ['{'] = '}',
        ['<'] = '>',
    };

    var syntaxErrorScores = new Dictionary<char, int>
    {
        [')'] = 3,
        [']'] = 57,
        ['}'] = 1197,
        ['>'] = 25137,
    };

    var autoCompleteScores = new Dictionary<char, long>
    {
        [')'] = 1,
        [']'] = 2,
        ['}'] = 3,
        ['>'] = 4,
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
            return (syntaxErrorScores[c], 0);
        }
    }

    var autoComplete = chunks.Aggregate(
        0L,
        (a, b) => a * 5 + autoCompleteScores[b]);

    return (0, autoComplete);
}