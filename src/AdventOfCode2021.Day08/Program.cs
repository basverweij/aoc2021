var input = await File.ReadAllLinesAsync("input.txt");

var displays = input.Select(ParseDisplay).ToArray();

var solution1 = displays.Sum(display => display.outputDigits.Count(digit => digit.Length is 2 or 3 or 4 or 7));

Console.WriteLine($"Day 8 - Puzzle 1: {solution1}");

var solution2 = displays.Sum(display => ProcessDisplay(display.signalPatterns, display.outputDigits));

Console.WriteLine($"Day 8 - Puzzle 2: {solution2}");

static (string[] signalPatterns, string[] outputDigits) ParseDisplay(
    string input)
{
    var parts = input.Split(" | ");

    var signalPatterns = parts[0].Split(' ').ToArray();

    var outputDigits = parts[1].Split(' ').ToArray();

    return (signalPatterns, outputDigits);
}

static int ProcessDisplay(
    string[] signalPatterns,
    string[] outputDigits)
{
    var mapping = BuildMapping(signalPatterns);

    var value = 0;

    var multiplier = 1;

    for (var i = 0; i < outputDigits.Length; i++)
    {
        value += MapDigit(
            mapping,
            outputDigits[outputDigits.Length - i - 1]) * multiplier;

        multiplier *= 10;
    }

    return value;
}

static Dictionary<char, char> BuildMapping(string[] signalPatterns)
{
    var a = signalPatterns
        .Single(p => p.Length == 3)
        .Except(signalPatterns.Single(p => p.Length == 2))
        .Single();

    var fives = signalPatterns.Where(p => p.Length == 5).ToArray();

    var horizontals = fives[0]
        .Intersect(fives[1])
        .Intersect(fives[2])
        .ToArray();

    var b = signalPatterns
        .Single(p => p.Length == 4)
        .Except(signalPatterns.Single(p => p.Length == 2))
        .Except(horizontals)
        .Single();

    var twoAndThree = fives.Where(p => !p.Contains(b)).ToArray();

    var c = twoAndThree[0]
        .Intersect(twoAndThree[1])
        .Except(horizontals)
        .Single();

    var f = signalPatterns
        .Single(p => p.Length == 2)
        .Single(x => x != c);

    var d = signalPatterns
        .Single(p => p.Length == 4)
        .Single(x => x != b && x != c && x != f);

    var e = fives[0]
        .Union(fives[1])
        .Union(fives[2])
        .Except(horizontals)
        .Single(x => x != b && x != c && x != f);

    var g = horizontals.Single(x => x != a && x != d);

    return new Dictionary<char, char>
    {
        [a] = 'a',
        [b] = 'b',
        [c] = 'c',
        [d] = 'd',
        [e] = 'e',
        [f] = 'f',
        [g] = 'g',
    };
}

static int MapDigit(
    IReadOnlyDictionary<char, char> mapping,
    string outputDigit)
{
    var digits = new Dictionary<string, int>()
    {
        { "abcefg", 0 },
        { "cf", 1 },
        { "acdeg", 2 },
        { "acdfg", 3 },
        { "bcdf", 4 },
        { "abdfg", 5 },
        { "abdefg", 6 },
        { "acf", 7 },
        { "abcdefg", 8 },
        { "abcdfg", 9 },
    };

    var mappedDigit = new string(outputDigit.Select(x => mapping[x]).OrderBy(x => x).ToArray());

    return digits[mappedDigit];
}
