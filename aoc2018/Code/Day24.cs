using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc2018.Code
{
    class Day24
    {
        public static int Solve(string input)
        {
            var (imm, inf) = Parse(input);

            while (imm.Count > 0 && inf.Count > 0)
            {
                var allTargets = PairUp(imm, inf);

                foreach (var pair in allTargets)
                {
                    Attack(pair.Item1, pair.Item2);
                }

                imm = imm.Where(g => g.UnitCount > 0).ToList();
                inf = inf.Where(g => g.UnitCount > 0).ToList();
            }

            var winningArmy = imm.Count > 0 ? imm : inf;
            return winningArmy.Sum(g => g.UnitCount);
        }

        public static IEnumerable<Tuple<Group, Group>> PairUp(List<Group> imm, List<Group> inf)
        {
            var immTargets = SelectTargets(imm, inf);
            var infTargets = SelectTargets(inf, imm);

            return infTargets
                .Union(immTargets)
                .OrderByDescending(kvp => kvp.Key.Initiative)
                .Select(kvp => new Tuple<Group, Group>(kvp.Key, kvp.Value));
        }

        public static (List<Group> imm, List<Group> inf) Parse(string input)
        {
            var armies = input.Replace("\r\n", "\n").Split("\n\n");

            var imm = ParseArmy(armies[0], "Immune system");
            var inf = ParseArmy(armies[1], "Infection");

            return (imm, inf);
        }

        public static List<Group> ParseArmy(string army, string name)
        {
            var imm = new List<Group>();
            var lines = army.Trim().Split("\n");
            var i = 0;

            // First line contains the army's name - skip
            foreach (var line in lines.Skip(1))
            {
                i++;
                var regex = new Regex(@"(\d+) units each with (\d+) hit points (\((.*)\) )?with an attack that does (\d+) (\w+) damage at initiative (\d+)");
                var match = regex.Match(line.Trim());
                
                var unitCount = int.Parse(match.Groups[1].Value);
                var hitPoints = int.Parse(match.Groups[2].Value);
                var attackDamage = int.Parse(match.Groups[5].Value);
                var initiative = int.Parse(match.Groups[7].Value);

                var attackType = ParseAttackType(match.Groups[6].Value);

                var group = new Group
                {
                    Name =  name + " " + i,
                    UnitCount = unitCount,
                    HitPoints = hitPoints,
                    AttackDamage = attackDamage,
                    Initiative = initiative,
                    AttackType = attackType,
                };

                // Immunities and weaknesses are hard to regex.
                // One, both or neither may be present, and the order is unpredictable.
                var immunitiesAndWeaknesses = match.Groups[4].Value;
                var parts = immunitiesAndWeaknesses.Split("; ");
                var immunityPart = parts.SingleOrDefault(p => p.Contains("immune to"));
                var weaknessPart = parts.SingleOrDefault(p => p.Contains("weak to"));

                if (immunityPart != null)
                {
                    group.Immunities = immunityPart.Replace("immune to ", "").Split(", ").Select(ParseAttackType).Aggregate((at, at2) => at | at2);
                }

                if (weaknessPart != null)
                {
                    group.Weaknesses = weaknessPart.Replace("weak to ", "").Split(", ").Select(ParseAttackType).Aggregate((at, at2) => at | at2);
                }

                imm.Add(group);
            }

            return imm;
        }

        private static AttackType ParseAttackType(string value)
        {
            if (string.IsNullOrEmpty(value))
                return AttackType.None;

            var s = value.Substring(0, 1).ToUpper() + value.Substring(1);
            var attackType = Enum.Parse<AttackType>(s);
            return attackType;
        }

        public static Dictionary<Group, Group> SelectTargets(List<Group> attackers, List<Group> defenders)
        {
            var sortedAttackers = attackers.OrderByDescending(g => g.EffectivePower).ThenByDescending(g => g.Initiative);
            var targets = new Dictionary<Group, Group>();
            var availableDefenders = new List<Group>();
            availableDefenders.AddRange(defenders);

            foreach (var attacker in sortedAttackers)
            {
                var damagePerDefender = new Dictionary<Group, int>();
                foreach (var defender in availableDefenders)
                {
                    var damage = DamageIfAttacked(attacker, defender);
                    damagePerDefender[defender] = damage;
                }

                var ordered = damagePerDefender
                    .OrderByDescending(kvp => kvp.Value)
                    .ThenByDescending(kvp => kvp.Key.EffectivePower)
                    .ThenByDescending(kvp => kvp.Key.Initiative);
                var bestTarget = ordered.FirstOrDefault();
                if (bestTarget.Value > 0)
                {
                    targets[attacker] = bestTarget.Key;
                    availableDefenders.Remove(bestTarget.Key);
                }
            }

            return targets;
        }

        public static int DamageIfAttacked(Group attacker, Group defender)
        {
            if (defender.Immunities.HasFlag(attacker.AttackType))
            {
                return 0;
            }

            if (defender.Weaknesses.HasFlag(attacker.AttackType))
            {
                return attacker.EffectivePower * 2;
            }

            return attacker.EffectivePower;
        }

        public static void Attack(Group attacker, Group defender)
        {
            if (attacker.UnitCount <= 0)
                return;

            var damage = DamageIfAttacked(attacker, defender);
            var unitsKilled = damage / defender.HitPoints;
            defender.UnitCount -= unitsKilled;
        }

        [DebuggerDisplay("{Name}")]
        public class Group
        {
            public string Name { get; set; }
            public int UnitCount { get; set; }
            public int HitPoints { get; set; }
            public int AttackDamage { get; set; }
            public int Initiative { get; set; }
            public AttackType AttackType { get; set; }
            public AttackType Weaknesses { get; set; }
            public AttackType Immunities { get; set; }

            public int EffectivePower => UnitCount * AttackDamage;
        }

        public enum Allegiance
        {
            ImmuneSystem,
            Infection
        }

        [Flags]
        public enum AttackType
        {
            None = 0,
            Fire = 1,
            Radiation = 2,
            Bludgeoning = 4,
            Slashing = 8,
            Cold = 16
        }
    }
}
