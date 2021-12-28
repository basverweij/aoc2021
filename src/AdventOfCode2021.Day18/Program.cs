//var input = await File.ReadAllLinesAsync("input.txt");

var input = new[]
{
    "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]",
    "[[[5,[2,8]],4],[5,[[9,9],0]]]",
    "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]",
    "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]",
    "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]",
    "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]",
    "[[[[5,4],[7,7]],8],[[8,3],8]]",
    "[[9,3],[[9,9],[6,[4,9]]]]",
    "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
    "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]",
};

var numbers = input.Select(Number.Parse).ToArray();

var sum = numbers.Skip(1).Aggregate(numbers[0], (a, b) => Number.Add(a, b));

Console.WriteLine(sum);

var solution1 = sum.Magnitude;

Console.WriteLine($"Day 18 - Puzzle 1: {solution1}");

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
        var sum = new Number(a, b);

        sum.Reduce();

        return sum;
    }

    public bool IsPair { get; set; }

    public int Value { get; set; }

    public Number? Left { get; set; }

    public Number? Right { get; set; }

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

    public override string ToString() => IsPair ? Value.ToString() : $"[{Left},{Right}]";

    public int Magnitude => IsPair ? Value : 3 * Left!.Magnitude + 2 * Right!.Magnitude;

    private bool Visit(
        INumberVisitor visitor)
    {
        var path = new Stack<Number>();

        path.Push(this);

        while (path.Any())
        {
            var number = path.Pop();

            if (visitor.Visit(path, number))
            {
                // stop if visitor returns true

                return true;
            }

            if (number.IsPair)
            {
                path.Push(number.Left!);

                path.Push(number.Right!);
            }
        }

        return false;
    }

    private void Reduce()
    {
        while (Visit(new ExplodeVisitor()) || Visit(new SplitVisitor()))
        {
            // keep reducing
        }
    }
}

interface INumberVisitor
{
    bool Visit(
        Stack<Number> path,
        Number number);
}

sealed class ExplodeVisitor :
    INumberVisitor
{
    public bool Visit(
        Stack<Number> path,
        Number number)
    {
        if (path.Count < 4 || !number.IsPair)
        {
            return false;
        }



        return true;
    }
}

sealed class SplitVisitor :
    INumberVisitor
{
    public bool Visit(
        Stack<Number> path,
        Number number)
    {
        if (number.IsPair ||
            number.Value < 10)
        {
            return false;
        }

        // split number

        number.Left = new(number.Value / 2);

        number.Right = new((number.Value + 1) / 2);

        number.Value = 0;

        number.IsPair = true;

        return true;
    }
}
