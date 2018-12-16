using System.Diagnostics;

namespace aoc2018.Code.Day15
{
    [DebuggerDisplay("{Row}, {Col}")]
    public struct Coords
    {
        public int Row { get; }
        public int Col { get; }

        public Coords(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public bool Equals(Coords other)
        {
            return Row == other.Row &&
                   Col == other.Col;
        }

        public override string ToString()
        {
            return "r=" + Row + " c=" + Col;
        }
    }
}
