var lines = await File.ReadAllLinesAsync("input.txt");

static (int position, int depth) ParseInstruction(
    string line)
{
    var parts = line.Split(' ');

    var amount = int.Parse(parts[1]);

    return parts[0] switch
    {
        "forward" => (amount, 0),
        "up" => (0, -amount),
        "down" => (0, amount),
        _ => throw new ArgumentOutOfRangeException($"invalid instruction: '{line}'"),
    };
};

var instructions = lines.Select(ParseInstruction).ToArray();

var (position, depth) = (0, 0);

foreach (var instruction in instructions)
{
    position += instruction.position;

    depth += instruction.depth;
}

var solution1 = position * depth;

Console.WriteLine($"Day 2 - Puzzle 1: {solution1}");
