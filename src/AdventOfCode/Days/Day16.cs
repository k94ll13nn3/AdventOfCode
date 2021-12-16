using System.Text;

namespace AdventOfCode.Days;

public class Day16 : Day
{
    private readonly string _input;

    public Day16() : base("16")
    {
        Dictionary<char, string> map = new()
        {
            ['0'] = "0000",
            ['1'] = "0001",
            ['2'] = "0010",
            ['3'] = "0011",
            ['4'] = "0100",
            ['5'] = "0101",
            ['6'] = "0110",
            ['7'] = "0111",
            ['8'] = "1000",
            ['9'] = "1001",
            ['A'] = "1010",
            ['B'] = "1011",
            ['C'] = "1100",
            ['D'] = "1101",
            ['E'] = "1110",
            ['F'] = "1111",
        };

        _input = Content.Aggregate("", (acc, c) => acc + map[c]);
    }

    public override string Title => "Packet Decoder";

    public override string ProcessFirst()
    {
        (Packet packet, _) = ExtractPacket(_input, 0);

        return $"{GetVersionSum(packet)}";
    }

    public override string ProcessSecond()
    {
        (Packet packet, _) = ExtractPacket(_input, 0);

        return $"{GetValue(packet)}";
    }

    private (Packet packet, int endingIndex) ExtractPacket(string input, int startingIndex)
    {
        int currentIndex = startingIndex;
        int version = Convert.ToInt32(input[currentIndex..(currentIndex + 3)], 2);
        currentIndex += 3;
        int type = Convert.ToInt32(input[currentIndex..(currentIndex + 3)], 2);
        currentIndex += 3;

        long? value = null;
        if (type == 4)
        {
            var sb = new StringBuilder();
            while (input[currentIndex] == '1')
            {
                sb.Append(input[(currentIndex + 1)..(currentIndex + 5)]);
                currentIndex += 5;
            }

            sb.Append(input[(currentIndex + 1)..(currentIndex + 5)]);
            currentIndex += 5;

            value = Convert.ToInt64(sb.ToString(), 2);
        }

        var subPackets = new List<Packet>();
        int? lengthType = null;
        if (type != 4)
        {
            lengthType = input[currentIndex++] - '0';
            if (lengthType == 0)
            {
                int seenSize = 0;
                int sizeOfSubPackets = Convert.ToInt32(input[currentIndex..(currentIndex + 15)], 2);
                currentIndex += 15;
                while (seenSize < sizeOfSubPackets)
                {
                    (Packet packet, int endingIndex) = ExtractPacket(input, currentIndex);
                    subPackets.Add(packet);
                    seenSize += (endingIndex - currentIndex);
                    currentIndex = endingIndex;
                }
            }

            if (lengthType == 1)
            {
                int seenPackets = 0;
                int numberOfSubPackets = Convert.ToInt32(input[currentIndex..(currentIndex + 11)], 2);
                currentIndex += 11;
                while (seenPackets < numberOfSubPackets)
                {
                    (Packet packet, int endingIndex) = ExtractPacket(input, currentIndex);
                    subPackets.Add(packet);
                    seenPackets++;
                    currentIndex = endingIndex;
                }
            }
        }

        return (new Packet(version, type, value, lengthType, subPackets), currentIndex);
    }

    private long GetValue(Packet packet)
    {
        return packet.Type switch
        {
            0 => packet.SubPackets.Sum(GetValue),
            1 => packet.SubPackets.Aggregate(1L, (acc, p) => acc * GetValue(p)),
            2 => packet.SubPackets.Min(GetValue),
            3 => packet.SubPackets.Max(GetValue),
            4 => packet.Value!.Value,
            5 => GetValue(packet.SubPackets[0]) > GetValue(packet.SubPackets[1]) ? 1 : 0,
            6 => GetValue(packet.SubPackets[0]) < GetValue(packet.SubPackets[1]) ? 1 : 0,
            7 => GetValue(packet.SubPackets[0]) == GetValue(packet.SubPackets[1]) ? 1 : 0,
            _ => throw new InvalidOperationException(),
        };
    }

    private static int GetVersionSum(Packet packet)
    {
        if (packet.SubPackets.Count == 0)
        {
            return packet.Version;
        }
        else
        {
            return packet.Version + packet.SubPackets.Select(GetVersionSum).Sum();
        }
    }

    private sealed record Packet(int Version, int Type, long? Value, int? LengthType, IList<Packet> SubPackets);
}
