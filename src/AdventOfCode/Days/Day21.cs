namespace AdventOfCode.Days;

public class Day21 : Day
{
    private static long P1W;
    private static long P2W;

    public Day21() : base("21")
    {
    }

    public override string Title => "Dirac Dice";

    public override string ProcessFirst()
    {
        int player1Position = int.Parse(Lines.First()[28..]);
        int player2Position = int.Parse(Lines.Skip(1).First()[28..]);

        int dice = 1;
        int rolls = 0;
        int player1Score = 0;
        int player2Score = 0;
        bool player1Turn = true;
        while (player1Score < 1000 && player2Score < 1000)
        {
            int score = (3 * dice) + 3;
            dice += 3;
            rolls += 3;

            if (player1Turn)
            {
                player1Position = 1 + ((player1Position + score - 1) % 10);
                player1Score += player1Position;
            }
            else
            {
                player2Position = 1 + ((player2Position + score - 1) % 10);
                player2Score += player2Position;
            }

            player1Turn = !player1Turn;
        }

        return $"{(player1Turn ? player1Score * rolls : player2Score * rolls)}";
    }

    public override string ProcessSecond()
    {
        int player1Position = int.Parse(Lines.First()[28..]);
        int player2Position = int.Parse(Lines.Skip(1).First()[28..]);

        Process(0, player1Position, 0, player2Position, true, 1);

        return $"{(P1W > P2W ? P1W : P2W)}";
    }

    private static void Process(int p1Sco, int p1Pos, int p2Sco, int p2Pos, bool player1Turn, long totalOccurences)
    {
        if (p1Sco >= 21)
        {
            P1W += totalOccurences;
            return;
        }

        if (p2Sco >= 21)
        {
            P2W += totalOccurences;
            return;
        }

        foreach ((int diceRolls, int occurences) in new[] { (3, 1), (4, 3), (5, 6), (6, 7), (7, 6), (8, 3), (9, 1) })
        {
            if (player1Turn)
            {
                int player1Position = 1 + ((p1Pos + diceRolls - 1) % 10);
                Process(p1Sco + player1Position, player1Position, p2Sco, p2Pos, false, occurences * totalOccurences);
            }
            else
            {
                int player2Position = 1 + ((p2Pos + diceRolls - 1) % 10);
                Process(p1Sco, p1Pos, p2Sco + player2Position, player2Position, true, occurences * totalOccurences);
            }
        }
    }
}
