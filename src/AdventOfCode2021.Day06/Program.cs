var input = await File.ReadAllTextAsync("input.txt");

var fish = input.Split(',').Select(int.Parse).ToList();

for (var i = 0; i < 80; i++)
{
    var n = fish.Count;

    for (var j = 0; j < n; j++)
    {
        if (fish[j] == 0)
        {
            fish[j] = 6;

            fish.Add(8);
        }
        else
        {
            fish[j]--;
        }
    }
}

var solution1 = fish.Count;

Console.WriteLine($"Day 6 - Puzzle 1: {solution1}");
