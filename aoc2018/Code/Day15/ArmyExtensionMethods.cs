using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code.Day15
{
    public static class ArmyExtensionMethods
    {
        public static List<Unit> InReadingOrder(this IEnumerable<Unit> units)
        {
            return units.OrderBy(p => p.Row).ThenBy(p => p.Col).ToList();
        }

        public static Unit TopLeftUnit(this IEnumerable<Unit> units, int index = 0)
        {
            return units.InReadingOrder()[index];
        }
    }
}
