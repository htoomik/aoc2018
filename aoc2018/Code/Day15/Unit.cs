using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code.Day15
{
    public class Unit
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public Race Race { get; }
        public int HitPoints { get; }

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

            var (target, routeLength) = ChooseTarget();
            var nextStep = _engine.ChooseNextStepTowards(this, target, routeLength);
            Step(nextStep);
        }

        public (Coords target, int routeLength) ChooseTarget()
        {
            var enemies = _engine.GetAllEnemies(this);
            var moveTargets = _engine.GetSquaresInRangeOf(enemies);
            var reachableTargets = _engine.GetReachableTargets(this, moveTargets);
            var routes = GetRoutes(reachableTargets);
            var minLength = routes.Min(r => r.Length);
            var closestTargets = routes.Where(r => r.Length == minLength);
            var topLeftTarget = closestTargets.Select(r => r.Target).OrderBy(e => e.Row).ThenBy(e => e.Col).First();
            return (topLeftTarget, minLength);
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
            var targetsInRange = _engine.GetAdjacentEnemies(this);
            if (!targetsInRange.Any())
                return;

            var theTarget = targetsInRange.OrderBy(t => t.HitPoints).ThenBy(t => t.Row).ThenBy(t => t.Col).First();
            Attack(theTarget);
        }

        private void Attack(Unit target)
        {
            throw new NotImplementedException();
        }

        public Coords GetCoords()
        {
            return new Coords(Row, Col);
        }
    }
}
