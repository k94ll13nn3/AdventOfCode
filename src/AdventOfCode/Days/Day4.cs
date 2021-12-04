namespace AdventOfCode.Days;

public class Day4 : Day
{
    private List<(int?[][] board, int lastDraw)> _winningBoards = new();

    public Day4() : base("4")
    {
    }

    public override string Title => "Giant Squid";

    public override string ProcessFirst()
    {
        _winningBoards = GetWinningBoards().ToList();
        (int?[][] board, int lastDraw) = _winningBoards[0];

        return $"{lastDraw * (board.Select(c => c.Sum()).Sum())}";
    }

    public override string ProcessSecond()
    {
        (int?[][] board, int lastDraw) = _winningBoards[1];

        return $"{lastDraw * (board.Select(c => c.Sum()).Sum())}";
    }

    private List<(int?[][] board, int lastDraw)> GetWinningBoards()
    {
        int[] bingoDraws = Lines.First().Split(",").Select(int.Parse).ToArray();

        var boards = new List<int?[][]>();

        for (int index = 2; index + 5 < Lines.Count(); index += 6)
        {
            int?[][] board = Lines.Take(index..(index + 5)).Select(c => c.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Cast<int?>().ToArray()).ToArray();
            boards.Add(board);
        }

        int numberOfBoards = boards.Count;
        var winnerBoards = new List<(int?[][] board, int lastDraw)>();
        for (int drawIndex = 0; boards.Count > 0; drawIndex++)
        {
            for (int i = 0; i < boards.Count; i++)
            {
                int?[][]? board = boards[i];
                for (int x = 0; x < board.Length; x++)
                {
                    for (int y = 0; y < board[0].Length; y++)
                    {
                        if (board[x][y] == bingoDraws[drawIndex])
                        {
                            board[x][y] = null;
                        }
                    }
                }

                if (IsBoardComplete(board))
                {
                    // Keep only first and last winners.
                    if (boards.Count == numberOfBoards || boards.Count == 1)
                    {
                        winnerBoards.Add((board, bingoDraws[drawIndex]));
                    }

                    boards.Remove(board);
                    i--;
                }
            }
        }

        return winnerBoards;
    }

    private static bool IsBoardComplete(int?[][] board)
    {
        for (int i = 0; i < board.Length; i++)
        {
            if (board.Select(c => c[i]).All(val => val is null))
            {
                return true;
            }
        }

        return board.Any(c => c.All(i => i is null));
    }
}
