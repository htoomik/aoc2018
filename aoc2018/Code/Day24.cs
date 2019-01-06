using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace aoc2018.Code
{
    public static class Day24
    {
        public static (int, string) Solve(string input)
        {
            var (imm, inf) = Parse(input);

            var log = new StringBuilder();
            var winningArmy = Battle(imm, inf, log);
            return (winningArmy.Sum(a => a.UnitCount), log.ToString());
        }

        public static (int boost, int result, string log) Solve2(string input)
        {
            var boost = 1;
            List<Group> winningArmy;
            var log = new StringBuilder();

            while (true)
            {
                var (imm, inf) = Parse(input);

                log.AppendLine();
                log.AppendLine("-------------------");
                log.AppendLine($"Boost = {boost}");
                log.AppendLine("-------------------");

                foreach (var g1 in imm)
                {
                    g1.AttackDamage += boost;
                }

                winningArmy = Battle(imm, inf, log);

                var winner = winningArmy == null ? "null" : winningArmy[0].Allegiance.ToString();
                log.AppendLine($"Boost = {boost}, Winner: {winner}");

                var goodGuysWon = winningArmy != null && winningArmy[0].Allegiance == Allegiance.ImmuneSystem;
                if (goodGuysWon)
                {
                    break;
                }

                boost++;
            }

            return (boost, winningArmy.Sum(g => g.UnitCount), log.ToString());
        }

        private static string LogStateOfArmies(List<Group> imm, List<Group> inf)
        {
            var immSummary = string.Join("\r\n", imm.Select(Summarize));
            var infSummary = string.Join("\r\n", inf.Select(Summarize));
            return $@"
Immune System:
{immSummary}
Infection:
{infSummary}
";
        }

        private static string Summarize(Group g)
        {
            return $"Group {g.Id} contains {g.UnitCount} units. {g.AttackType}, AD {g.AttackDamage}, HP {g.HitPoints}, EP {g.EffectivePower}, I {g.Initiative}, W {g.Weaknesses}, I {g.Immunities}";
        }

        private static List<Group> Battle(List<Group> imm, List<Group> inf, StringBuilder log)
        {
            var r = 1;
            while (imm.Count > 0 && inf.Count > 0)
            {
                log.AppendLine();
                log.AppendLine($"Round {r++}");

                // Find targets
                var allTargets = PairUp(imm, inf, log);

                // If stalemate, exit
                if (!allTargets.Any())
                {
                    log.AppendLine("Nobody found anyone to attack. State of armies:");
                    log.AppendLine(LogStateOfArmies(imm, inf));
                    return null;
                }

                // Attack!
                foreach (var pair in allTargets)
                {
                    Attack(pair.Item1, pair.Item2, log);
                }

                // Remove empty groups
                imm = imm.Where(g => g.UnitCount > 0).ToList();
                inf = inf.Where(g => g.UnitCount > 0).ToList();
            }

            return imm.Count > 0 ? imm : inf;
        }

        public static List<Tuple<Group, Group>> PairUp(List<Group> imm, List<Group> inf, StringBuilder log)
        {
            var immTargets = SelectTargets(imm, inf);
            var infTargets = SelectTargets(inf, imm);

            return infTargets
                .Union(immTargets)
                .OrderByDescending(kvp => kvp.Key.Initiative)
                .Select(kvp => new Tuple<Group, Group>(kvp.Key, kvp.Value))
                .ToList();
        }

        public static (List<Group> imm, List<Group> inf) Parse(string input)
        {
            var armies = input.Replace("\r\n", "\n").Split("\n\n");

            var imm = ParseArmy(armies[0], Allegiance.ImmuneSystem);
            var inf = ParseArmy(armies[1], Allegiance.Infection);

            return (imm, inf);
        }

        public static List<Group> ParseArmy(string armyDefinition, Allegiance allegiance)
        {
            var army = new List<Group>();
            var lines = armyDefinition.Trim().Split("\n");
            var i = 0;

            // First line contains the army's name - skip
            foreach (var line in lines.Skip(1))
            {
                var regex = new Regex(@"(\d+) units each with (\d+) hit points (\((.*)\) )?with an attack that does (\d+) (\w+) damage at initiative (\d+)");
                var match = regex.Match(line.Trim());
                
                var unitCount = int.Parse(match.Groups[1].Value);
                var hitPoints = int.Parse(match.Groups[2].Value);
                var attackDamage = int.Parse(match.Groups[5].Value);
                var initiative = int.Parse(match.Groups[7].Value);

                var attackType = ParseAttackType(match.Groups[6].Value);

                var group = new Group
                {
                    Allegiance = allegiance,
                    Id = i,
                    UnitCount = unitCount,
                    HitPoints = hitPoints,
                    AttackDamage = attackDamage,
                    Initiative = initiative,
                    AttackType = attackType
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

                army.Add(group);

                i++;
            }

            return army;
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
            var sortedAttackers = attackers
                .OrderByDescending(g => g.EffectivePower)
                .ThenByDescending(g => g.Initiative);

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
                // Group will not attack if it will do no damage
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

            var attackPower = defender.Weaknesses.HasFlag(attacker.AttackType) 
                ? attacker.EffectivePower * 2 
                : attacker.EffectivePower;

            // Group will not attack if it will do no damage
            return attackPower > defender.HitPoints ? attackPower : 0;
        }

        private static void Attack(Group attacker, Group defender, StringBuilder log)
        {
            // Can't attack if you're already dead
            if (attacker.UnitCount <= 0)
            {
                return;
            }

            var damage = DamageIfAttacked(attacker, defender);
            var unitsKilled = damage / defender.HitPoints;
            
            log.AppendLine($"{attacker.Allegiance} {attacker.Id} attacks {defender.Allegiance} {defender.Id} for {damage}, {unitsKilled} of {defender.UnitCount} killed");

            defender.UnitCount -= unitsKilled;
        }

        public class Group
        {
            public Allegiance Allegiance { get; set; }
            public int Id { get; set; }

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
