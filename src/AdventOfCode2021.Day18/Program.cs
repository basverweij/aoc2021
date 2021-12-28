var input = await File.ReadAllLinesAsync("input.txt");

var numbers = input.Select(Number.Parse).ToArray();

var sum = numbers.Skip(1).Aggregate(numbers[0], (a, b) => Number.Add(a, b));

var solution1 = sum.Magnitude;

Console.WriteLine($"Day 18 - Puzzle 1: {solution1}");

var sums = numbers.SelectMany((a, i) => numbers.Where((_, j) => j != i).Select(b => Number.Add(a, b))).ToArray();

var solution2 = sums.Max(n => n.Magnitude);

Console.WriteLine($"Day 18 - Puzzle 2: {solution2}");

sealed class Number
{
    internal static Number Parse(
         string s)
    {
        if (s[0] == '[')
        {
            // pair

            var level = 0;

            for (var i = 1; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case ',' when level == 0:
                        return new Number(
                            Parse(s[1..i]),
                            Parse(s[(i + 1)..^1]));

                    case '[':
                        level++;
                        break;

                    case ']':
                        level--;
                        break;
                }
            }

            throw new ArgumentException($"invalid number: '{s}'");
        }

        // value

        return new Number(int.Parse(s));
    }

    internal static Number Add(
        Number a,
        Number b)
    {
        var sum = new Number(new(a), new(b));

        sum.Reduce();

        return sum;
    }

    public bool IsPair { get; private set; }

    public int Value { get; private set; }

    public Number? Left { get; private set; }

    public Number? Right { get; private set; }

    public Number(
        Number left,
        Number right)
    {
        IsPair = true;

        Left = left;

        Right = right;
    }

    public Number(
        int value)
    {
        IsPair = false;

        Value = value;
    }

    private Number(Number number)
    {
        IsPair = number.IsPair;

        if (IsPair)
        {
            Left = new(number.Left!);

            Right = new(number.Right!);
        }
        else
        {
            Value = number.Value;
        }
    }

    public override string ToString() => IsPair ? $"[{Left},{Right}]" : Value.ToString();

    public int Magnitude => IsPair ? 3 * Left!.Magnitude + 2 * Right!.Magnitude : Value;

    private IEnumerable<(Number number, int level)> All()
    {
        var path = new Stack<(Number number, int level)>();

        path.Push((this, 0));

        while (path.TryPop(out var p))
        {
            yield return (p.number, p.level);

            if (p.number.IsPair)
            {
                path.Push((p.number.Right!, p.level + 1));

                path.Push((p.number.Left!, p.level + 1));
            }
        }
    }

    private void Reduce()
    {
        while (true)
        {
            var all = All().ToArray();

            if (!Explode(all) && !Split(all))
            {
                return;
            }
        }
    }

    private static bool Explode(
        (Number number, int level)[] all)
    {
        for (var i = 0; i < all.Length; i++)
        {
            if (all[i].level == 4 &&
                all[i].number.IsPair)
            {
                for (var j = i - 1; j >= 0; j--)
                {
                    if (!all[j].number.IsPair)
                    {
                        all[j].number.Value += all[i].number.Left!.Value;

                        break;
                    }
                }

                for (var j = i + 3; j < all.Length; j++)
                {
                    if (!all[j].number.IsPair)
                    {
                        all[j].number.Value += all[i].number.Right!.Value;

                        break;
                    }
                }

                all[i].number.Value = 0;

                all[i].number.IsPair = false;

                all[i].number.Left = null;

                all[i].number.Right = null;

                return true;
            }
        }

        return false;
    }

    private static bool Split(
        (Number number, int level)[] all)
    {
        for (var i = 0; i < all.Length; i++)
        {
            if (!all[i].number.IsPair &&
                all[i].number.Value > 9)
            {
                all[i].number.Left = new(all[i].number.Value / 2);

                all[i].number.Right = new((all[i].number.Value + 1) / 2);

                all[i].number.IsPair = true;

                all[i].number.Value = 0;

                return true;
            }
        }

        return false;
    }
}