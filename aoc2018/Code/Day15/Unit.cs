using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace aoc2018.Code.Day15
{
    [DebuggerDisplay("{Race} {Row},{Col} - {HitPoints}")]
    public class Unit
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public Race Race { get; }
        public int HitPoints { get; set; }
        public int AttackPower { get; set; }

        private readonly Engine _engine;

        public Unit(int row, int col, Race race, Engine engine = null)
        {
            _engine = engine;

            Row = row;
            Col = col;
            Race = race;
            HitPoints = 200;
        }

        public void Move()
        {
            var targetsInRange = _engine.GetAdjacentEnemies(this);
            if (targetsInRange.Any())
                return;

            var canMove = ChooseTarget(out var target, out var routeLength);
            if (!canMove)
                return;

            var nextStep = _engine.ChooseNextStepTowards(this, target.Value, routeLength.Value);
            Step(nextStep);
        }

        public bool ChooseTarget(out Coords? target, out int? routeLength)
        {
            target = null;
            routeLength = null;

            var enemies = _engine.GetAllEnemies(this);
            var moveTargets = _engine.GetSquaresInRangeOf(enemies);
            if (!moveTargets.Any())
                return false;

            var reachableTargets = _engine.GetReachableTargets(this, moveTargets);
            if (!reachableTargets.Any())
                return false;

            var routes = GetRoutes(reachableTargets);
            var minLength = routes.Min(r => r.Length);
            var closestTargets = routes.Where(r => r.Length == minLength);
            var closestTarget = closestTargets.Select(r => r.Target).TopLeft();

            routeLength = minLength;
            target = closestTarget;
            return true;
        }

        private void Step(Coords coords)
        {
            Row = coords.Row;
            Col = coords.Col;
        }

        private List<Route> GetRoutes(List<Coords> targets)
        {
            var routes = new List<Route>();
            foreach (var target in targets)
            {
                routes.Add(_engine.FindShortestRoute(this, target));
            }

            return routes;
        }

        public void Attack()
        {
            var theTarget = _engine.ChooseAttackTarget(this);
            if (theTarget != null)
                Attack(theTarget);
        }

        private void Attack(Unit target)
        {
            target.HitPoints -= AttackPower;
            if (target.HitPoints <= 0)
            {
                _engine.Units.Remove(target);
            }
        }

        public Coords GetCoords()
        {
            return new Coords(Row, Col);
        }

        public override string ToString()
        {
            return Race.ToString()[0] + " " + GetCoords() + " hp=" + HitPoints;
        }
    }
}
