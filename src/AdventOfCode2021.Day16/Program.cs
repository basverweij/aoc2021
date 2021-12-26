var input = File.ReadAllText("input.txt");

ReadOnlySpan<byte> bits = input.SelectMany(ParseHexDigits).ToArray().AsSpan<byte>();

var packet = ReadPacket(ref bits);

var solution1 = packet.GetVersionSum();

Console.WriteLine($"Day 16 - Puzzle 1: {solution1}");

static IEnumerable<byte> ParseHexDigits(
    char digit)
{
    var digits = new Dictionary<char, byte>()
    {
        ['0'] = 0b0000,
        ['1'] = 0b0001,
        ['2'] = 0b0010,
        ['3'] = 0b0011,
        ['4'] = 0b0100,
        ['5'] = 0b0101,
        ['6'] = 0b0110,
        ['7'] = 0b0111,
        ['8'] = 0b1000,
        ['9'] = 0b1001,
        ['A'] = 0b1010,
        ['B'] = 0b1011,
        ['C'] = 0b1100,
        ['D'] = 0b1101,
        ['E'] = 0b1110,
        ['F'] = 0b1111,
    };

    var value = digits[digit];

    yield return (byte)((value & 0b1000) >> 3);

    yield return (byte)((value & 0b0100) >> 2);

    yield return (byte)((value & 0b0010) >> 1);

    yield return (byte)(value & 0b0001);
}

static Packet ReadPacket(
    ref ReadOnlySpan<byte> bits)
{
    var version = ReadValue(ref bits, 3);

    var typeId = ReadValue(ref bits, 3);

    if (typeId == 4)
    {
        // literal value

        var literalValue = ReadLiteralValue(ref bits);

        return new(version, typeId, 0, 0, literalValue);
    }
    else
    {
        // operator

        var lengthId = ReadValue(ref bits, 1);

        var length = lengthId == 0 ?
            ReadValue(ref bits, 15) : // length of sub-packets in bits
            ReadValue(ref bits, 11); // number of sub-packets

        var packet = new Packet(version, typeId, lengthId, length, 0);

        if (lengthId == 0)
        {
            var remainingLength = bits.Length - length;

            while (bits.Length > remainingLength)
            {
                var subPacket = ReadPacket(ref bits);

                packet.SubPackets.Add(subPacket);
            }
        }
        else
        {
            for (var i = 0; i < length; i++)
            {
                var subPacket = ReadPacket(ref bits);

                packet.SubPackets.Add(subPacket);
            }
        }

        return packet;
    }
}

static int ReadValue(
    ref ReadOnlySpan<byte> bits,
    int length)
{
    var value = 0;

    for (var i = 0; i < length; i++)
    {
        value <<= 1;

        value |= bits[i];
    }

    bits = bits[length..];

    return value;
}

static long ReadLiteralValue(
    ref ReadOnlySpan<byte> bits)
{
    var value = 0L;

    while (true)
    {
        var flag = ReadValue(ref bits, 1);

        value += ReadValue(ref bits, 4);

        if (flag == 0)
        {
            break;
        }

        value <<= 4;
    }

    return value;
}

sealed record Packet(
    int Version,
    int TypeId,
    int LengthId,
    int Length,
    long LiteralValue)
{
    public List<Packet> SubPackets { get; init; } = new();

    public int GetVersionSum() => Version + SubPackets.Sum(p => p.GetVersionSum());
}
