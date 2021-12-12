var input = await File.ReadAllLinesAsync("input.txt");

var displays = input.Select(ParseDisplay).ToArray();

var solution1 = displays.Sum(display => display.outputDigits.Count(digit => digit.Length is 2 or 3 or 4 or 7));

Console.WriteLine($"Day 8 - Puzzle 1: {solution1}");

(string[] signalPatterns, string[] outputDigits) ParseDisplay(
    string input)
{
    var parts = input.Split(" | ");

    var signalPatterns = parts[0].Split(' ').ToArray();

    var outputDigits = parts[1].Split(' ').ToArray();

    return (signalPatterns, outputDigits);
}