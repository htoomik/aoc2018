using System.IO;
using System.Linq;
using System.Text;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test24
    {
        private readonly ITestOutputHelper _output;
        
        public Test24(ITestOutputHelper output)
        {
            _output = output;
        }

        private const string Input = @"
Immune System:
17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2
989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3

Infection:
801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1
4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4";

        [Fact]
        public void TestParse()
        {
            var (imm, inf) = Day24.Parse(Input.Trim());
            Assert.Equal(2, imm.Count);
            Assert.Equal(2, inf.Count);

            Assert.Equal(17, imm[0].UnitCount);
            Assert.Equal(5390, imm[0].HitPoints);
            Assert.Equal(4507, imm[0].AttackDamage);
            Assert.Equal(2, imm[0].Initiative);
            Assert.Equal(Day24.AttackType.Fire, imm[0].AttackType);
            Assert.Equal(Day24.AttackType.Radiation | Day24.AttackType.Bludgeoning, imm[0].Weaknesses);
            Assert.Equal(Day24.AttackType.None, imm[0].Immunities);

            Assert.Equal(989, imm[1].UnitCount);
            Assert.Equal(1274, imm[1].HitPoints);
            Assert.Equal(25, imm[1].AttackDamage);
            Assert.Equal(3, imm[1].Initiative);
            Assert.Equal(Day24.AttackType.Slashing, imm[1].AttackType);
            Assert.Equal(Day24.AttackType.Slashing | Day24.AttackType.Bludgeoning, imm[1].Weaknesses);
            Assert.Equal(Day24.AttackType.Fire, imm[1].Immunities);
        }

        [Theory]
        [InlineData("1 units each with 1 hit points with an attack that does 1 cold damage at initiative 1")]
        [InlineData("1 units each with 1 hit points (weak to radiation) with an attack that does 1 cold damage at initiative 1")]
        [InlineData("1 units each with 1 hit points (immune to radiation) with an attack that does 1 cold damage at initiative 1")]
        [InlineData("1 units each with 1 hit points (immune to radiation; weak to fire) with an attack that does 1 cold damage at initiative 1")]
        [InlineData("1 units each with 1 hit points (weak to radiation; immune to fire) with an attack that does 1 cold damage at initiative 1")]
        public void TestParseSpecials(string input)
        {
            Day24.ParseArmy(input, Day24.Allegiance.ImmuneSystem);
        }

        [Fact]
        public void TestDamageIfAttacked()
        {
            var (imm, inf) = Day24.Parse(Input.Trim());

            var inf1AttacksImm1 = Day24.DamageIfAttacked(inf[0], imm[0]);
            Assert.Equal(185832, inf1AttacksImm1);

            var inf1AttacksImm2 = Day24.DamageIfAttacked(inf[0], imm[1]);
            Assert.Equal(185832, inf1AttacksImm2);

            var inf2AttacksImm2 = Day24.DamageIfAttacked(inf[1], imm[1]);
            Assert.Equal(107640 , inf2AttacksImm2);

            var imm1AttacksInf1 = Day24.DamageIfAttacked(imm[0], inf[0]);
            Assert.Equal(76619, imm1AttacksInf1);

            var imm1AttacksInf2 = Day24.DamageIfAttacked(imm[0], inf[1]);
            Assert.Equal(153238, imm1AttacksInf2);

            var imm2AttacksInf1 = Day24.DamageIfAttacked(imm[1], inf[0]);
            Assert.Equal(24725, imm2AttacksInf1);
        }

        [Fact]
        public void TestSelectTargets()
        {
            var (imm, inf) = Day24.Parse(Input.Trim());
            var log = new StringBuilder();

            var targets1 = Day24.SelectTargets(inf, imm);
            Assert.Equal(targets1[inf[0]], imm[0]);
            Assert.Equal(targets1[inf[1]], imm[1]);

            var targets2 = Day24.SelectTargets(imm, inf);
            Assert.Equal(targets2[imm[1]], inf[0]);
            Assert.Equal(targets2[imm[0]], inf[1]);
        }

        [Fact]
        public void TestPairUp()
        {
            var (imm, inf) = Day24.Parse(Input.Trim());
            var log = new StringBuilder();

            var targets = Day24.PairUp(imm, inf, log).ToList();

            Assert.Equal(inf[1], targets[0].Item1);
            Assert.Equal(imm[1], targets[0].Item2);

            Assert.Equal(imm[1], targets[1].Item1);
            Assert.Equal(inf[0], targets[1].Item2);
        }

        [Fact]
        public void Test()
        {
            var (result, log) = Day24.Solve(Input);
            Assert.Equal(5216, result);
            _output.WriteLine(log);
        }

        [Fact]
        public void Test2()
        {
            var (boost, result, log) = Day24.Solve2(Input);
            Assert.Equal(1570, boost);
            Assert.Equal(51, result);
            _output.WriteLine(log);
        }

        [Fact]
        public void Test2a()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input24_a.txt");
            var (boost, result, log) = Day24.Solve2(input);
            Assert.Equal(90, boost);
            Assert.Equal(434, result);

            File.WriteAllText("C:\\Code\\aoc2018\\output24a.txt", log);
        }

        [Fact]
        public void TestSelectTargets_a()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input24_a.txt");
            var (imm, inf) = Day24.Parse(input.Trim());

            foreach (var g in imm)
            {
                g.AttackDamage += 90;
            }

            var log = new StringBuilder();

            var targets1 = Day24.SelectTargets(inf, imm);
            var targets2 = Day24.SelectTargets(imm, inf);
            
            _output.WriteLine(log.ToString());

            var inf3 = inf.Single(a => a.Id == 3);
            var inf3target = targets1[inf3];
            Assert.Equal(0, inf3target.Id);

            var imm1 = imm.Single(a => a.Id == 1);
            var imm1Target = targets2[imm1];
            Assert.Equal(7, imm1Target.Id);
        }

        [Fact]
        public void Solve()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input24.txt");
            var result = Day24.Solve(input);
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Solve2()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input24.txt");
            var (boost, result, log) = Day24.Solve2(input);
            _output.WriteLine(boost.ToString());
            File.WriteAllText("C:\\Code\\aoc2018\\output24.txt", log);
            _output.WriteLine(log);
        }
    }
}
