using Rules = System.Collections.Generic.IReadOnlyDictionary<(char left, char right), char>;

var input = await File.ReadAllLinesAsync("input.txt");

var template = ParseTemplate(input[0]);

var rules = ParseRules(input.Skip(2));

for (var i = 0; i < 10; i++)
{
    template = InsertPairs(template, rules);
}

var solution1 = GetSolution(template, input[0][^1]);

Console.WriteLine($"Day 14 - Puzzle 1: {solution1}");

for (var i = 0; i < 30; i++)
{
    template = InsertPairs(template, rules);
}

var solution2 = GetSolution(template, input[0][^1]);

Console.WriteLine($"Day 14 - Puzzle 2: {solution2}");

static IReadOnlyDictionary<(char left, char right), long> ParseTemplate(
    string template)
{
    return Enumerable
        .Range(0, template.Length - 1)
        .Select(i => (template[i], template[i + 1]))
        .GroupBy(p => p)
        .ToDictionary(p => p.Key, p => p.LongCount());
}

static Rules ParseRules(
    IEnumerable<string> lines)
{
    return lines
        .Select(line => line.Split(" -> "))
        .ToDictionary(
            parts => (parts[0][0], parts[0][1]),
            parts => parts[1][0]);
}

static IReadOnlyDictionary<(char left, char right), long> InsertPairs(
    IReadOnlyDictionary<(char left, char right), long> template,
    Rules rules)
{
    return template
        .SelectMany(kvp => InsertPair(kvp, rules))
        .GroupBy(kvp => kvp.Key)
        .ToDictionary(g => g.Key, g => g.Sum(kvp => kvp.Value));
}

static IEnumerable<KeyValuePair<(char left, char right), long>> InsertPair(
    KeyValuePair<(char left, char right), long> pair,
    Rules rules)
{
    var insert = rules[pair.Key];

    yield return new((pair.Key.left, insert), pair.Value);

    yield return new((insert, pair.Key.right), pair.Value);
}

static long GetSolution(
    IReadOnlyDictionary<(char left, char right), long> template,
    char last)
{
    // only count 'lefts' as the 'rights' will be the same, except for the last element

    var counts = template
        .GroupBy(kvp => kvp.Key.left)
        .ToDictionary(g => g.Key, g => g.Sum(kvp => kvp.Value));

    // add 1 additional count for the right-most element as this is not included in the left counts

    if (!counts.TryGetValue(
        last,
        out var count))
    {
        count = 0;
    }

    counts[last] = count + 1;

    return counts.Values.Max() - counts.Values.Min();
}
