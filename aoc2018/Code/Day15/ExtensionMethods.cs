using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code.Day15
{
    public static class ExtensionMethods
    {
        public static List<Unit> InReadingOrder(this IEnumerable<Unit> units)
        {
            return units.OrderBy(p => p.Row).ThenBy(p => p.Col).ToList();
        }

        public static List<Coords> InReadingOrder(this IEnumerable<Coords> coords)
        {
            return coords.OrderBy(p => p.Row).ThenBy(p => p.Col).ToList();
        }

        public static Unit TopLeft(this IEnumerable<Unit> units, int index = 0)
        {
            return units.InReadingOrder()[index];
        }

        public static Coords TopLeft(this IEnumerable<Coords> coords, int index = 0)
        {
            return coords.InReadingOrder()[index];
        }
    }
}
