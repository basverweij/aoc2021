using Rules = System.Collections.Generic.IReadOnlyDictionary<(char left, char right), char>;

var input = await File.ReadAllLinesAsync("input.txt");

var template = input[0];

var rules = ParseRules(input.Skip(2));

for (var i = 0; i < 10; i++)
{
    template = InsertPairs(template, rules);
}

var counts = template.GroupBy(c => c);

var solution1 = counts.Max(c => c.Count()) - counts.Min(c => c.Count());

Console.WriteLine($"Day 14 - Puzzle 1: {solution1}");

static Rules ParseRules(
    IEnumerable<string> lines)
{
    return lines
        .Select(line => line.Split(" -> "))
        .ToDictionary(
            parts => (parts[0][0], parts[0][1]),
            parts => parts[1][0]);
}

static string InsertPairs(
    string template,
    Rules rules)
{
    var elements = new List<char>();

    for (var i = 0; i < template.Length - 1; i++)
    {
        var (left, right) = (template[i], template[i + 1]);

        elements.Add(left);

        elements.Add(rules[(left, right)]);
    }

    elements.Add(template[^1]);

    return new string(elements.ToArray());
}