var input = await File.ReadAllTextAsync("input.txt");

var parts = input[15..].Split(", y=").SelectMany(p => p.Split("..")).Select(int.Parse).ToArray();

var targetX = (min: parts[0], max: parts[1]);

var targetY = (min: parts[2], max: parts[3]);

var solution1 = int.MinValue;

for (var y = 1; y <= targetX.max; y++)
{
    if (CanLaunch(y, targetX, targetY, out var maxY) &&
        maxY > solution1)
    {
        solution1 = maxY;
    }
}

Console.WriteLine($"Day 17 - Puzzle 1: {solution1}");

bool CanLaunch(
    int yVelocity,
    (int min, int max) targetX,
    (int min, int max) targetY,
    out int maxY)
{
    maxY = int.MinValue;

    for (var xVelocity = 1; xVelocity <= targetX.max; xVelocity++)
    {
        if (SimulateLaunch(yVelocity, xVelocity, targetX, targetY, out var simulationMaxY))
        {
            maxY = simulationMaxY;

            return true;
        }
    }

    return false;
}

bool SimulateLaunch(
    int yVelocity,
    int xVelocity,
    (int min, int max) targetX,
    (int min, int max) targetY,
    out int maxY)
{
    maxY = int.MinValue;

    var (x, y) = (0, 0);

    while (y >= targetY.min)
    {
        x += xVelocity;

        y += yVelocity;

        if (y > maxY)
        {
            maxY = y;
        }

        if (x >= targetX.min && x <= targetX.max &&
            y >= targetY.min && y <= targetY.max)
        {
            return true;
        }

        xVelocity += xVelocity switch
        {
            0 => 0,
            < 0 => 1,
            > 0 => -1,
        };

        yVelocity--;
    }

    return false;
}
